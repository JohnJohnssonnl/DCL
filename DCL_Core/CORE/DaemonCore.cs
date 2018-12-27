using DCL_Core.LiteDB;
using DCL_Core.Contracts;
using System.Collections.Generic;
using System.Linq;
using System;
using DCL_Core.Cryptography;
using DCL_Core.P2P;

//TODO: A lot :-)
namespace DCL_Core.CORE
{
    public class DaemonCore
    {
        public HandleLiteDB         Database { get; set; }
        public CompletedTransaction CompletedTrans;
        public P2PClient            P2PClient { get; set; }
        public P2PServer            P2PServer { get; set; }
        Int32                       Port;

        public DaemonCore New()
        {
            DaemonCore DaemonCore = new DaemonCore();
            Database = new HandleLiteDB();
            Database.OpenDB();

            //Do the main stuff here
            return DaemonCore;
        }

        public void InitializeDaemon()
        {
            //Start P2P
            P2PClient = new P2PClient();
            P2PServer = new P2PServer();
        }
        public void StartP2PServer(Int32 _port)
        {
            P2PServer.Start(_port);  //Start server
        }


        public void CloseConnections()
        {
            //Close P2P
            P2PClient.Close();            
        }
        public void TestServer(Int32 _port)
        {
            P2PClient.Connect($"ws://127.0.0.1:{_port}/DCL_CORE");
        }

        public void TransferShares(     string _fromAddress, 
                                        string _toAddress, 
                                        double _amount)
        {
            IList<Share> PostingShares;

            PostingShares = CollectActiveShares(_fromAddress, _amount);   //Get the transactions

            if (PostingShares != null)
            {
                //Now start building the transaction
                CompletedTrans = BuildTransaction(PostingShares, _fromAddress, _toAddress, _amount);

                if(CompletedTrans != null)  //If the transaction has been completed, broadcast it
                {
                    //Now broadcast
                }
            }
        }

        public CompletedTransaction     BuildTransaction(   IList<Share> _SendingFromShares,
                                                            string _fromAddress,
                                                            string _toAddress,
                                                            double _amount)
        {
            bool mustPost = false;
            
            IEnumerator<Share> ShareEnumerator = _SendingFromShares.GetEnumerator();
            Share SpendingShare;
            double amountFulFilled = 0;
            double amountReturn = 0;

            while (ShareEnumerator.MoveNext())
            {
                SpendingShare = ShareEnumerator.Current;

                IsValidShare(SpendingShare, _fromAddress);

                amountFulFilled += SpendingShare.Amount;

                if (amountFulFilled >= _amount)
                {
                    amountReturn    = amountFulFilled - _amount;
                    mustPost        = true; //We have no reason not to post now
                }
            }

            if(mustPost)
            {
                try
                {
                    //Post the transaction to the ledger
                    Transaction postTransaction = new Transaction
                    {
                        TransactionId = Guid.NewGuid(),
                        DateTimeStamp = DateTime.Now,
                        Amount = _amount,
                        Sender = _fromAddress,
                        Receiver = _toAddress,
                    };

                    postTransaction.MD5Hash = MD5Hash.GenerateKey(postTransaction);

                    Database.InsertTransaction(postTransaction);
                    CompletedTrans.Transaction      = postTransaction;
                    CompletedTrans.TransactionId    = postTransaction.TransactionId;

                    ShareEnumerator = _SendingFromShares.GetEnumerator();

                    while (ShareEnumerator.MoveNext())
                    {
                        SpendingShare = ShareEnumerator.Current;

                        //Post the Spend transaction(s) to the ledger
                        ShareSpend postSpendTransaction = new ShareSpend
                        {
                            TransactionId   = postTransaction.TransactionId,
                            DateTimeStamp   = DateTime.Now,
                            Amount          = SpendingShare.Amount,
                            ShareId         = SpendingShare.ShareId
                        };

                        postSpendTransaction.MD5Hash = MD5Hash.GenerateKey(postSpendTransaction);

                        Database.InsertSpendShare(postSpendTransaction);

                        CompletedTrans.AddSpendShare(postSpendTransaction);
                    }

                    //Post the new (unspend) Shares to the ledger
                    Share unspendShare = new Share
                    {
                        ShareId         = new Guid(),
                        GenesisTxId     = postTransaction.TransactionId,
                        DateTimeStamp   = DateTime.Now,
                        Amount          = _amount,
                        Owner           = _toAddress
                    };

                    unspendShare.MD5Hash = MD5Hash.GenerateKey(unspendShare);
                    Database.InsertUnspendShare(unspendShare);
                    CompletedTrans.AddUnspendShare(unspendShare);

                    if (amountReturn > 0 )
                    {
                        //TODO: Return amount
                        Share unspendShareRet = new Share
                        {
                            ShareId = new Guid(),
                            GenesisTxId = postTransaction.TransactionId,
                            DateTimeStamp = DateTime.Now,
                            Amount = amountReturn,
                            Owner = _toAddress
                        };

                        unspendShareRet.MD5Hash = MD5Hash.GenerateKey(unspendShareRet);
                        Database.InsertUnspendShare(unspendShareRet);
                        CompletedTrans.AddUnspendShare(unspendShare);
                    }

                    //Now build the completed transaction to broadcast
                    CompletedTrans.MD5Hash = MD5Hash.GenerateKey(CompletedTrans);

                    Database.InsertCompletedTrans(CompletedTrans);
                    return CompletedTrans;
                }
                catch
                {
                    Console.WriteLine("Error when trying to post transaction");
                    return null;
                }
            }
            Console.WriteLine("Insufficient balance for posting");

            return null;
        }

        private bool IsValidShare(Share _Share, string _fromAddress)
        {
            return IsShareSpend(_Share) && _fromAddress == _Share.Owner;
        }
        public bool IsShareSpend(Share _Share)
        {
            //Checks if a share is already spend
            var col = Database.ChainDB.GetCollection<ShareSpend>("ShareSpend");

            return col.Exists(x => x.ShareId.ToString() == _Share.ShareId.ToString());
        }
        public IList<Share> GetOwnerShares(string _Owner)
        {
            var col = Database.ChainDB.GetCollection<Share>("Share");
            var results = col.Find(x => x.Owner == _Owner);

            return results.ToList();
        }
        //public IList<Share> CollectActiveShares(string _fromAddress, double _amountTransaction)
        public IList<Share> CollectActiveShares(string _fromAddress, double _amountTransaction)
        {
            IList<Share>        Shares;
            IList <Share>       UsableShares = new List<Share>();
            Share               LocalShare;
            double              amountFree = 0;
            IEnumerator<Share>  ShareEnumerator;

            Shares = GetOwnerShares(_fromAddress);

            ShareEnumerator = Shares.GetEnumerator();

            while (ShareEnumerator.MoveNext())
            {
                LocalShare = ShareEnumerator.Current;

                if (!IsShareSpend(LocalShare))
                {
                    amountFree += LocalShare.Amount;
                    UsableShares.Add(LocalShare);

                    if (amountFree >= _amountTransaction)   //If we found enough shares value to send from, break
                    {
                        return UsableShares;
                    }
                }
            }

            return null;
        }
    }
}
