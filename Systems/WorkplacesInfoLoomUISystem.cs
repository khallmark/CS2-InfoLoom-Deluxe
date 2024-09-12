using System;
using System.Runtime.CompilerServices;
using Colossal.UI.Binding;
using Game;
using Game.Buildings;
using Game.Companies;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI;
using Unity.Collections;
using Unity.Entities;

namespace InfoLoom_Deluxe.Systems
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
            public WorkplacesAtLevelInfo(int level)
            {
                Level = level;
                Total = 0; Service = 0; Commercial = 0; Leisure = 0;
                Extractor = 0; Industrial = 0; Office = 0;
                Employee = 0; Open = 0; Commuter = 0;
            }
        }

        private SimulationSystem simulationSystem;
        private EntityQuery workplaceQuery;
        private RawValueBinding uiResults;
        private NativeArray<WorkplacesAtLevelInfo> results;

        private const string LOG_TAG = "[InfoLoom] WorkplacesInfoLoomUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                workplaceQuery = GetEntityQuery(new EntityQueryDesc
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

                AddBinding(uiResults = new RawValueBinding("workplaces", "ilWorkplaces", UpdateResults));

                results = new NativeArray<WorkplacesAtLevelInfo>(7, Allocator.Persistent);

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
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 22)
                    return;

                ResetResults();

                // Implement workplace calculation here
                // This part needs to be implemented based on your specific requirements

                uiResults.Update();

                Mod.Log.Debug($"{LOG_TAG}Workplaces information updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateResults(IJsonWriter writer)
        {
            try
            {
                writer.ArrayBegin(results.Length);
                for (int i = 0; i < results.Length; i++)
                    WriteData(writer, results[i]);
                writer.ArrayEnd();

                Mod.Log.Debug($"{LOG_TAG}Workplaces data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing workplaces data to JSON: {e.Message}");
            }
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
                results[i] = new WorkplacesAtLevelInfo(i);
            }
        }

        protected override void OnDestroy()
        {
            try
            {
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
