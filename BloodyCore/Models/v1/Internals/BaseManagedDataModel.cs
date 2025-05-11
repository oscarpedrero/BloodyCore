using System.Collections.Generic;
using Unity.Entities;

namespace Bloody.Core.Models.v1.Internals
{
    public class BaseManagedDataModel
    {
        private readonly World _world;
        private readonly BaseEntityModel _entityModel;

        internal BaseManagedDataModel(World world, BaseEntityModel entityModel)
        {
            _world = world;
            _entityModel = entityModel;
        }

        public ProjectM.ManagedBlueprintData ManagedBlueprintData => _world.GetManagedComponentDataInternal<ProjectM.ManagedBlueprintData>(_entityModel);
        public ProjectM.ManagedAbilityGroupData ManagedAbilityGroupData => _world.GetManagedComponentDataInternal<ProjectM.ManagedAbilityGroupData>(_entityModel);
        public ProjectM.ManagedDescriptionData ManagedDescriptionData => _world.GetManagedComponentDataInternal<ProjectM.ManagedDescriptionData>(_entityModel);
        public VBloodPortraitData VBloodPortraitData => _world.GetManagedComponentDataInternal<VBloodPortraitData>(_entityModel);
        public ProjectM.ManagedCharacterHUD ManagedCharacterHUD => _world.GetManagedComponentDataInternal<ProjectM.ManagedCharacterHUD>(_entityModel);
        public ProjectM.Portrait.ServantPortraitData ServantPortraitData => _world.GetManagedComponentDataInternal<ProjectM.Portrait.ServantPortraitData>(_entityModel);
        public ProjectM.ManagedItemData ManagedItemData => _world.GetManagedComponentDataInternal<ProjectM.ManagedItemData>(_entityModel);
        public ProjectM.ManagedDataDropGroup ManagedDataDropGroup => _world.GetManagedComponentDataInternal<ProjectM.ManagedDataDropGroup>(_entityModel);
        //public ProjectM.UI.ManagedUnitBloodTypeData ManagedUnitBloodTypeData => _world.GetManagedComponentDataInternal<ProjectM.UI.ManagedUnitBloodTypeData>(_entityModel);
        public ProjectM.UI.ManagedSCTTypeData ManagedSCTTypeData => _world.GetManagedComponentDataInternal<ProjectM.UI.ManagedSCTTypeData>(_entityModel);
        public ProjectM.ManagedJournalTooltip ManagedJournalTooltip => _world.GetManagedComponentDataInternal<ProjectM.ManagedJournalTooltip>(_entityModel);
        public ProjectM.FeatureCollectionComponent FeatureCollectionComponent => _world.GetManagedComponentDataInternal<ProjectM.FeatureCollectionComponent>(_entityModel);
        public ProjectM.ManagedMissionData ManagedMissionData => _world.GetManagedComponentDataInternal<ProjectM.ManagedMissionData>(_entityModel);
        public ProjectM.UI.ManagedBuildMenuGroupData ManagedBuildMenuGroupData => _world.GetManagedComponentDataInternal<ProjectM.UI.ManagedBuildMenuGroupData>(_entityModel);
        public ProjectM.ScreenEdgeIconDataComponent ScreenEdgeIconDataComponent => _world.GetManagedComponentDataInternal<ProjectM.ScreenEdgeIconDataComponent>(_entityModel);
        public ProjectM.ManagedPerkData ManagedPerkData => _world.GetManagedComponentDataInternal<ProjectM.ManagedPerkData>(_entityModel);
        public ProjectM.ManagedStationBonusData ManagedStationBonusData => _world.GetManagedComponentDataInternal<ProjectM.ManagedStationBonusData>(_entityModel);
        public ProjectM.ManagedMissionInjureDataAsset ManagedMissionInjureDataAsset => _world.GetManagedComponentDataInternal<ProjectM.ManagedMissionInjureDataAsset>(_entityModel);
        public ProjectM.ManagedAchievementData ManagedAchievementData => _world.GetManagedComponentDataInternal<ProjectM.ManagedAchievementData>(_entityModel);
        public ProjectM.ManagedMapIconData ManagedMapIconData => _world.GetManagedComponentDataInternal<ProjectM.ManagedMapIconData>(_entityModel);
        public ProjectM.ManagedTechData ManagedTechData => _world.GetManagedComponentDataInternal<ProjectM.ManagedTechData>(_entityModel);
        public ProjectM.UI.ManagedBuildMenuCompositionData ManagedBuildMenuCompositionData => _world.GetManagedComponentDataInternal<ProjectM.UI.ManagedBuildMenuCompositionData>(_entityModel);
    }
}