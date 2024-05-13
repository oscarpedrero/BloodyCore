using BloodyCore.Helper;
using ProjectM.Network;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace BloodyCore.API
{
    public class UserSystem
    {
        public static Entity GetAnyUser()
        {
            var _entities = QueryComponents.GetEntitiesByComponentTypes<User>();
            foreach (var _entity in _entities)
            {
                return _entity;
            }
            return Entity.Null;
        }

        public static NativeArray<Entity> GetAllUsers()
        {
            var _entities = QueryComponents.GetEntitiesByComponentTypes<User>();
            return _entities;

        }

        public static bool isNewUser(Entity userEntity)
        {

            var userComponent = getUserComponente(userEntity);
            return userComponent.CharacterName.IsEmpty;

        }

        public static string getCharacterName(Entity userEntity)
        {

            return getUserComponente(userEntity).CharacterName.ToString();

        }

        public static User getUserComponente(Entity userEntity)
        {
            return BloodyCore.Systems.EntityManager.GetComponentData<User>(userEntity);
        }

        public static IEnumerable<Entity> GetAllUsersOnline()
        {

            NativeArray<Entity> userEntities = BloodyCore.Systems.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<User>()).ToEntityArray(Allocator.Temp);
            int len = userEntities.Length;
            for (int i = 0; i < len; ++i)
            {
                yield return userEntities[i];
            }

        }
    }
}
