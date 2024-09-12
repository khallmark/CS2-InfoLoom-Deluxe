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
    public partial class IndustrialDemandUISystem : UISystemBase
    {
        private SimulationSystem simulationSystem;
        private IndustrialDemandSystem industrialDemandSystem;
        private RawValueBinding uiIndustrialDemand;
        private NativeArray<float> industrialData;

        private const string LOG_TAG = "[InfoLoom] IndustrialDemandUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                industrialDemandSystem = World.GetOrCreateSystemManaged<IndustrialDemandSystem>();

                AddBinding(uiIndustrialDemand = new RawValueBinding("cityInfo", "ilIndustrial", UpdateIndustrialDemand));

                industrialData = new NativeArray<float>(10, Allocator.Persistent);

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
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 110)
                    return;

                base.OnUpdate();

                UpdateIndustrialData();

                uiIndustrialDemand.Update();

                Mod.Log.Debug($"{LOG_TAG}Updated industrial demand data. Demand: {industrialData[0]}");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateIndustrialData()
        {
            try
            {
                if (industrialDemandSystem == null)
                {
                    Mod.Log.Warning($"{LOG_TAG}IndustrialDemandSystem is null");
                    return;
                }

                industrialData[0] = industrialDemandSystem.industrialDemand;
                industrialData[1] = industrialDemandSystem.emptyBuildings[0];
                industrialData[2] = industrialDemandSystem.emptyBuildings[1];
                industrialData[3] = industrialDemandSystem.propertylessCompanies[0];
                industrialData[4] = industrialDemandSystem.propertylessCompanies[1];
                industrialData[5] = industrialDemandSystem.taxRates[0];
                industrialData[6] = industrialDemandSystem.taxRates[1];
                industrialData[7] = industrialDemandSystem.localDemand;
                industrialData[8] = industrialDemandSystem.inputUtilization;
                industrialData[9] = industrialDemandSystem.employeeCapacityRatio[0];

                Mod.Log.Debug($"{LOG_TAG}Industrial data updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Warning($"{LOG_TAG}Error updating industrial data: {e.Message}");
            }
        }

        private void UpdateIndustrialDemand(IJsonWriter writer)
        {
            try
            {
                writer.TypeBegin("IndustrialDemand");
                writer.PropertyName("demand");
                writer.Write(industrialData[0]);
                writer.PropertyName("emptyBuildingsIndustrial");
                writer.Write(industrialData[1]);
                writer.PropertyName("emptyBuildingsOffice");
                writer.Write(industrialData[2]);
                writer.PropertyName("propertylessCompaniesIndustrial");
                writer.Write(industrialData[3]);
                writer.PropertyName("propertylessCompaniesOffice");
                writer.Write(industrialData[4]);
                writer.PropertyName("taxRateIndustrial");
                writer.Write(industrialData[5]);
                writer.PropertyName("taxRateOffice");
                writer.Write(industrialData[6]);
                writer.PropertyName("localDemand");
                writer.Write(industrialData[7]);
                writer.PropertyName("inputUtilization");
                writer.Write(industrialData[8]);
                writer.PropertyName("employeeCapacityRatio");
                writer.Write(industrialData[9]);
                writer.TypeEnd();

                Mod.Log.Debug($"{LOG_TAG}Industrial demand data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing industrial demand data to JSON: {e.Message}");
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (industrialData.IsCreated)
                    industrialData.Dispose();

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
