import React from 'react';
import { bindValue, useValue } from "cs2/api";

export const useDataUpdate = (event: string, onUpdate: (data: any) => void) => {
    React.useEffect(() => {
        const binding = bindValue(event, null as any);
        const subscription = binding.subscribe((value) => {
            onUpdate(value);
        });

        return () => {
            subscription.dispose();
            binding.dispose();
        };
    }, [event, onUpdate]);
};

// No changes needed for the useDataUpdate hook
