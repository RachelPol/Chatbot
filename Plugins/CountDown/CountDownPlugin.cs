using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;

namespace CountDown
{
    public class CountDownPlugin : IPluginWithScheduler
    {
        IScheduler _scheduler;

        public CountDownPlugin(IScheduler scheduler) => _scheduler = scheduler;

        public static string _Id = "count-down";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            int interval;
            if (string.IsNullOrWhiteSpace(input.Message))
            {
                return new PluginOutput("Input cannot be empty.");
            }

            if (int.TryParse(input.Message, out interval))
            {
                _scheduler.Schedule(TimeSpan.FromSeconds(interval), Id, "");
                return new PluginOutput("Countdown started.");
            }
            else
            {
                return new PluginOutput("The provided input is not a valid integer.");
            }
        }

        public void OnScheduler(string data)
        {
            Console.WriteLine("Fired.");
        }
    }
}
