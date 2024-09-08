using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;

namespace InfoLoom.Systems
{
    [CompilerGenerated]
    public partial class IndustrialDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private IndustrialDemandSystem m_IndustrialDemandSystem;
        private RawValueBinding m_uiIndustrialDemand;
        private NativeArray<float> m_IndustrialData;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_IndustrialDemandSystem = World.GetOrCreateSystemManaged<IndustrialDemandSystem>();

            AddBinding(m_uiIndustrialDemand = new RawValueBinding("cityInfo", "ilIndustrial", UpdateIndustrialDemand));

            m_IndustrialData = new NativeArray<float>(10, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem.frameIndex % 128 != 110)
                return;

            base.OnUpdate();

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

            m_uiIndustrialDemand.Update();
        }

        private void UpdateIndustrialDemand(IJsonWriter writer)
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
        }

        protected override void OnDestroy()
        {
            m_IndustrialData.Dispose();
            base.OnDestroy();
        }
    }
}
