using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            //zad1();
            //zad2();
            //zad3();
            //zad4();
            zad5();
            Console.Read();
        }        

        static void Thread1(Object stateInfo)
        {
            //rozpoczęcie wykonywania nowego wątku
            var time1=((object[])stateInfo)[0];
            Thread.Sleep((int)time1);
            Console.WriteLine("thread 1: czekałem {0}",(int)time1, "ms");
        }

        static void Thread2(Object stateInfo)
        {
            var time2 = ((object[])stateInfo)[0];
            Thread.Sleep((int)time2);
            Console.WriteLine("thread 2: czekałem {0}", (int)time2, "ms");
        }

        static void zad1()
        {
            //dodanie nowych do kolejki obsługującej wątki
            ThreadPool.QueueUserWorkItem(Thread1, new object[] { 300 });
            ThreadPool.QueueUserWorkItem(Thread2, new object[] { 100 });
            //uruchomienie
            Thread.Sleep(1000);
        }
        
        static void ServerThread(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            Console.WriteLine("Uruchomiono serwer");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];             
                client.GetStream().Read(buffer, 0, 1024);                
                Console.WriteLine("Serwer: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer));
                byte[] message = new ASCIIEncoding().GetBytes("wiadomosc od serwera");
                client.GetStream().Write(message, 0, message.Length);
                client.Close();
            }
            Thread.Sleep(500);
        }

        static void ClientThread(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message1 = new ASCIIEncoding().GetBytes("wiadomosc od klienta 1");
            client.GetStream().Write(message1, 0, message1.Length);
            byte[] buffer1 = new byte[1024];
            client.GetStream().Read(buffer1, 0, 1024);
            Console.WriteLine("Klient 1: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer1));
            Thread.Sleep(500);
        }        

        static void ClientThread2(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message2 = new ASCIIEncoding().GetBytes("wiadomosc od klienta 2");
            client.GetStream().Write(message2, 0, message2.Length);
            byte[] buffer2 = new byte[1024];
            client.GetStream().Read(buffer2, 0, 1024);
            Console.WriteLine("Klient 2: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer2));
            Thread.Sleep(500);
        }

        static void zad2()
        {
            ThreadPool.QueueUserWorkItem(ServerThread);
            ThreadPool.QueueUserWorkItem(ClientThread);
            ThreadPool.QueueUserWorkItem(ClientThread2);
        }

        static void ServerThread2(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            Console.WriteLine("Uruchomiono serwer");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ThreadClientaAfterConnected);
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);               
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Serwer: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer));
                Console.ResetColor();
                byte[] message = new ASCIIEncoding().GetBytes("wiadomosc od serwera");
                client.GetStream().Write(message, 0, message.Length);
                client.Close();
                Thread.Sleep(500);
            }
        }

        static void ClientThread11(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message1 = new ASCIIEncoding().GetBytes("wiadomosc od klienta 1");
            client.GetStream().Write(message1, 0, message1.Length);
            byte[] buffer1 = new byte[1024];
            client.GetStream().Read(buffer1, 0, 1024);            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Klient 1: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer1));
            Console.ResetColor();
            Thread.Sleep(500);
        }

        static void ClientThread22(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message2 = new ASCIIEncoding().GetBytes("wiadomosc od klienta 2");
            client.GetStream().Write(message2, 0, message2.Length);
            byte[] buffer2 = new byte[1024];
            client.GetStream().Read(buffer2, 0, 1024);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Klient 2: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer2));
            Console.ResetColor();
            Thread.Sleep(500);
        }

        static void ThreadClientaAfterConnected(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] messageA = new ASCIIEncoding().GetBytes("wiadomosc od klienta");
            client.GetStream().Write(messageA, 0, messageA.Length);
            byte[] bufferA = new byte[1024];
            client.GetStream().Read(bufferA, 0, 1024);
            client.Close();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Klient : Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(bufferA));
            Console.ResetColor();
            Thread.Sleep(500);
        }

        static void zad3()
        {
            ThreadPool.QueueUserWorkItem(ServerThread2);
            ThreadPool.QueueUserWorkItem(ClientThread11);
            ThreadPool.QueueUserWorkItem(ClientThread22);
        }

        //tylko jeden wątek może wykonywać kod zawarty w sekcji lock
        //dzięki temu  możliwe jest wymuszenie kolejności działania klientów 
        //najpierw 1 następnie 2

        static void ServerThreadL(Object stateInfo)
        {
            object lockObject = new object();
            lock (lockObject)
            {
                TcpListener server = new TcpListener(IPAddress.Any, 2048);
                server.Start();
                Console.WriteLine("Uruchomiono serwer");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(ThreadClientaAfterConnectedL);
                    byte[] buffer = new byte[1024];
                    client.GetStream().Read(buffer, 0, 1024);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Serwer: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer));
                    Console.ResetColor();
                    byte[] message = new ASCIIEncoding().GetBytes("wiadomosc od serwera");
                    client.GetStream().Write(message, 0, message.Length);
                    client.Close();
                    Thread.Sleep(500);
                }
            }
        }

        static void ClientThreadL(Object stateInfo)
        {
            object lockObject = new object();
            lock (lockObject)
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                byte[] message1 = new ASCIIEncoding().GetBytes("wiadomosc od klienta 1");
                client.GetStream().Write(message1, 0, message1.Length);
                byte[] buffer1 = new byte[1024];
                client.GetStream().Read(buffer1, 0, 1024);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Klient 1: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer1));
                Console.ResetColor();
                Thread.Sleep(500);
            }
        }

        static void ClientThread2L(Object stateInfo)
        {
            object lockObject = new object();
            lock (lockObject)
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                byte[] message2 = new ASCIIEncoding().GetBytes("wiadomosc od klienta 2");
                client.GetStream().Write(message2, 0, message2.Length);
                byte[] buffer2 = new byte[1024];
                client.GetStream().Read(buffer2, 0, 1024);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Klient 2: Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(buffer2));
                Console.ResetColor();
                Thread.Sleep(500);
            }
        }

        static void ThreadClientaAfterConnectedL(Object stateInfo)
        {
            object lockObject = new object();
            lock (lockObject)
            {
                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
                byte[] messageA = new ASCIIEncoding().GetBytes("wiadomosc od klienta");
                client.GetStream().Write(messageA, 0, messageA.Length);
                byte[] bufferA = new byte[1024];
                client.GetStream().Read(bufferA, 0, 1024);
                client.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Klient : Odebrano wiadomosc o tresci {0}", Encoding.UTF8.GetString(bufferA));
                Console.ResetColor();
                Thread.Sleep(500);
            }
        }

        static void zad4()
        {
            ThreadPool.QueueUserWorkItem(ServerThreadL);
            ThreadPool.QueueUserWorkItem(ClientThreadL);
            ThreadPool.QueueUserWorkItem(ClientThread2L);
        }

        //Zadanie5
        private static int quantity;
        private static int sumNumber;
        private static int[] numbers = null;
        private static Random rnd;
        private static List<AutoResetEvent> are;
        private static int sum;
        private static List<int> index;

        static void zad5()
        {
            Console.WriteLine("Ile liczb ma zawierać tablica?");
            index = new List<int>();
            quantity = Convert.ToInt32(Console.ReadLine());
            numbers = new int[quantity];
            sum = 0;
            initRandom();
            displayTable();

            sumNumber = quantity;


            for (int i = 0; i < sumNumber; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SumElements));
                Thread.Sleep(200);
            }

            Console.WriteLine("Suma: " + sum);
        }

        static void initRandom()
        {
            rnd = new Random();
            for (int i = 0; i < quantity; i++)
            {
                numbers[i] = rnd.Next(0, 1000);
            }
        }

        static void displayTable()
        {
            for (int i = 0; i < quantity; i++)
            {
                Console.Write(numbers[i] + "\t");
            }
            Console.WriteLine();
        }

        static void SumElements(object obj)
        {
            object lockObj = new object();
            lock (lockObj)
            {
                Random sRnd = new Random();
                int index = sRnd.Next(1, quantity - 1);
                sum += numbers[index];
                Console.WriteLine("Element, tab[{0}], {1}", index, numbers[index]);
            }
        }
        
    }
}
