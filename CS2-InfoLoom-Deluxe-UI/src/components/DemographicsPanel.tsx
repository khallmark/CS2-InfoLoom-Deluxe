import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

interface DemographicsData {
    allCitizens: number;
    tourists: number;
    commuters: number;
    movingAway: number;
    population: number;
    dead: number;
    students: number;
    workers: number;
    homeless: number;
    oldestCitizen: number;
}

interface DemographicsPanelProps {
    focusKey?: FocusKey;
    onClose: () => void;
}

export const DemographicsPanel: React.FC<DemographicsPanelProps> = ({ focusKey, onClose }) => {
    const [demographicsData, setDemographicsData] = React.useState<DemographicsData | null>(null);

    useDataUpdate("populationInfo.structureTotals", setDemographicsData);

    if (!demographicsData) return null;

    return (
        <Panel title="Demographics" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow left="All Citizens" right={demographicsData.allCitizens} />
                <InfoRow left="Tourists" right={demographicsData.tourists} />
                <InfoRow left="Commuters" right={demographicsData.commuters} />
                <InfoRow left="Moving Away" right={demographicsData.movingAway} />
                <InfoRow left="Population" right={demographicsData.population} />
                <InfoRow left="Dead" right={demographicsData.dead} />
                <InfoRow left="Students" right={demographicsData.students} />
                <InfoRow left="Workers" right={demographicsData.workers} />
                <InfoRow left="Homeless" right={demographicsData.homeless} />
                <InfoRow left="Oldest citizen" right={demographicsData.oldestCitizen} />
            </InfoSection>
        </Panel>
    );
};
