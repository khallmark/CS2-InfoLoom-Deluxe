import React, { useState } from 'react';
import { Panel, InfoRow, InfoSection, Chart, Dropdown } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey, OutsideConnectionTransferType } from "cs2/ui";

interface TradeInfo {
    resourceName: string;
    internalSupply: number;
    importExport: number;
    buyPrice: number;
    sellPrice: number;
    localDemand: number;
    globalDemand: number;
    productionRate: number;
    storageCapacity: number;
    tradeBalance: number;
    weight: number;
    transportCosts: Record<OutsideConnectionTransferType, number>;
    cityModifierEffect: number;
    tradeCooldown: number;
    historicalData: number[];
}

interface TradePanelProps {
    focusKey?: FocusKey;
    onClose: () => void;
}

export const TradePanel: React.FC<TradePanelProps> = ({ focusKey, onClose }) => {
    const [tradeData, setTradeData] = React.useState<TradeInfo[]>([]);
    const [selectedResource, setSelectedResource] = React.useState<string | null>(null);
    const [selectedTransportType, setSelectedTransportType] = useState<OutsideConnectionTransferType>(OutsideConnectionTransferType.Road);

    useDataUpdate("economy.tradeInfo", setTradeData);

    if (tradeData.length === 0) return null;

    const totalTradeBalance = tradeData.reduce((sum, resource) => sum + resource.tradeBalance, 0);

    const renderResourceDetails = (resource: TradeInfo) => (
        <InfoSection key={resource.resourceName}>
            <InfoRow left={resource.resourceName} right="" uppercase />
            <InfoRow left="Internal Supply" right={`${resource.internalSupply.toFixed(2)}`} subRow />
            <InfoRow left="Import/Export" right={`${resource.importExport.toFixed(2)}`} subRow />
            <InfoRow left="Buy Price" right={`$${resource.buyPrice.toFixed(2)}`} subRow />
            <InfoRow left="Sell Price" right={`$${resource.sellPrice.toFixed(2)}`} subRow />
            <InfoRow left="Local Demand" right={`${resource.localDemand.toFixed(2)}`} subRow />
            <InfoRow left="Global Demand" right={`${resource.globalDemand.toFixed(2)}`} subRow />
            <InfoRow left="Production Rate" right={`${resource.productionRate.toFixed(2)}/week`} subRow />
            <InfoRow left="Storage Capacity" right={`${resource.storageCapacity.toFixed(2)}`} subRow />
            <InfoRow 
                left="Trade Balance" 
                right={`$${resource.tradeBalance.toFixed(2)}`} 
                subRow
                tooltip={resource.tradeBalance > 0 ? "Net export" : "Net import"}
            />
            <InfoRow left="Resource Weight" right={`${resource.weight.toFixed(2)}`} subRow />
            <InfoRow left="City Modifier Effect" right={`${(resource.cityModifierEffect * 100).toFixed(2)}%`} subRow />
            <InfoRow left="Trade Cooldown" right={`${resource.tradeCooldown} frames`} subRow />
            <Dropdown
                label="Transport Type"
                options={Object.values(OutsideConnectionTransferType)}
                value={selectedTransportType}
                onChange={setSelectedTransportType}
            />
            <InfoRow 
                left={`${selectedTransportType} Transport Cost`} 
                right={`$${resource.transportCosts[selectedTransportType].toFixed(2)}`} 
                subRow
            />
        </InfoSection>
    );

    const chartData = {
        labels: tradeData.map(r => r.resourceName),
        datasets: [{
            label: 'Trade Balance',
            data: tradeData.map(r => r.tradeBalance),
            backgroundColor: tradeData.map(r => r.tradeBalance > 0 ? 'rgba(75, 192, 192, 0.6)' : 'rgba(255, 99, 132, 0.6)')
        }]
    };

    return (
        <Panel title="Trade Information" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow 
                    left="Total Trade Balance" 
                    right={`$${totalTradeBalance.toFixed(2)}`}
                    tooltip={totalTradeBalance > 0 ? "Net exporter" : "Net importer"}
                />
            </InfoSection>

            <Chart
                type="bar"
                data={chartData}
                options={{
                    onClick: (_, elements) => {
                        if (elements.length > 0) {
                            const index = elements[0].index;
                            setSelectedResource(tradeData[index].resourceName);
                        }
                    },
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Trade Balance ($)'
                            }
                        }
                    }
                }}
            />

            {selectedResource ? (
                renderResourceDetails(tradeData.find(r => r.resourceName === selectedResource)!)
            ) : (
                <InfoSection>
                    <InfoRow left="Click on a bar in the chart to see detailed information for that resource." right="" />
                </InfoSection>
            )}

            <InfoSection>
                <InfoRow left="Top Exports" right="" uppercase />
                {tradeData
                    .filter(r => r.tradeBalance > 0)
                    .sort((a, b) => b.tradeBalance - a.tradeBalance)
                    .slice(0, 5)
                    .map(r => (
                        <InfoRow 
                            key={r.resourceName}
                            left={r.resourceName} 
                            right={`$${r.tradeBalance.toFixed(2)}`} 
                            subRow
                        />
                    ))
                }
            </InfoSection>

            <InfoSection>
                <InfoRow left="Top Imports" right="" uppercase />
                {tradeData
                    .filter(r => r.tradeBalance < 0)
                    .sort((a, b) => a.tradeBalance - b.tradeBalance)
                    .slice(0, 5)
                    .map(r => (
                        <InfoRow 
                            key={r.resourceName}
                            left={r.resourceName} 
                            right={`$${Math.abs(r.tradeBalance).toFixed(2)}`} 
                            subRow
                        />
                    ))
                }
            </InfoSection>

            <Chart
                type="line"
                data={{
                    labels: tradeData[0].historicalData.map((_, index) => `T-${index}`),
                    datasets: tradeData.map(r => ({
                        label: r.resourceName,
                        data: r.historicalData,
                        borderColor: r.tradeBalance > 0 ? 'rgba(75, 192, 192, 1)' : 'rgba(255, 99, 132, 1)',
                        fill: false
                    }))
                }}
                options={{
                    responsive: true,
                    title: {
                        display: true,
                        text: 'Historical Trade Balance'
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Trade Balance ($)'
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'Time'
                            }
                        }
                    }
                }}
            />
        </Panel>
    );
};
