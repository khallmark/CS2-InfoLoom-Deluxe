import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

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
    focusKey?: FocusKey;
    onClose: () => void;
}

export const DemandPanel: React.FC<DemandPanelProps> = ({ focusKey, onClose }) => {
    const [demandData, setDemandData] = React.useState<DemandData | null>(null);

    useDataUpdate("cityInfo.ilDemand", setDemandData);

    if (!demandData) return null;

    return (
        <Panel title="Demand" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow left="BUILDING DEMAND" right="" uppercase />
                <InfoRow left="Residential Low" right={`${demandData.buildingDemand[0]}%`} />
                <InfoRow left="Residential Medium" right={`${demandData.buildingDemand[1]}%`} />
                <InfoRow left="Residential High" right={`${demandData.buildingDemand[2]}%`} />
                <InfoRow left="Commercial" right={`${demandData.buildingDemand[3]}%`} />
                <InfoRow left="Industrial" right={`${demandData.buildingDemand[4]}%`} />
                <InfoRow left="Office" right={`${demandData.buildingDemand[6]}%`} />
            </InfoSection>
            <InfoSection>
                <InfoRow left="ZONING DEMAND" right="" uppercase />
                <InfoRow left="Residential Low" right={`${Math.round(demandData.residentialLow * 100)}%`} />
                <InfoRow left="Residential Medium" right={`${Math.round(demandData.residentialMedium * 100)}%`} />
                <InfoRow left="Residential High" right={`${Math.round(demandData.residentialHigh * 100)}%`} />
                <InfoRow left="Commercial" right={`${Math.round(demandData.commercial * 100)}%`} />
                <InfoRow left="Industrial" right={`${Math.round(demandData.industrial * 100)}%`} />
                <InfoRow left="Office" right={`${Math.round(demandData.office * 100)}%`} />
            </InfoSection>
        </Panel>
    );
};
