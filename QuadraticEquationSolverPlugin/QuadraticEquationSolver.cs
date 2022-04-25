using BasePlugin;
using BasePlugin.Interfaces;
using BasePlugin.Records;
namespace QuadraticEquationSolverPlugin
{
    public class QuadraticEquationSolver : IPlugin
    {
        public string Id => throw new NotImplementedException();
        char ch = ' ';
        double a, b, c;
        public PluginOutput Execute(PluginInput input)
        {
            if (input.Message == "")
            {
                input.Callbacks.StartSession();
                ch = 'a';
                return new PluginOutput("Quadratic equation solver started. \na=", input.PersistentData);
            }
            return null;

        }
    }
}