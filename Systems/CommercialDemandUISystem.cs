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
    public partial class CommercialDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private CommercialDemandSystem m_CommercialDemandSystem;
        private RawValueBinding m_uiCommercialDemand;
        private NativeArray<float> m_CommercialData;

        private const string LOG_TAG = "[InfoLoom] CommercialDemandUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_CommercialDemandSystem = World.GetOrCreateSystemManaged<CommercialDemandSystem>();

                AddBinding(m_uiCommercialDemand = new RawValueBinding("cityInfo", "ilCommercial", UpdateCommercialDemand));

                m_CommercialData = new NativeArray<float>(7, Allocator.Persistent);
                
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
                if (m_SimulationSystem.frameIndex % 128 != 99)
                    return;

                base.OnUpdate();

                UpdateCommercialData();

                m_uiCommercialDemand.Update();

                Mod.Log.Debug($"{LOG_TAG}Updated commercial demand data. Demand: {m_CommercialData[0]}");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateCommercialData()
        {
            try
            {
                m_CommercialData[0] = m_CommercialDemandSystem.commercialDemand;
                m_CommercialData[1] = m_CommercialDemandSystem.emptyBuildings;
                m_CommercialData[2] = m_CommercialDemandSystem.propertylessCompanies;
                m_CommercialData[3] = m_CommercialDemandSystem.taxRate;
                m_CommercialData[4] = m_CommercialDemandSystem.serviceUtilization.standard;
                m_CommercialData[5] = m_CommercialDemandSystem.serviceUtilization.leisure;
                m_CommercialData[6] = m_CommercialDemandSystem.employeeCapacityRatio;

                Mod.Log.Debug($"{LOG_TAG}Commercial data updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Warning($"{LOG_TAG}Error updating commercial data: {e.Message}");
            }
        }

        private void UpdateCommercialDemand(IJsonWriter writer)
        {
            try
            {
                writer.TypeBegin("CommercialDemand");
                writer.PropertyName("demand");
                writer.Write(m_CommercialData[0]);
                writer.PropertyName("emptyBuildings");
                writer.Write(m_CommercialData[1]);
                writer.PropertyName("propertylessCompanies");
                writer.Write(m_CommercialData[2]);
                writer.PropertyName("taxRate");
                writer.Write(m_CommercialData[3]);
                writer.PropertyName("serviceUtilizationStandard");
                writer.Write(m_CommercialData[4]);
                writer.PropertyName("serviceUtilizationLeisure");
                writer.Write(m_CommercialData[5]);
                writer.PropertyName("employeeCapacityRatio");
                writer.Write(m_CommercialData[6]);
                writer.TypeEnd();

                Mod.Log.Debug($"{LOG_TAG}Commercial demand data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing commercial demand data to JSON: {e.Message}");
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (m_CommercialData.IsCreated)
                    m_CommercialData.Dispose();
                
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
