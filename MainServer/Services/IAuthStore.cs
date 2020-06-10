using System;

namespace MainServer.Services
{
    public interface IAuthStore
    {
        int this[Guid guid] { get; }

        void Add(Guid guid, int id);
        bool Remove(Guid guid, int id);
    }
}