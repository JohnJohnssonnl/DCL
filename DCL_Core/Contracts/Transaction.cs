using System;
using System.Collections;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable] [DataContract]
    public class Transaction
    {
        [DataMember]
        public Guid     TransactionId   { get; set; }
        [DataMember]
        public double   FeeAmount       { get; set; }
        [DataMember]
        public string   Sender          { get; set; }
        [DataMember]
        public string   Receiver        { get; set; }
        [DataMember]
        public DateTime DateTimeStamp   { get; set; }
        [DataMember]
        public double   Amount          { get; set; }
        [DataMember]
        public string   MD5Hash         { get; set; }
    }
}
