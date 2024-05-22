using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Bloody.Core;
using Bloody.Core.API.v1;
using Bloody.Core.GameData.v1;
using Bloody.Core.Helper.v1;
using HarmonyLib;
using System.Linq;
using Unity.Entities;

namespace BloodyCoreTest;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("gg.deca.Bloodstone")]
[BepInDependency("trodi.Bloody.Core")]
[Bloodstone.API.Reloadable]
public class Plugin : BasePlugin
{
    Harmony _harmony;

    public static Bloody.Core.Helper.v1.Logger Logger;
    public static SystemsCore SystemsCore;

    public override void Load()
    {

        Core.InitBloodyCore();

        Logger = new(Log);
        // Plugin startup logic
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} version {MyPluginInfo.PLUGIN_VERSION} is loaded!");

        // Harmony patching
        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        EventsHandlerSystem.OnInitialize += GameDataOnInitialize;

    }

    private static void GameDataOnInitialize(World world)
    {
        Logger.LogInfo("GameDataOnInitialize");
        SystemsCore = Core.SystemsCore;

        Logger.LogWarning("All Users:");
        foreach (var userModel in GameData.Users.All)
        {
            Logger.LogMessage($"{userModel.CharacterName} Connected: {userModel.IsConnected}");
            foreach (var inventoryItem in userModel.Inventory.Items)
            {
                Logger.LogMessage($"\tSlot: {inventoryItem.Slot} Item: {inventoryItem.Item.Name} ({inventoryItem.Stacks})");
            }
        }

        var weapons = GameData.Items.Weapons.Take(10);
        Logger.LogWarning("Some Weapons:");
        foreach (var itemModel in weapons)
        {
            Logger.LogMessage($"{itemModel.PrefabGUID.GuidHash} {itemModel.PrefabName}");
            Logger.LogMessage($"{itemModel.ItemType} {itemModel.Internals.EquippableData.Value.WeaponType}");
        }

    }

    public override bool Unload()
    {
        EventsHandlerSystem.OnInitialize -= GameDataOnInitialize;
        _harmony?.UnpatchSelf();
        return true;
    }

}
