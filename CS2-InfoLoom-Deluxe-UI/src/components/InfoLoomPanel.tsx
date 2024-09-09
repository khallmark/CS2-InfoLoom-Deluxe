import React, { useState } from 'react';
import { Panel, Button } from "cs2/ui";
import { LocalizedString } from "cs2/l10n";
import { WorkforcePanel } from "./WorkforcePanel";
import { WorkplacesPanel } from "./WorkplacesPanel";
import { DemographicsPanel } from "./DemographicsPanel";
import { DemandPanel } from "./DemandPanel";
import { TradePanel } from "./TradePanel";
import { ResidentialPanel } from "./ResidentialPanel";
import { CommercialPanel } from "./CommercialPanel";
import { IndustrialPanel } from "./IndustrialPanel";
import { ConsumptionPanel } from "./ConsumptionPanel";

export const InfoLoomPanel: React.FC = () => {
    const [isVisible, setIsVisible] = useState(false);
    const [visiblePanels, setVisiblePanels] = useState({
        demographics: false,
        workforce: false,
        workplaces: false,
        demand: false,
        residential: false,
        commercial: false,
        industrial: false,
        trade: false,
        consumption: false,
    });

    const togglePanel = (panel: keyof typeof visiblePanels) => {
        setVisiblePanels(prev => ({ ...prev, [panel]: !prev[panel] }));
    };

    if (!isVisible) {
        return <Button onClick={() => setIsVisible(true)}><LocalizedString id="OPEN_INFOLOOM" /></Button>;
    }

    return (
        <Panel title="INFOLOOM" onClose={() => setIsVisible(false)}>
            {Object.entries(visiblePanels).map(([key, value]) => (
                <Button key={key} onClick={() => togglePanel(key as keyof typeof visiblePanels)}>
                    <LocalizedString id={`TOGGLE_${key.toUpperCase()}`} />
                </Button>
            ))}
            {visiblePanels.demographics && <DemographicsPanel onClose={() => togglePanel('demographics')} />}
            {visiblePanels.workforce && <WorkforcePanel onClose={() => togglePanel('workforce')} />}
            {visiblePanels.workplaces && <WorkplacesPanel onClose={() => togglePanel('workplaces')} />}
            {visiblePanels.demand && <DemandPanel onClose={() => togglePanel('demand')} />}
            {visiblePanels.residential && <ResidentialPanel onClose={() => togglePanel('residential')} />}
            {visiblePanels.commercial && <CommercialPanel onClose={() => togglePanel('commercial')} />}
            {visiblePanels.industrial && <IndustrialPanel onClose={() => togglePanel('industrial')} />}
            {visiblePanels.trade && <TradePanel onClose={() => togglePanel('trade')} />}
            {visiblePanels.consumption && <ConsumptionPanel onClose={() => togglePanel('consumption')} />}
        </Panel>
    );
};
