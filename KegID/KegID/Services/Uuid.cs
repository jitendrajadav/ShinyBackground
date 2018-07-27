using System;

namespace KegID.Services
{
    public class UuidManager : IUuidManager
    {
        public string GetUuId()
        {
           return Guid.NewGuid().ToString();
        }
    }
}
