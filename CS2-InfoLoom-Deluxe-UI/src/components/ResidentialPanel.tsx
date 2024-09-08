import React from 'react';
import { Panel, InfoRow, InfoSection } from "cs2/ui";
import { useDataUpdate } from "../hooks/useDataUpdate";
import { FocusKey } from "cs2/ui";

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
    focusKey?: FocusKey;
    onClose: () => void;
}

export const ResidentialPanel: React.FC<ResidentialPanelProps> = ({ focusKey, onClose }) => {
    const [residentialData, setResidentialData] = React.useState<ResidentialData | null>(null);

    useDataUpdate("cityInfo.ilResidential", setResidentialData);

    if (!residentialData) return null;

    const BuildingDemandSection = () => (
        <InfoSection>
            <InfoRow left="" right={<><span>LOW</span><span>MEDIUM</span><span>HIGH</span></>} uppercase />
            <InfoRow left="Total properties" right={<>
                <span>{residentialData.properties.low}</span>
                <span>{residentialData.properties.medium}</span>
                <span>{residentialData.properties.high}</span>
            </>} />
            <InfoRow left="Occupied properties" right={<>
                <span>{residentialData.occupied.low}</span>
                <span>{residentialData.occupied.medium}</span>
                <span>{residentialData.occupied.high}</span>
            </>} subRow />
            <InfoRow left="Empty properties" right={<>
                <span>{residentialData.properties.low - residentialData.occupied.low}</span>
                <span>{residentialData.properties.medium - residentialData.occupied.medium}</span>
                <span>{residentialData.properties.high - residentialData.occupied.high}</span>
            </>} />
            <InfoRow 
                left={<>No demand at {residentialData.freeRatio}%</>}
                right={<>
                    <span>{Math.max(1, Math.floor(residentialData.freeRatio * residentialData.properties.low / 100))}</span>
                    <span>{Math.max(1, Math.floor(residentialData.freeRatio * residentialData.properties.medium / 100))}</span>
                    <span>{Math.max(1, Math.floor(residentialData.freeRatio * residentialData.properties.high / 100))}</span>
                </>}
                subRow
            />
            <InfoRow left="BUILDING DEMAND" right={<>
                <span>{Math.max(0, Math.floor((1 - (residentialData.properties.low - residentialData.occupied.low) / Math.max(1, Math.floor(residentialData.freeRatio * residentialData.properties.low / 100))) * 100))}%</span>
                <span>{Math.max(0, Math.floor((1 - (residentialData.properties.medium - residentialData.occupied.medium) / Math.max(1, Math.floor(residentialData.freeRatio * residentialData.properties.medium / 100))) * 100))}%</span>
                <span>{Math.max(0, Math.floor((1 - (residentialData.properties.high - residentialData.occupied.high) / Math.max(1, Math.floor(residentialData.freeRatio * residentialData.properties.high / 100))) * 100))}%</span>
            </>} />
        </InfoSection>
    );

    return (
        <Panel title="Residential Data" focusKey={focusKey} onClose={onClose}>
            <BuildingDemandSection />
            
            <InfoSection>
                <InfoRow left="STUDY POSITIONS" right={residentialData.studyPositions} />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left="HAPPINESS" 
                    right={`${residentialData.happiness.current}`}
                    tooltip={`${residentialData.happiness.neutral} is neutral`}
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left="UNEMPLOYMENT" 
                    right={`${residentialData.unemployment.current}%`}
                    tooltip={`${(residentialData.unemployment.neutral / 10).toFixed(1)}% is neutral`}
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left="HOMELESS" 
                    right={residentialData.homeless}
                    tooltip={`${residentialData.homelessThreshold} is neutral for ${residentialData.households} households`}
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow 
                    left="TAX RATE (weighted)" 
                    right={`${(residentialData.taxRate / 10).toFixed(1)}%`}
                    tooltip="10% is neutral"
                />
            </InfoSection>
            
            <InfoSection>
                <InfoRow left="HOUSEHOLD DEMAND" right={`${residentialData.householdDemand}%`} />
            </InfoSection>
            
            <InfoSection>
                <InfoRow left="STUDENT CHANCE" right={`${residentialData.studentChance}%`} />
            </InfoSection>
        </Panel>
    );
};
