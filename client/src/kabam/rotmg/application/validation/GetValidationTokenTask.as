package kabam.rotmg.application.validation {
import kabam.lib.tasks.BaseTask;
import kabam.rotmg.appengine.api.AppEngineClient;
import kabam.rotmg.application.setup.ApplicationSetup;
import kabam.rotmg.core.StaticInjectorContext;

import robotlegs.bender.framework.api.ILogger;
import robotlegs.bender.framework.impl.Logger;

public class GetValidationTokenTask extends BaseTask {
    [Inject]
    public var client:AppEngineClient;
    [Inject]
    private var logger:Logger = StaticInjectorContext.getInjector().getInstance(ILogger);

    override protected function startTask():void {
        var isProduction:String = (WebMain.ENV == "production").toString();
        var dns:String =  StaticInjectorContext.getInjector().getInstance(ApplicationSetup).getAppEngineUrl();

        this.logger.debug("[Production: {0}] Connecting to AppEngine DNS '{1}'.", [isProduction, dns]);

        this.client.setMaxRetries(2);
        this.client.complete.addOnce(this.onComplete);
        this.client.sendRequest("/app/getToken", null);
    }

    private function onComplete(success:Boolean, data:*):void {
        this.logger.debug("Response from request '/app/getToken':\n- Status: {0};\n- Data: {1}", [success ? "success" : "failed", data]);

        if (success)
            WebMain.TOKEN = data;

        completeTask(success, data);

    }
}
}
