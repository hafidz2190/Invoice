using System.Collections.Generic;
using System.Linq;

namespace Invoice.Service
{
    public static class ClientMonitorMap
    {
        private static readonly Dictionary<string, HashSet<string>> _connectionMap = new Dictionary<string, HashSet<string>>();

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
                HashSet<string> connections;

                if (!_connectionMap.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connectionMap.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public static IEnumerable<string> GetConnections(string key)
        {
            HashSet<string> connections;

            if (_connectionMap.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public static void Remove(string key, string connectionId)
        {
            lock (_connectionMap)
            {
                HashSet<string> connections;

                if (!_connectionMap.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connectionMap.Remove(key);
                    }
                }
            }
        }

        public static Dictionary<string, HashSet<string>> GetConnectionMap()
        {
            return _connectionMap;
        }
    }
}