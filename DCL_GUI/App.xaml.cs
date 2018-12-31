using Avalonia;
using Avalonia.Markup.Xaml;

namespace DCL_GUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
