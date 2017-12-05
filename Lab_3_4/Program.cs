using IOLaboratorium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IOLaboratorium.ZadaniaTAP;

namespace ConsoleApp1
{
    delegate int myDelegate(int a, int b);
    class Program
    {        
            public static async Task OperationTask(object data)
            {
                Console.WriteLine("begin task");
                await Task.Run(() =>
                {
                    Console.WriteLine("begin async");
                    Thread.Sleep(100);
                    //kod operacji asynchronicznej
                    Console.WriteLine("end async");
                });
                Console.WriteLine("end task");
            }



        static void Main(string[] args)
            {
            ZadaniaTAP.Server server = new ZadaniaTAP.Server();
            server.Run();

            ZadaniaTAP.Client client1 = new ZadaniaTAP.Client();
            ZadaniaTAP.Client client2 = new ZadaniaTAP.Client();

            client1.Connect();
            client2.Connect();

            Task.WaitAll(server.ServerTask);
            Task.WaitAll(client1.ClientTask);
            Task.WaitAll(client2.ClientTask);

            }
    }
}
