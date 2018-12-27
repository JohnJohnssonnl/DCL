using System;
using System.Collections;
using DCL_Core.CORE;
using DCL_Core.P2P;

namespace DCL_Core
{
    class Program
    {
        public static void DisplayMainMenu()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("DCL CORE Main menu:");
            Console.WriteLine("1: Start server");
            Console.WriteLine("2: Handshake to server");
            Console.WriteLine("3: Get list of connected servers");
            Console.WriteLine("9: Exit");
            Console.WriteLine("=========================");
        }

        static void Main(string[] args)
           {
            Int32 Port;

            DaemonCore daemon = new DaemonCore();
            daemon.InitializeDaemon();

            int selection = 0;

            while (selection != 9)
            {
                switch (selection)
                {
                    case 1:
                    {
                        Console.WriteLine("Enter the port to run server on");
                        Port = int.Parse(Console.ReadLine());
                        daemon.StartP2PServer(Port);
                        break;
                    }
                    case 2:
                    {
                        Console.WriteLine("Enter the port to connect to");
                        Port = int.Parse(Console.ReadLine());
                        daemon.TestServer(Port);
                        break;
                    }
                    case 3:
                    {
                        var         connServers = daemon.P2PClient.GetServers();
                        IEnumerator connServersEnumerator = connServers.GetEnumerator();

                        while (connServersEnumerator.MoveNext())
                        {
                            Console.WriteLine("Connected to server {0}", connServersEnumerator.Current);
                        }
                        break;
                    }
                    default: break;
                }

                Program.DisplayMainMenu();
                string action = Console.ReadLine();
                Console.Clear();
                selection = int.Parse(action);
            }

            daemon.CloseConnections();
        }
    }
}
