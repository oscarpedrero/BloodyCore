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

### API Components
- **BuffSystem** - Apply and manage buffs/debuffs
- **FontColorChatSystem** - Colored chat messages and formatting
- **GameData** - Access to game entities, items, NPCs, and users
- **Helper Utilities** - Logging, prefab management, and core utilities

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

## 📚 API Documentation

### Event System

Subscribe to game events:

```csharp
EventsHandlerSystem.OnInitialize += () => {
    Logger.LogInfo("World initialized!");
};

EventsHandlerSystem.OnDeath += (DeathEvent deathEvent) => {
    if (deathEvent.Died.Has<PlayerCharacter>()) {
        Logger.LogInfo($"Player died at {deathEvent.DeathPosition}");
    }
};

EventsHandlerSystem.OnDeathVBlood += (DeathEventListenerSystem sender, NativeArray<DeathEvent> deathEvents) => {
    // Handle VBlood deaths
};
```

### Spawn System

Spawn entities with callbacks:

```csharp
// Spawn at specific 2D position
SpawnSystem.SpawnUnitWithCallback(user, prefabGUID, position2D, lifetime, (Entity spawned) => {
    // Configure spawned entity
});

// Spawn at specific 3D position (v2.0.1+)
SpawnSystem.SpawnUnitWithCallback(user, prefabGUID, position3D, lifetime, (Entity spawned) => {
    // Configure spawned entity with custom height
});
```

### User System

Access player data:

```csharp
// Get user by character name
var user = UserSystem.GetUserByCharacterName("PlayerName");

// Get all online users
foreach (var userEntity in UserSystem.GetAllUsers()) {
    var userModel = GameData.Users.FromEntity(userEntity);
    Logger.LogInfo($"User: {userModel.CharacterName}");
}
```

### Buff System

Apply buffs to entities:

```csharp
// Apply buff with duration
BuffSystem.ApplyBuff(user, targetEntity, buffPrefabGUID, duration);

// Apply permanent buff
BuffSystem.ApplyPermanentBuff(user, targetEntity, buffPrefabGUID);
```

### Coroutine Handler

Schedule repeated actions:

```csharp
// Start repeating action
var coroutineId = CoroutineHandler.StartRepeatingCoroutine(() => {
    Logger.LogInfo("This runs every 5 seconds");
}, 5f);

// Stop coroutine
CoroutineHandler.StopCoroutine(coroutineId);
```

### Game Data Access

Access game information:

```csharp
// Get item by name
var item = GameData.Items.GetPrefabByName("Blood Essence");

// Get NPC by GUID
var npc = GameData.Npcs.GetPrefabByGuid(npcGUID);

// Get user model
var userModel = GameData.Users.FromEntity(userEntity);
```

## 🛠️ Helper Utilities

### Logger
```csharp
Logger.LogInfo("Information message");
Logger.LogWarning("Warning message");
Logger.LogError("Error message");
Logger.LogDebug("Debug message");
```

### Prefab Management
```csharp
// Create prefab from GUID
var prefabGUID = Prefabs.CreatePrefabGUID(guidValue);

// Get prefab name
var name = Prefabs.GetPrefabName(prefabGUID);
```

### Entity Queries
```csharp
// Query entities with components
var query = QueryComponents.GetEntityQuery<PlayerCharacter, User>();
```

## 🌟 What's New in v2.0.1

- **Added**: `SpawnUnitWithCallback` overload accepting float3 position for precise Y-coordinate spawning
- **Fixed**: Boss spawning now correctly respects Y-coordinate when spawning on elevated structures

## 🔄 Version History

### v2.0.1 (2025-01-09)
- Added float3 position support for SpawnSystem
- Fixed Y-coordinate spawning for elevated positions

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
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("trodi.Bloody.Core")]
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
    }
    
    private void Initialize()
    {
        // Your initialization code
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

## 🐛 Known Issues

- Some events may not fire in all game contexts

## 📞 Support

- **GitHub Issues**: [Report bugs or request features](https://github.com/oscarpedrero/BloodyCore/issues)
- **Discord**: [V Rising Mod Community](https://discord.gg/vrisingmods)

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Credits

- **trodi** - Original author and maintainer
- **oscarpedrero** - GitHub repository maintainer
- **V Rising Mod Community** - Testing and feedback

---

*BloodyCore v2.0.1 - The foundation for V Rising mods*