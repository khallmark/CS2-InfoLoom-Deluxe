import React, { useState } from 'react';
import { Panel, InfoRow, InfoSection, Tooltip, Scrollable } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { useRem, useFormattedLargeNumber } from "cs2/utils";
import { InputActionHints } from "cs2/input";

interface ResidentialData {
    properties: { low: number; medium: number; high: number };
    occupied: { low: number; medium: number; high: number };
    freeRatio: number;
    studyPositions: number;
    happiness: { current: number; neutral: number };
    unemployment: { current: number; neutral: number };
    homeless: number;
    households: number;
    homelessThreshold: number;
    taxRate: number;
    householdDemand: number;
    studentChance: number;
}

interface ResidentialPanelProps {
    onClose: () => void;
}

export const ResidentialPanel: React.FC<ResidentialPanelProps> = ({ onClose }) => {
    const [residentialData, setResidentialData] = useState<ResidentialData | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const rem = useRem();

    useDataUpdate("cityInfo.ilResidential", (data) => {
        setResidentialData(data);
        setIsLoading(false);
    });

    if (isLoading) {
        return (
            <Panel title="RESIDENTIAL_DATA" onClose={onClose}>
                <LocalizedString id="LOADING" />
            </Panel>
        );
    }

    if (!residentialData) {
        return (
            <Panel title="RESIDENTIAL_DATA" onClose={onClose}>
                <LocalizedString id="ERROR_LOADING_DATA" />
            </Panel>
        );
    }

    return (
        <Panel title="RESIDENTIAL_DATA" onClose={onClose}>
            <InputActionHints />
            <Scrollable style={{ maxHeight: `${30 * rem}px` }}>
                <InfoSection>
                    <InfoRow left="" right={<><span><LocalizedString id="LOW" /></span><span><LocalizedString id="MEDIUM" /></span><span><LocalizedString id="HIGH" /></span></>} uppercase />
                    <InfoRow 
                        left={<LocalizedString id="TOTAL_PROPERTIES" />} 
                        right={<>
                            <span>{useFormattedLargeNumber(residentialData.properties.low)}</span>
                            <span>{useFormattedLargeNumber(residentialData.properties.medium)}</span>
                            <span>{useFormattedLargeNumber(residentialData.properties.high)}</span>
                        </>} 
                    />
                    <InfoRow 
                        left={<LocalizedString id="OCCUPIED_PROPERTIES" />} 
                        right={<>
                            <span>{useFormattedLargeNumber(residentialData.occupied.low)}</span>
                            <span>{useFormattedLargeNumber(residentialData.occupied.medium)}</span>
                            <span>{useFormattedLargeNumber(residentialData.occupied.high)}</span>
                        </>} 
                        subRow 
                    />
                    <InfoRow 
                        left={<LocalizedString id="EMPTY_PROPERTIES" />} 
                        right={<>
                            <span>{useFormattedLargeNumber(residentialData.properties.low - residentialData.occupied.low)}</span>
                            <span>{useFormattedLargeNumber(residentialData.properties.medium - residentialData.occupied.medium)}</span>
                            <span>{useFormattedLargeNumber(residentialData.properties.high - residentialData.occupied.high)}</span>
                        </>} 
                    />
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="FREE_RATIO_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="FREE_RATIO" />} 
                            right={<LocalizedNumber value={residentialData.freeRatio} />} 
                        />
                    </Tooltip>
                    <Tooltip tooltip={<LocalizedString id="HOUSEHOLD_DEMAND_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="HOUSEHOLD_DEMAND" />} 
                            right={<LocalizedNumber value={residentialData.householdDemand} />} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <InfoRow 
                        left={<LocalizedString id="STUDY_POSITIONS" />} 
                        right={useFormattedLargeNumber(residentialData.studyPositions)} 
                    />
                    <Tooltip tooltip={<LocalizedString id="STUDENT_CHANCE_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="STUDENT_CHANCE" />} 
                            right={<LocalizedNumber value={residentialData.studentChance} />} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="HAPPINESS_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="HAPPINESS" />} 
                            right={<LocalizedNumber value={residentialData.happiness.current} />} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="UNEMPLOYMENT_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="UNEMPLOYMENT" />} 
                            right={<LocalizedNumber value={residentialData.unemployment.current} />} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="HOMELESS_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="HOMELESS" />} 
                            right={useFormattedLargeNumber(residentialData.homeless)} 
                        />
                    </Tooltip>
                </InfoSection>
                <InfoSection>
                    <Tooltip tooltip={<LocalizedString id="TAX_RATE_DESC" />}>
                        <InfoRow 
                            left={<LocalizedString id="TAX_RATE" />} 
                            right={<LocalizedNumber value={residentialData.taxRate} />} 
                        />
                    </Tooltip>
                </InfoSection>
            </Scrollable>
        </Panel>
    );
};
