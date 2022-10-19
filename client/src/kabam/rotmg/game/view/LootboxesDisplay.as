﻿﻿package kabam.rotmg.game.view {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.assets.services.IconFactory;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.SignalWaiter;

import org.osflash.signals.Signal;

public class LootboxesDisplay extends Sprite {

    private static const FONT_SIZE:int = 18;
    public static const IMAGE_NAME:String = "legendaries8x8Embed";
    public static const IMAGE_ID:int = 16;
    public static const waiter:SignalWaiter = new SignalWaiter();

    public var lootbox1Text:TextFieldDisplayConcrete;
    public var lootbox2Text:TextFieldDisplayConcrete;
    public var lootbox3Text:TextFieldDisplayConcrete;
    public var lootbox4Text:TextFieldDisplayConcrete;
    public var lootbox7Text:TextFieldDisplayConcrete;
    public var lootbox1Icon:Bitmap;
    public var lootbox2Icon:Bitmap;
    public var lootbox3Icon:Bitmap;
    public var lootbox4Icon:Bitmap;
    public var lootbox7Icon:Bitmap;
    private var lootBox1_:int = -1;
    private var lootBox2_:int = -1;
    private var lootBox3_:int = -1;
    private var lootBox4_:int = -1;
    private var lootBox7_:int = -1;
    private var gs:GameSprite;
    public var openAccountDialog:Signal;

    public function LootboxesDisplay(_arg_1:GameSprite = null) {
        this.openAccountDialog = new Signal();
        super();
        this.gs = _arg_1;
        this.lootbox1Text = this.makeTextField();
        waiter.push(this.lootbox1Text.textChanged);
        addChild(this.lootbox1Text);
        var _local_5:BitmapData = AssetLibrary.getImageFromSet(IMAGE_NAME, IMAGE_ID);
        _local_5 = TextureRedrawer.redraw(_local_5, 40, true, 0);
        this.lootbox1Icon = new Bitmap(_local_5);
        addChild(this.lootbox1Icon);

            this.lootbox2Text = this.makeTextField();
            waiter.push(this.lootbox2Text.textChanged);
            addChild(this.lootbox2Text);
        var _local_8:BitmapData = AssetLibrary.getImageFromSet(IMAGE_NAME, 17);
        _local_8 = TextureRedrawer.redraw(_local_8, 40, true, 0);
            this.lootbox2Icon = new Bitmap(_local_8);
            addChild(this.lootbox2Icon);

        this.lootbox3Text = this.makeTextField();
        waiter.push(this.lootbox3Text.textChanged);
        addChild(this.lootbox3Text);
        var _local_6:BitmapData = AssetLibrary.getImageFromSet(IMAGE_NAME, 18);
        _local_6 = TextureRedrawer.redraw(_local_6, 40, true, 0);
        this.lootbox3Icon = new Bitmap(_local_6);
        addChild(this.lootbox3Icon);

        this.lootbox4Text = this.makeTextField();
        waiter.push(this.lootbox4Text.textChanged);
        addChild(this.lootbox4Text);
        var _local_7:BitmapData = AssetLibrary.getImageFromSet(IMAGE_NAME, 19);
        _local_7 = TextureRedrawer.redraw(_local_7, 40, true, 0);
        this.lootbox4Icon = new Bitmap(_local_7);
        addChild(this.lootbox4Icon);

        this.lootbox7Text = this.makeTextField();
        waiter.push(this.lootbox7Text.textChanged);
        addChild(this.lootbox7Text);
        var _local_9:BitmapData = AssetLibrary.getImageFromSet(IMAGE_NAME, 459);
        _local_9 = TextureRedrawer.redraw(_local_9, 40, true, 0);
        this.lootbox7Icon = new Bitmap(_local_9);
        addChild(this.lootbox7Icon);

        this.draw(0, 0, 0, 0, 0);
        mouseEnabled = true;
        doubleClickEnabled = true;
        addEventListener(MouseEvent.DOUBLE_CLICK, this.onDoubleClick, false, 0, true);
        waiter.complete.add(this.onAlignHorizontal);
    }

    private function onAlignHorizontal():void {



    }

    private function onDoubleClick(_arg_1:MouseEvent):void {
        if (((((!(this.gs)) || (this.gs.evalIsNotInCombatMapArea()))) || ((Parameters.data_.clickForGold == true)))) {
            this.openAccountDialog.dispatch();
        }
    }

    public function makeTextField(_arg_1:uint = 0xFFFFFF):TextFieldDisplayConcrete {
        var _local_2:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(FONT_SIZE).setColor(_arg_1).setTextHeight(16);
        _local_2.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4, 2)];
        return (_local_2);
    }

    public function draw(_arg_1:int, _arg_2:int, _arg_3:int, _arg_4:int, _arg_7:int):void {
        this.lootBox1_ = _arg_1;
        this.lootBox3_ = _arg_3;
        this.lootBox4_ = _arg_4;
        this.lootBox7_ = _arg_7;
        this.lootBox2_ = _arg_2;
        this.lootbox3Text.setStringBuilder(new StaticStringBuilder(this.lootBox3_.toString()));
        this.lootbox2Text.setStringBuilder(new StaticStringBuilder(this.lootBox2_.toString()));
        this.lootbox4Text.setStringBuilder(new StaticStringBuilder(this.lootBox4_.toString()));
        this.lootbox7Text.setStringBuilder(new StaticStringBuilder(this.lootBox7_.toString()));
        this.lootbox1Text.setStringBuilder(new StaticStringBuilder(this.lootBox1_.toString()));
        if (waiter.isEmpty()) {
            this.onAlignHorizontal();
        }
    }



}
}//package kabam.rotmg.game.view
