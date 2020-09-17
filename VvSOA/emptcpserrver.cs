using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;using System.Text.RegularExpressions;
using System.Linq;

namespace VvSOA
{
    class EmployeeTCPServer
    {
        static TcpListener listener;
        const int LIMIT = 5;

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
                    sw.WriteLine("Соединение установлено");

                    while (true)
                    {
                                               
                        Byte[] bytes = new Byte[256];
                        string data = null;
                        int i = s.Read(bytes, 0, bytes.Length);
                        data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        bytes = System.Text.Encoding.UTF8.GetBytes(splitMsg(data));
                        s.Write(bytes, 0, bytes.Length);
                        Console.Write(data);
                    }
                    s.Close();
                }
                catch (Exception e)
                {
                }
                soc.Close();
            }
        }

        public static string splitMsg(string msg)
        {
            string[] words = Regex.Split(msg.ToLower(), @"\W").Distinct().ToArray();

            Array.Sort(words);
            string result = "";
            foreach(string word in words){
                if (word == "") continue;
                result += word + "\n";
            }

            return result;
        }
    }
}
