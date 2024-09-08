import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

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
    focusKey?: FocusKey;
    onClose: () => void;
}

export const WorkforcePanel: React.FC<WorkforcePanelProps> = ({ focusKey, onClose }) => {
    const [workforceData, setWorkforceData] = React.useState<WorkforceInfo[]>([]);

    useDataUpdate("populationInfo.ilWorkforce", setWorkforceData);

    const educationLevels = ['Uneducated', 'Poorly Educated', 'Educated', 'Well Educated', 'Highly Educated'];

    return (
        <Panel title="Workforce Structure" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow left="Education" right="Total" uppercase />
                {workforceData.slice(0, 5).map((info, index) => (
                    <React.Fragment key={index}>
                        <InfoRow 
                            left={educationLevels[index]} 
                            right={`${info.total} (${((info.total / workforceData[5].total) * 100).toFixed(1)}%)`} 
                        />
                        <InfoRow left="Workers" right={info.worker} subRow />
                        <InfoRow left="Unemployed" right={info.unemployed} subRow />
                        <InfoRow left="Homeless" right={info.homeless} subRow />
                        <InfoRow left="Employable" right={info.employable} subRow />
                        <InfoRow left="Outside" right={info.outside} subRow />
                        <InfoRow left="Under" right={info.under} subRow />
                    </React.Fragment>
                ))}
                <InfoRow left="TOTAL" right={workforceData[5]?.total} uppercase />
            </InfoSection>
        </Panel>
    );
};
