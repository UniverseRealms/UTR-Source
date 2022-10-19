﻿package kabam.rotmg.pets.view.dialogs {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.dialogs.Dialog;

import flash.events.Event;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.text.model.TextKey;

public class LeavePetYard extends PetDialog {

    private var gameSprite:GameSprite;
    private var closeDialogSignal:CloseDialogsSignal;

    public function LeavePetYard(_arg1:GameSprite) {
        this.closeDialogSignal = StaticInjectorContext.getInjector().getInstance(CloseDialogsSignal);
        this.gameSprite = _arg1;
        super("LeavePetYardDialog.title", "LeavePetYardDialog.text", TextKey.PET_DIALOG_REMAIN, TextKey.PET_DIALOG_LEAVE, null);
        addEventListener(Dialog.LEFT_BUTTON, this.onClose);
        addEventListener(Dialog.RIGHT_BUTTON, this.onExitToNexus);
        titleText_.setMultiLine(true);
    }

    private function onClose(_arg1:Event):void {
        this.closeDialogSignal.dispatch();
    }

    private function onExitToNexus(_arg1:Event):void {
        this.gameSprite.gsc_.escape();
        this.closeDialogSignal.dispatch();
    }


}
}
