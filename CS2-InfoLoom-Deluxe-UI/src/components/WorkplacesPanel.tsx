import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

interface WorkplacesInfo {
    level: number;
    total: number;
    service: number;
    commercial: number;
    leisure: number;
    extractor: number;
    industry: number;
    office: number;
    employee: number;
    open: number;
    commuter: number;
}

interface WorkplacesPanelProps {
    focusKey?: FocusKey;
    onClose: () => void;
}

export const WorkplacesPanel: React.FC<WorkplacesPanelProps> = ({ focusKey, onClose }) => {
    const [workplacesData, setWorkplacesData] = React.useState<WorkplacesInfo[]>([]);

    useDataUpdate("workplaces.ilWorkplaces", setWorkplacesData);

    const educationLevels = ['Uneducated', 'Poorly Educated', 'Educated', 'Well Educated', 'Highly Educated'];

    return (
        <Panel title="Workplace Distribution" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow left="Education" right={<>
                    <span>Total</span>
                    <span>Service</span>
                    <span>Commercial</span>
                    <span>Leisure</span>
                    <span>Extractor</span>
                    <span>Industry</span>
                    <span>Office</span>
                    <span>Employees</span>
                    <span>Open</span>
                    <span>Commuters</span>
                </>} uppercase />
                {workplacesData.slice(0, 5).map((info, index) => (
                    <InfoRow 
                        key={index}
                        left={educationLevels[index]} 
                        right={<>
                            <span>{info.total}</span>
                            <span>{info.service}</span>
                            <span>{info.commercial}</span>
                            <span>{info.leisure}</span>
                            <span>{info.extractor}</span>
                            <span>{info.industry}</span>
                            <span>{info.office}</span>
                            <span>{info.employee}</span>
                            <span>{info.open}</span>
                            <span>{info.commuter}</span>
                        </>}/>
                ))}
                <InfoRow left="TOTAL" right={<>
                    <span>{workplacesData[5]?.total}</span>
                    <span>{workplacesData[5]?.service}</span>
                    <span>{workplacesData[5]?.commercial}</span>
                    <span>{workplacesData[5]?.leisure}</span>
                    <span>{workplacesData[5]?.extractor}</span>
                    <span>{workplacesData[5]?.industry}</span>
                    <span>{workplacesData[5]?.office}</span>
                    <span>{workplacesData[5]?.employee}</span>
                    <span>{workplacesData[5]?.open}</span>
                    <span>{workplacesData[5]?.commuter}</span>
                </>} uppercase />
            </InfoSection>
        </Panel>
    );
};
