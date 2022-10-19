package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.game.view.SorDisplay;
import kabam.rotmg.game.view.SorDisplayFire;
import kabam.rotmg.game.view.SorDisplayWater;
import kabam.rotmg.game.view.SorDisplayEarth;
import kabam.rotmg.game.view.SorDisplayAir;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.model.TabStripModel;

public class SorTabContent extends Sprite {
    private var background:Sprite;
    private var storageText:TextFieldDisplayConcrete;
    public var sorDisplay:SorDisplay;
    public var airDisplay:SorDisplayAir;
    public var waterDisplay:SorDisplayWater;
    public var earthDisplay:SorDisplayEarth;
    public var fireDisplay:SorDisplayFire;
    private var player:Player;

    public function SorTabContent(plr:Player) {
        this.player = plr;
        this.background = new Sprite();
        this.storageText = new TextFieldDisplayConcrete().setSize(16).setColor(0xFFFFFF).setBold(true);
        this.storageText.setStringBuilder(new StaticStringBuilder("Storage"));
        this.storageText.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4, 2)];

        super();

        this.addChildren();
        addChild(this.background);

        this.init();
        this.positionChildren();
        this.showStorageDisplay();
    }

    private function addChildren():void {
        this.background.addChild(this.storageText);
    }

    private function init():void {
        this.background.name = TabStripModel.PETS;
    }

    private function showStorageDisplay():void {
        this.sorDisplay = new SorDisplay(player);
        this.sorDisplay.x = this.storageText.x;
        this.sorDisplay.y = this.storageText.y + 20;
        addChild(this.sorDisplay);

        this.airDisplay = new SorDisplayAir(player);
        this.airDisplay.x = this.sorDisplay.x + 40;
        this.airDisplay.y = this.sorDisplay.y;
        addChild(this.airDisplay);

        this.waterDisplay = new SorDisplayWater(player);
        this.waterDisplay.x = this.sorDisplay.x + 80;
        this.waterDisplay.y = this.sorDisplay.y;
        addChild(this.waterDisplay);

        this.earthDisplay = new SorDisplayEarth(player);
        this.earthDisplay.x = this.sorDisplay.x + 40;
        this.earthDisplay.y = this.sorDisplay.y + 40;
        addChild(this.earthDisplay);

        this.fireDisplay = new SorDisplayFire(player);
        this.fireDisplay.x = this.sorDisplay.x + 80;
        this.fireDisplay.y = this.sorDisplay.y + 40;
        addChild(this.fireDisplay);
    }

    private function positionChildren():void {
        this.storageText.x = this.storageText.x + 10;
    }

}
}
