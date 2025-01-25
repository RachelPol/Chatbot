﻿using BasePlugin.Interfaces;
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
            int lastCount = 0;
            if (!int.TryParse(input.PersistentData, out lastCount))
            {
                lastCount = 0;
            }
            var result = (lastCount + 1).ToString();
            return new PluginOutput(result, result);
        }
    }
}
