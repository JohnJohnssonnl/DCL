using System;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class LoginRequest
    {
        [DataMember]
        public bool CreateNewWallet { get; set; }
        [DataMember]
        public String PassCode { get; set; }
        [DataMember]
        public string WalletName { get; set; }
    }
}