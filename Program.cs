using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPChatAsiakas
{
    class Program
    {
        /// <summary>
        /// Pääohjelma UDPClientille
        /// </summary>
        /// <param name="args">syöte</param>
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            int port = 25000;

            IPEndPoint iep = new IPEndPoint(IPAddress.Loopback, port);
            byte[] rec = new byte[256];

            EndPoint ep = (EndPoint)iep;
            s.ReceiveTimeout = 100000000;
            String msg;
            Boolean on = true;
            do
            {
                Console.Write(">");
                msg = Console.ReadLine();
                if (msg.Equals("quit")) on = false;
                else
                {
                    s.SendTo(Encoding.ASCII.GetBytes(msg), ep);

                    while(!Console.KeyAvailable)
                    {
                        IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                        EndPoint palvelinep = (EndPoint)remote;
                        int paljon = 0;

                        try
                        {
                            s.ReceiveFrom(rec, ref palvelinep);
                            String rec_string = Encoding.ASCII.GetString(rec);
                            char[] delim = { ';' };
                            String[] osat = rec_string.Split(delim, 2);
                            if (osat.Length < 2)
                            {
                                Console.WriteLine("Viesti ei ole kunnollinen!");
                            }
                            else
                            {
                                Console.WriteLine("{0}:{1}", osat[0], osat[1]);
                                s.SendTo(Encoding.ASCII.GetBytes(rec_string), palvelinep);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Aika kului loppuun.");
                        }
                    }
                }
            } while (on);

            Console.ReadKey();
            s.Close();
        }
    }
}
