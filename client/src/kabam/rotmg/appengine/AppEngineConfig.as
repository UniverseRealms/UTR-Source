package kabam.rotmg.appengine {
import kabam.rotmg.appengine.api.AppEngineClient;
import kabam.rotmg.appengine.api.RetryLoader;
import kabam.rotmg.appengine.impl.AppEngineRetryLoader;
import kabam.rotmg.appengine.impl.SimpleAppEngineClient;

import org.swiftsuspenders.Injector;

import robotlegs.bender.framework.api.IConfig;

public class AppEngineConfig implements IConfig {

    [Inject]
    public var injector:Injector;


    public function configure():void {
        this.injector.map(RetryLoader).toType(AppEngineRetryLoader);
        this.injector.map(AppEngineClient).toType(SimpleAppEngineClient);
    }
}
}
