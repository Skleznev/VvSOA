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
                Console.Write("Введите предложение: ");
                string msg = Console.ReadLine();

                Byte[] bytes = new Byte[256];                bytes = System.Text.Encoding.UTF8.GetBytes(msg);
                s.Write(bytes, 0, bytes.Length);                bytes = new Byte[256];                int i = s.Read(bytes, 0, bytes.Length);                msg = System.Text.Encoding.UTF8.GetString(bytes, 0, i);                Console.Write(msg + "\n");
            }
            s.Close();
        }
        finally
        {
            
            client.Close();
        }
    }
}