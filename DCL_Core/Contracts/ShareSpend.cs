using System;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class ShareSpend
    {
        [DataMember]
        public Guid     ShareId         { get; set; }
        [DataMember]
        public Guid     TransactionId   { get; set; }
        [DataMember]
        public DateTime DateTimeStamp   { get; set; }
        [DataMember]
        public double   Amount          { get; set; }
        [DataMember]
        public string   MD5Hash         { get; set; }
        [DataMember]
        public string   Signature       { get; set; }
    }
}
