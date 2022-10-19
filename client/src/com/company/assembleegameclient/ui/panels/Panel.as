package com.company.assembleegameclient.ui.panels {
import com.company.assembleegameclient.game.GameSprite;

import flash.display.Sprite;

public class Panel extends Sprite {

    public static const WIDTH:int = (200 - 12);//188
    public static const HEIGHT:int = (100 - 16);//84

    public var gs_:GameSprite;

    public function Panel(_arg1:GameSprite) {
        this.gs_ = _arg1;
    }

    public function draw():void {
    }


}
}
