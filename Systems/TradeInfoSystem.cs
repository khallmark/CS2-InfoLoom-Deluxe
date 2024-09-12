using System;
using System.Collections.Generic;
using Game;
using Game.City;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Unity.Entities;

namespace InfoLoom_Deluxe.Systems
{
    public partial class TradeInfoSystem : GameSystemBase
    {
        private TradeSystem tradeSystem;
        private SimulationSystem simulationSystem;
        private EntityQuery cityQuery;

        private const string LOG_TAG = "[InfoLoom] TradeInfoSystem: ";

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                tradeSystem = World.GetOrCreateSystemManaged<TradeSystem>();
                simulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                cityQuery = GetEntityQuery(ComponentType.ReadOnly<Game.City.City>());

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
                if (simulationSystem == null || simulationSystem.frameIndex % 128 != 0)
                    return;

                var tradeInfo = GatherTradeInfo();
                // Here you would update your UI binding
                // For example: UpdateBinding("economy.tradeInfo", tradeInfo);

                Mod.Log.Debug($"{LOG_TAG}Trade information updated successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnUpdate: {e.Message}");
            }
        }

        private List<TradeInfo> GatherTradeInfo()
        {
            var tradeInfo = new List<TradeInfo>();
            try
            {
                if (!cityQuery.TryGetSingletonEntity(out Entity cityEntity))
                {
                    Mod.Log.Warn($"{LOG_TAG}City entity not found");
                    return tradeInfo;
                }

                var cityEffects = GetBuffer<Game.City.CityModifier>(cityEntity);

                foreach (Resource resource in Enum.GetValues(typeof(Resource)))
                {
                    if (resource == Resource.None) continue;

                    int resourceIndex = EconomyUtils.GetResourceIndex(resource);
                    float internalSupply = tradeSystem.GetResourceAmount(resourceIndex);
                    float importExport = tradeSystem.GetTradeBalance(resourceIndex);

                    var tradeCost = tradeSystem.GetTradeCost(resource);
                    var resourceData = tradeSystem.GetResourceData(resource);

                    var transportCosts = new Dictionary<OutsideConnectionTransferType, float>();
                    foreach (OutsideConnectionTransferType type in Enum.GetValues(typeof(OutsideConnectionTransferType)))
                    {
                        if (type == OutsideConnectionTransferType.None) continue;
                        transportCosts[type] = tradeSystem.GetTradePrice(resource, type, true, cityEffects);
                    }

                    tradeInfo.Add(new TradeInfo
                    {
                        ResourceName = resource.ToString(),
                        InternalSupply = internalSupply,
                        ImportExport = importExport,
                        BuyPrice = tradeCost.m_BuyCost,
                        SellPrice = tradeCost.m_SellCost,
                        LocalDemand = tradeSystem.GetLocalDemand(resourceIndex),
                        GlobalDemand = tradeSystem.GetGlobalDemand(resourceIndex),
                        ProductionRate = tradeSystem.GetProductionRate(resourceIndex),
                        StorageCapacity = tradeSystem.GetStorageCapacity(resourceIndex),
                        TradeBalance = tradeSystem.GetTradeBalance(resourceIndex),
                        Weight = resourceData.m_Weight,
                        TransportCosts = transportCosts,
                        CityModifierEffect = CityUtils.GetModifierEffect(cityEffects, CityModifierType.ImportCost),
                        TradeCooldown = TradeSystem.kTransferCooldown,
                        HistoricalData = tradeSystem.GetHistoricalTradeBalance(resourceIndex)
                    });
                }

                Mod.Log.Debug($"{LOG_TAG}Trade info gathered successfully for {tradeInfo.Count} resources");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error gathering trade info: {e.Message}");
            }
            return tradeInfo;
        }

        protected override void OnDestroy()
        {
            try
            {
                base.OnDestroy();
                Mod.Log.Info($"{LOG_TAG}System destroyed successfully");
            }
            catch (Exception e)
            {
                Mod.Log.Error($"{LOG_TAG}Error during OnDestroy: {e.Message}");
            }
        }
    }

    public struct TradeInfo
    {
        public string ResourceName;
        public float InternalSupply;
        public float ImportExport;
        public float BuyPrice;
        public float SellPrice;
        public float LocalDemand;
        public float GlobalDemand;
        public float ProductionRate;
        public float StorageCapacity;
        public float TradeBalance;
        public float Weight;
        public Dictionary<OutsideConnectionTransferType, float> TransportCosts;
        public float CityModifierEffect;
        public int TradeCooldown;
        public float[] HistoricalData;
    }
}
