using Game.Economy;
using Game.Simulation;
using Unity.Entities;
using Unity.Collections;
using System;
using System.Collections.Generic;

namespace InfoLoom.Systems
{
    public partial class TradeInfoSystem : GameSystemBase
    {
        private TradeSystem m_TradeSystem;
        private SimulationSystem m_SimulationSystem;
        private EntityQuery m_CityQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_TradeSystem = World.GetOrCreateSystemManaged<TradeSystem>();
            m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
            m_CityQuery = GetEntityQuery(ComponentType.ReadOnly<Game.City.City>());
        }

        protected override void OnUpdate()
        {
            if (m_SimulationSystem.frameIndex % 128 != 0) return;

            var tradeInfo = GatherTradeInfo();
            // Here you would update your UI binding
            // For example: UpdateBinding("economy.tradeInfo", tradeInfo);
        }

        private List<TradeInfo> GatherTradeInfo()
        {
            var tradeInfo = new List<TradeInfo>();
            var cityEntity = m_CityQuery.GetSingletonEntity();
            var cityEffects = GetBuffer<Game.City.CityModifier>(cityEntity);

            foreach (Resource resource in Enum.GetValues(typeof(Resource)))
            {
                if (resource == Resource.None) continue;

                int resourceIndex = EconomyUtils.GetResourceIndex(resource);
                float internalSupply = m_TradeSystem.GetResourceAmount(resourceIndex);
                float importExport = m_TradeSystem.GetTradeBalance(resourceIndex);
                
                var tradeCost = m_TradeSystem.GetTradeCost(resource);
                var resourceData = m_TradeSystem.GetResourceData(resource);

                var transportCosts = new Dictionary<OutsideConnectionTransferType, float>();
                foreach (OutsideConnectionTransferType type in Enum.GetValues(typeof(OutsideConnectionTransferType)))
                {
                    if (type == OutsideConnectionTransferType.None) continue;
                    transportCosts[type] = m_TradeSystem.GetTradePrice(resource, type, true, cityEffects);
                }

                tradeInfo.Add(new TradeInfo
                {
                    ResourceName = resource.ToString(),
                    InternalSupply = internalSupply,
                    ImportExport = importExport,
                    BuyPrice = tradeCost.m_BuyCost,
                    SellPrice = tradeCost.m_SellCost,
                    LocalDemand = m_TradeSystem.GetLocalDemand(resourceIndex),
                    GlobalDemand = m_TradeSystem.GetGlobalDemand(resourceIndex),
                    ProductionRate = m_TradeSystem.GetProductionRate(resourceIndex),
                    StorageCapacity = m_TradeSystem.GetStorageCapacity(resourceIndex),
                    TradeBalance = m_TradeSystem.GetTradeBalance(resourceIndex),
                    Weight = resourceData.m_Weight,
                    TransportCosts = transportCosts,
                    CityModifierEffect = CityUtils.GetModifierEffect(cityEffects, CityModifierType.ImportCost),
                    TradeCooldown = TradeSystem.kTransferCooldown,
                    HistoricalData = m_TradeSystem.GetHistoricalTradeBalance(resourceIndex)
                });
            }

            return tradeInfo;
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
