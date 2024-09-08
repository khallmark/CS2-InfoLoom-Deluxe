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
    public partial class ResidentialDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private ResidentialDemandSystem m_ResidentialDemandSystem;
        private RawValueBinding m_uiResidentialDemand;
        private NativeArray<float> m_ResidentialDemand;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_ResidentialDemandSystem = World.GetOrCreateSystemManaged<ResidentialDemandSystem>();

            AddBinding(m_uiResidentialDemand = new RawValueBinding("cityInfo", "ilResidential", UpdateResidentialDemand));

            m_ResidentialDemand = new NativeArray<float>(3, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem.frameIndex % 128 != 88)
                return;

            base.OnUpdate();

            m_ResidentialDemand[0] = m_ResidentialDemandSystem.residentialLowDemand;
            m_ResidentialDemand[1] = m_ResidentialDemandSystem.residentialMediumDemand;
            m_ResidentialDemand[2] = m_ResidentialDemandSystem.residentialHighDemand;

            m_uiResidentialDemand.Update();
        }

        private void UpdateResidentialDemand(IJsonWriter writer)
        {
            writer.TypeBegin("ResidentialDemand");
            writer.PropertyName("lowDemand");
            writer.Write(m_ResidentialDemand[0]);
            writer.PropertyName("mediumDemand");
            writer.Write(m_ResidentialDemand[1]);
            writer.PropertyName("highDemand");
            writer.Write(m_ResidentialDemand[2]);
            writer.TypeEnd();
        }

        protected override void OnDestroy()
        {
            m_ResidentialDemand.Dispose();
            base.OnDestroy();
        }
    }
}
