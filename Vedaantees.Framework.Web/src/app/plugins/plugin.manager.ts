import {Plugin} from "./plugin";

export class PluginManager
{
    getType(name) {
        return Plugin.prototype.getType(name);
    }

    getProviders()
    {
        return Plugin.prototype.getProviders();
    }
}