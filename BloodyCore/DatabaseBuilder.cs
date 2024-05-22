/*
using System;
using System.Collections.Generic;
using System.Linq;
using Bloodstone.API;
using Il2CppSystem.IO;
using ProjectM;
using ProjectM.Portrait;
using ProjectM.Shared;
using ProjectM.UI;
using Stunlock.Core;
using Unity.Entities;

namespace Bloody.Core;

public class DatabaseBuilder(MyWorld world)
{
    private readonly Dictionary<int, Dictionary<string, object>> _cache = new();

    public void Build()
    {
        var entities = world.EntityManager.GetAllEntities().ToArray().ToList();
        Plugin.Logger.LogInfo($"Iterating {entities.Count} entities");
        for (var i = 0; i < entities.Count; i++)
        {
            var entity = entities[i];

            if (i % 1000 == 0)
            {
                Plugin.Logger.LogInfo($"# {i + 1}/{entities.Count}");
            }

            if (!world.EntityManager.TryGetComponentData<PrefabGUID>(entity, out var prefabGuid)) continue;
            if (!world.PrefabNames.ContainsKey(prefabGuid.GuidHash)) continue;

            if (!_cache.ContainsKey(prefabGuid.GuidHash)) _cache[prefabGuid.GuidHash] = new Dictionary<string, object>();
            var componentData = GetAllComponentData(prefabGuid, entity);

            if (componentData.Count <= 2) continue;
            foreach (var (key, value) in componentData)
            {
                if (value == null) continue;
                _cache[prefabGuid.GuidHash][key] = value;
            }
        }

        Plugin.Logger.LogInfo($"Adding prefabs");
        foreach (var hash in world.PrefabNames.Keys)
        {
            if (!_cache.ContainsKey(hash)) _cache[hash] = new Dictionary<string, object>();
            var componentData = GetAllComponentData(new PrefabGUID(hash));
            if (componentData.Count == 0) continue;
            foreach (var (key, value) in componentData)
            {
                _cache[hash][key] = value;
            }
        }

        var json = JsonHelper.Serialize(_cache.Values.Where(v => v.Count > 2));
        File.WriteAllText("Output.json", json);
    }


    private Dictionary<string, object> GetAllComponentData(PrefabGUID prefabGuid, Entity? entity = null)
    {
        var result = new Dictionary<string, object>
        {
            { "PrefabGuid", prefabGuid },
        };

        if (world.PrefabNames.TryGetValue(prefabGuid.GuidHash, out var prefabName))
        {
            result.Add("PrefabName", prefabName);
        }

        AddManagedAllData(result, prefabGuid);
        if (entity != null) AddAllComponents(result, entity.Value);

        result = result.Where(r => r.Value is not null).ToDictionary(r => r.Key, r => r.Value);
        return result;
    }

    private void AddManagedAllData(Dictionary<string, object> result, PrefabGUID prefabGuid)
    {
        AddManagedData<ManagedItemData>(result, prefabGuid);
        AddManagedData<ManagedBlueprintData>(result, prefabGuid);
        AddManagedData<ManagedAbilityGroupData>(result, prefabGuid);
        AddManagedData<ManagedPerkData>(result, prefabGuid);
        AddManagedData<ManagedCharacterHUD>(result, prefabGuid);
        AddManagedData<ManagedStationBonusData>(result, prefabGuid);
        AddManagedData<ManagedDescriptionData>(result, prefabGuid);
        AddManagedData<ManagedBuildMenuGroupData>(result, prefabGuid);
        AddManagedData<ManagedBuildMenuCompositionData>(result, prefabGuid);
        AddManagedData<ManagedBuildMenuCategoryData>(result, prefabGuid);
        AddManagedData<ManagedBuildMenuTagData>(result, prefabGuid);
        AddManagedData<ManagedJournalTooltip>(result, prefabGuid);
        AddManagedData<ManagedMissionData>(result, prefabGuid);
        AddManagedData<ManagedAchievementData>(result, prefabGuid);
        AddManagedData<ManagedDataDropGroup>(result, prefabGuid);
        AddManagedData<ManagedMissionInjureDataAsset>(result, prefabGuid);
        AddManagedData<ManagedUnitBloodTypeData>(result, prefabGuid);
        AddManagedData<VBloodPortraitData>(result, prefabGuid);
        AddManagedData<ServantPortraitData>(result, prefabGuid);
        AddManagedData<AbilityTooltipData>(result, prefabGuid);
        AddManagedData<ManagedTechData>(result, prefabGuid);
        AddManagedData<ManagedSpellSchoolData>(result, prefabGuid);
    }

    private void AddAllComponents(Dictionary<string, object> result, Entity entity)
    {
        AddComponent<AbilityGroupState>(result, entity);
        AddComponent<AbilityGroupConsumeItemOnCast>(result, entity);
        AddComponent<AbilityChargesData>(result, entity);
        AddComponent<AbilityCooldownData>(result, entity);
        AddComponent<AbilityCastTimeData>(result, entity);
        AddBuffer<AbilityCastCondition>(result, entity);
        AddBuffer<AbilityGroupStartAbilitiesBuffer>(result, entity);
        AddComponent<VBloodAbilityData>(result, entity);
        AddComponent<VBloodShapeshiftData>(result, entity);
        AddBuffer<UnitBloodTypeBuffs>(result, entity);
        AddComponent<BlueprintData>(result, entity);
        AddBuffer<BlueprintRequirementBuffer>(result, entity);
        AddComponent<Health>(result, entity);
        AddComponent<Buff>(result, entity);
        AddComponent<Description>(result, entity);
        AddComponent<BuffCategory>(result, entity);
        AddBuffer<ReplaceAbilityOnSlotBuff>(result, entity);
        AddComponent<ItemData>(result, entity);
        AddComponent<CastAbilityOnConsume>(result, entity);
        AddComponent<VBloodItemSource>(result, entity);
        AddComponent<EquippableData>(result, entity);
        AddComponent<ArmorLevelSource>(result, entity);
        AddComponent<WeaponLevelSource>(result, entity);
        AddComponent<SpellLevelSource>(result, entity);
        AddComponent<Salvageable>(result, entity);
        AddBuffer<RecipeRequirementBuffer>(result, entity);
        AddComponent<Durability>(result, entity);
        AddBuffer<ModifyUnitStatBuff_DOTS>(result, entity);
        AddComponent<CharacterHUD>(result, entity);
        AddComponent<EntityCategory>(result, entity);
        AddComponent<UnitLevel>(result, entity);
        AddComponent<BloodConsumeSource>(result, entity);
        AddComponent<VBloodUnit>(result, entity);
        AddComponent<UnitStats>(result, entity);
        AddComponent<ResistanceData>(result, entity);
        AddComponent<ServantData>(result, entity);
        AddComponent<FactionReference>(result, entity);
        AddBuffer<AbilityGroupSlotBuffer>(result, entity);
        AddBuffer<VBloodUnlockTechBuffer>(result, entity);
        AddComponent<AchievementData>(result, entity);
        AddBuffer<AchievementSubTaskEntry>(result, entity);
        AddComponent<AchievementSubTaskData>(result, entity);
        AddComponent<RecipeData>(result, entity);
        AddBuffer<ItemRepairBuffer>(result, entity);
        AddBuffer<RecipeRequirementBuffer>(result, entity);
        AddBuffer<RecipeOutputBuffer>(result, entity);
        AddBuffer<RecipeOutputUnitBuffer>(result, entity);
        AddComponent<MissionData>(result, entity);
        AddBuffer<PerksBuffer>(result, entity);
        AddComponent<ResearchStation>(result, entity);
        AddComponent<StationServants>(result, entity);
        AddComponent<CastleWorkstation>(result, entity);
        AddBuffer<StationBonusBuffer>(result, entity);
        AddBuffer<ResearchBuffer>(result, entity);
        AddComponent<Workstation>(result, entity);
        AddComponent<Refinementstation>(result, entity);
        AddComponent<UnitSpawnerstation>(result, entity);
        AddBuffer<StationBonusBuffer>(result, entity);
        AddBuffer<WorkstationRecipesBuffer>(result, entity);
        AddBuffer<RefinementstationRecipesBuffer>(result, entity);
        AddBuffer<LocalizedStringBuilderParameter>(result, entity);
        AddComponent<VBloodItemSource>(result, entity);
        AddBuffer<SpellModArithmeticModifiable>(result, entity);
        AddComponent<SpellModAbilityGroupCharges>(result, entity);
        AddComponent<AbilityChargesData>(result, entity);
        AddComponent<AbilityChargesState>(result, entity);
        AddBuffer<TechItemRequirementBuffer>(result, entity);
        //AddComponent<Translation>(result, entity);
        //AddComponent<LocalToWorld>(result, entity);
    }

    private void AddManagedData<T>(Dictionary<string, object> result, PrefabGUID prefabGuid) where T : class
    {
        result[typeof(T).Name] = world.GameDataSystem.ManagedDataRegistry.GetOrDefaultWithoutLogging<T>(prefabGuid);
    }

    private void AddComponent<T>(Dictionary<string, object> result, Entity entity)
    {
        if (world.EntityManager.TryGetComponentData<T>(entity, out var value))
        {
            result[typeof(T).Name] = value;
        }
    }

    private void AddBuffer<T>(Dictionary<string, object> result, Entity entity) where T : struct
    {
        if (!world.EntityManager.HasBuffer<T>(entity)) return;
        var buffer = GetBufferInternal<T>(entity);
        if (buffer != null)
        {
            result[typeof(T).Name] = buffer;
        }
    }

    private List<T> GetBufferInternal<T>(Entity entity) where T : struct
    {
        try
        {
            if (!world.EntityManager.TryGetBuffer<T>(entity, out var buffer)) return null;
            if (buffer.Length == 0) return null;
            var results = new List<T>();
            foreach (var item in buffer)
            {
                results.Add(item);
            }

            return results;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
*/