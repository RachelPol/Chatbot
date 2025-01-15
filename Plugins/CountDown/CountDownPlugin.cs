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

        public  PluginOutput Execute(PluginInput input)
        {
           int interval;
            if (string.IsNullOrWhiteSpace(input.Message) || !int.TryParse(input.Message, out interval))
            {
                Console.WriteLine("Please enter a valid number for the countdown interval:");

                // ממתין לקבלת קלט עד שהוא יהיה חוקי
                while (!int.TryParse(Console.ReadLine(), out interval))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number:");
                }
            }
            _scheduler.Schedule(TimeSpan.FromSeconds(interval), Id, "");
            return new PluginOutput("Countdown started.");

        }

        public void OnScheduler(string data)
        {
            Console.WriteLine("Fired.");
        }
    }
}
