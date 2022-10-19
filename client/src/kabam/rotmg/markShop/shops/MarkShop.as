package kabam.rotmg.markShop.shops {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.util.Currency;

import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.core.StaticInjectorContext;

import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;

import kabam.rotmg.markShop.MarkDisplay;
import kabam.rotmg.markShop.NodeDisplay;
import kabam.rotmg.util.components.LegacyBuyButton;

public class MarkShop extends Sprite {

    private const background:ShopBackground = new ShopBackground();
    private var shop:Sprite;
    private var player:Player;
    private var newMark:MarkDisplay;
    private var currMark:MarkDisplay;

    public var closeButton:DeprecatedTextButton;
    public var buyButton:LegacyBuyButton;
    public var selectedMark:int;

    public function MarkShop(p:Player): void {
        this.player = p;
        this.shop = new Sprite();
        selectedMark = 0;
    }

    public function makeShopUI():void {
        shop.addChild(this.background);
        this.setupMarks();
        addChild(this.shop);
        this.centerModal();
    }

    private function setupMarks():void {
        var id:int = 0;
        for(var y:int = 0; y < 4; y++)
            for(var x:int = 0; x < 2; x++){
                if(id == 7) id = 12;
                var MarkIcon:MarkDisplay = new MarkDisplay(id);
                id++;
                MarkIcon.x = 35 + 204 * x;
                MarkIcon.y = 45 + 88 * y;
                MarkIcon.addEventListener(MouseEvent.CLICK, this.selectMark);
                shop.addChild(MarkIcon);
            }

        currMark = new MarkDisplay(this.player.mark_);
        currMark.x = 159 - 22;
        currMark.y = 89;
        shop.addChild(currMark);

        newMark = new MarkDisplay(0);
        newMark.x = 159 - 22;
        newMark.y = 221 + 44;
        shop.addChild(newMark);

        this.buyButton = new LegacyBuyButton("", 12, 40, Currency.ONRANE);
        this.buyButton.x = 156;
        this.buyButton.y = 271 + 84;
        this.buyButton.setEnabled(true);
        this.validateButton();
        shop.addChild(this.buyButton);

        this.closeButton = new DeprecatedTextButton(14, "Close");
        this.closeButton.x = 154;
        this.closeButton.y = 271 + 84 + 28;
        this.closeButton.addEventListener(MouseEvent.CLICK, this.onClose);
        shop.addChild(this.closeButton);
    }

    private function validateButton():void {
        if(currMark.getMarkId() != 0) {
            this.buyButton.setPrice(20, Currency.ONRANE);
        }
        else {
            this.buyButton.setPrice(40, Currency.ONRANE);
        }

        if(currMark.getMarkId() == newMark.getMarkId())
            this.buyButton.setEnabled(false);
        else
            this.buyButton.setEnabled(true);
    }

    private function selectMark(e:MouseEvent):void {
        SoundEffectLibrary.play("button_click");
        shop.removeChild(newMark);
        selectedMark = e.target.getMarkId();
        newMark = new MarkDisplay(e.target.getMarkId());
        newMark.x = 159 - 22;
        newMark.y = 221 + 44;
        this.validateButton();
        shop.addChild(newMark);
    }

    private function centerModal():void {
        this.scaleX = 800 / stage.stageWidth;
        this.scaleY = 600 / stage.stageHeight;
        this.x = (((WebMain.STAGE.stageWidth - 200) / 2) - (this.width / 2));
        this.y = ((WebMain.STAGE.stageHeight / 2) - (this.height / 2));
    }

    private function onClose(e:MouseEvent):void {
        if(Boolean(parent) && Boolean(parent.contains(this)))
        {
            SoundEffectLibrary.play("button_click");
            parent.removeChild(this);
        }
    }

    public function updateShop(m:int):void {
        shop.removeChild(this.currMark);
        currMark = new MarkDisplay(m);
        currMark.x = 159 - 22;
        currMark.y = 89;
        shop.addChild(currMark);
        this.validateButton();
    }
}
}
