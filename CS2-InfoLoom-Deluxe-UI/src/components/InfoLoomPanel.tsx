import React from 'react';
import { Panel, Button } from "cs2/ui";
import { WorkforcePanel } from "./WorkforcePanel";
import { WorkplacesPanel } from "./WorkplacesPanel";
import { DemographicsPanel } from "./DemographicsPanel";
import { DemandPanel } from "./DemandPanel";
import { TradePanel } from "./TradePanel";
import { ResidentialPanel } from "./ResidentialPanel";
import { CommercialPanel } from "./CommercialPanel";
import { IndustrialPanel } from "./IndustrialPanel";

export const InfoLoomPanel: React.FC = () => {
    const [isVisible, setIsVisible] = React.useState(false);
    const [visiblePanels, setVisiblePanels] = React.useState({
        demographics: true,
        workforce: true,
        workplaces: true,
        demand: true,
        residential: true,
        commercial: true,
        industrial: true,
        trade: true,
    });

    const togglePanel = (panel: keyof typeof visiblePanels) => {
        setVisiblePanels(prev => ({ ...prev, [panel]: !prev[panel] }));
    };

    if (!isVisible) {
        return <Button onClick={() => setIsVisible(true)}>Open InfoLoom</Button>;
    }

    return (
        <Panel title="InfoLoom" onClose={() => setIsVisible(false)}>
            {visiblePanels.demographics && <DemographicsPanel focusKey="demographics" onClose={() => togglePanel('demographics')} />}
            {visiblePanels.workforce && <WorkforcePanel focusKey="workforce" onClose={() => togglePanel('workforce')} />}
            {visiblePanels.workplaces && <WorkplacesPanel focusKey="workplaces" onClose={() => togglePanel('workplaces')} />}
            {visiblePanels.demand && <DemandPanel focusKey="demand" onClose={() => togglePanel('demand')} />}
            {visiblePanels.residential && <ResidentialPanel focusKey="residential" onClose={() => togglePanel('residential')} />}
            {visiblePanels.commercial && <CommercialPanel focusKey="commercial" onClose={() => togglePanel('commercial')} />}
            {visiblePanels.industrial && <IndustrialPanel focusKey="industrial" onClose={() => togglePanel('industrial')} />}
            {visiblePanels.trade && <TradePanel focusKey="trade" onClose={() => togglePanel('trade')} />}
        </Panel>
    );
};
