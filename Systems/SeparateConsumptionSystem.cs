using Game;
using Game.Economy;
using Game.Simulation;
using Game.UI;
using System;
using System.Collections.Generic;
using Unity.Entities;
using Colossal.UI.Binding;

namespace InfoLoom_Deluxe.Systems
{
    public partial class SeparateConsumptionSystem : UISystemBase
    {
        private const string LOG_TAG = "[InfoLoom] SeparateConsumptionSystem: ";
        private RawValueBinding m_uiConsumptionData;
        private SimulationSystem m_SimulationSystem;
        private TradeSystem m_TradeSystem;
        private EntityQuery m_CityQuery;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_TradeSystem = World.GetOrCreateSystemManaged<TradeSystem>();
                m_CityQuery = GetEntityQuery(ComponentType.ReadOnly<Game.City.City>());
                AddBinding(m_uiConsumptionData = new RawValueBinding("infoLoomPanel", "consumptionData", UpdateConsumptionUI));
                Mod.Log.Info($"{LOG_TAG}System created and UI binding added");
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
                if (m_SimulationSystem.frameIndex % 128 != 0)
                    return;

                m_uiConsumptionData.Update();
                Mod.Log.Debug($"{LOG_TAG}Consumption data updated");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private void UpdateConsumptionUI(IJsonWriter writer)
        {
            try
            {
                writer.TypeBegin("ConsumptionData");
                writer.PropertyName("goods");
                writer.ArrayBegin();

                foreach (Resource resource in Enum.GetValues(typeof(Resource)))
                {
                    if (resource == Resource.None) continue;

                    int resourceIndex = EconomyUtils.GetResourceIndex(resource);
                    float industrialSupply = m_TradeSystem.GetIndustrialSupply(resourceIndex);
                    float industrialDemand = m_TradeSystem.GetIndustrialDemand(resourceIndex);
                    float commercialSupply = m_TradeSystem.GetCommercialSupply(resourceIndex);
                    float commercialDemand = m_TradeSystem.GetCommercialDemand(resourceIndex);
                    float surplus = industrialSupply + commercialSupply - industrialDemand - commercialDemand;
                    float importExport = m_TradeSystem.GetTradeBalance(resourceIndex);

                    writer.TypeBegin("Good");
                    writer.PropertyName("name");
                    writer.Write(resource.ToString());
                    writer.PropertyName("industrialSupply");
                    writer.Write(industrialSupply);
                    writer.PropertyName("industrialDemand");
                    writer.Write(industrialDemand);
                    writer.PropertyName("commercialSupply");
                    writer.Write(commercialSupply);
                    writer.PropertyName("commercialDemand");
                    writer.Write(commercialDemand);
                    writer.PropertyName("surplus");
                    writer.Write(surplus);
                    writer.PropertyName("importExport");
                    writer.Write(importExport);
                    writer.TypeEnd();
                }

                writer.ArrayEnd();
                writer.TypeEnd();

                Mod.Log.Debug($"{LOG_TAG}Consumption data written to JSON successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error updating consumption data: {e.Message}");
            }
        }

        protected override void OnDestroy()
        {
            try
            {
                base.OnDestroy();
                Mod.Log.Info($"{LOG_TAG}System destroyed");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnDestroy: {e.Message}");
            }
        }
    }
}
