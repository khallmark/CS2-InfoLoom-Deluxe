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
    public partial class WorkforceInfoLoomUISystem : UISystemBase
    {
        private struct WorkforceAtLevelInfo
        {
            public int Level;
            public int Total;
            public int Worker;
            public int Unemployed;
            public int Homeless;
            public int Employable;
            public int Outside;
            public int Under;
            public WorkforceAtLevelInfo(int _level) { Level = _level; }
        }

        private SimulationSystem m_SimulationSystem;
        private EntityQuery m_AllAdultGroup;
        private RawValueBinding m_uiResults;
        private NativeArray<WorkforceAtLevelInfo> m_Results;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_AllAdultGroup = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[1] { ComponentType.ReadOnly<Citizen>() },
                None = new ComponentType[3]
                {
                    ComponentType.ReadOnly<Game.Citizens.Student>(),
                    ComponentType.ReadOnly<Deleted>(),
                    ComponentType.ReadOnly<Temp>()
                }
            });

            AddBinding(m_uiResults = new RawValueBinding("populationInfo", "ilWorkforce", UpdateResults));

            m_Results = new NativeArray<WorkforceAtLevelInfo>(6, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem.frameIndex % 128 != 33)
                return;

            ResetResults();

            // Implement workforce calculation here

            m_uiResults.Update();
        }

        private void UpdateResults(IJsonWriter writer)
        {
            writer.ArrayBegin(m_Results.Length);
            for (int i = 0; i < m_Results.Length; i++)
                WriteData(writer, m_Results[i]);
            writer.ArrayEnd();
        }

        private void WriteData(IJsonWriter writer, WorkforceAtLevelInfo info)
        {
            writer.TypeBegin("WorkforceAtLevelInfo");
            writer.PropertyName("level");
            writer.Write(info.Level);
            writer.PropertyName("total");
            writer.Write(info.Total);
            writer.PropertyName("worker");
            writer.Write(info.Worker);
            writer.PropertyName("unemployed");
            writer.Write(info.Unemployed);
            writer.PropertyName("homeless");
            writer.Write(info.Homeless);
            writer.PropertyName("employable");
            writer.Write(info.Employable);
            writer.PropertyName("outside");
            writer.Write(info.Outside);
            writer.PropertyName("under");
            writer.Write(info.Under);
            writer.TypeEnd();
        }

        private void ResetResults()
        {
            for (int i = 0; i < 6; i++)
            {
                m_Results[i] = new WorkforceAtLevelInfo(i);
            }
        }

        protected override void OnDestroy()
        {
            m_Results.Dispose();
            base.OnDestroy();
        }
    }
}
