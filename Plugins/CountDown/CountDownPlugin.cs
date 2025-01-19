using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;
using System.Collections.Generic;

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
            if (input.Message == "")
            {
                input.Callbacks.StartSession();
                return new PluginOutput("CountDown started. Enter 'Exit' to stop.");
            }
            else if (input.Message.ToLower() == "exit")
            {
                input.Callbacks.EndSession();
                return new PluginOutput("CountDown stopped.");
            }
            else
            {
                try
                {
                    var interval = int.Parse(input.Message);
                    _scheduler.Schedule(TimeSpan.FromSeconds(interval), Id, "");
                    return new PluginOutput("Countdown started.");
                }
                catch (Exception ex)
                {
                    return new PluginOutput("Error");
                }
            }
        }

        public void OnScheduler(string data)
        {
            Console.WriteLine("Fired.");
        }
    }
}
