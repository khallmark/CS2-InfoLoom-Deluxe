using System;
using System.Runtime.CompilerServices;
using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using Unity.Collections;
using Unity.Entities;

namespace InfoLoom_Deluxe.Systems
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
            public PopulationAtAgeInfo(int age) { Age = age; Total = 0; School1 = 0; School2 = 0; School3 = 0; School4 = 0; Work = 0; Other = 0; }
        }

        private SimulationSystem simulationSystem;
        private RawValueBinding uiTotals;
        private RawValueBinding uiResults;
        private EntityQuery timeDataQuery;
        private EntityQuery citizenQuery;
        private NativeArray<int> totals;
        private NativeArray<PopulationAtAgeInfo> results;

        private const string LOG_TAG = "[InfoLoom] PopulationStructureUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                timeDataQuery = GetEntityQuery(ComponentType.ReadOnly<TimeData>());
                citizenQuery = GetEntityQuery(new EntityQueryDesc
                {
                    All = new ComponentType[1] { ComponentType.ReadOnly<Citizen>() },
                    None = new ComponentType[2] { ComponentType.ReadOnly<Deleted>(), ComponentType.ReadOnly<Temp>() }
                });

                AddBinding(uiTotals = new RawValueBinding("populationInfo", "structureTotals", UpdateTotals));
                AddBinding(uiResults = new RawValueBinding("populationInfo", "structureDetails", UpdateResults));

                totals = new NativeArray<int>(10, Allocator.Persistent);
                results = new NativeArray<PopulationAtAgeInfo>(110, Allocator.Persistent);

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
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 44)
                    return;

                base.OnUpdate();
                ResetResults();

                // Implement population structure calculation here
                // This part needs to be implemented based on your specific requirements

                uiTotals.Update();
                uiResults.Update();

                Mod.Log.Debug($"{LOG_TAG}Population structure updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateTotals(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(totals.Length);
                for (int i = 0; i < totals.Length; i++)
                    writer.Write(totals[i]);
                writer.ArrayEnd();

                Mod.Log.Debug($"{LOG_TAG}Population totals written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing population totals to JSON: {e.Message}");
            }
        }

        private void UpdateResults(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(results.Length);
                for (int i = 0; i < results.Length; i++)
                {
                    WriteData(writer, results[i]);
                }
                writer.ArrayEnd();

                Mod.Log.Debug($"{LOG_TAG}Population structure details written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing population structure details to JSON: {e.Message}");
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
            for (int i = 0; i < totals.Length; i++)
            {
                totals[i] = 0;
            }
            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new PopulationAtAgeInfo(i);
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (totals.IsCreated)
                    totals.Dispose();
                if (results.IsCreated)
                    results.Dispose();

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
