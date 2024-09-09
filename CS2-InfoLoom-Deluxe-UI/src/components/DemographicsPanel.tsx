import React, { useState } from 'react';
import { Panel, InfoRow, InfoSection, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

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
    onClose: () => void;
}

export const DemographicsPanel: React.FC<DemographicsPanelProps> = ({ onClose }) => {
    const [demographicsData, setDemographicsData] = useState<DemographicsData | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const rem = useRem();

    useDataUpdate("populationInfo.structureTotals", (data) => {
        setDemographicsData(data);
        setIsLoading(false);
    });

    if (isLoading) {
        return (
            <Panel title="DEMOGRAPHICS" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    if (!demographicsData) {
        return (
            <Panel title="DEMOGRAPHICS" onClose={onClose}>
                <LocalizedString id="ERROR_LOADING_DATA" />
            </Panel>
        );
    }

    return (
        <Panel title="DEMOGRAPHICS" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoSection>
                    <InfoRow left={<LocalizedString id="ALL_CITIZENS" />} right={useFormattedLargeNumber(demographicsData.allCitizens)} />
                    <InfoRow left={<LocalizedString id="TOURISTS" />} right={useFormattedLargeNumber(demographicsData.tourists)} />
                    <InfoRow left={<LocalizedString id="COMMUTERS" />} right={useFormattedLargeNumber(demographicsData.commuters)} />
                    <InfoRow left={<LocalizedString id="MOVING_AWAY" />} right={useFormattedLargeNumber(demographicsData.movingAway)} />
                    <InfoRow left={<LocalizedString id="POPULATION" />} right={useFormattedLargeNumber(demographicsData.population)} />
                    <InfoRow left={<LocalizedString id="DEAD" />} right={useFormattedLargeNumber(demographicsData.dead)} />
                    <InfoRow left={<LocalizedString id="STUDENTS" />} right={useFormattedLargeNumber(demographicsData.students)} />
                    <InfoRow left={<LocalizedString id="WORKERS" />} right={useFormattedLargeNumber(demographicsData.workers)} />
                    <InfoRow left={<LocalizedString id="HOMELESS" />} right={useFormattedLargeNumber(demographicsData.homeless)} />
                    <InfoRow left={<LocalizedString id="OLDEST_CITIZEN" />} right={<LocalizedNumber value={demographicsData.oldestCitizen} />} />
                </InfoSection>
            </Scrollable>
        </Panel>
    );
};
