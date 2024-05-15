using Bloody.Core.Helper;
using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace Bloody.Core.API
{
    public class NetworkSystem
    {
        public static Entity getEntityFromNetworkId(NetworkId networkid)
        {
            Entity entity;
            var networkIdLookupEntity = QueryComponents.GetEntitiesByComponentTypes<NetworkIdSystem.Singleton>(EntityQueryOptions.IncludeSystems)[0];
            var singleton = networkIdLookupEntity.Read<NetworkIdSystem.Singleton>();
            singleton.GetNetworkIdLookupRW().TryGetValue(networkid, out entity);
            return entity;
        }
    }
}
