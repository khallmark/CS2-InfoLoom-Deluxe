using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace InfoLoom.Systems
{
    [CompilerGenerated]
    public partial class BuildingDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private ResidentialDemandSystem m_ResidentialDemandSystem;
        private CommercialDemandSystem m_CommercialDemandSystem;
        private IndustrialDemandSystem m_IndustrialDemandSystem;
        private RawValueBinding m_uiBuildingDemand;
        private NativeArray<int> m_BuildingDemand;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_ResidentialDemandSystem = World.GetOrCreateSystemManaged<ResidentialDemandSystem>();
            m_CommercialDemandSystem = World.GetOrCreateSystemManaged<CommercialDemandSystem>();
            m_IndustrialDemandSystem = World.GetOrCreateSystemManaged<IndustrialDemandSystem>();

            if (m_SimulationSystem == null || m_ResidentialDemandSystem == null || m_CommercialDemandSystem == null || m_IndustrialDemandSystem == null)
            {
                Debug.LogError("One or more demand systems could not be initialized.");
                return;
            }

            AddBinding(m_uiBuildingDemand = new RawValueBinding("cityInfo", "ilBuildingDemand", UpdateBuildingDemand));

            m_BuildingDemand = new NativeArray<int>(7, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem == null || m_ResidentialDemandSystem == null || m_CommercialDemandSystem == null || m_IndustrialDemandSystem == null)
            {
                Debug.LogError("One or more demand systems are null during update.");
                return;
            }

            if (m_SimulationSystem.frameIndex % 128 != 77)
                return;

            base.OnUpdate();

            try
            {
                m_BuildingDemand[0] = m_ResidentialDemandSystem.buildingDemand.z;
                m_BuildingDemand[1] = m_ResidentialDemandSystem.buildingDemand.y;
                m_BuildingDemand[2] = m_ResidentialDemandSystem.buildingDemand.x;
                m_BuildingDemand[3] = m_CommercialDemandSystem.buildingDemand;
                m_BuildingDemand[4] = m_IndustrialDemandSystem.industrialBuildingDemand;
                m_BuildingDemand[5] = m_IndustrialDemandSystem.storageBuildingDemand;
                m_BuildingDemand[6] = m_IndustrialDemandSystem.officeBuildingDemand;

                m_uiBuildingDemand.Update();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error updating building demand: {ex.Message}");
            }
        }

        private void UpdateBuildingDemand(IJsonWriter writer)
        {
            writer.ArrayBegin(m_BuildingDemand.Length);
            for (int i = 0; i < m_BuildingDemand.Length; i++)
                writer.Write(m_BuildingDemand[i]);
            writer.ArrayEnd();
        }

        protected override void OnDestroy()
        {
            if (m_BuildingDemand.IsCreated)
            {
                m_BuildingDemand.Dispose();
            }
            base.OnDestroy();
        }
    }
}
