using System;
using System.Collections.Generic;
using System.Linq;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using Bloody.Core.Models.v1;
using Bloody.Core.Helper.v1;

namespace Bloody.Core.GameData.v1;

public class Users
{
    internal static ulong CurrentUserSteamId { get; set; }

    private Users() { }

    private static Users _instance;
    internal static Users Instance => _instance ??= new Users();

    public UserModel GetCurrentUser()
    {
        return GetUserByPlatformId(CurrentUserSteamId);
    }

    public UserModel GetUserByPlatformId(ulong platformId)
    {
        return All.FirstOrDefault(u => u.PlatformId == platformId);
    }

    public UserModel GetUserByCharacterName(string characterName)
    {
        return All.FirstOrDefault(u => u.CharacterName == characterName);
    }

    public UserModel FromEntity(Entity userEntity)
    {
        if (!Core.World.EntityManager.HasComponent<User>(userEntity))
        {
            throw new Exception("Given entity does not have User component.");
        }

        return new UserModel(userEntity);
    }

    public IEnumerable<UserModel> Online
    {
        get { return All.Where(u => u.IsConnected); }
    }

    public IEnumerable<UserModel> All
    {
        get
        {
            {
                var userEntities = QueryComponents.GetEntitiesByComponentTypes<User>(EntityQueryOptions.IncludeDisabledEntities);
                foreach (var userEntity in userEntities)
                {
                    yield return FromEntity(userEntity);
                }
            }
        }
    }
}