import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

interface IndustrialData {
    emptyBuildings: number[];
    propertylessCompanies: number[];
    taxRates: number[];
    localDemand: number;
    inputUtilization: number;
    employeeCapacityRatio: number[];
    availableWorkforce: number[];
    storage: {
        emptyBuildings: number;
        propertylessCompanies: number;
        demandedTypes: number;
    };
}

interface IndustrialPanelProps {
    focusKey?: FocusKey;
    onClose: () => void;
}

export const IndustrialPanel: React.FC<IndustrialPanelProps> = ({ focusKey, onClose }) => {
    const [industrialData, setIndustrialData] = React.useState<IndustrialData | null>(null);

    useDataUpdate("cityInfo.ilIndustrial", setIndustrialData);

    if (!industrialData) return null;

    return (
        <Panel title="Industrial and Office Data" focusKey={focusKey} onClose={onClose}>
            <InfoSection>
                <InfoRow left="" right={<><span>INDUSTRIAL</span><span>OFFICE</span></>} uppercase />
                <InfoRow left="EMPTY BUILDINGS" right={<><span>{industrialData.emptyBuildings[0]}</span><span>{industrialData.emptyBuildings[1]}</span></>} />
                <InfoRow left="PROPERTYLESS COMPANIES" right={<><span>{industrialData.propertylessCompanies[0]}</span><span>{industrialData.propertylessCompanies[1]}</span></>} />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left={<>AVERAGE TAX RATE<br/><span>10% is the neutral rate</span></>}
                    right={<><span>{(industrialData.taxRates[0] / 10).toFixed(1)}%</span><span>{(industrialData.taxRates[1] / 10).toFixed(1)}%</span></>}
                    tooltip="Tax rates for industrial and office zones"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left={<>LOCAL DEMAND (ind)<br/><span>100% when production = demand</span></>}
                    right={`${industrialData.localDemand}%`}
                    tooltip="Local demand for industrial products"
                />
                <InfoRow 
                    left={<>INPUT UTILIZATION (ind)<br/><span>110% is the neutral ratio, capped at 400%</span></>}
                    right={`${industrialData.inputUtilization}%`}
                    tooltip="Utilization of input resources"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left={<>EMPLOYEE CAPACITY RATIO<br/><span>72% is the neutral ratio</span></>}
                    right={<><span>{(industrialData.employeeCapacityRatio[0] / 10).toFixed(1)}%</span><span>{(industrialData.employeeCapacityRatio[1] / 10).toFixed(1)}%</span></>}
                    tooltip="Ratio of current employees to maximum capacity"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow left="AVAILABLE WORKFORCE" right="" uppercase />
                <InfoRow left="Educated" right={industrialData.availableWorkforce[0]} subRow />
                <InfoRow left="Uneducated" right={industrialData.availableWorkforce[1]} subRow />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left={<>STORAGE<br/><span>The game will spawn warehouses when DEMANDED TYPES exist.</span></>}
                    right=""
                    tooltip="Information about storage and warehouses"
                />
                <InfoRow left="Empty buildings" right={industrialData.storage.emptyBuildings} subRow />
                <InfoRow left="Propertyless companies" right={industrialData.storage.propertylessCompanies} subRow />
                <InfoRow left="DEMANDED TYPES" right={industrialData.storage.demandedTypes} subRow />
            </InfoSection>
        </Panel>
    );
};
