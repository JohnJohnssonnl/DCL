using System;
using System.Collections;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class Share
    {
        [DataMember]
        public Guid     ShareId         { get; set; }
        [DataMember]
        public string   Owner           { get; set; }
        [DataMember]
        public DateTime DateTimeStamp   { get; set; }
        [DataMember]
        public double   Amount          { get; set; }
        [DataMember]
        public string   MD5Hash         { get; set; }
        [DataMember]
        public Guid     GenesisTxId     { get; set; }   //The share has been created by this transaction
    }
}
