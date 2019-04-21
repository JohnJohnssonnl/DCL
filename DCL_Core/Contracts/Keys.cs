using System;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class Keys
    {
        [DataMember]
        public string PrivateKey { get; set; }
        [DataMember]
        public byte[] PrivateKeyByte { get; set; }
        [DataMember]
        public string PublicKey { get; set; }
        [DataMember]
        public byte[] PublicKeyByte { get; set; }
        [DataMember]
        public byte[] NetworkByte { get; set; }
        [DataMember]
        public byte[] HashedKey { get; set; }
        
    }
}