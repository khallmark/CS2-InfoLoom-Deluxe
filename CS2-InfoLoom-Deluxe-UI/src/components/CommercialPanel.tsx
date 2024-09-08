import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

interface CommercialData {
    emptyBuildings: number;
    propertylessCompanies: number;
    taxRate: number;
    serviceUtilization: { standard: number; leisure: number };
    salesCapacity: { standard: number; leisure: number };
    employeeCapacityRatio: number;
    availableWorkforce: { educated: number; uneducated: number };
}

interface CommercialPanelProps {
    focusKey?: FocusKey;
    onClose: () => void;
}

export const CommercialPanel: React.FC<CommercialPanelProps> = ({ focusKey, onClose }) => {
    const [commercialData, setCommercialData] = React.useState<CommercialData | null>(null);

    useDataUpdate("cityInfo.ilCommercial", setCommercialData);

    if (!commercialData) return null;

    return (
        <Panel title="Commercial Data" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow left="EMPTY BUILDINGS" right={commercialData.emptyBuildings} />
                <InfoRow left="PROPERTYLESS COMPANIES" right={commercialData.propertylessCompanies} />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left={<>AVERAGE TAX RATE<br/><span>10% is the neutral rate</span></>}
                    right={`${(commercialData.taxRate / 10).toFixed(1)}%`}
                    tooltip="Average tax rate for commercial zones"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow left="" right={<><span>Standard</span><span>Leisure</span></>} uppercase />
                <InfoRow 
                    left={<>SERVICE UTILIZATION<br/><span>30% is the neutral ratio</span></>}
                    right={<><span>{commercialData.serviceUtilization.standard}%</span><span>{commercialData.serviceUtilization.leisure}%</span></>}
                    tooltip="Utilization of commercial services"
                />
                <InfoRow 
                    left={<>SALES CAPACITY<br/><span>100% when capacity = consumption</span></>}
                    right={<><span>{commercialData.salesCapacity.standard}%</span><span>{commercialData.salesCapacity.leisure}%</span></>}
                    tooltip="Sales capacity compared to consumption"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left={<>EMPLOYEE CAPACITY RATIO<br/><span>75% is the neutral ratio</span></>}
                    right={`${(commercialData.employeeCapacityRatio / 10).toFixed(1)}%`}
                    tooltip="Ratio of current employees to maximum capacity"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow left="AVAILABLE WORKFORCE" right="" uppercase />
                <InfoRow left="Educated" right={commercialData.availableWorkforce.educated} subRow />
                <InfoRow left="Uneducated" right={commercialData.availableWorkforce.uneducated} subRow />
            </InfoSection>
        </Panel>
    );
};
