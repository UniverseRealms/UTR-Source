package com.company.assembleegameclient.objects.components.forge {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.SorForger;
import com.company.assembleegameclient.ui.panels.ButtonPanel;
import flash.events.MouseEvent;

import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;

import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.memMarket.MemMarket;
import kabam.rotmg.sorForge.SorForgerUI;

public class ForgePanel extends ButtonPanel {

    [Inject]
    public var openNoModalDialog:OpenDialogNoModalSignal;

    public function ForgePanel(gs:GameSprite) {
        super(gs, "Anvil", "Open");
    }

    override protected function onButtonClick(event:MouseEvent) : void
    {
        this.openNoModalDialog.dispatch(new SorForgerUI(this.gs_));
    }
}
}
