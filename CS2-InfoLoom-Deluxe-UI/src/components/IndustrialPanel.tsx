import React, { useState } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

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
    onClose: () => void;
}

export const IndustrialPanel: React.FC<IndustrialPanelProps> = ({ onClose }) => {
    const [industrialData, setIndustrialData] = useState<IndustrialData | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const rem = useRem();

    useDataUpdate("cityInfo.ilIndustrial", (data) => {
        setIndustrialData(data);
        setIsLoading(false);
    });

    if (isLoading) {
        return (
            <Panel title="INDUSTRIAL_DATA" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    if (!industrialData) {
        return (
            <Panel title="INDUSTRIAL_DATA" onClose={onClose}>
                <LocalizedString id="ERROR_LOADING_DATA" />
            </Panel>
        );
    }

    return (
        <Panel title="INDUSTRIAL_DATA" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoSection>
                    <InfoRow left="" right={<><span><LocalizedString id="INDUSTRIAL" /></span><span><LocalizedString id="OFFICE" /></span></>} uppercase />
                    <InfoRow 
                        left={<LocalizedString id="EMPTY_BUILDINGS" />} 
                        right={<>
                            <span>{useFormattedLargeNumber(industrialData.emptyBuildings[0])}</span>
                            <span>{useFormattedLargeNumber(industrialData.emptyBuildings[1])}</span>
                        </>} 
                    />
                    <InfoRow 
                        left={<LocalizedString id="PROPERTYLESS_COMPANIES" />} 
                        right={<>
                            <span>{useFormattedLargeNumber(industrialData.propertylessCompanies[0])}</span>
                            <span>{useFormattedLargeNumber(industrialData.propertylessCompanies[1])}</span>
                        </>} 
                    />
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="TAX_RATE_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="TAX_RATE" />} 
                            right={<>
                                <span><LocalizedNumber value={industrialData.taxRates[0]} /></span>
                                <span><LocalizedNumber value={industrialData.taxRates[1]} /></span>
                            </>} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="LOCAL_DEMAND_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="LOCAL_DEMAND" />} 
                            right={<LocalizedNumber value={industrialData.localDemand} />} 
                        />
                    </Tooltip>
                    <Tooltip tooltip={<LocalizedString id="INPUT_UTILIZATION_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="INPUT_UTILIZATION" />} 
                            right={<LocalizedNumber value={industrialData.inputUtilization} />} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="EMPLOYEE_CAPACITY_RATIO_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="EMPLOYEE_CAPACITY_RATIO" />} 
                            right={<>
                                <span><LocalizedNumber value={industrialData.employeeCapacityRatio[0]} /></span>
                                <span><LocalizedNumber value={industrialData.employeeCapacityRatio[1]} /></span>
                            </>} 
                        />
                    </Tooltip>
                    <InfoRow 
                        left={<LocalizedString id="AVAILABLE_WORKFORCE" />} 
                        right="" 
                        uppercase 
                    />
                    <InfoRow 
                        left={<LocalizedString id="EDUCATED" />} 
                        right={useFormattedLargeNumber(industrialData.availableWorkforce[0])} 
                        subRow 
                    />
                    <InfoRow 
                        left={<LocalizedString id="UNEDUCATED" />} 
                        right={useFormattedLargeNumber(industrialData.availableWorkforce[1])} 
                        subRow 
                    />
                </InfoSection>
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id="STORAGE" />} 
                        right="" 
                        uppercase 
                    />
                    <InfoRow 
                        left={<LocalizedString id="EMPTY_BUILDINGS" />} 
                        right={useFormattedLargeNumber(industrialData.storage.emptyBuildings)} 
                        subRow 
                    />
                    <InfoRow 
                        left={<LocalizedString id="PROPERTYLESS_COMPANIES" />} 
                        right={useFormattedLargeNumber(industrialData.storage.propertylessCompanies)} 
                        subRow 
                    />
                    <InfoRow 
                        left={<LocalizedString id="DEMANDED_TYPES" />} 
                        right={useFormattedLargeNumber(industrialData.storage.demandedTypes)} 
                        subRow 
                    />
                </InfoSection>
            </Scrollable>
        </Panel>
    );
};
