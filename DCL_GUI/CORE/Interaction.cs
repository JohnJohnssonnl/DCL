using System;
using System.Threading.Tasks;
using DCL_Core.Wallet;
using DCL_Core.Contracts;


namespace DCL_GUI.Core
{
    public class Interaction
    {
        public WalletManager WalletManager = new WalletManager();

        public Boolean Login(string _WalletName, string _passWordValue, bool _CreateNewWallet = false)
        {
            LoginRequest Request = new LoginRequest();

            Request.CreateNewWallet = _CreateNewWallet;
            Request.WalletName      = _WalletName;
            Request.PassCode        = _passWordValue;

            return WalletManager.Login(Request);
        }

        public void Logout()
        {
            WalletManager.LogOut();
        }
    }
}
