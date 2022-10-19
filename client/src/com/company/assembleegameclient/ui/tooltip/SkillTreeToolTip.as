package com.company.assembleegameclient.ui.tooltip {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.misc.UILabel;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.LineBreakDesign;
import com.company.assembleegameclient.util.FilterUtil;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.TierUtil;
import com.company.ui.SimpleText;
import com.company.util.AssetLibrary;
import com.company.util.BitmapUtil;
import com.company.util.KeyCodes;
import com.company.util.MoreStringUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.events.TimerEvent;
import flash.filters.DropShadowFilter;
import flash.utils.Timer;

import kabam.rotmg.constants.ActivationType;
import kabam.rotmg.messaging.impl.data.StatData;
import kabam.rotmg.skilltree.ui.SkillTreeButton;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.text.view.stringBuilder.StringBuilder;

public class SkillTreeToolTip extends ToolTip {

    private static const MAX_WIDTH:int = 230;
    private var Text:String;
    private var Size:int;
    private var TextBox:SimpleText;
    private var DescBox:SimpleText;
    private var PurchasedBox:SimpleText;
    private var ButtonData:SkillTreeButton;
    private const dropFilter:DropShadowFilter = new DropShadowFilter(0,0,0,0.5,6,6);

    public function SkillTreeToolTip(button:SkillTreeButton, size:int) {
        this.ButtonData = button;
        this.Text = GetTitle();
        this.Size = size;
        var backgroundColor:uint = 0x363636;
        var outlineColor:uint = 0x9B9B9B;
        super(backgroundColor, 1, outlineColor, 1, true);
        makeText();
        makeDesc();
        makePurchased();
    }

    private function GetTitle():String
    {
        switch(this.ButtonData.buttonType)
        {
            case -1:
                return "Close" + this.ButtonData.buttonId.toString();
            case 0:
                return "Bronze" + this.ButtonData.buttonId.toString();
            case 1:
                return "Silver" + this.ButtonData.buttonId.toString();
            case 2:
                return "Gold" + this.ButtonData.buttonId.toString();

        }
        return "Empty";
    }

    private function makeText():void
    {
        this.TextBox = new SimpleText(this.Size + 2, 11776947, false, MAX_WIDTH, 0);
        this.TextBox.wordWrap = true;
        this.TextBox.text = this.Text;
        this.TextBox.updateMetrics();
        this.TextBox.filters = [dropFilter];
        this.TextBox.x = 4;
        this.TextBox.y = 2;
        addChild(this.TextBox);
    }

    private function makeDesc():void
    {
        this.DescBox = new SimpleText(this.Size, 11776947, false, MAX_WIDTH, 0);
        this.DescBox.wordWrap = true;
        this.DescBox.text = ButtonData.desc;
        this.DescBox.updateMetrics();
        this.DescBox.filters = [dropFilter];
        this.DescBox.x = this.TextBox.x;
        this.DescBox.y = this.TextBox.y + 10 + this.DescBox.height;
        addChild(this.DescBox);
    }

    private function makePurchased():void
    {
        this.PurchasedBox = new SimpleText(this.Size - 2, 8879221, false, MAX_WIDTH, 0);
        this.PurchasedBox.wordWrap = true;
        this.PurchasedBox.text = "Purchased : " + ButtonData.purchased + "\nLocked : " + ButtonData.SSButton.disabled;
        this.PurchasedBox.updateMetrics();
        this.PurchasedBox.filters = [dropFilter];
        this.PurchasedBox.x = this.TextBox.x;
        this.PurchasedBox.y = this.DescBox.y + 10 + this.PurchasedBox.height;
        addChild(this.PurchasedBox);
    }

}
}
