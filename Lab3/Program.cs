using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            /*myDelegate foo = (a, b) => a + b;
            int r = foo(3, 4);
            Console.WriteLine(r);
            Console.ReadKey();*/
            int test = 0;
            byte[] buffer = new byte[128];
            Console.WriteLine("begin main");
            Task task = OperationTask(buffer);
            Thread.Sleep(test);
            Console.WriteLine("progress main");
            task.Wait();
            Console.WriteLine("end main");
            Console.ReadKey();
        }
    }
}
