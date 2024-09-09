import React, { useState } from 'react';
import { Panel, DataView, Section } from "cs2/ui";
import { LocalizedNumber, LocalizedString } from "cs2/l10n";
import { useValue } from "cs2/api";

interface Good {
    name: string;
    industrialSupply: number;
    industrialDemand: number;
    commercialSupply: number;
    commercialDemand: number;
    surplus: number;
    importExport: number;
}

interface ConsumptionData {
    goods: Good[];
}

const GoodSection: React.FC<{ good: Good }> = ({ good }) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <Section title={good.name} isOpen={isOpen} onToggle={() => setIsOpen(!isOpen)}>
            <DataView
                data={[
                    { label: <LocalizedString id="INDUSTRIAL_SUPPLY" />, value: <LocalizedNumber value={good.industrialSupply} /> },
                    { label: <LocalizedString id="INDUSTRIAL_DEMAND" />, value: <LocalizedNumber value={good.industrialDemand} /> },
                    { label: <LocalizedString id="COMMERCIAL_SUPPLY" />, value: <LocalizedNumber value={good.commercialSupply} /> },
                    { label: <LocalizedString id="COMMERCIAL_DEMAND" />, value: <LocalizedNumber value={good.commercialDemand} /> },
                    { label: <LocalizedString id="SURPLUS_DEFICIT" />, value: <LocalizedNumber value={good.surplus} /> },
                    { label: <LocalizedString id="IMPORT_EXPORT" />, value: <LocalizedNumber value={good.importExport} /> },
                ]}
            />
        </Section>
    );
};

export const ConsumptionPanel: React.FC = () => {
    const consumptionData = useValue<ConsumptionData>("infoLoomPanel", "consumptionData");

    return (
        <Panel title={<LocalizedString id="CONSUMPTION_OVERVIEW" />}>
            {consumptionData?.goods.map((good, index) => (
                <GoodSection key={index} good={good} />
            ))}
        </Panel>
    );
};
