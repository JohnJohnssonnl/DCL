using System;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class WalletData
    {
        [DataMember]
        public Keys     KeySet { get; set; }
        [DataMember]    
        public string   PublicAddress { get; set; }
        [DataMember]
        public byte[]   PassCode { get; set; }
        [DataMember]
        public string   WalletName { get; set; }
        [DataMember]
        public double   Balance { get; set; }
    }
}