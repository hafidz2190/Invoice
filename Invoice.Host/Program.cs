using Invoice.Builder;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.IO;

namespace Invoice.Host
{
    public class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, object> mainObjectMap;

            try
            {
                string workspaceDirectory = Path.Combine("Workspaces", "W0");

                if (args.Length > 0)
                    workspaceDirectory = args[0];

                MainObjectBuilder mainObjectBuilder = new MainObjectBuilder(workspaceDirectory);
                mainObjectMap = mainObjectBuilder.BuildMainObject();
            }
            catch (Exception e)
            {
                throw e;
            }

            NancyHost nancyHost = (NancyHost)mainObjectMap["NancyHost"];
            SignalRHost<Startup> signalRHost = (SignalRHost<Startup>)mainObjectMap["SignalRHost"];

            try
            {
                Console.WriteLine("Nancy Host is starting.");

                nancyHost.Start();

                Console.WriteLine("Nancy Host has started.");

                Console.WriteLine("SignalR Host is starting.");

                signalRHost.Open();

                Console.WriteLine("SignalR Host has started.");

                Console.ReadKey();
            }
            catch (Exception e)
            {
                nancyHost.Stop();
                signalRHost.Close();

                throw e;
            }
        }
    }
}
