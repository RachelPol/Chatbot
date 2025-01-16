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
            List<string> temp = new();
            string s = input.Message;
            s= s.ToLower();
            if (string.IsNullOrEmpty(input.PersistentData) == false)
            {
                list = JsonSerializer.Deserialize<PersistentDataStructure>(input.PersistentData).List;
            }

            if (input.Message == "")
            {
                input.Callbacks.StartSession();
                return new PluginOutput("List started. Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'List' to view all list. Enter 'Exit' to stop.", input.PersistentData);
            }
            else if (s== "exit" )
            {
                input.Callbacks.EndSession();
                return new PluginOutput("List stopped.", input.PersistentData);
            }
            else if (s.StartsWith("add") )
            {
                var str = input.Message.Substring("add".Length).Trim();
                list.Add(str);
                
           var data = new PersistentDataStructure(list);

           return new PluginOutput($"New task: {str}", JsonSerializer.Serialize(data));
            }
            //else if (s.StartsWith("delete"))
            //{
            //    if (list.Count > 0) 
            //    {
            //        int index;
            //        var str = input.Message.Substring("delete".Length).Trim();
            //        bool x = int.TryParse(str, out index);
            //        if (x)
            //        { 
            //        }
            //        temp = list.ToList(); 
            //        temp.RemoveAt(int.Parse(str)); 
            //        list = temp; 
            //        var data = new PersistentDataStructure(list);

            //        return new PluginOutput($"Deleted last task", JsonSerializer.Serialize(data));
            //    }
            //    else
            //    {
            //        return new PluginOutput("The list is empty. No tasks to delete.", input.PersistentData);
            //    }
            //}

            else if (s.StartsWith("delete"))
            {
                if (list.Count > 0)
                {
                    var str = input.Message.Substring("delete".Length).Trim();
                    bool x = int.TryParse(str, out int index);
                    if (x)
                    { 
                        if (index >= 0 && index < list.Count-1)
                        {
                            list.RemoveAt(index); // מחק את האיבר לפי האינדקס
                            var data = new PersistentDataStructure(list);
                            return new PluginOutput($"Deleted task at index {index}: {list[index]}", JsonSerializer.Serialize(data));
                        }
                        else
                        {
                            return new PluginOutput("Error! The index is out of range. Please provide a valid index.", input.PersistentData);
                        }
                    }
                    else
                    {
                        return new PluginOutput("Error! Please enter a valid number for the index to delete.", input.PersistentData);
                    }
                }
                else
                {
                    return new PluginOutput("The list is empty. No tasks to delete.", input.PersistentData);
                }
            }

            else if (s == "list" )
            {
                string listtasks = string.Join("\r\n", list);
                return new PluginOutput($"All list tasks:\r\n{listtasks}", input.PersistentData);
            }
            else
            {
                return new PluginOutput("Error! Enter 'Add' to add task. Enter 'Delete' to delete task. Enter 'List' to view all list. Enter 'Exit' to stop.");
            }
        }
    }
}
