using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;

namespace Counter
{
    public class CounterPlugin : IPlugin
    {
        public static string _Id => "counter";
        public string Id => _Id;
        static int c = 1;
        public PluginOutput Execute(PluginInput input)
        {
           
            if (input.Message == "")
            {
                input.Callbacks.StartSession();
                //input.Message = "1";
                input.Callbacks.EndSession();
                return new PluginOutput($"Count number:{c++}", input.PersistentData);
            }
            else
            {
                var lastCount = int.Parse(input.PersistentData);
                var result = (lastCount + 1).ToString();
                return new PluginOutput(result, result);
            }
        }
    }
}
