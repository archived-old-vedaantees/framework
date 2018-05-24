export function Plugin(name, deps){

    return (constructor)=>{
        Plugin.prototype.register[name]={
                ctor:constructor,
                deps:deps || []
        };
    };
}

Plugin.prototype.registry = {};

Plugin.prototype.getProviders = ()=> {
    
    var registry = this.registry;
    return Object.keys(registry).map((key)=>{
        return {

            provider:key,
            useClass:registry[key].ctor,
            deps:registry[key].deps
        };
    });
};

Plugin.prototype.getPluginType = (name)=>  this.registry[name].ctor;
