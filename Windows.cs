using System.Reflection;

namespace SharpNEX.Engine.Platform.Windows;

public class Windows : IPlatform
{
    public IWindow CreateWindow(string title, int width, int height) 
        => new WinWindow(title, width, height);

    public IRenderer CreateRenderer(IWindow window, string rendererType)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var type = assembly.GetTypes()
            .FirstOrDefault(t =>
                t is { IsClass: true, IsAbstract: false } &&
                typeof(IWinRenderer).IsAssignableFrom(t) &&
                string.Equals(t.Name, rendererType, StringComparison.OrdinalIgnoreCase));

        if (type == null)
        {
            throw new ArgumentException($"Renderer '{rendererType}' not found");
        }

        return (IRenderer)Activator.CreateInstance(type, window)!;
    }

    public IInput CreateInput()
    {
        throw new NotImplementedException();
    }
}