package kabam.rotmg.application {
import com.company.assembleegameclient.parameters.Parameters;

import flash.display.LoaderInfo;

import kabam.rotmg.application.setup.ApplicationSetup;
import kabam.rotmg.application.setup.LocalhostSetup;
import kabam.rotmg.application.setup.ProductionSetup;
import kabam.rotmg.build.api.BuildData;

import org.swiftsuspenders.Injector;

import robotlegs.bender.framework.api.IConfig;

public class ApplicationConfig implements IConfig {

    [Inject]
    public var injector:Injector;
    [Inject]
    public var data:BuildData;
    [Inject]
    public var loaderInfo:LoaderInfo;

    public function configure():void {
        var app:ApplicationSetup;
        app = new ProductionSetup();
        //if(loaderInfo.parameters["build"] == "release" || Parameters.IS_AIR)
        //    app = new ProductionSetup();
        if(Parameters.DEV_MODE)
                app = new LocalhostSetup();

        this.injector.map(ApplicationSetup).toValue(app);
    }
}
}
