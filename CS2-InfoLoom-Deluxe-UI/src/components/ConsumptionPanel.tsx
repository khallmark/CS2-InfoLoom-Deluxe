import React, { useState, useCallback } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

interface Good {
    name: string;
    industrialSupply: number;
    industrialDemand: number;
    commercialSupply: number;
    commercialDemand: number;
    surplus: number;
    importExport: number;
}

interface ConsumptionData {
    goods: Good[];
}

interface ConsumptionPanelProps {
    onClose: () => void;
}

export const ConsumptionPanel: React.FC<ConsumptionPanelProps> = ({ onClose }) => {
    const [consumptionData, setConsumptionData] = useState<ConsumptionData | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedGood, setSelectedGood] = useState<Good | null>(null);
    const rem = useRem();

    useDataUpdate("infoLoomPanel.consumptionData", (data) => {
        setConsumptionData(data);
        setIsLoading(false);
    });

    const handleGoodClick = useCallback((good: Good) => {
        setSelectedGood(good === selectedGood ? null : good);
    }, [selectedGood]);

    if (isLoading) {
        return (
            <Panel title="CONSUMPTION_OVERVIEW" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    if (!consumptionData) {
        return (
            <Panel title="CONSUMPTION_OVERVIEW" onClose={onClose}>
                <LocalizedString id="ERROR_LOADING_DATA" />
            </Panel>
        );
    }

    return (
        <Panel title="CONSUMPTION_OVERVIEW" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                {consumptionData.goods.map((good, index) => (
                    <InfoSection key={index}>
                        <InfoRow 
                            left={good.name} 
                            right={useFormattedLargeNumber(good.surplus)} 
                            uppercase 
                        />
                        <button onClick={() => handleGoodClick(good)}>
                            <LocalizedString id={selectedGood === good ? "HIDE_DETAILS" : "SHOW_DETAILS"} />
                        </button>
                        {selectedGood === good && (
                            <>
                                <Tooltip tooltip={<LocalizedString id="INDUSTRIAL_SUPPLY_DESC" />}>
                                    <InfoRow 
                                        left={<LocalizedString id="INDUSTRIAL_SUPPLY" />} 
                                        right={<LocalizedNumber value={good.industrialSupply} />} 
                                        subRow
                                    />
                                </Tooltip>
                                <Tooltip tooltip={<LocalizedString id="INDUSTRIAL_DEMAND_DESC" />}>
                                    <InfoRow 
                                        left={<LocalizedString id="INDUSTRIAL_DEMAND" />} 
                                        right={<LocalizedNumber value={good.industrialDemand} />} 
                                        subRow
                                    />
                                </Tooltip>
                                <Tooltip tooltip={<LocalizedString id="COMMERCIAL_SUPPLY_DESC" />}>
                                    <InfoRow 
                                        left={<LocalizedString id="COMMERCIAL_SUPPLY" />} 
                                        right={<LocalizedNumber value={good.commercialSupply} />} 
                                        subRow
                                    />
                                </Tooltip>
                                <Tooltip tooltip={<LocalizedString id="COMMERCIAL_DEMAND_DESC" />}>
                                    <InfoRow 
                                        left={<LocalizedString id="COMMERCIAL_DEMAND" />} 
                                        right={<LocalizedNumber value={good.commercialDemand} />} 
                                        subRow
                                    />
                                </Tooltip>
                                <Tooltip tooltip={<LocalizedString id="SURPLUS_DEFICIT_DESC" />}>
                                    <InfoRow 
                                        left={<LocalizedString id="SURPLUS_DEFICIT" />} 
                                        right={<LocalizedNumber value={good.surplus} />} 
                                        subRow
                                    />
                                </Tooltip>
                                <Tooltip tooltip={<LocalizedString id="IMPORT_EXPORT_DESC" />}>
                                    <InfoRow 
                                        left={<LocalizedString id="IMPORT_EXPORT" />} 
                                        right={<LocalizedNumber value={good.importExport} />} 
                                        subRow
                                    />
                                </Tooltip>
                            </>
                        )}
                    </InfoSection>
                ))}
            </Scrollable>
        </Panel>
    );
};
