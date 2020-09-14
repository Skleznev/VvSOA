using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace VvSOA
{
    class EmployeeTCPServer
    {
        static TcpListener listener;
        const int LIMIT = 5;

        static Dictionary<string, string> employees =
 new Dictionary<string, string>()
 {
 {"john", "manager"},
 {"jane", "steno"},
 {"jim", "clerk"},
 {"jack", "salesman"}
 };

        static void Main(string[] args)
        {

            Int32 port = 13000;            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(localAddr, port);
            listener.Start();

            for (int i = 0; i < LIMIT; i++)
            {
                Thread t = new Thread(new ThreadStart(Service));
                t.Start();
            }

            Console.ReadKey();

        }

        public static void Service()
        {
            //постоянно ждем входящего соединения
            while (true)
            {
                Socket soc = listener.AcceptSocket();
                try
                {
                    Stream s = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(s);
                    StreamWriter sw = new StreamWriter(s);
                    sw.AutoFlush = true; // enable automatic flushing
                    sw.WriteLine("{0} Employees available",
                    employees.Count);

                    while (true)
                    {
                        string name = sr.ReadLine();
                        if (name == "" || name == null) break;
                        string job =
                        employees[name];
                        if (job == null) job = "No such employee";
                        sw.WriteLine(job);
                    }
                    s.Close();
                }
                catch (Exception e)
                {
                }
                soc.Close();
            }
        }

    }
}
