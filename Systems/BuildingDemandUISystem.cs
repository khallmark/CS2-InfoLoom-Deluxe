using System;
using System.Runtime.CompilerServices;
using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using Unity.Collections;
using Unity.Entities;

namespace InfoLoom_Deluxe.Systems
{
    [CompilerGenerated]
    public partial class BuildingDemandUISystem : UISystemBase
    {
        private SimulationSystem simulationSystem;
        private ResidentialDemandSystem residentialDemandSystem;
        private CommercialDemandSystem commercialDemandSystem;
        private IndustrialDemandSystem industrialDemandSystem;
        private RawValueBinding uiBuildingDemand;
        private NativeArray<int> buildingDemand;

        private const string LOG_TAG = "[InfoLoom] BuildingDemandUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                residentialDemandSystem = World.GetOrCreateSystemManaged<ResidentialDemandSystem>();
                commercialDemandSystem = World.GetOrCreateSystemManaged<CommercialDemandSystem>();
                industrialDemandSystem = World.GetOrCreateSystemManaged<IndustrialDemandSystem>();

                AddBinding(uiBuildingDemand = new RawValueBinding("cityInfo", "ilBuildingDemand", UpdateBuildingDemand));

                buildingDemand = new NativeArray<int>(7, Allocator.Persistent);

                Mod.Log.Info($"{LOG_TAG}System created and initialized successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnCreate: {e.Message}");
            }
        }

        protected override void OnUpdate()
        {
            try
            {
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 77)
                    return;

                base.OnUpdate();

                UpdateBuildingDemandData();

                uiBuildingDemand.Update();

                Mod.Log.Debug($"{LOG_TAG}Updated building demand data");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateBuildingDemandData()
        {
            try
            {
                if (residentialDemandSystem == null || commercialDemandSystem == null || industrialDemandSystem == null)
                {
                    Mod.Log.Warn($"{LOG_TAG}One or more demand systems are null");
                    return;
                }

                buildingDemand[0] = residentialDemandSystem.buildingDemand.z; // low res
                buildingDemand[1] = residentialDemandSystem.buildingDemand.y; // med res
                buildingDemand[2] = residentialDemandSystem.buildingDemand.x; // high res
                buildingDemand[3] = commercialDemandSystem.buildingDemand; // commercial
                buildingDemand[4] = industrialDemandSystem.industrialBuildingDemand; // industry
                buildingDemand[5] = industrialDemandSystem.storageBuildingDemand; // storage
                buildingDemand[6] = industrialDemandSystem.officeBuildingDemand; // office

                Mod.Log.Debug($"{LOG_TAG}Building demand data updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Warn($"{LOG_TAG}Error updating building demand data: {e.Message}");
            }
        }

        private void UpdateBuildingDemand(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(buildingDemand.Length);
                for (int i = 0; i < buildingDemand.Length; i++)
                    writer.Write(buildingDemand[i]);
                writer.ArrayEnd();

                Mod.Log.Debug($"{LOG_TAG}Building demand data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing building demand data to JSON: {e.Message}");
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (buildingDemand.IsCreated)
                    buildingDemand.Dispose();

                base.OnDestroy();
                Mod.Log.Info($"{LOG_TAG}System destroyed and resources cleaned up successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnDestroy: {e.Message}");
            }
        }
    }
}
