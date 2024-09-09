import React, { useState, useCallback, useMemo } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

interface WorkforceInfo {
    level: number;
    total: number;
    worker: number;
    unemployed: number;
    homeless: number;
    employable: number;
    outside: number;
    under: number;
}

interface WorkforcePanelProps {
    onClose: () => void;
}

export const WorkforcePanel: React.FC<WorkforcePanelProps> = ({ onClose }) => {
    const [workforceData, setWorkforceData] = useState<WorkforceInfo[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedLevel, setSelectedLevel] = useState<number | null>(null);
    const [comparisonMode, setComparisonMode] = useState<'absolute' | 'percentage'>('absolute');
    const rem = useRem();

    useDataUpdate("populationInfo.ilWorkforce", (data) => {
        setWorkforceData(data);
        setIsLoading(false);
    });

    const educationLevels = ['Uneducated', 'Poorly Educated', 'Educated', 'Well Educated', 'Highly Educated'];

    const totalWorkforce = useMemo(() => workforceData.reduce((sum, info) => sum + info.total, 0), [workforceData]);

    const handleLevelClick = useCallback((level: number) => {
        setSelectedLevel(level === selectedLevel ? null : level);
    }, [selectedLevel]);

    const toggleComparisonMode = useCallback(() => {
        setComparisonMode(prev => prev === 'absolute' ? 'percentage' : 'absolute');
    }, []);

    if (isLoading) {
        return (
            <Panel title="WORKFORCE_STRUCTURE" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    return (
        <Panel title="WORKFORCE_STRUCTURE" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoRow 
                    left={<LocalizedString id="COMPARISON_MODE" />}
                    right={<button onClick={toggleComparisonMode}><LocalizedString id={comparisonMode === 'absolute' ? "ABSOLUTE_VALUES" : "PERCENTAGE_VALUES"} /></button>}
                />
                {workforceData.map((info, index) => {
                    const percentage = (info.total / totalWorkforce) * 100;
                    return (
                        <InfoSection key={index}>
                            <InfoRow 
                                left={<LocalizedString id={`EDUCATION_LEVEL_${educationLevels[index].toUpperCase()}`} />} 
                                right={
                                    comparisonMode === 'absolute' 
                                        ? useFormattedLargeNumber(info.total)
                                        : <LocalizedNumber value={percentage} />
                                } 
                                uppercase
                            />
                            {selectedLevel === index && (
                                <>
                                    <InfoRow left={<LocalizedString id="WORKERS" />} right={useFormattedLargeNumber(info.worker)} subRow />
                                    <InfoRow left={<LocalizedString id="UNEMPLOYED" />} right={useFormattedLargeNumber(info.unemployed)} subRow />
                                    <InfoRow left={<LocalizedString id="HOMELESS" />} right={useFormattedLargeNumber(info.homeless)} subRow />
                                    <InfoRow left={<LocalizedString id="EMPLOYABLE" />} right={useFormattedLargeNumber(info.employable)} subRow />
                                    <InfoRow left={<LocalizedString id="OUTSIDE_WORKERS" />} right={useFormattedLargeNumber(info.outside)} subRow />
                                    <InfoRow left={<LocalizedString id="UNDEREMPLOYED" />} right={useFormattedLargeNumber(info.under)} subRow />
                                </>
                            )}
                        </InfoSection>
                    );
                })}
            </Scrollable>
        </Panel>
    );
};
