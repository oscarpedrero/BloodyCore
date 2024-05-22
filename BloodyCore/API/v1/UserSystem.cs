using Bloody.Core.Helper.v1;
using ProjectM;
using ProjectM.Network;
using Stunlock.Core;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace Bloody.Core.API.v1
{
    public class UserSystem
    {
        public static Entity GetAnyUser()
        {
            var _entities = QueryComponents.GetEntitiesByComponentTypes<User, PlayerCharacter>();
            foreach (var _entity in _entities)
            {
                return _entity;
            }
            return Entity.Null;
        }

        public static Entity GetOneUserOnline()
        {
            var _entities = QueryComponents.GetEntitiesByComponentTypes<User>();
            foreach (var _entity in _entities)
            {
                if (_entity.Read<User>().IsConnected) return _entity;
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
            return Core.SystemsCore.EntityManager.GetComponentData<User>(userEntity);
        }

        public static IEnumerable<Entity> GetAllUsersOnline()
        {

            NativeArray<Entity> userEntities = Core.SystemsCore.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<User>()).ToEntityArray(Allocator.Temp);
            int len = userEntities.Length;
            for (int i = 0; i < len; ++i)
            {
                yield return userEntities[i];
            }

        }

        public static bool TryAddInventoryItemOrDrop(Entity user, PrefabGUID itemGuid, int stacks)
        {
            AddItemResponse _addItemResponse = TryAddUserInventoryItem(user, itemGuid, stacks);
            if (_addItemResponse.Success)
            {
                return true;
            }

            CreateDroppedItemEntityForUser(user, itemGuid, stacks);

            return true;
        }

        public static AddItemResponse TryAddUserInventoryItem(Entity user, PrefabGUID itemGuid, int stacks)
        {
            return Core.SystemsCore.ServerScriptMapper._ServerGameManager.TryAddInventoryItem(user, itemGuid, stacks);
        }

        public static void CreateDroppedItemEntityForUser(Entity user, PrefabGUID itemGuid, int stacks)
        {

            InventoryUtilitiesServer.CreateDropItem(Core.SystemsCore.EntityManager, user, itemGuid, stacks, new Entity());
        }
    }
}
