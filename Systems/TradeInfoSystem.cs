using Game.Economy;
using Game.Simulation;
using Unity.Entities;
using Unity.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfoLoom.Systems
{
    public partial class TradeInfoSystem : GameSystemBase
    {
        private TradeSystem m_TradeSystem;
        private SimulationSystem m_SimulationSystem;
        private EntityQuery m_CityQuery;

        private const string LOG_TAG = "[InfoLoom] TradeInfoSystem: ";

        protected override void OnCreate()
        {
            try
            {
                base.OnCreate();
                m_TradeSystem = World.GetOrCreateSystemManaged<TradeSystem>();
                m_SimulationSystem = World.GetOrCreateSystemManaged<SimulationSystem>();
                m_CityQuery = GetEntityQuery(ComponentType.ReadOnly<Game.City.City>());

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
                if (m_SimulationSystem == null || m_SimulationSystem.frameIndex % 128 != 0) 
                    return;

                var tradeInfo = GatherTradeInfo();
                // Here you would update your UI binding
                // For example: UpdateBinding("economy.tradeInfo", tradeInfo);

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Trade information updated successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnUpdate: {e.Message}");
                Debug.LogException(e);
            }
        }

        private List<TradeInfo> GatherTradeInfo()
        {
            var tradeInfo = new List<TradeInfo>();
            try
            {
                if (!m_CityQuery.TryGetSingletonEntity(out Entity cityEntity))
                {
                    Debug.LogWarning($"{LOG_TAG}City entity not found");
                    return tradeInfo;
                }

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

                if (Debug.isDebugBuild)
                    Debug.Log($"{LOG_TAG}Trade info gathered successfully for {tradeInfo.Count} resources");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error gathering trade info: {e.Message}");
                Debug.LogException(e);
            }
            return tradeInfo;
        }

        protected override void OnDestroy()
        {
            try
            {
                base.OnDestroy();
                Debug.Log($"{LOG_TAG}System destroyed successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LOG_TAG}Error during OnDestroy: {e.Message}");
                Debug.LogException(e);
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
