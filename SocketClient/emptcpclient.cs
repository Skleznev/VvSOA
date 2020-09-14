using System;
using System.IO;
using System.Net.Sockets;
class EmployeeTCPClient
{
    public static void Main(string[] args)
    {
        //Порт нашего сервера
        Int32 port = 13000;
        string serverAddr = "127.0.0.1";
        //Если адрес сервера передан как параметр
        if (args.Length > 0)
            serverAddr = args[0];

        TcpClient client = new TcpClient(serverAddr, port);
        try
        {
            Stream s = client.GetStream();
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true;
            Console.WriteLine(sr.ReadLine());
            while (true)
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();
                sw.WriteLine(name);
                if (name == "") break;
                Console.WriteLine(sr.ReadLine());
            }
            s.Close();
        }
        finally
        {
            
            client.Close();
        }
    }
}