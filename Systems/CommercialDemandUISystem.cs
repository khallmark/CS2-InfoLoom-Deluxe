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
    public partial class CommercialDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private CommercialDemandSystem m_CommercialDemandSystem;
        private RawValueBinding m_uiCommercialDemand;
        private NativeArray<float> m_CommercialData;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_CommercialDemandSystem = World.GetOrCreateSystemManaged<CommercialDemandSystem>();

            AddBinding(m_uiCommercialDemand = new RawValueBinding("cityInfo", "ilCommercial", UpdateCommercialDemand));

            m_CommercialData = new NativeArray<float>(7, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem.frameIndex % 128 != 99)
                return;

            base.OnUpdate();

            m_CommercialData[0] = m_CommercialDemandSystem.commercialDemand;
            m_CommercialData[1] = m_CommercialDemandSystem.emptyBuildings;
            m_CommercialData[2] = m_CommercialDemandSystem.propertylessCompanies;
            m_CommercialData[3] = m_CommercialDemandSystem.taxRate;
            m_CommercialData[4] = m_CommercialDemandSystem.serviceUtilization.standard;
            m_CommercialData[5] = m_CommercialDemandSystem.serviceUtilization.leisure;
            m_CommercialData[6] = m_CommercialDemandSystem.employeeCapacityRatio;

            m_uiCommercialDemand.Update();
        }

        private void UpdateCommercialDemand(IJsonWriter writer)
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
        }

        protected override void OnDestroy()
        {
            m_CommercialData.Dispose();
            base.OnDestroy();
        }
    }
}
