package kabam.rotmg.sorForge.components {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.panels.Panel;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.AssetLibrary;
import com.company.util.BitmapUtil;
import com.company.util.ImageSet;
import com.hurlant.util.der.Integer;

import flash.display.Bitmap;
import flash.display.BitmapData;

import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.sorForge.SorForge_ImageEmbed;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

import mx.controls.Text;

public class SorForgePanel extends Panel {

    private const titleText:TextFieldDisplayConcrete = PetsViewAssetFactory.returnTextfield(0xFFFFFF, 16, true);

    internal var sorIcon:SorForge_ImageEmbed;
    internal var sacredIcon:BitmapData = TextureRedrawer.resize(AssetLibrary.getImageFromSet("lofiObjSc", 116), null, 100, true, 0, 0);
    private var icon:Bitmap;
    private var title:String = "Sacred Forge";
    private var buttonText:String = "Forge";
    private var objectType:int;
    internal var forgeButton:DeprecatedTextButton;

    public function SorForgePanel(_arg_1:GameSprite, _arg_2:int) {
        super(_arg_1);
        this.objectType = _arg_2;
        this.titleText.setStringBuilder(new LineBuilder().setParams(this.title));
        this.titleText.x = 48;
        this.titleText.y = 28;
        addChild(this.titleText);
        this.forgeButton = new DeprecatedTextButton(16, this.buttonText);
        this.forgeButton.textChanged.addOnce(this.alignButton);
        addChild(this.forgeButton);
    }

    private static function cropAndGlowIcon(_arg_1:BitmapData):BitmapData {
        _arg_1 = GlowRedrawer.outlineGlow(_arg_1, 0xFFFFFF);
        return (BitmapUtil.cropToBitmapData(_arg_1, 10, 10, (_arg_1.width - 20), (_arg_1.height - 20)));
    }

    public function init():void {
        this.sorIcon = new SorForge_ImageEmbed();
        this.sorIcon.x = 60;
        this.sorIcon.y = 52;
        this.sorIcon.scaleX = 1.6;
        this.sorIcon.scaleY = 1.8;
        addChild(this.sorIcon);
        this.icon = new Bitmap(cropAndGlowIcon(this.sacredIcon));
        this.icon.x = -4;
        this.icon.y = -8;
        addChild(this.icon);
    }

    private function alignButton():void {
        this.forgeButton.x = ((WIDTH / 2) - (this.forgeButton.width / 2));
        this.forgeButton.y = ((HEIGHT - this.forgeButton.height) - 4);
    }


}
}//package kabam.rotmg.pets.view.components
