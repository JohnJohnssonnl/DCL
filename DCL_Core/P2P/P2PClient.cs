using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace DCL_Core.P2P
{
    public class P2PClient
    {
        //TODO: Store wsDict to db, so next time we start the program, we already have a mapping of potential peers
        IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public void Connect(string url)
        {
            if (!wsDict.ContainsKey(url))
            {
                WebSocket ws = new WebSocket(url);
                ws.OnMessage += (sender, e) =>
                {
                    if (e.Data == "Hi Client")
                    {
                        Console.WriteLine(e.Data);
                    }
                    if (e.Data == "ListOfPeers")
                    {
                        //Send list of peers to Server
                        Send(url, "ListOfPeers" + JsonConvert.SerializeObject(GetServers()));
                    }
                    else
                    {
                        //TODO: Broadcast data
                     
                    }
                };
                ws.Connect();
                ws.Send("Hi Server");
                //Send some more data between server and clients
                wsDict.Add(url, ws);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }
        
        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }
        }
    }
}
