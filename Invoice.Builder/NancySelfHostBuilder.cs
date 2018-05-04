using Nancy.Hosting.Self;
using System;

namespace Invoice.Builder
{
    public class NancySelfHostBuilder
    {
        public NancyHost BuildNancyHost(string baseAddress)
        {
            HostConfiguration hostConfiguration = new HostConfiguration { UrlReservations = new UrlReservations() { CreateAutomatically = true } };
            NancyHost nancyHost = new NancyHost(hostConfiguration, new Uri(baseAddress));

            return nancyHost;
        }
    }
}
