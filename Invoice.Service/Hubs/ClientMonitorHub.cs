using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Invoice.Service
{
    public class ClientMonitorHub : Hub
    {
        public override Task OnConnected()
        {
            string key = Context.QueryString["Key"];
            string connectionId = Context.ConnectionId;

            BroadcastConnectionMap();

            if (string.IsNullOrEmpty(key))
                return base.OnConnected();

            ClientMonitorMap.Add(key, connectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string key = Context.QueryString["Key"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(key))
                return base.OnDisconnected(stopCalled);

            ClientMonitorMap.Remove(key, connectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string key = Context.QueryString["Key"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(key))
                return base.OnReconnected();

            if (!ClientMonitorMap.GetConnections(key).Contains(connectionId))
            {
                ClientMonitorMap.Add(key, connectionId);
            }

            return base.OnReconnected();
        }

        public void BroadcastConnectionMap()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ClientMonitorHub>();
            context.Clients.All.addMessage(ClientConnectionMap.GetConnectionMap());
        }

        public void ClientCommand(string message)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMessage>(message);
            var connection = ClientConnectionMap.GetConnectionMap(json.ClientId);

            if (connection.Value.IsRequesting)
            {
                return;
            }

            connection.Value.IsRequesting = true;

            var context = GlobalHost.ConnectionManager.GetHubContext<ClientConnectionHub>();
            System.Collections.Generic.List<string> ConnectionIds = connection.Value.ConnectionIds.ToList();

            for (int i = 0; i < ConnectionIds.Count; i++)
            {
                string connectionId = ConnectionIds[i];
                context.Clients.Client(connectionId).addMessage(json.Message);
            }
        }

        class RequestMessage
        {
            public string ClientId { get; set; }
            public string Message { get; set; }
        }
    }
}