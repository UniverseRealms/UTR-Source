﻿package kabam.rotmg.ui.commands {
import com.company.assembleegameclient.screens.AccountLoadingScreen;

import kabam.rotmg.core.signals.SetScreenSignal;

import robotlegs.bender.framework.api.ILogger;

public class ShowLoadingUICommand {

    [Inject]
    public var setScreen:SetScreenSignal;
    [Inject]
    public var logger:ILogger;


    public function execute():void {
        this.setScreen.dispatch(new AccountLoadingScreen());
    }


}
}
