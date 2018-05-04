using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Service
{
    public class ClientConnectionHub : Hub
    {
        public override Task OnConnected()
        {
            string key = Context.QueryString["Key"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(key))
                return base.OnConnected();

            ClientConnectionMap.Add(key, connectionId);
            BroadcastClientConnectionMap();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string key = Context.QueryString["Key"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(key))
                return base.OnDisconnected(stopCalled);

            ClientConnectionMap.Remove(key, connectionId);
            BroadcastClientConnectionMap();

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string key = Context.QueryString["Key"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(key))
                return base.OnReconnected();

            if (!ClientConnectionMap.GetConnections(key).Contains(connectionId))
            {
                ClientConnectionMap.Add(key, connectionId);
                BroadcastClientConnectionMap();
            }

            return base.OnReconnected();
        }

        public void BroadcastClientConnectionMap()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ClientMonitorHub>();
            context.Clients.All.addMessage(ClientConnectionMap.GetConnectionMap());
        }

        public void SendMessage(string username, string message)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ClientConnectionHub>();
            var connection = ClientConnectionMap.GetConnectionMap(username);
            List<string> ConnectionIds = connection.Value.ConnectionIds.ToList();

            for (int i = 0; i < ConnectionIds.Count; i++)
            {
                string connectionId = ConnectionIds[i];
                context.Clients.User(connectionId).addMessage(message);
            }
        }
    }
}