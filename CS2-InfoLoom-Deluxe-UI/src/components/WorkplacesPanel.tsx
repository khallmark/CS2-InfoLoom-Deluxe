import React, { useState, useCallback, useMemo } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

interface WorkplacesInfo {
    level: number;
    total: number;
    service: number;
    commercial: number;
    leisure: number;
    extractor: number;
    industrial: number;
    office: number;
    employee: number;
    open: number;
    commuter: number;
}

interface WorkplacesPanelProps {
    onClose: () => void;
}

export const WorkplacesPanel: React.FC<WorkplacesPanelProps> = ({ onClose }) => {
    const [workplacesData, setWorkplacesData] = useState<WorkplacesInfo[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [selectedLevel, setSelectedLevel] = useState<number | null>(null);
    const [displayMode, setDisplayMode] = useState<'total' | 'byType'>('total');
    const [sortBy, setSortBy] = useState<keyof WorkplacesInfo>('total');
    const rem = useRem();

    useDataUpdate("workplaces.ilWorkplaces", (data) => {
        setWorkplacesData(data);
        setIsLoading(false);
    });

    const educationLevels = ['Uneducated', 'Poorly Educated', 'Educated', 'Well Educated', 'Highly Educated', 'All Levels'];

    const handleLevelClick = useCallback((level: number) => {
        setSelectedLevel(level === selectedLevel ? null : level);
    }, [selectedLevel]);

    const toggleDisplayMode = useCallback(() => {
        setDisplayMode(prev => prev === 'total' ? 'byType' : 'total');
    }, []);

    const handleSortChange = useCallback((newSortBy: keyof WorkplacesInfo) => {
        setSortBy(newSortBy);
    }, []);

    const sortedWorkplacesData = useMemo(() => {
        return [...workplacesData].sort((a, b) => b[sortBy] - a[sortBy]);
    }, [workplacesData, sortBy]);

    if (isLoading) {
        return (
            <Panel title="WORKPLACES_STRUCTURE" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    return (
        <Panel title="WORKPLACES_STRUCTURE" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoRow 
                    left={<LocalizedString id="DISPLAY_MODE" />}
                    right={<button onClick={toggleDisplayMode}><LocalizedString id={displayMode === 'total' ? "TOTAL_WORKPLACES" : "WORKPLACES_BY_TYPE"} /></button>}
                />
                <InfoRow 
                    left={<LocalizedString id="SORT_BY" />}
                    right={
                        <select value={sortBy} onChange={(e) => handleSortChange(e.target.value as keyof WorkplacesInfo)}>
                            {Object.keys(workplacesData[0]).map(key => (
                                <option key={key} value={key}><LocalizedString id={`SORT_BY_${key.toUpperCase()}`} /></option>
                            ))}
                        </select>
                    }
                />
                {sortedWorkplacesData.map((info, index) => (
                    <InfoSection key={index}>
                        <InfoRow 
                            left={<LocalizedString id={`EDUCATION_LEVEL_${educationLevels[index].toUpperCase()}`} />} 
                            right={useFormattedLargeNumber(info.total)} 
                            uppercase
                        />
                        {selectedLevel === index && displayMode === 'byType' && (
                            <>
                                <InfoRow left={<LocalizedString id="SERVICE" />} right={useFormattedLargeNumber(info.service)} subRow />
                                <InfoRow left={<LocalizedString id="COMMERCIAL" />} right={useFormattedLargeNumber(info.commercial)} subRow />
                                <InfoRow left={<LocalizedString id="LEISURE" />} right={useFormattedLargeNumber(info.leisure)} subRow />
                                <InfoRow left={<LocalizedString id="EXTRACTOR" />} right={useFormattedLargeNumber(info.extractor)} subRow />
                                <InfoRow left={<LocalizedString id="INDUSTRIAL" />} right={useFormattedLargeNumber(info.industrial)} subRow />
                                <InfoRow left={<LocalizedString id="OFFICE" />} right={useFormattedLargeNumber(info.office)} subRow />
                                <InfoRow left={<LocalizedString id="EMPLOYEES" />} right={useFormattedLargeNumber(info.employee)} subRow />
                                <InfoRow left={<LocalizedString id="OPEN_POSITIONS" />} right={useFormattedLargeNumber(info.open)} subRow />
                                <InfoRow left={<LocalizedString id="COMMUTERS" />} right={useFormattedLargeNumber(info.commuter)} subRow />
                            </>
                        )}
                    </InfoSection>
                ))}
            </Scrollable>
        </Panel>
    );
};
