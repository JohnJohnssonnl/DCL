using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DCL_GUI
{
    public class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
