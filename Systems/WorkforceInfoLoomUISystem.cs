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
            public WorkforceAtLevelInfo(int _level) { Level = _level; Total = 0; Worker = 0; Unemployed = 0; Homeless = 0; Employable = 0; Outside = 0; Under = 0; }
        }

        private SimulationSystem m_SimulationSystem;
        private EntityQuery m_AllAdultGroup;
        private RawValueBinding m_uiResults;
        private NativeArray<WorkforceAtLevelInfo> m_Results;

        private const string LOG_TAG = "[InfoLoom] WorkforceInfoLoomUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
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
                if (m_SimulationSystem == null || m_SimulationSystem.frameIndex % 128 != 33)
                    return;

                ResetResults();

                // Implement workforce calculation here
                // This part needs to be implemented based on your specific requirements

                m_uiResults.Update();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Workforce information updated successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnUpdate: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void UpdateResults(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(m_Results.Length);
                for (int i = 0; i < m_Results.Length; i++)
                    WriteData(writer, m_Results[i]);
                writer.ArrayEnd();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Workforce data written to JSON successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error writing workforce data to JSON: {e.Message}");
                Debug.LogException(e);
            }
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
            try
            {
                if (m_Results.IsCreated)
                    m_Results.Dispose();
                
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
