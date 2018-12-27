using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;
using DCL_Core;
using System.Collections;

namespace DCL_Core.P2P
{
    public class P2PServer : WebSocketBehavior
    {
        bool                chainSynched    = false;
        WebSocketServer     wss             = null;
        IList<string>       DiscoveredPeerList;

        public void Start(Int32 _port)
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{ _port }");
            wss.AddWebSocketService<P2PServer>("/DCL_CORE");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{ _port }");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            string serializedData;

            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");      //Return handshake
                Send("ListOfPeers");    //Ask for a list of peers at client (every time the client is started, the list of peers is stores and therefore we should, theoretically, get a longer and longer chain of peers)
            }
            else if (e.Data.Substring(0,11) == "ListOfPeers")    //Get the list of peers
            {
                serializedData      = e.Data.Substring(11);
                DiscoveredPeerList = JsonConvert.DeserializeObject<IList<string>>(serializedData);  //Get the received peers
            }
            else
            {
                //TODO: Broadcast data
                Console.WriteLine(e.Data.Substring(1, 11));
            }
        }
    }
}
