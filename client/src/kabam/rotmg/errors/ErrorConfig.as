package kabam.rotmg.errors {
import kabam.rotmg.errors.control.ErrorSignal;
import kabam.rotmg.errors.control.LogErrorCommand;
import kabam.rotmg.errors.view.ErrorMediator;

import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;
import robotlegs.bender.framework.api.IConfig;

public class ErrorConfig implements IConfig {

    [Inject]
    public var mediatorMap:IMediatorMap;
    [Inject]
    public var commandMap:ISignalCommandMap;

    public function configure():void {
        this.mediatorMap.map(WebMain).toMediator(ErrorMediator);
        this.commandMap.map(ErrorSignal).toCommand(LogErrorCommand);
    }
}
}
