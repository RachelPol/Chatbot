using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;

namespace Counter
{
    public class CounterPlugin : IPlugin
    {
        public static string _Id => "counter";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            if (int.TryParse(input.PersistentData, out int lastCount))
            {
                // ההמרה הצליחה, ערך lastCount מוגדר
            }
            else
            {
                lastCount = 0; // ערך ברירת מחדל במקרה של שגיאה
            }

            var result = (lastCount + 1).ToString();
            return new PluginOutput(result, result);
        }
    }
}
