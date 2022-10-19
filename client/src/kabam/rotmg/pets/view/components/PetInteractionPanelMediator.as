package kabam.rotmg.pets.view.components
{
import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;
import kabam.rotmg.dialogs.control.OpenDialogSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class PetInteractionPanelMediator extends Mediator {

    [Inject]
    public var view:PetInteractionPanel;
    [Inject]
    public var openNoModalDialog:OpenDialogNoModalSignal;
    [Inject]
    public var openDialog:OpenDialogSignal;


    override public function initialize():void {
        this.view.init();
    }

    override public function destroy():void {
        super.destroy();
    }
}
}
