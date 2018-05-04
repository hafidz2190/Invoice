using System.Collections.Generic;
using System.Linq;

namespace Invoice.Service
{
    public static class ClientConnectionMap
    {
        private static readonly Dictionary<string, ClientConnectionModel> _connectionMap = new Dictionary<string, ClientConnectionModel>();

        public static int Count
        {
            get
            {
                return _connectionMap.Count;
            }
        }

        public static void Add(string key, string connectionId)
        {
            lock (_connectionMap)
            {
                ClientConnectionModel connections;

                if (!_connectionMap.TryGetValue(key, out connections))
                {
                    connections = new ClientConnectionModel
                    {
                        IsRequesting = false,
                        ConnectionIds = new HashSet<string>()
                    };

                    _connectionMap.Add(key, connections);
                }

                lock (connections)
                {
                    connections.ConnectionIds.Add(connectionId);
                }
            }
        }

        public static IEnumerable<string> GetConnections(string key)
        {
            ClientConnectionModel connections;

            if (_connectionMap.TryGetValue(key, out connections))
            {
                return connections.ConnectionIds;
            }

            return Enumerable.Empty<string>();
        }

        public static void Remove(string key, string connectionId)
        {
            lock (_connectionMap)
            {
                ClientConnectionModel connections;

                if (!_connectionMap.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.ConnectionIds.Remove(connectionId);

                    if (connections.ConnectionIds.Count == 0)
                    {
                        _connectionMap.Remove(key);
                    }
                }
            }
        }

        public static Dictionary<string, ClientConnectionModel> GetConnectionMap()
        {
            return _connectionMap;
        }

        public static KeyValuePair<string, ClientConnectionModel> GetConnectionMap(string key)
        {
            return _connectionMap.Where(m => m.Key == key).FirstOrDefault();
        }
    }

    public class ClientConnectionModel
    {
        public bool IsRequesting { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}