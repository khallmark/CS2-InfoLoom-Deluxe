using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;

namespace InfoLoom.Systems
{
    [CompilerGenerated]
    public partial class BuildingDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private BuildingDemandSystem m_BuildingDemandSystem;
        private RawValueBinding m_uiBuildingDemand;
        private NativeArray<float> m_BuildingDemand;

        private const string LOG_TAG = "[InfoLoom] BuildingDemandUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_BuildingDemandSystem = World.GetOrCreateSystemManaged<BuildingDemandSystem>();

                AddBinding(m_uiBuildingDemand = new RawValueBinding("cityInfo", "ilBuildingDemand", UpdateBuildingDemand));

                m_BuildingDemand = new NativeArray<float>(7, Allocator.Persistent);
                
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
                if (m_SimulationSystem == null || m_SimulationSystem.frameIndex % 128 != 77)
                    return;

                base.OnUpdate();

                UpdateBuildingDemandData();

                m_uiBuildingDemand.Update();

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
                if (m_BuildingDemandSystem == null)
                {
                    Mod.Log.Warning($"{LOG_TAG}BuildingDemandSystem is null");
                    return;
                }

                m_BuildingDemand[0] = m_BuildingDemandSystem.residentialLowDemand;
                m_BuildingDemand[1] = m_BuildingDemandSystem.residentialMediumDemand;
                m_BuildingDemand[2] = m_BuildingDemandSystem.residentialHighDemand;
                m_BuildingDemand[3] = m_BuildingDemandSystem.commercialDemand;
                m_BuildingDemand[4] = m_BuildingDemandSystem.industrialDemand;
                m_BuildingDemand[5] = m_BuildingDemandSystem.officeDemand;
                m_BuildingDemand[6] = m_BuildingDemandSystem.storageDemand;

                Mod.Log.Debug($"{LOG_TAG}Building demand data updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Warning($"{LOG_TAG}Error updating building demand data: {e.Message}");
            }
        }

        private void UpdateBuildingDemand(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(m_BuildingDemand.Length);
                for (int i = 0; i < m_BuildingDemand.Length; i++)
                    writer.Write(m_BuildingDemand[i]);
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
                if (m_BuildingDemand.IsCreated)
                    m_BuildingDemand.Dispose();
                
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
