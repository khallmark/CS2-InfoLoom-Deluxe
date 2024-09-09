import React, { useState, useCallback, useMemo } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

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
    transportCosts: Record<string, number>;
    cityModifierEffect: number;
    tradeCooldown: number;
    historicalData: number[];
}

interface TradePanelProps {
    onClose: () => void;
}

export const TradePanel: React.FC<TradePanelProps> = ({ onClose }) => {
    const [tradeData, setTradeData] = useState<TradeInfo[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedResource, setSelectedResource] = useState<string | null>(null);
    const [sortBy, setSortBy] = useState<keyof TradeInfo>('tradeBalance');
    const [sortOrder, setSortOrder] = useState<'asc' | 'desc'>('desc');
    const rem = useRem();

    useDataUpdate("economy.tradeInfo", (data) => {
        setTradeData(data);
        setIsLoading(false);
    });

    const handleResourceClick = useCallback((resourceName: string) => {
        setSelectedResource(resourceName === selectedResource ? null : resourceName);
    }, [selectedResource]);

    const toggleSortOrder = useCallback(() => {
        setSortOrder(prev => prev === 'asc' ? 'desc' : 'asc');
    }, []);

    const handleSortChange = useCallback((newSortBy: keyof TradeInfo) => {
        setSortBy(newSortBy);
    }, []);

    const sortedTradeData = useMemo(() => {
        return [...tradeData].sort((a, b) => {
            if (sortOrder === 'asc') {
                return a[sortBy] > b[sortBy] ? 1 : -1;
            } else {
                return b[sortBy] > a[sortBy] ? 1 : -1;
            }
        });
    }, [tradeData, sortBy, sortOrder]);

    const totalTradeBalance = useMemo(() => tradeData.reduce((sum, resource) => sum + resource.tradeBalance, 0), [tradeData]);

    if (isLoading) {
        return (
            <Panel title="TRADE_INFORMATION" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    return (
        <Panel title="TRADE_INFORMATION" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id="TOTAL_TRADE_BALANCE" />} 
                        right={<LocalizedNumber value={totalTradeBalance} />}
                        tooltip={totalTradeBalance > 0 ? <LocalizedString id="NET_EXPORTER" /> : <LocalizedString id="NET_IMPORTER" />}
                    />
                </InfoSection>
                <InfoRow 
                    left={<LocalizedString id="SORT_BY" />}
                    right={
                        <select value={sortBy} onChange={(e) => handleSortChange(e.target.value as keyof TradeInfo)}>
                            {Object.keys(tradeData[0]).map(key => (
                                <option key={key} value={key}><LocalizedString id={`SORT_BY_${key.toUpperCase()}`} /></option>
                            ))}
                        </select>
                    }
                />
                <InfoRow 
                    left={<LocalizedString id="SORT_ORDER" />}
                    right={<button onClick={toggleSortOrder}><LocalizedString id={sortOrder === 'asc' ? "SORT_ASCENDING" : "SORT_DESCENDING"} /></button>}
                />
                {sortedTradeData.map((resource, index) => (
                    <InfoSection key={index}>
                        <InfoRow 
                            left={<LocalizedString id={`RESOURCE_${resource.resourceName.toUpperCase()}`} />} 
                            right={<LocalizedNumber value={resource.tradeBalance} />}
                            uppercase
                        />
                        {selectedResource === resource.resourceName && (
                            <>
                                <InfoRow left={<LocalizedString id="INTERNAL_SUPPLY" />} right={<LocalizedNumber value={resource.internalSupply} />} subRow />
                                <InfoRow left={<LocalizedString id="IMPORT_EXPORT" />} right={<LocalizedNumber value={resource.importExport} />} subRow />
                                <InfoRow left={<LocalizedString id="BUY_PRICE" />} right={<LocalizedNumber value={resource.buyPrice} />} subRow />
                                <InfoRow left={<LocalizedString id="SELL_PRICE" />} right={<LocalizedNumber value={resource.sellPrice} />} subRow />
                                <InfoRow left={<LocalizedString id="LOCAL_DEMAND" />} right={<LocalizedNumber value={resource.localDemand} />} subRow />
                                <InfoRow left={<LocalizedString id="GLOBAL_DEMAND" />} right={<LocalizedNumber value={resource.globalDemand} />} subRow />
                                <InfoRow left={<LocalizedString id="PRODUCTION_RATE" />} right={<LocalizedNumber value={resource.productionRate} />} subRow />
                                <InfoRow left={<LocalizedString id="STORAGE_CAPACITY" />} right={<LocalizedNumber value={resource.storageCapacity} />} subRow />
                            </>
                        )}
                    </InfoSection>
                ))}
            </Scrollable>
        </Panel>
    );
};
