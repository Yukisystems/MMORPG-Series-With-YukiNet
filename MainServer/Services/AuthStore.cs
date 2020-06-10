using System;
using System.Collections.Generic;

namespace MainServer.Services
{
    public class AuthStore : IAuthStore
    {
        private readonly Dictionary<Guid, int> AuthClients = new Dictionary<Guid, int>();

        public void Add(Guid guid, int id)
        {
            AuthClients.Add(guid, id);
        }

        public bool Remove(Guid guid, int id)
        {
            return AuthClients.Remove(guid);
        }

        public int this[Guid guid] => AuthClients.ContainsKey(guid) ? AuthClients[guid] : -1;
    }
}