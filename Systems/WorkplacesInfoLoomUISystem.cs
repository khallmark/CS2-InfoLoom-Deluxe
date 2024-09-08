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
    public partial class WorkplacesInfoLoomUISystem : UISystemBase
    {
        private struct WorkplacesAtLevelInfo
        {
            public int Level;
            public int Total;
            public int Service;
            public int Commercial;
            public int Leisure;
            public int Extractor;
            public int Industrial;
            public int Office;
            public int Employee;
            public int Open;
            public int Commuter;
            public WorkplacesAtLevelInfo(int _level) { Level = _level; }
        }

        private SimulationSystem m_SimulationSystem;
        private EntityQuery m_WorkplaceQuery;
        private RawValueBinding m_uiResults;
        private NativeArray<WorkplacesAtLevelInfo> m_Results;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_WorkplaceQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[3]
                {
                    ComponentType.ReadOnly<Employee>(),
                    ComponentType.ReadOnly<WorkProvider>(),
                    ComponentType.ReadOnly<PrefabRef>()
                },
                Any = new ComponentType[2]
                {
                    ComponentType.ReadOnly<PropertyRenter>(),
                    ComponentType.ReadOnly<Building>()
                },
                None = new ComponentType[1] { ComponentType.ReadOnly<Temp>() }
            });

            AddBinding(m_uiResults = new RawValueBinding("workplaces", "ilWorkplaces", UpdateResults));

            m_Results = new NativeArray<WorkplacesAtLevelInfo>(7, Allocator.Persistent);
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem.frameIndex % 128 != 22)
                return;

            ResetResults();

            // Implement workplace calculation here

            m_uiResults.Update();
        }

        private void UpdateResults(IJsonWriter writer)
        {
            writer.ArrayBegin(m_Results.Length);
            for (int i = 0; i < m_Results.Length; i++)
                WriteData(writer, m_Results[i]);
            writer.ArrayEnd();
        }

        private void WriteData(IJsonWriter writer, WorkplacesAtLevelInfo info)
        {
            writer.TypeBegin("workplacesAtLevelInfo");
            writer.PropertyName("level");
            writer.Write(info.Level);
            writer.PropertyName("total");
            writer.Write(info.Total);
            writer.PropertyName("service");
            writer.Write(info.Service);
            writer.PropertyName("commercial");
            writer.Write(info.Commercial);
            writer.PropertyName("leisure");
            writer.Write(info.Leisure);
            writer.PropertyName("extractor");
            writer.Write(info.Extractor);
            writer.PropertyName("industry");
            writer.Write(info.Industrial);
            writer.PropertyName("office");
            writer.Write(info.Office);
            writer.PropertyName("employee");
            writer.Write(info.Employee);
            writer.PropertyName("open");
            writer.Write(info.Open);
            writer.PropertyName("commuter");
            writer.Write(info.Commuter);
            writer.TypeEnd();
        }

        private void ResetResults()
        {
            for (int i = 0; i < 7; i++)
            {
                m_Results[i] = new WorkplacesAtLevelInfo(i);
            }
        }

        protected override void OnDestroy()
        {
            m_Results.Dispose();
            base.OnDestroy();
        }
    }
}
