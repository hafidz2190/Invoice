using Invoice.Service;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace Invoice.Builder
{
    public static class SignalRHostBuilder<TStartup>
    {
        public static SignalRHost<TStartup> BuildSignalRHost(string baseAddress)
        {
            return new SignalRHost<TStartup>(baseAddress);
        }
    }

    public class SignalRHost<TStartup>
    {
        private IDisposable _host;
        private string _baseAddress;

        public SignalRHost(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public void Open()
        {
            AppDomain.CurrentDomain.Load(typeof(ClientConnectionHub).Assembly.FullName);

            _host = WebApp.Start<TStartup>(_baseAddress);
        }

        public void Close()
        {
            if (_host == null)
                return;

            _host.Dispose();
        }
    }

    // signalr settings class
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };

            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(hubConfiguration);
        }
    }
}
