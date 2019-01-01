using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DCL_GUI.GUI_IL;

namespace DCL_GUI
{
    public class MainMenu : Window
    {
        public MainMenu()
        {
            Interaction Interaction = new Interaction();
            Button LoginButton;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            LoginButton = this.FindControl<Button>("LoginButton");

            LoginButton.Click += async (sender, e) => await Interaction.Login();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
