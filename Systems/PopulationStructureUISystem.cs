using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace InfoLoom.Systems
{
    [CompilerGenerated]
    public partial class PopulationStructureUISystem : UISystemBase
    {
        private struct PopulationAtAgeInfo
        {
            public int Age;
            public int Total;
            public int School1;
            public int School2;
            public int School3;
            public int School4;
            public int Work;
            public int Other;
            public PopulationAtAgeInfo(int _age) { Age = _age; Total = 0; School1 = 0; School2 = 0; School3 = 0; School4 = 0; Work = 0; Other = 0; }
        }

        private SimulationSystem m_SimulationSystem;
        private RawValueBinding m_uiTotals;
        private RawValueBinding m_uiResults;
        private EntityQuery m_TimeDataQuery;
        private EntityQuery m_CitizenQuery;
        private NativeArray<int> m_Totals;
        private NativeArray<PopulationAtAgeInfo> m_Results;

        private const string LOG_TAG = "[InfoLoom] PopulationStructureUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_TimeDataQuery = GetEntityQuery(ComponentType.ReadOnly<TimeData>());
                m_CitizenQuery = GetEntityQuery(new EntityQueryDesc
                {
                    All = new ComponentType[1] { ComponentType.ReadOnly<Citizen>() },
                    None = new ComponentType[2] { ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Temp>() }
                });

                AddBinding(m_uiTotals = new RawValueBinding("populationInfo", "structureTotals", UpdateTotals));
                AddBinding(m_uiResults = new RawValueBinding("populationInfo", "structureDetails", UpdateResults));

                m_Totals = new NativeArray<int>(10, Allocator.Persistent);
                m_Results = new NativeArray<PopulationAtAgeInfo>(110, Allocator.Persistent);

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
                if (m_SimulationSystem == null || m_SimulationSystem.frameIndex % 128 != 44)
                    return;

                base.OnUpdate();
                ResetResults();

                // Implement population structure calculation here
                // This part needs to be implemented based on your specific requirements

                m_uiTotals.Update();
                m_uiResults.Update();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Population structure updated successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnUpdate: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void UpdateTotals(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(m_Totals.Length);
                for (int i = 0; i < m_Totals.Length; i++)
                    writer.Write(m_Totals[i]);
                writer.ArrayEnd();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Population totals written to JSON successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error writing population totals to JSON: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void UpdateResults(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(m_Results.Length);
                for (int i = 0; i < m_Results.Length; i++)
                {
                    WriteData(writer, m_Results[i]);
                }
                writer.ArrayEnd();

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Population structure details written to JSON successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error writing population structure details to JSON: {e.Message}");
                Debug.LogException(e);
            }
        }

        private void WriteData(IJsonWriter writer, PopulationAtAgeInfo info)
        {
            writer.TypeBegin("populationAtAgeInfo");
            writer.PropertyName("age");
            writer.Write(info.Age);
            writer.PropertyName("total");
            writer.Write(info.Total);
            writer.PropertyName("school1");
            writer.Write(info.School1);
            writer.PropertyName("school2");
            writer.Write(info.School2);
            writer.PropertyName("school3");
            writer.Write(info.School3);
            writer.PropertyName("school4");
            writer.Write(info.School4);
            writer.PropertyName("work");
            writer.Write(info.Work);
            writer.PropertyName("other");
            writer.Write(info.Other);
            writer.TypeEnd();
        }

        private void ResetResults()
        {
            for (int i = 0; i < m_Totals.Length; i++)
            {
                m_Totals[i] = 0;
            }
            for (int i = 0; i < m_Results.Length; i++)
            {
                m_Results[i] = new PopulationAtAgeInfo(i);
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (m_Totals.IsCreated)
                    m_Totals.Dispose();
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
