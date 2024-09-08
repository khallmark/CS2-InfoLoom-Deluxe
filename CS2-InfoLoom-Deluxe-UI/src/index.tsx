import { ModRegistrar } from "cs2/modding";
import { InfoLoomPanel } from "./components/InfoLoomPanel";

const register: ModRegistrar = (moduleRegistry) => {
    // Add the main InfoLoom panel to the game menu
    moduleRegistry.append('Menu', InfoLoomPanel);
}

export default register;
