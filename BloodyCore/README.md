# BloodyCore v2.0.1

**BloodyCore** is a foundational framework mod for V Rising that provides essential APIs and utilities for other mods. It serves as the backbone for the Bloody mod ecosystem, offering robust systems for events, spawning, user management, and more.

## 🚀 Features

### Core Systems
- **Event Handling System** - Comprehensive event hooks for game mechanics
- **Spawn System** - Advanced entity spawning with callbacks and positioning
- **User Management** - Player data handling and utilities
- **Network System** - Client-server communication helpers
- **Coroutine Handler** - Managed timing and repeated actions
- **Custom Events** - Extensible event system for mod interactions
- **Buff System** - Apply and manage buffs/debuffs
- **Font Color Chat System** - Colored chat messages and formatting

### API Components
- **GameData** - Access to game entities, items, NPCs, and users
- **Helper Utilities** - Logging, prefab management, and core utilities
- **Query Components** - Entity query system for ECS
- **Models** - Character, NPC, User, and Item models

## 📋 Requirements

- [BepInEx 6.0.0-be.733](https://thunderstore.io/c/v-rising/p/BepInEx/BepInExPack_V_Rising/)
- [VRising.Unhollowed.Client 1.1.*](https://nuget.bepinex.dev/packages/VRising.Unhollowed.Client/)
- .NET 6.0

## 🔧 Installation

### For Players
1. Install BepInEx for V Rising
2. Download BloodyCore.dll from releases
3. Place in `BepInEx/plugins/` folder
4. Launch the game

### For Developers
Add to your `.csproj`:
```xml
<PackageReference Include="Bloody.Core" Version="2.0.1" />
```

Or use local reference:
```xml
<Reference Include="Bloody.Core">
  <HintPath>../BloodyCore/BloodyCore/bin/Release/net6.0/Bloody.Core.dll</HintPath>
</Reference>
```

## 📚 Complete API Documentation

### Core.cs - Main Entry Point

```csharp
// Check environment
Core.IsServer // true if running on server
Core.IsClient // true if running on client
Core.World // Access to Unity ECS World
Core.SystemsCore // Access to core game systems
Core.EntityManager // Direct entity manager access
```

### Event Handling System

Complete event subscription system:

```csharp
// World initialization
EventsHandlerSystem.OnInitialize += () => {
    Logger.LogInfo("World initialized!");
};

// Death events with full death data
EventsHandlerSystem.OnDeath += (DeathEvent deathEvent) => {
    var victim = deathEvent.Died;
    var killer = deathEvent.Killer;
    var position = deathEvent.DeathPosition;
    
    if (victim.Has<PlayerCharacter>()) {
        var user = victim.GetComponent<PlayerCharacter>().UserEntity;
        Logger.LogInfo($"Player died at {position}");
    }
};

// VBlood death events
EventsHandlerSystem.OnDeathVBlood += (DeathEventListenerSystem sender, NativeArray<DeathEvent> deathEvents) => {
    foreach (var deathEvent in deathEvents) {
        var vblood = deathEvent.Died;
        var killer = deathEvent.Killer;
        // Handle VBlood death
    }
};

// Damage events
EventsHandlerSystem.OnDamage += (Entity victim, Entity attacker, ref DealDamageEvent dealDamageEvent) => {
    var damage = dealDamageEvent.MainDamage;
    var damageType = dealDamageEvent.MainDamageType;
    Logger.LogInfo($"Damage dealt: {damage} of type {damageType}");
};

// Player buff events
EventsHandlerSystem.OnPlayerBuffed += (Entity player, Entity buffEntity) => {
    var buff = buffEntity.GetComponent<PrefabGUID>();
    Logger.LogInfo($"Player received buff: {buff.GuidHash}");
};

// Player buff removed events
EventsHandlerSystem.OnPlayerBuffRemoved += (Entity player, Entity buffEntity) => {
    var buff = buffEntity.GetComponent<PrefabGUID>();
    Logger.LogInfo($"Player lost buff: {buff.GuidHash}");
};
```

### Spawn System

Advanced entity spawning with multiple options:

```csharp
// Basic spawn at 2D position
SpawnSystem.SpawnUnitWithCallback(user, prefabGUID, new float2(x, z), lifetime, (Entity spawned) => {
    // Configure spawned entity
    spawned.Add<Immortal>(); // Make immortal
    spawned.SetComponent(new Health { Value = 1000, MaxHealth = 1000 });
});

// Spawn at 3D position with custom height (v2.0.1+)
SpawnSystem.SpawnUnitWithCallback(user, prefabGUID, new float3(x, y, z), lifetime, (Entity spawned) => {
    // Spawns at exact Y coordinate when on structures
});

// Spawn with team
var team = TeamEnum.Team1; // or Team2, Team3, etc.
SpawnSystem.SpawnUnitWithCallback(user, prefabGUID, position, lifetime, (Entity spawned) => {
    spawned.SetComponent(new Team { Value = team });
});

// Spawn with custom stats
SpawnSystem.SpawnUnitWithCallback(user, prefabGUID, position, lifetime, (Entity spawned) => {
    spawned.SetComponent(new UnitStats {
        PhysicalPower = 100,
        SpellPower = 50,
        PhysicalResistance = 0.5f,
        SpellResistance = 0.3f
    });
});

// Constants available
SpawnSystem.DEFAULT_MINRANGE // Default minimum spawn range
SpawnSystem.DEFAULT_MAXRANGE // Default maximum spawn range
```

### User System

Comprehensive player management:

```csharp
// Get user by character name
var userEntity = UserSystem.GetUserByCharacterName("PlayerName");
if (userEntity != Entity.Null) {
    var user = userEntity.GetComponent<User>();
    var steamId = user.PlatformId;
    var character = user.LocalCharacter._Entity;
}

// Get all online users
foreach (var userEntity in UserSystem.GetAllUsers()) {
    var user = userEntity.GetComponent<User>();
    var character = user.LocalCharacter._Entity;
    var playerName = character.GetComponent<PlayerCharacter>().Name.ToString();
}

// Check if user is online
bool isOnline = UserSystem.IsUserOnline(userEntity);

// Get user by Steam ID
var userBySteam = UserSystem.GetUserBySteamId(steamId);

// Get character entity from user
var characterEntity = UserSystem.GetUserCharacterEntity(userEntity);
```

### Buff System

Apply and manage buffs:

```csharp
// Apply temporary buff
BuffSystem.ApplyBuff(user, targetEntity, buffPrefabGUID, duration);

// Apply permanent buff
BuffSystem.ApplyPermanentBuff(user, targetEntity, buffPrefabGUID);

// Remove buff
BuffSystem.RemoveBuff(targetEntity, buffPrefabGUID);

// Check if entity has buff
bool hasBuff = BuffSystem.HasBuff(targetEntity, buffPrefabGUID);

// Get all buffs on entity
var buffs = BuffSystem.GetBuffs(targetEntity);
foreach (var buff in buffs) {
    var prefab = buff.GetComponent<PrefabGUID>();
    var duration = buff.GetComponent<LifeTime>().Duration;
}

// Apply buff with custom modifiers
BuffSystem.ApplyBuffWithModifiers(user, target, buffGUID, duration, (Entity buffEntity) => {
    // Modify buff entity components
    buffEntity.SetComponent(new ModifyUnitStatBuff_DOTS {
        Value = 10, // Damage per tick
        Type = ModifyUnitStatType.Add
    });
});
```

### Network System

Server-client communication:

```csharp
// Send message to specific user
NetworkSystem.SendMessageToUser(userEntity, "Hello player!");

// Send message to all users
NetworkSystem.SendMessageToAllUsers("Server announcement!");

// Send colored message
NetworkSystem.SendColoredMessage(userEntity, "Error!", FontColorChatSystem.Red);

// Broadcast to team
NetworkSystem.SendMessageToTeam(TeamEnum.Team1, "Team message");

// Send with custom formatting
var message = FontColorChatSystem.ColorText("Important:", FontColorChatSystem.Yellow) + " Update available!";
NetworkSystem.SendMessageToUser(userEntity, message);
```

### Font Color Chat System

Rich text formatting:

```csharp
// Available colors
FontColorChatSystem.White    // #FFFFFFxx
FontColorChatSystem.Silver   // #C0C0C0xx  
FontColorChatSystem.Yellow   // #FFFF00xx
FontColorChatSystem.Green    // #00FF00xx
FontColorChatSystem.Blue     // #0000FFxx
FontColorChatSystem.Red      // #FF0000xx
FontColorChatSystem.Purple   // #A020F0xx
FontColorChatSystem.Orange   // #FFA500xx

// Color text
string colored = FontColorChatSystem.ColorText("Hello", FontColorChatSystem.Green);

// Multiple colors in one message
string message = $"{FontColorChatSystem.ColorText("Success:", FontColorChatSystem.Green)} {FontColorChatSystem.ColorText("Boss defeated!", FontColorChatSystem.Yellow)}";

// Bold text
string bold = FontColorChatSystem.Bold("Important!");

// Italic text  
string italic = FontColorChatSystem.Italic("Note:");

// Combined formatting
string fancy = FontColorChatSystem.Bold(FontColorChatSystem.ColorText("CRITICAL", FontColorChatSystem.Red));
```

### Coroutine Handler

Managed timing and repeated actions:

```csharp
// Start one-time delayed action
CoroutineHandler.StartCoroutine(() => {
    Logger.LogInfo("This runs after 5 seconds");
}, 5f);

// Start repeating action
var coroutineId = CoroutineHandler.StartRepeatingCoroutine(() => {
    Logger.LogInfo("This runs every 10 seconds");
}, 10f);

// Stop specific coroutine
CoroutineHandler.StopCoroutine(coroutineId);

// Stop all coroutines
CoroutineHandler.StopAllCoroutines();

// Conditional repeating action
var id = CoroutineHandler.StartRepeatingCoroutine(() => {
    if (someCondition) {
        CoroutineHandler.StopCoroutine(id); // Stop itself
    }
}, 1f);

// Action with entity reference
Entity bossEntity = // ... spawn boss
CoroutineHandler.StartRepeatingCoroutine(() => {
    if (bossEntity.Has<Health>()) {
        var health = bossEntity.GetComponent<Health>();
        Logger.LogInfo($"Boss health: {health.Value}/{health.MaxHealth}");
    }
}, 2f);
```

### Custom Events System

Create and handle custom mod events:

```csharp
// Define custom event data
public struct BossSpawnedEvent {
    public Entity Boss;
    public float3 Position;
    public string BossName;
}

// Register custom event
CustomEventsHandlerSystem.RegisterEvent<BossSpawnedEvent>();

// Subscribe to custom event
CustomEventsHandlerSystem.OnCustomEvent<BossSpawnedEvent> += (BossSpawnedEvent data) => {
    Logger.LogInfo($"Boss {data.BossName} spawned at {data.Position}");
};

// Trigger custom event
CustomEventsHandlerSystem.TriggerEvent(new BossSpawnedEvent {
    Boss = bossEntity,
    Position = position,
    BossName = "Ancient Dragon"
});

// Unsubscribe from event
CustomEventsHandlerSystem.OnCustomEvent<BossSpawnedEvent> -= YourHandler;
```

### Game Data Access

Access game information:

```csharp
// === Items ===
// Get item by name
var item = GameData.Items.GetPrefabByName("Blood Essence");
var itemGuid = item.PrefabGUID;
var itemName = item.PrefabName;

// Get item by GUID
var itemByGuid = GameData.Items.GetPrefabByGuid(-77477508); // Blood Essence GUID

// Get all items
foreach (var itemData in GameData.Items.GetAllItems()) {
    Logger.LogInfo($"Item: {itemData.PrefabName} ({itemData.PrefabGUID})");
}

// === NPCs ===
// Get NPC by name
var npc = GameData.Npcs.GetPrefabByName("CHAR_Bandit_Thug");

// Get NPC by GUID
var npcByGuid = GameData.Npcs.GetPrefabByGuid(-1905691330); // Alpha Wolf GUID

// Get all NPCs
foreach (var npcData in GameData.Npcs.GetAllNpcs()) {
    Logger.LogInfo($"NPC: {npcData.PrefabName} ({npcData.PrefabGUID})");
}

// === Users ===
// Convert entity to user model
var userModel = GameData.Users.FromEntity(userEntity);
if (userModel != null) {
    Logger.LogInfo($"Character: {userModel.CharacterName}");
    Logger.LogInfo($"Steam ID: {userModel.PlatformId}");
    Logger.LogInfo($"Admin: {userModel.IsAdmin}");
    Logger.LogInfo($"Level: {userModel.Level}");
}

// Get all online users as models
foreach (var user in GameData.Users.GetAllOnlineUsers()) {
    Logger.LogInfo($"Online: {user.CharacterName} (Level {user.Level})");
}
```

### Helper Utilities

#### Logger
```csharp
// Different log levels
Logger.LogInfo("Information message");
Logger.LogWarning("Warning message");
Logger.LogError("Error message");
Logger.LogDebug("Debug message"); // Only in debug builds
Logger.LogMessage("Generic message");

// Log with context
Logger.LogInfo($"Player {playerName} joined");
Logger.LogError($"Failed to spawn boss: {exception.Message}");

// Conditional logging
if (Logger.EnableDebugLogging) {
    Logger.LogDebug("Detailed debug information");
}
```

#### Prefabs
```csharp
// Create prefab GUID from hash
var prefabGUID = Prefabs.CreatePrefabGUID(-77477508);

// Get prefab name
var name = Prefabs.GetPrefabName(prefabGUID);

// Check if prefab exists
bool exists = Prefabs.PrefabExists(prefabGUID);

// Common prefab GUIDs
var bloodEssence = Prefabs.BloodEssence; // -77477508
var greaterBloodEssence = Prefabs.GreaterBloodEssence; // 862477668
var pristineBloodEssence = Prefabs.PristineBloodEssence; // -1423498013

// Get prefab entity
var prefabEntity = Prefabs.GetPrefabEntity(prefabGUID);
```

#### Query Components
```csharp
// Create entity query
var playerQuery = QueryComponents.GetEntityQuery<PlayerCharacter, User>();

// Query with multiple components
var vampireQuery = QueryComponents.GetEntityQuery<PlayerCharacter, Blood, Equipment>();

// Use query
var entities = Core.EntityManager.GetEntityQuery(playerQuery);
foreach (var entity in entities.ToEntityArray(Allocator.Temp)) {
    // Process entities
}

// Query with exclusions
var query = Core.EntityManager.CreateEntityQuery(
    ComponentType.ReadOnly<PlayerCharacter>(),
    ComponentType.Exclude<Dead>()
);
```

#### Utils Core
```csharp
// Get prefab GUID from entity
var prefabGuid = UtilsCore.GetPrefabGUID(entity);

// Get translation (position) from entity
var position = UtilsCore.GetTranslation(entity);

// Set translation
UtilsCore.SetTranslation(entity, new float3(x, y, z));

// Get team from entity
var team = UtilsCore.GetTeam(entity);

// Check if entity exists and is valid
bool isValid = UtilsCore.IsEntityValid(entity);

// Destroy entity safely
UtilsCore.DestroyEntity(entity);

// Get component safely
if (UtilsCore.TryGetComponent<Health>(entity, out var health)) {
    Logger.LogInfo($"Health: {health.Value}");
}
```

### Models

#### UserModel
```csharp
public class UserModel {
    public string CharacterName { get; set; }
    public ulong PlatformId { get; set; }
    public bool IsOnline { get; set; }
    public bool IsAdmin { get; set; }
    public Entity Entity { get; set; }
    public Entity Character { get; set; }
    public int Level { get; set; }
    public float3 Position { get; set; }
    public ClanTeam ClanEntity { get; set; }
}
```

#### CharacterModel
```csharp
public class CharacterModel {
    public string Name { get; set; }
    public int Level { get; set; }
    public float3 Position { get; set; }
    public Entity Entity { get; set; }
    public UnitStats Stats { get; set; }
    public Health Health { get; set; }
}
```

#### NpcModel
```csharp
public class NpcModel {
    public string Name { get; set; }
    public PrefabGUID PrefabGUID { get; set; }
    public int Level { get; set; }
    public Entity Entity { get; set; }
    public float3 Position { get; set; }
    public UnitStats Stats { get; set; }
}
```

#### ItemModel
```csharp
public class ItemModel {
    public string Name { get; set; }
    public PrefabGUID PrefabGUID { get; set; }
    public int StackCount { get; set; }
    public Entity Entity { get; set; }
    public int ItemId { get; set; }
}
```

## 🌟 What's New in v2.0.1

- **Added**: `SpawnUnitWithCallback` overload accepting float3 position for precise Y-coordinate spawning
- **Fixed**: Boss spawning now correctly respects Y-coordinate when spawning on elevated structures

## 🔄 Version History

### v2.0.1 (2025-01-09)
- Added float3 position support for SpawnSystem
- Fixed Y-coordinate spawning for elevated positions
- Complete documentation rewrite

### v2.0.0 (2024-12-28)
- Complete rewrite for Oakveil compatibility
- Removed deprecated features
- Improved performance and stability
- Better error handling

### v1.x.x
- Legacy versions for pre-Oakveil V Rising

## 🤝 For Mod Developers

### Basic Mod Structure

```csharp
using BepInEx;
using BepInEx.Unity.IL2CPP;
using Bloody.Core;
using Bloody.Core.API.v1;
using Bloody.Core.GameData.v1;
using Bloody.Core.Helper.v1;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("trodi.Bloody.Core")]
[Reloadable]
public class Plugin : BasePlugin
{
    public override void Load()
    {
        if (!Core.IsServer) {
            Log.LogInfo("Client mod - skipping server initialization");
            return;
        }
        
        // Initialize your mod
        EventsHandlerSystem.OnInitialize += Initialize;
        EventsHandlerSystem.OnDeath += OnDeath;
        
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} loaded!");
    }
    
    private void Initialize()
    {
        Logger.LogInfo("World initialized, setting up mod features...");
        // Your initialization code
    }
    
    private void OnDeath(DeathEvent deathEvent)
    {
        if (deathEvent.Died.Has<PlayerCharacter>()) {
            var user = deathEvent.Died.GetComponent<PlayerCharacter>().UserEntity;
            var userModel = GameData.Users.FromEntity(user);
            Logger.LogInfo($"Player {userModel.CharacterName} died!");
        }
    }
    
    public override bool Unload()
    {
        EventsHandlerSystem.OnInitialize -= Initialize;
        EventsHandlerSystem.OnDeath -= OnDeath;
        CoroutineHandler.StopAllCoroutines();
        return true;
    }
}
```

### Best Practices

1. **Always check server/client context**:
   ```csharp
   if (Core.IsServer) { /* Server code */ }
   if (Core.IsClient) { /* Client code */ }
   ```

2. **Use callbacks for spawning**:
   ```csharp
   SpawnSystem.SpawnUnitWithCallback(user, prefab, pos, duration, (entity) => {
       // Configure entity after spawn
   });
   ```

3. **Handle events properly**:
   ```csharp
   public override void Load() {
       EventsHandlerSystem.OnDeath += HandleDeath;
   }
   
   public override bool Unload() {
       EventsHandlerSystem.OnDeath -= HandleDeath;
       return true;
   }
   ```

4. **Clean up coroutines**:
   ```csharp
   public override bool Unload() {
       CoroutineHandler.StopAllCoroutines();
       return true;
   }
   ```

5. **Use proper logging**:
   ```csharp
   Logger.LogInfo("Normal operation");
   Logger.LogWarning("Potential issue");
   Logger.LogError("Error occurred");
   Logger.LogDebug("Debug info"); // Only in debug builds
   ```

6. **Entity validation**:
   ```csharp
   if (entity != Entity.Null && entity.Has<Health>()) {
       // Safe to use entity
   }
   ```

## 🐛 Known Issues

- Some events may not fire in all game contexts
- Coroutines are tied to game update loop and may pause during loading screens

## 📞 Support

- **GitHub Issues**: [Report bugs or request features](https://github.com/oscarpedrero/BloodyCore/issues)
- **Discord**: [V Rising Mod Community](https://discord.gg/vrisingmods) - Ping @trodi for support

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Credits

- **trodi** - Original author and maintainer
- **oscarpedrero** - GitHub repository maintainer
- **V Rising Mod Community** - Testing and feedback
- **BepInEx Team** - For the modding framework
- **Stunlock Studios** - For creating V Rising

---

*BloodyCore v2.0.1 - The foundation for V Rising mods*