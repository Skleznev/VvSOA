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
                    bool isConnected = true;
                    while (isConnected)
                    {
                                               
                        Byte[] bytes = new Byte[256];
                        string data = null;
                        int i = s.Read(bytes, 0, bytes.Length);
                        data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);

                        switch (data)
                        {
                            case "List":
                                using (StreamReader file = File.OpenText("list.txt"))
                                {
                                    string result = "";
                                    string line = "";
                                    while ((line = file.ReadLine()) != null)
                                    {
                                        result += line;
                                    }
                                    bytes = System.Text.Encoding.UTF8.GetBytes(result);
                                    s.Write(bytes, 0, bytes.Length);
                                }
                                break;
                            case " ":
                                bytes = System.Text.Encoding.UTF8.GetBytes("Соединение завершено");
                                s.Write(bytes, 0, bytes.Length);
                                isConnected = false;
                                break;
                            default:
                                bytes = System.Text.Encoding.UTF8.GetBytes("Message added: " + data);
                                s.Write(bytes, 0, bytes.Length);

                                using (StreamWriter file = new StreamWriter("list.txt", true))
                                {
                                    file.Write(data + "; ");
                                    Console.WriteLine("\nЗапись добавлена");
                                }


                                break;

                        }


                        
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

        
    }
}
