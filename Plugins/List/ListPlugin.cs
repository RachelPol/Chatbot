using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ListPlugin
{
    record PersistentDataStructure(List<string> List);

    public class ListPlugin : IPlugin
    {
        public static string _Id = "list";
        public string Id => _Id;

        public PluginOutput Execute(PluginInput input)
        {
            List<string> list = new();

            if (string.IsNullOrEmpty(input.PersistentData) == false)
            {
                list = JsonSerializer.Deserialize<PersistentDataStructure>(input.PersistentData).List;
            }

            if (string.IsNullOrWhiteSpace(input.Message))
            {
                input.Callbacks.StartSession();
                return new PluginOutput("List started. Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'List' to view all list. Enter 'Exit' to stop.", input.PersistentData);
            }
            else if (input.Message.ToLower() == "exit")
            {
                input.Callbacks.EndSession();
                return new PluginOutput("List stopped.", input.PersistentData);
            }
            else if (input.Message.ToLower().StartsWith("add"))
            {
                var str = input.Message.Substring(input.Message.ToLower().IndexOf("add") + "add".Length).Trim();
                if (string.IsNullOrWhiteSpace(str))
                {
                    return new PluginOutput("error: no task specified", input.PersistentData);
                }
                list.Add(str);

                var data = new PersistentDataStructure(list);

                return new PluginOutput($"New task: {str}", JsonSerializer.Serialize(data));
            }
            else if (input.Message.ToLower().StartsWith("delete"))
            {
                if (list.Count == 0)
                    return new PluginOutput("error: there are no tasks to delete", input.PersistentData);
                var parts = input.Message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string rt = list.Count > 0 ? list[^1] : "";
                var data = new PersistentDataStructure(list);

                if (parts.Length > 1 && int.TryParse(parts[1], out int index))
                {
                    if (index >= 1 && index <= list.Count)
                    {
                        list.RemoveAt(index - 1);
                        return new PluginOutput($"Deleted task at position {index}: {rt}", JsonSerializer.Serialize(data));
                    }
                    else
                    {
                        return new PluginOutput($"error: invalid index. Enter a number between 1 and {list.Count}.", input.PersistentData);
                    }
                }
                else
                {
                    list.RemoveAt(list.Count - 1);
                    return new PluginOutput($"Delete last task: {rt}", JsonSerializer.Serialize(data));
                }
            }


            else if (input.Message.ToLower() == "list")
            {
                if (list.Count == 0)
                {
                    return new PluginOutput("the list is empty", input.PersistentData);
                }
                string listtasks = string.Join("\r\n", list.Select((task, index) => $"{index + 1}. {task}"));
                return new PluginOutput($"All list tasks:\r\n{listtasks}", input.PersistentData);
            }

            else
            {
                return new PluginOutput("Error! Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'List' to view all list. Enter 'Exit' to stop.");
            }
        
        }
    }
}
