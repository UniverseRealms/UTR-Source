package kabam.rotmg.pets.view.components {
import flash.display.DisplayObject;
import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.market.ui.MarketOverview;
import kabam.rotmg.ui.view.HUDView;

import org.osflash.signals.Signal;

public class MarketCloseButton extends Sprite {

    public static var marketCloseButton:Class = DialogCloseButton_marketCloseButtonAsset;

    public const clicked:Signal = new Signal();
    public const closeClicked:Signal = new Signal();

    public var disabled:Boolean = false;

    public function MarketCloseButton(_arg1:Number = -1) {
        super();
        if (_arg1 < 0) {
            addChild(new marketCloseButton());
        }
        else {
            addChild(new marketCloseButton());
            scaleX = (scaleX * _arg1);
            scaleY = (scaleY * _arg1);
        }
        buttonMode = true;
        addEventListener(MouseEvent.CLICK, this.onClicked);
    }

    public function setDisabled(_arg1:Boolean):void {
        this.disabled = _arg1;
        if (_arg1) {
            removeEventListener(MouseEvent.CLICK, this.onClicked);
        }
        else {
            addEventListener(MouseEvent.CLICK, this.onClicked);
        }
    }

    public function disableLegacyCloseBehavior():void {
        this.disabled = true;
        removeEventListener(MouseEvent.CLICK, this.onClicked);
    }

    private function onClicked(_arg1:MouseEvent):void {
        if (!this.disabled) {
            MarketOverview.MarketPlaceOpened = false;
            removeEventListener(MouseEvent.CLICK, this.onClicked);
            this.closeClicked.dispatch();
            this.clicked.dispatch();
        }
    }


}
}
