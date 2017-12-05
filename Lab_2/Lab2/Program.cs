using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        private static DelegateType delegateType;
        delegate int DelegateType(object arguments);
                
        private static void myAsyncCallback(IAsyncResult ar)
        {
            FileStream fs = (FileStream)((object[])ar.AsyncState)[0];
            byte[] buf = (byte[])((object[])ar.AsyncState)[1];
            Console.WriteLine(Encoding.ASCII.GetString(buf));
            fs.Close();
            Console.WriteLine();
        }

        static void zadanie6()
        {
            byte[] buffer = new byte[100];
            string filename = "a.txt";
            FileStream fs = new FileStream(filename, FileMode.Open);
            IAsyncResult ar = fs.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[] { fs, buffer });

            fs = (FileStream)((object[])ar.AsyncState)[0];
            byte[] buf = (byte[])((object[])ar.AsyncState)[1];
            Console.WriteLine(Encoding.ASCII.GetString(buf));
            fs.Close();
            fs.EndRead(ar);
            Console.Read();
        }

        static void zadanie7()
        {
            byte[] buffer = new byte[1024];
            //string filename = Directory.GetCurrentDirectory() + "\a.txt";
            string filename = "a.txt";
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            var ar = fs.BeginRead(buffer, 0, buffer.Length, null, new object[] { fs, buffer });
            fs.EndRead(ar);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            fs.Close();
        }

        static void factorial_it(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            int res = (int)obj[1];
            int n = (int)obj[0];
            int buf = 1;
            if (n == 0)
            {
                res = 1;
            }
            else
            {
                while (n > 0)
                {
                    buf *= n;
                    n--;
                }
                res = buf;
            }
        }

        static int factorial_it2(object arg)
        {
            int n = (int)arg;
            int res = 1;
            if (n == 0)
            {
                return res;
            }
            else
            {
                while (n > 0)
                {
                    res *= n;
                    n--;
                }
                return res;
            }
        }

        static void fibonacci_it(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            int result = (int)obj[1];
            int n = (int)obj[0];
            int x = 0;
            int y = 1;

            for (int i = 0; i < n; i++)
            {
                int buf = x;
                x = y;
                y = buf + y;
            }

            result = x;
        }

        public static int fibonacci_it2(object arguments)
        {
            int number = (int)arguments;
            int x = 0;
            int y = 1;
            for (int i = 0; i < number; i++)
            {
                int buf = x;
                x = y;
                y = buf + y;
            }
            return x;
        }

        /*
        static void fibonacci_rec(IAsyncResult state)
        {
            object[] obj = (object[])state.AsyncState;
            int res = (int)obj[1];
            int n = (int)obj[0];
            if ((n == 1) || (n == 2))
            {
                res = 1;
            }
            else
            {
                res = fibonacci_rec(n - 1) + fibonacci_rec(n - 2);
            }
        }
        */

        static void zadanie8()
        {
            delegateType = new DelegateType(factorial_it2);
            IAsyncResult ar1 = delegateType.BeginInvoke(3, null, null);
            int result1 = delegateType.EndInvoke(ar1);

            delegateType = new DelegateType(fibonacci_it2);
            IAsyncResult ar2 = delegateType.BeginInvoke(3, null, null);
            int result2 = delegateType.EndInvoke(ar2);

            Console.WriteLine(result1);
            Console.WriteLine(result2);

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            zadanie8();
            //zadanie7();
            //zadanie6()
            Console.ReadKey();
        }
    }
}
