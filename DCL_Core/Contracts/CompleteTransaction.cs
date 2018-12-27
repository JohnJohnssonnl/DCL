using System;
using System.Runtime.Serialization;

namespace DCL_Core.Contracts
{
    [Serializable]
    [DataContract]
    public class CompletedTransaction
    {
        //This object holds data which can be broadcasted to other nodes
        [DataMember]
        public Share[]      UnspendShares   { get; set; }   //Newly created shares
        [DataMember]
        public String       MD5Hash         { get; set; }   //Hash
        [DataMember]
        public Guid         TransactionId   { get; set; }   //TransactionId
        [DataMember]
        public ShareSpend[] SpendShares     { get; set; }   //Spend shares
        [DataMember]
        public Transaction  Transaction     { get; set; }   //Underlying tx

        public int AddSpendShare(ShareSpend _shareSpendToAdd)
        {
            var NewSpendShares = SpendShares;
            Array.Resize(ref NewSpendShares, SpendShares.Length+1);
            SpendShares = NewSpendShares;
            SpendShares[SpendShares.Length - 1] = _shareSpendToAdd;
            return SpendShares.Length;
        }
        public int AddUnspendShare (Share _shareUnspendToAdd)
        {
            var NewUnspendShares = UnspendShares;
            Array.Resize(ref NewUnspendShares, UnspendShares.Length + 1);
            UnspendShares = NewUnspendShares;
            UnspendShares[UnspendShares.Length - 1] = _shareUnspendToAdd;
            return UnspendShares.Length;
        }
    }
}
