using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DCL_GUI.Core;

namespace DCL_GUI
{
    public class MainMenu : Window
    {
        Button      LoginButton;
        Button      LogoutButton;
        Button      AboutButton;
        Button      TransferSharesButton;
        Button      TransactionsButton;
        Button      SettingsButton;
        TextBox     WalletNameValue;
        TextBox     PasswordValue;
        TextBox     P2PServerPort;
        CheckBox    CreateNewWalletValue;
        TextBlock   PublicAddress;
        TextBlock   HelloMessage;
        TextBlock   WelcomeBack;
        TextBlock   Balance;
        TextBlock   PassWordLabel;
        TextBlock   WalletNameLabel;
        TextBlock   P2PServerPortLabel;
        TextBlock   AboutText;
        TextBlock   BalanceLabel;
        TextBlock   PublicAddressLabel;
        IList<Control> LoginAvailableControls;
        IList<Control> LogOutAvailableControls;

        Interaction Interaction = new Interaction();

        public MainMenu()
        {
            

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif            
        }

        private void InitializeComponent()
        {
            LoginAvailableControls = new List<Control>();
            LogOutAvailableControls = new List<Control>();
            AvaloniaXamlLoader.Load(this);
            WalletNameValue         = this.FindControl<TextBox>("WalletNameValue");
            PasswordValue           = this.FindControl<TextBox>("PasswordValue");
            P2PServerPort           = this.FindControl<TextBox>("P2PServerPort");
            CreateNewWalletValue    = this.FindControl<CheckBox>("CreateNewWalletValue");
            PublicAddress           = this.FindControl<TextBlock>("PublicAddress");
            Balance                 = this.FindControl<TextBlock>("Balance");
            AboutText               = this.FindControl<TextBlock>("AboutText");
            PublicAddressLabel      = this.FindControl<TextBlock>("PublicAddressLabel");
            BalanceLabel            = this.FindControl<TextBlock>("BalanceLabel");
            P2PServerPortLabel      = this.FindControl<TextBlock>("P2PServerPortLabel");
            PassWordLabel           = this.FindControl<TextBlock>("PassWordLabel");
            WalletNameLabel         = this.FindControl<TextBlock>("WalletNameLabel");
            HelloMessage            = this.FindControl<TextBlock>("HelloMessage");
            WelcomeBack             = this.FindControl<TextBlock>("WelcomeBack");
            LoginButton             = this.Find<Button>("LoginButton");
            LogoutButton            = this.Find<Button>("LogoutButton");
            AboutButton             = this.Find<Button>("AboutButton");
            SettingsButton          = this.Find<Button>("SettingsButton");
            TransferSharesButton    = this.Find<Button>("TransferSharesButton");
            TransactionsButton      = this.Find<Button>("TransactionsButton");

            //Defaults
            AboutText.IsVisible             = false;
            P2PServerPortLabel.IsVisible    = false;
            P2PServerPort.IsVisible         = false;

            //Add eventhandlers
            AboutButton.Click       += (sender, e) => AboutButtonClickedSync();
            SettingsButton.Click    += (sender, e) => SettingsButtonClickedSync();
            LoginButton.Click       += (sender, e) => LoginClickedSync();
            LogoutButton.Click      += (sender, e) => LogoutClickedSync();

            //Add to lists to easily toggle loggedin and loggedout
            LoginAvailableControls.Add(PublicAddress);
            LoginAvailableControls.Add(Balance);
            LoginAvailableControls.Add(LogoutButton);
            LoginAvailableControls.Add(PublicAddressLabel);
            LoginAvailableControls.Add(BalanceLabel);
            LoginAvailableControls.Add(TransferSharesButton);
            LoginAvailableControls.Add(TransactionsButton);
            LoginAvailableControls.Add(WelcomeBack);

            LogOutAvailableControls.Add(WalletNameValue);
            LogOutAvailableControls.Add(PasswordValue);
            LogOutAvailableControls.Add(CreateNewWalletValue);
            LogOutAvailableControls.Add(LoginButton);
            LogOutAvailableControls.Add(PassWordLabel);
            LogOutAvailableControls.Add(WalletNameLabel);
            LogOutAvailableControls.Add(HelloMessage);

            UpdateDesign();
        }

        private void AboutButtonClickedSync()
        {
            //Sync as it doesn't take much time to set visiblity
            TextBlock AboutText = this.FindControl<TextBlock>("AboutText");
            AboutText.IsVisible = AboutText.IsVisible ? false : true;
        }
        private void SettingsButtonClickedSync()
        {
            P2PServerPortLabel.IsVisible    = P2PServerPortLabel.IsVisible ? false : true;
            P2PServerPort.IsVisible         = P2PServerPort.IsVisible ? false : true;
        }

        private void LoginClickedSync()
        {
            if (Interaction.Login(      WalletNameValue.Text, 
                                        PasswordValue.Text, 
                                        CreateNewWalletValue.IsChecked.Value))
            {
                //Has logged in OK, TODO
                PublicAddress.Text      = Interaction.WalletManager.PublicAddress();
                Balance.Text            = Convert.ToString(Interaction.WalletManager.Balance());        //Double to string
                WelcomeBack.Text        = $"Welcome back {WalletNameValue.Text}, good to see you!";
            }
            PasswordValue.Text = "";    //Empty pass, always


            UpdateDesign();
        }
        private void LogoutClickedSync()
        {
            Interaction.Logout();
            PublicAddress.Text  = "";
            Balance.Text        = "0.00";
          
            UpdateDesign();
        }

        private void UpdateDesign()
        {
            IEnumerator< Control> LoginAvailableControlsEnumerable = LoginAvailableControls.GetEnumerator();
            IEnumerator< Control> LogOutAvailableControlsEnumerable = LogOutAvailableControls.GetEnumerator();
            bool isLoggedIn = Interaction.WalletManager.isLoggedIn();

            while (LoginAvailableControlsEnumerable.MoveNext())
            {
                var localVariable       = LoginAvailableControlsEnumerable.Current;
                localVariable.IsVisible = isLoggedIn;
            }
            while (LogOutAvailableControlsEnumerable.MoveNext())
            {
                var localVariable = LogOutAvailableControlsEnumerable.Current;
                localVariable.IsVisible = !isLoggedIn;
            }
        }
    }
}
