import React, { useState } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

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
    onClose: () => void;
}

export const CommercialPanel: React.FC<CommercialPanelProps> = ({ onClose }) => {
    const [commercialData, setCommercialData] = useState<CommercialData | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const rem = useRem();

    useDataUpdate("cityInfo.ilCommercial", (data) => {
        setCommercialData(data);
        setIsLoading(false);
    });

    if (isLoading) {
        return (
            <Panel title="COMMERCIAL_DATA" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    if (!commercialData) {
        return (
            <Panel title="COMMERCIAL_DATA" onClose={onClose}>
                <LocalizedString id="ERROR_LOADING_DATA" />
            </Panel>
        );
    }

    return (
        <Panel title="COMMERCIAL_DATA" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id="EMPTY_BUILDINGS" />} 
                        right={useFormattedLargeNumber(commercialData.emptyBuildings)} 
                    />
                    <InfoRow 
                        left={<LocalizedString id="PROPERTYLESS_COMPANIES" />} 
                        right={useFormattedLargeNumber(commercialData.propertylessCompanies)} 
                    />
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="TAX_RATE_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="TAX_RATE" />} 
                            right={<LocalizedNumber value={commercialData.taxRate} />} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id="SERVICE_UTILIZATION" />} 
                        right={<>
                            <span><LocalizedString id="STANDARD" />: <LocalizedNumber value={commercialData.serviceUtilization.standard} /></span>
                            <span><LocalizedString id="LEISURE" />: <LocalizedNumber value={commercialData.serviceUtilization.leisure} /></span>
                        </>} 
                    />
                    <InfoRow 
                        left={<LocalizedString id="SALES_CAPACITY" />} 
                        right={<>
                            <span><LocalizedString id="STANDARD" />: <LocalizedNumber value={commercialData.salesCapacity.standard} /></span>
                            <span><LocalizedString id="LEISURE" />: <LocalizedNumber value={commercialData.salesCapacity.leisure} /></span>
                        </>} 
                    />
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="EMPLOYEE_CAPACITY_RATIO_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="EMPLOYEE_CAPACITY_RATIO" />} 
                            right={<LocalizedNumber value={commercialData.employeeCapacityRatio} />} 
                        />
                    </Tooltip>
                    <InfoRow 
                        left={<LocalizedString id="AVAILABLE_WORKFORCE" />} 
                        right="" 
                        uppercase 
                    />
                    <InfoRow 
                        left={<LocalizedString id="EDUCATED" />} 
                        right={useFormattedLargeNumber(commercialData.availableWorkforce.educated)} 
                        subRow 
                    />
                    <InfoRow 
                        left={<LocalizedString id="UNEDUCATED" />} 
                        right={useFormattedLargeNumber(commercialData.availableWorkforce.uneducated)} 
                        subRow 
                    />
                </InfoSection>
            </Scrollable>
        </Panel>
    );
};
