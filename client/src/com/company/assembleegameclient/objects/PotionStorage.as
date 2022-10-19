package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.Panel;

import kabam.rotmg.potionStorage.PotionPanel;

public class PotionStorage extends GameObject implements IInteractiveObject {

    public function PotionStorage(_arg_1:XML) {
        super(_arg_1);
        isInteractive_ = true;
    }

    public function getPanel(_arg_1:GameSprite):Panel {
        return (new PotionPanel(_arg_1, objectType_));
    }


}
}


