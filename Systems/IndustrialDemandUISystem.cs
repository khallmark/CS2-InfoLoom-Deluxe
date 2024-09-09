using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace InfoLoom.Systems
{
    [CompilerGenerated]
    public partial class IndustrialDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private IndustrialDemandSystem m_IndustrialDemandSystem;
        private RawValueBinding m_uiIndustrialDemand;
        private NativeArray<float> m_IndustrialData;

        private const string LOG_TAG = "[InfoLoom] IndustrialDemandUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_IndustrialDemandSystem = World.GetOrCreateSystemManaged<IndustrialDemandSystem>();

                AddBinding(m_uiIndustrialDemand = new RawValueBinding("cityInfo", "ilIndustrial", UpdateIndustrialDemand));

                m_IndustrialData = new NativeArray<float>(10, Allocator.Persistent);
                
                Debug.Log($"{LOG_TAG}System created and initialized successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnCreate: {e.Message}");
                Debug.LogException(e);
            }
        }

        protected override void OnUpdate()
        {
            try
            {
                if (m_SimulationSystem == null || m_SimulationSystem.frameIndex % 128 != 110)
                    return;

                base.OnUpdate();

                UpdateIndustrialData();

                m_uiIndustrialDemand.Update();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Updated industrial demand data. Demand: {m_IndustrialData[0]}");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnUpdate: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void UpdateIndustrialData()
        {
            try
            {
                if (m_IndustrialDemandSystem == null)
                {
                    Debug.LogWarning($"{LOG_TAG}IndustrialDemandSystem is null");
                    return;
                }

                m_IndustrialData[0] = m_IndustrialDemandSystem.industrialDemand;
                m_IndustrialData[1] = m_IndustrialDemandSystem.emptyBuildings[0];
                m_IndustrialData[2] = m_IndustrialDemandSystem.emptyBuildings[1];
                m_IndustrialData[3] = m_IndustrialDemandSystem.propertylessCompanies[0];
                m_IndustrialData[4] = m_IndustrialDemandSystem.propertylessCompanies[1];
                m_IndustrialData[5] = m_IndustrialDemandSystem.taxRates[0];
                m_IndustrialData[6] = m_IndustrialDemandSystem.taxRates[1];
                m_IndustrialData[7] = m_IndustrialDemandSystem.localDemand;
                m_IndustrialData[8] = m_IndustrialDemandSystem.inputUtilization;
                m_IndustrialData[9] = m_IndustrialDemandSystem.employeeCapacityRatio[0];

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Industrial data updated successfully");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{LOG_TAG}Error updating industrial data: {e.Message}");
            }
        }

        private void UpdateIndustrialDemand(IJsonWriter writer)
        {
            try
            {
                writer.TypeBegin("IndustrialDemand");
                writer.PropertyName("demand");
                writer.Write(m_IndustrialData[0]);
                writer.PropertyName("emptyBuildingsIndustrial");
                writer.Write(m_IndustrialData[1]);
                writer.PropertyName("emptyBuildingsOffice");
                writer.Write(m_IndustrialData[2]);
                writer.PropertyName("propertylessCompaniesIndustrial");
                writer.Write(m_IndustrialData[3]);
                writer.PropertyName("propertylessCompaniesOffice");
                writer.Write(m_IndustrialData[4]);
                writer.PropertyName("taxRateIndustrial");
                writer.Write(m_IndustrialData[5]);
                writer.PropertyName("taxRateOffice");
                writer.Write(m_IndustrialData[6]);
                writer.PropertyName("localDemand");
                writer.Write(m_IndustrialData[7]);
                writer.PropertyName("inputUtilization");
                writer.Write(m_IndustrialData[8]);
                writer.PropertyName("employeeCapacityRatio");
                writer.Write(m_IndustrialData[9]);
                writer.TypeEnd();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Industrial demand data written to JSON successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error writing industrial demand data to JSON: {e.Message}");
                Debug.LogException(e);
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (m_IndustrialData.IsCreated)
                    m_IndustrialData.Dispose();
                
                base.OnDestroy();
                Debug.Log($"{LOG_TAG}System destroyed and resources cleaned up successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnDestroy: {e.Message}");
                Debug.LogException(e);
            }
        }
    }
}
