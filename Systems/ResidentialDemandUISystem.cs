using System;
using System.Runtime.CompilerServices;
using Colossal.UI.Binding;
using Game;
using Game.Simulation;
using Game.UI;
using Unity.Collections;

namespace InfoLoom_Deluxe.Systems
{
    [CompilerGenerated]
    public partial class ResidentialDemandUISystem : UISystemBase
    {
        private const string LOG_TAG = "[InfoLoom] ResidentialDemandUISystem: ";
        private NativeArray<float> residentialDemand;
        private ResidentialDemandSystem residentialDemandSystem;
        private SimulationSystem simulationSystem;
        private RawValueBinding uiResidentialDemand;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                residentialDemandSystem = World.GetOrCreateSystemManaged<ResidentialDemandSystem>();

                AddBinding(uiResidentialDemand =
                    new RawValueBinding("cityInfo", "ilResidential", UpdateResidentialDemand));

                residentialDemand = new NativeArray<float>(3, Allocator.Persistent);

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
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 88)
                    return;

                base.OnUpdate();

                UpdateResidentialData();

                uiResidentialDemand.Update();

                Mod.Log.Debug($"{LOG_TAG}Updated residential demand data. Low Demand: {residentialDemand[0]}");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateResidentialData()
        {
            try
            {
                if (residentialDemandSystem == null)
                {
                    Mod.Log.Warn($"{LOG_TAG}ResidentialDemandSystem is null");
                    return;
                }

                residentialDemand[0] = residentialDemandSystem.LowDemand;
                residentialDemand[1] = residentialDemandSystem.MediumDemand;
                residentialDemand[2] = residentialDemandSystem.HighDemand;

                Mod.Log.Debug($"{LOG_TAG}Residential data updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Warn($"{LOG_TAG}Error updating residential data: {e.Message}");
            }
        }

        private void UpdateResidentialDemand(IJsonWriter writer)
        {
            try
            {
                writer.TypeBegin("ResidentialDemand");
                writer.PropertyName("lowDemand");
                writer.Write(residentialDemand[0]);
                writer.PropertyName("mediumDemand");
                writer.Write(residentialDemand[1]);
                writer.PropertyName("highDemand");
                writer.Write(residentialDemand[2]);
                writer.TypeEnd();

                Mod.Log.Debug($"{LOG_TAG}Residential demand data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error writing residential demand data to JSON: {e.Message}");
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                if (residentialDemand.IsCreated)
                    residentialDemand.Dispose();

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
