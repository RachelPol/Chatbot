using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;
using System.Collections.Generic;
using System.Threading;

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
            if (string.IsNullOrEmpty(input.Message))
            {
                return new PluginOutput("Invalid input. Please provide an integer value for the interval.");
            }

            if (!int.TryParse(input.Message, out int interval))
            {
                return new PluginOutput("Invalid input. Please provide a valid integer value.");
            }

            if (interval <= 0)
            {
                return new PluginOutput("Interval must be a positive integer value.");
            }

            try
            {
                _scheduler.Schedule(TimeSpan.FromSeconds(interval), Id, "");
                Console.WriteLine("Countdown started!");
                while (interval >= 0)
                {
                    Console.WriteLine(interval);
                    interval--;
                    // הוסף כאן כל פעולה נוספת שתרצה לבצע בכל איטרציה, למשל, עדכון ממשק משתמש
                    Thread.Sleep(1000); // המתנה של שניה אחת בין כל איטרציה
                }
                

                return new PluginOutput("Countdown finished.");
                

            }
            catch (Exception ex)
            {
                return new PluginOutput($"Failed to schedule countdown: {ex.Message}");
            }
        }

        public void OnScheduler(string data)
        {

            Console.WriteLine("Fired.");
        }
    }
}
