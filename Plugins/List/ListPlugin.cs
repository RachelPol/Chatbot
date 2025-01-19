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

            if (!string.IsNullOrEmpty(input.PersistentData))
            {
                try
                {
                    list = JsonSerializer.Deserialize<PersistentDataStructure>(input.PersistentData).List;
                }
                catch
                {
                    return new PluginOutput("input error", input.PersistentData);
                }
            }

            if (string.IsNullOrWhiteSpace(input.Message))
            {
                input.Callbacks.StartSession();
                return new PluginOutput(
                    "List started. Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'Delete [task name]' to delete a specific task. Enter 'List' to view all tasks. Enter 'Exit' to stop.",
                    input.PersistentData);
            }
            else if (input.Message.ToLower() == "exit")
            {
                input.Callbacks.EndSession();
                return new PluginOutput("List stopped.", input.PersistentData);
            }
            else if (input.Message.ToLower().StartsWith("add"))
            {
                var str = input.Message.Substring("add".Length).Trim();
                if (string.IsNullOrEmpty(str))
                {
                    return new PluginOutput("Please enter a task to add.", input.PersistentData);
                }

                list.Add(str);
                var data = new PersistentDataStructure(list);
                return new PluginOutput($"New task added: {str}", JsonSerializer.Serialize(data));
            }
            else if (input.Message.ToLower().StartsWith("delete"))
            {
                var taskToDelete = input.Message.Substring("delete".Length).Trim();

                if (string.IsNullOrEmpty(taskToDelete))
                {
                    if (list.Count == 0)
                    {
                        return new PluginOutput("There are no tasks to delete.", input.PersistentData);
                    }

                    string removedTask = list[^1];
                    list.RemoveAt(list.Count - 1);
                    var data = new PersistentDataStructure(list);
                    return new PluginOutput($"Deleted last task: {removedTask}", JsonSerializer.Serialize(data));
                }
                else
                {
                    if (list.Contains(taskToDelete))
                    {
                        list.Remove(taskToDelete);
                        var data = new PersistentDataStructure(list);
                        return new PluginOutput($"Deleted task: {taskToDelete}", JsonSerializer.Serialize(data));
                    }
                    else
                    {
                        return new PluginOutput($"Task '{taskToDelete}' not found.", input.PersistentData);
                    }
                }
            }
            else if (input.Message.ToLower() == "list")
            {
                if (list.Count == 0)
                {
                    return new PluginOutput("The list is empty.", input.PersistentData);
                }

                string listTasks = string.Join("\r\n", list);
                return new PluginOutput($"All tasks:\r\n{listTasks}", input.PersistentData);
            }
            else
            {
                return new PluginOutput("Invalid command. Use 'Add', 'Delete', 'List', or 'Exit'.", input.PersistentData);
            }
        }
    }
}
