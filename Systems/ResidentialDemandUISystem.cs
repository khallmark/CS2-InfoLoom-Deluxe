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
    public partial class ResidentialDemandUISystem : UISystemBase
    {
        private SimulationSystem m_SimulationSystem;
        private ResidentialDemandSystem m_ResidentialDemandSystem;
        private RawValueBinding m_uiResidentialDemand;
        private NativeArray<float> m_ResidentialDemand;

        private const string LOG_TAG = "[InfoLoom] ResidentialDemandUISystem: ";

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_ResidentialDemandSystem = World.GetOrCreateSystemManaged<ResidentialDemandSystem>();

                AddBinding(m_uiResidentialDemand = new RawValueBinding("cityInfo", "ilResidential", UpdateResidentialDemand));

                m_ResidentialDemand = new NativeArray<float>(3, Allocator.Persistent);

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
                if (m_SimulationSystem == null || m_SimulationSystem.frameIndex % 128 != 88)
                    return;

                base.OnUpdate();

                UpdateResidentialData();

                m_uiResidentialDemand.Update();

                Mod.Log.Debug($"{LOG_TAG}Updated residential demand data. Low Demand: {m_ResidentialDemand[0]}");
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
                if (m_ResidentialDemandSystem == null)
                {
                    Mod.Log.Warning($"{LOG_TAG}ResidentialDemandSystem is null");
                    return;
                }

                m_ResidentialDemand[0] = m_ResidentialDemandSystem.residentialLowDemand;
                m_ResidentialDemand[1] = m_ResidentialDemandSystem.residentialMediumDemand;
                m_ResidentialDemand[2] = m_ResidentialDemandSystem.residentialHighDemand;

                Mod.Log.Debug($"{LOG_TAG}Residential data updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Warning($"{LOG_TAG}Error updating residential data: {e.Message}");
            }
        }

        private void UpdateResidentialDemand(IJsonWriter writer)
        {
            try
            {
                writer.TypeBegin("ResidentialDemand");
                writer.PropertyName("lowDemand");
                writer.Write(m_ResidentialDemand[0]);
                writer.PropertyName("mediumDemand");
                writer.Write(m_ResidentialDemand[1]);
                writer.PropertyName("highDemand");
                writer.Write(m_ResidentialDemand[2]);
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
                if (m_ResidentialDemand.IsCreated)
                    m_ResidentialDemand.Dispose();
                
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
