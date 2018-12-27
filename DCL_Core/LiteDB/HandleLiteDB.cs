using LiteDB;
using DCL_Core.Contracts;
using System;

namespace DCL_Core.LiteDB
{
    public class HandleLiteDB
    {
        public LiteDatabase ChainDB;

        public LiteDatabase OpenDB()
        {
            ChainDB = new LiteDatabase(@"DCL_Core.db");
            
            return ChainDB;
        }

        public void InsertTransaction(Transaction _Transaction)
        {
            var col = ChainDB.GetCollection<Transaction>("Transaction");
            col.EnsureIndex(x => x.TransactionId, true);

            col.Insert(_Transaction);
        }
        public void InsertCompletedTrans(CompletedTransaction _completedTransaction)
        {
            var col = ChainDB.GetCollection<CompletedTransaction>("CompleteTransaction");
            col.EnsureIndex(x => x.TransactionId, true);

            col.Insert(_completedTransaction);
        }

        public void InsertSpendShare(ShareSpend _ShareSpend)
        {
            var col = ChainDB.GetCollection<ShareSpend>("ShareSpend");
            col.EnsureIndex(x => x.ShareId, true);

            col.Insert(_ShareSpend);
        }
        public void InsertUnspendShare(Share _Share)
        {
            var col = ChainDB.GetCollection<Share>("Share");
            col.EnsureIndex(x => x.ShareId, true);

            col.Insert(_Share);
        }
    }
}
