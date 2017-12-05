using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;


namespace IOLaboratorium
{
    public class ZadaniaTAP
    {
        #region Zadanie 1
        public struct TResultDataStructure
        {
            private int a;
            private int b;

            public TResultDataStructure(int a_, int b_)
            {
                a = a_;
                b = b_;
            }

            public int B { get => b; set => b = value; }
            public int A { get => a; set => a = value; }
        }
        public Task<TResultDataStructure> AsyncMethod1(byte[] buffer)
        {
            TaskCompletionSource<TResultDataStructure> tcs = new TaskCompletionSource<TResultDataStructure>();
            Task.Run(() =>
            {
                tcs.SetResult(new TResultDataStructure(/*fragment zadania 1*/));
            });
            return tcs.Task;
        }
        public TResultDataStructure Zadanie1()
        {
            var task = AsyncMethod1(null);
            task.Wait();
            return task.GetAwaiter().GetResult();
        }
        #endregion
        #region Zadanie 2
        private bool zadanie2 = false;
        private static object client;

        public bool Z2 {
            get { return zadanie2; }
            set { zadanie2 = value; }
        }
        public void Zadanie2()
        {
            //ZADANIE 2. ODKOMENTUJ I POPRAW  
            /*
                Task.Run(
                    () ==
                    {
                       Z2 = true;
                    }
             */
        }
        #endregion
        #region Zadanie 3
        public async Task<XmlDocument> Zadanie3(string address)
        {
            //WebClient webClient;
            //webClient.DownloadStringTaskAsync(new Uri(address));
            return new XmlDocument();
        }
        #endregion
        #region Zadanie 4-8
        public class Server
        {
            #region Variables
            TcpListener server;
            int port;
            IPAddress address;
            bool running = false;
            CancellationTokenSource cts = new CancellationTokenSource();
            Task serverTask;
            public Task ServerTask {
                get { return serverTask; }
            }
            #endregion
            #region Properties
            public IPAddress Address {
                get { return address; }
                set {
                    if (!running) address = value;
                    else;
                }
            }
            public int Port {
                get { return port; }
                set {
                    if (!running)
                        port = value;
                    else;
                }
            }
            #endregion
            #region Constructors
            public Server()
            {
                Address = IPAddress.Any;
                port = 2048;
            }
            public Server(int port)
            {
                this.port = port;
            }
            public Server(IPAddress address)
            {
                this.address = address;
            }
            #endregion
            #region Methods

            public async Task RunAsync(CancellationToken ct)
            {

                server = new TcpListener(address, port);

                try
                {
                    server.Start();
                    running = true;
                }
                catch (SocketException ex)
                {
                    throw (ex);
                }
                while (true && !ct.IsCancellationRequested)
                {

                    TcpClient client = await server.AcceptTcpClientAsync();
                    byte[] buffer = new byte[1024];
                    using (ct.Register(() => client.GetStream().Close()))
                    {
                        client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct).ContinueWith(
                            async (t) =>
                            {
                                int i = t.Result;
                                while (true)
                                {
                                    client.GetStream().WriteAsync(buffer, 0, i, ct);
                                    try
                                    {
                                        i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length, ct);
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                }
                            });
                    }
                }

            }
            public void RequestCancellation()
            {
                cts.Cancel();
                //serverTask.Wait();
                //serverTask.Dispose();
                server.Stop();
            }
            public void Run()
            {

                serverTask = RunAsync(cts.Token);
            }
            public void StopRunning()
            {
                RequestCancellation();
                //serverTask.Dispose();
            }
            #endregion
        }

        public class Client
        {
            #region variables
            TcpClient client;

            public Client()
            {
            }
            public Task ClientTask
            {
                get { return ClientTask; }
            }
            #endregion
            #region properties
            #endregion
            #region Constructors
            #endregion
            #region Methods
            public void Connect()
            {
                client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            }
            public async Task<string> Ping(string message)
            {
                byte[] buffer = new ASCIIEncoding().GetBytes(message);
                client.GetStream().WriteAsync(buffer, 0, buffer.Length);
                buffer = new byte[1024];
                var t = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, t);
            }
            public async Task<IEnumerable<string>> keepPinging(string message, CancellationToken token)
            {
                List<string> messages = new List<string>();
                bool done = false;
                while (!done)
                {
                    if (token.IsCancellationRequested)
                        done = true;
                    messages.Add(await Ping(message));
                }
                return messages;
            }
            #endregion
        }
        #endregion

        static async Task ClientTask()
        {
            #region variables
            TcpClient client;
            #endregion
            client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));

        }
    }
}
