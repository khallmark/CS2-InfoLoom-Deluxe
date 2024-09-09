import React, { useState, useCallback, useMemo } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

interface DemandData {
    residentialLow: number;
    residentialMedium: number;
    residentialHigh: number;
    commercial: number;
    industrial: number;
    office: number;
    buildingDemand: number[];
}

interface DemandPanelProps {
    onClose: () => void;
}

export const DemandPanel: React.FC<DemandPanelProps> = ({ onClose }) => {
    const [demandData, setDemandData] = useState<DemandData | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [displayMode, setDisplayMode] = useState<'zoning' | 'building'>('zoning');
    const rem = useRem();

    useDataUpdate("cityInfo.ilDemand", (data) => {
        setDemandData(data);
        setIsLoading(false);
    });

    const toggleDisplayMode = useCallback(() => {
        setDisplayMode(prev => prev === 'zoning' ? 'building' : 'zoning');
    }, []);

    const sortedDemandData = useMemo(() => {
        if (!demandData) return [];
        const dataToSort = displayMode === 'zoning' 
            ? [
                { type: 'residentialLow', value: demandData.residentialLow },
                { type: 'residentialMedium', value: demandData.residentialMedium },
                { type: 'residentialHigh', value: demandData.residentialHigh },
                { type: 'commercial', value: demandData.commercial },
                { type: 'industrial', value: demandData.industrial },
                { type: 'office', value: demandData.office },
            ]
            : [
                { type: 'residentialLow', value: demandData.buildingDemand[0] },
                { type: 'residentialMedium', value: demandData.buildingDemand[1] },
                { type: 'residentialHigh', value: demandData.buildingDemand[2] },
                { type: 'commercial', value: demandData.buildingDemand[3] },
                { type: 'industrial', value: demandData.buildingDemand[4] },
                { type: 'office', value: demandData.buildingDemand[6] },
            ];
        return dataToSort.sort((a, b) => b.value - a.value);
    }, [demandData, displayMode]);

    const panelTitle = <LocalizedString id="DEMAND" />;

    if (isLoading) {
        return (
            <Panel 
                title={panelTitle.props.id}
                onClose={onClose}
            >
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    if (!demandData) {
        return (
            <Panel 
                title={panelTitle.props.id}
                onClose={onClose}
            >
                <LocalizedString id="ERROR_LOADING_DATA" />
            </Panel>
        );
    }

    return (
        <Panel 
            title={panelTitle.props.id}
            onClose={onClose}
        >
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoRow 
                    left={<LocalizedString id="DISPLAY_MODE" />}
                    right={<button onClick={toggleDisplayMode}><LocalizedString id={displayMode === 'zoning' ? "ZONING_DEMAND" : "BUILDING_DEMAND"} /></button>}
                />
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id={displayMode === 'zoning' ? "ZONING_DEMAND" : "BUILDING_DEMAND"} />} 
                        right="" 
                        uppercase 
                    />
                    {sortedDemandData.map((item, index) => (
                        <InfoRow 
                            key={index}
                            left={<LocalizedString id={item.type.toUpperCase()} />} 
                            right={<LocalizedNumber value={item.value} />} 
                            subRow 
                        />
                    ))}
                </InfoSection>
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id="TOTAL_DEMAND" />} 
                        right={<LocalizedNumber value={sortedDemandData.reduce((sum, item) => sum + item.value, 0)} />} 
                        uppercase 
                    />
                </InfoSection>
            </Scrollable>
        </Panel>
    );
};
