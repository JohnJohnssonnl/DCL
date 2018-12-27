using System;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class Keys
    {
        [DataMember]
        public byte[] PrivateSpendKey { get; set; }
        [DataMember]
        public byte[] PrivateViewKey { get; set; }
        [DataMember]
        public byte[] PublicSpendKey { get; set; }
        [DataMember]
        public byte[] PublicViewKey { get; set; }
        [DataMember]
        public byte[] NetworkByte { get; set; }
        [DataMember]
        public byte[] HashedKey { get; set; }
        [DataMember]
        public string PublicAddress { get; set; }
        [DataMember]
        public byte[] PassCode { get; set; }
        [DataMember]
        public string WalletName { get; set; }
    }
}