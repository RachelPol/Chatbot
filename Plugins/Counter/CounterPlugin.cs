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
            if (input.Message == "")
            {
                input.Callbacks.StartSession();
                return new PluginOutput("Counter started. Enter 'Exit' to stop.");
            }
            else if (input.Message.ToLower() == "exit")
            {
                input.Callbacks.EndSession();
                return new PluginOutput("Counter stopped.");
            }
            else
            {
                try
                {
                    var lastCount = int.Parse(input.Message.ToString());
                    var result = (lastCount + 1).ToString();
                    return new PluginOutput(result, result);

                } 
                catch (Exception ex)
                {
                    return new PluginOutput("enter Number");
                }
                   
               
            }
            
           
        }
    }
}
