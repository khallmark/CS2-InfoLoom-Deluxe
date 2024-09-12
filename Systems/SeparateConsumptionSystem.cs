using System;
using Colossal.UI.Binding;
using Game;
using Game.Economy;
using Game.Simulation;
using Game.UI;
using Unity.Entities;

namespace InfoLoom_Deluxe.Systems
{
    public partial class SeparateConsumptionSystem : UISystemBase
    {
        private const string LOG_TAG = "[InfoLoom] SeparateConsumptionSystem: ";
        private RawValueBinding uiConsumptionData;
        private SimulationSystem simulationSystem;
        private TradeSystem tradeSystem;
        private EntityQuery cityQuery;

        public override GameMode gameMode => GameMode.Game;

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                tradeSystem = World.GetOrCreateSystemManaged<TradeSystem>();
                cityQuery = GetEntityQuery(ComponentType.ReadOnly<Game.City.City>());
                AddBinding(uiConsumptionData = new RawValueBinding("infoLoomPanel", "consumptionData", UpdateConsumptionUI));
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
                if (simulationSystem.frameIndex % 128 != 0)
                    return;

                uiConsumptionData.Update();
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
                    float industrialSupply = tradeSystem.GetIndustrialSupply(resourceIndex);
                    float industrialDemand = tradeSystem.GetIndustrialDemand(resourceIndex);
                    float commercialSupply = tradeSystem.GetCommercialSupply(resourceIndex);
                    float commercialDemand = tradeSystem.GetCommercialDemand(resourceIndex);
                    float surplus = industrialSupply + commercialSupply - industrialDemand - commercialDemand;
                    float importExport = tradeSystem.GetTradeBalance(resourceIndex);

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
