using Formulatrix.Common.Utility;
using Nancy.Hosting.Self;
using System.Collections.Generic;
using System.IO;

namespace Invoice.Builder
{
    public class MainObjectBuilder
    {
        private string _workspaceDirectory;

        public MainObjectBuilder(string workingDirectory)
        {
            _workspaceDirectory = workingDirectory;
        }

        public IDictionary<string, object> BuildMainObject()
        {
            IPropertySource systemConfig = new PropertyFile(Path.Combine(_workspaceDirectory, "Configs"), "System.config");

            string nancyHostBaseAddress = systemConfig.GetValue("NancyHost.BaseAddress", string.Empty);
            string signalRHostBaseAddress = systemConfig.GetValue("SignalRHost.BaseAddress", string.Empty);
            string configurationFilePath = Path.Combine(_workspaceDirectory, "Configs", "hibernate.cfg.xml");

            NancySelfHostBuilder nancySelfHostBuilder = new NancySelfHostBuilder();
            NancyHost nancyHost = nancySelfHostBuilder.BuildNancyHost(nancyHostBaseAddress);

            SignalRHost<Startup> signalRHost = SignalRHostBuilder<Startup>.BuildSignalRHost(signalRHostBaseAddress);

            DataManagerBuilder dataManagerBuilder = new DataManagerBuilder();
            dataManagerBuilder.BuildSessionFactory(configurationFilePath);

            IDictionary<string, object> mainObjectMap = new Dictionary<string, object>();
            mainObjectMap["NancyHost"] = nancyHost;
            mainObjectMap["SignalRHost"] = signalRHost;

            return mainObjectMap;
        }
    }
}
