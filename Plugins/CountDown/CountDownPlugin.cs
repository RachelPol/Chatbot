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
            int interval;
            // ננסה להמיר את ההודעה למספר שלם
            if (!int.TryParse(input.Message, out  interval) || string.IsNullOrWhiteSpace(input.Message))
            {
                Console.WriteLine("please enter a valid number for the countdown interval");
                while(!int.TryParse(Console.ReadLine(), out interval))
                {
                    Console.WriteLine("invalid input .");
                }
            }
            _scheduler.Schedule(TimeSpan.FromSeconds(interval), Id, "");
            return new PluginOutput("Countdown started");
   
        }

        public void OnScheduler(string data)
        {
            Console.WriteLine("Fired.");
        }
    }
}
