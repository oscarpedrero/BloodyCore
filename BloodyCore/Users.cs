using System;
using System.Collections.Generic;
using System.Linq;
using ProjectM.Network;
using Unity.Collections;
using Unity.Entities;
using Bloody.Core.Models;

namespace Bloody.Core;

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
                var userEntities = Core.SystemsCore.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<User>())
                    .ToEntityArray(Allocator.Temp);
                foreach (var userEntity in userEntities)
                {
                    yield return FromEntity(userEntity);
                }
            }
        }
    }
}