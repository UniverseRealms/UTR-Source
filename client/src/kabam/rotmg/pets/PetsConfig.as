package kabam.rotmg.pets {
import com.company.assembleegameclient.ui.dialogs.DialogCloser;
import com.company.assembleegameclient.ui.dialogs.DialogCloserMediator;

import kabam.rotmg.market.MarketNPCPanel;
import kabam.rotmg.market.MarketNPCPanelMediator;
import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.pets.view.components.DialogCloseButton;
import kabam.rotmg.pets.view.components.DialogCloseButtonMediator;
import kabam.rotmg.pets.view.components.PetInteractionPanel;
import kabam.rotmg.pets.view.components.PetInteractionPanelMediator;

import org.swiftsuspenders.Injector;

import robotlegs.bender.extensions.commandCenter.api.ICommandCenter;
import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;
import robotlegs.bender.framework.api.IConfig;

public class PetsConfig implements IConfig {

    [Inject]
    public var injector:Injector;
    [Inject]
    public var mediatorMap:IMediatorMap;
    [Inject]
    public var commandMap:ISignalCommandMap;
    [Inject]
    public var commandCenter:ICommandCenter;


    public function configure():void {
        this.injector.map(PetsViewAssetFactory).asSingleton();
        this.mediatorMap.map(MarketNPCPanel).toMediator(MarketNPCPanelMediator);
        this.mediatorMap.map(PetInteractionPanel).toMediator(PetInteractionPanelMediator);
        this.mediatorMap.map(DialogCloseButton).toMediator(DialogCloseButtonMediator);
        this.mediatorMap.map(DialogCloser).toMediator(DialogCloserMediator);
    }


}
}
