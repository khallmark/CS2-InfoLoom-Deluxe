using System;
using System.Runtime.CompilerServices;
using Colossal.UI.Binding;
using Game;
using Game.Citizens;
using Game.Common;
using Game.Simulation;
using Game.Tools;
using Game.UI;
using Unity.Collections;
using Unity.Entities;

namespace InfoLoom_Deluxe.Systems
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
            public WorkforceAtLevelInfo(int level) { Level = level; Total = 0; Worker = 0; Unemployed = 0; Homeless = 0; Employable = 0; Outside = 0; Under = 0; }
        }

        private SimulationSystem simulationSystem;
        private EntityQuery allAdultGroup;
        private RawValueBinding uiResults;
        private NativeArray<WorkforceAtLevelInfo> results;

        private const string LOG_TAG = "[InfoLoom] WorkforceInfoLoomUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                allAdultGroup = GetEntityQuery(new EntityQueryDesc
                {
                    All = new ComponentType[1] { ComponentType.ReadOnly<Citizen>() },
                    None = new ComponentType[3]
                    {
                        ComponentType.ReadOnly<Game.Citizens.Student>(),
                        ComponentType.ReadOnly<Deleted>(),
                        ComponentType.ReadOnly<Temp>()
                    }
                });

                AddBinding(uiResults = new RawValueBinding("populationInfo", "ilWorkforce", UpdateResults));

                results = new NativeArray<WorkforceAtLevelInfo>(6, Allocator.Persistent);

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
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 33)
                    return;

                ResetResults();

                // Implement workforce calculation here
                // This part needs to be implemented based on your specific requirements

                uiResults.Update();

                Mod.Log.Debug($"{LOG_TAG}Workforce information updated successfully");
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

                Mod.Log.Debug($"{LOG_TAG}Workforce data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing workforce data to JSON: {e.Message}");
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
                results[i] = new WorkforceAtLevelInfo(i);
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
