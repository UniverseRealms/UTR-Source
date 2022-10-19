﻿package kabam.rotmg.pets.view.components.slot {
import flash.display.Bitmap;
import flash.display.Shape;
import flash.display.Sprite;
import flash.events.Event;
import flash.geom.ColorTransform;

import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class FeedFuseSlot extends Sprite {

    protected var outerSlot:Shape;
    protected var innerSlot:Shape;
    protected var bg:Shape;
    private var titleField:TextFieldDisplayConcrete;
    private var titleStringBuilder:LineBuilder;
    private var subtitleField:TextFieldDisplayConcrete;
    private var subtitleStringBuilder:AppendingLineBuilder;
    protected var itemSprite:Sprite;
    protected var itemBitmap:Bitmap;
    protected var icon:Sprite;
    public var itemId:int = -1;
    public var slotId:int = -1;
    public var objectId:int = -1;

    public function FeedFuseSlot() {
        this.outerSlot = PetsViewAssetFactory.returnPetSlotShape(46, 0x545454, 0, false, true);
        this.innerSlot = PetsViewAssetFactory.returnPetSlotShape(40, 0x545454, 3, false, true);
        this.bg = PetsViewAssetFactory.returnPetSlotShape(46, 0x545454, 0, true, false);
        this.titleField = PetsViewAssetFactory.returnPetSlotTitle();
        this.titleStringBuilder = new LineBuilder();
        this.subtitleField = PetsViewAssetFactory.returnMediumCenteredTextfield(16777103, 100);
        this.subtitleStringBuilder = new AppendingLineBuilder();
        this.itemSprite = new Sprite();
        this.itemBitmap = new Bitmap();
        super();
        this.addChildren();
        this.subtitleField.textChanged.add(this.positionSubtitleText);
        this.titleField.textChanged.add(this.positionSubtitleText);
        addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
    }

    public function setTitle(_arg1:String, _arg2:Object):void {
        this.titleStringBuilder.setParams(_arg1, _arg2);
        this.titleField.setStringBuilder(this.titleStringBuilder);
    }

    public function setSubtitle(_arg1:String, _arg2:Object):void {
        this.subtitleStringBuilder.clear();
        this.subtitleStringBuilder.pushParams(_arg1, _arg2);
        this.subtitleField.setStringBuilder(this.subtitleStringBuilder);
    }

    public function hideOuterSlot(_arg1:Boolean):void {
        this.outerSlot.visible = !(_arg1);
        var _local2:int = ((_arg1) ? 40 : 46);
        var _local3:int = ((_arg1) ? 3 : 0);
        this.bg.graphics.clear();
        this.bg.graphics.beginFill(0x464646);
        this.bg.graphics.drawRoundRect(0, _local3, _local2, _local2, 16, 16);
        this.bg.x = ((100 - _local2) * 0.5);
    }

    public function highlight(_arg1:Boolean, _arg2:int = 16777103, _arg3:Boolean = false):void {
        var _local5:ColorTransform;
        var _local4:ColorTransform = this.innerSlot.transform.colorTransform;
        _local4.color = ((_arg1) ? _arg2 : 0x545454);
        this.innerSlot.transform.colorTransform = _local4;
        if (this.outerSlot.visible) {
            _local5 = this.outerSlot.transform.colorTransform;
            _local5.color = ((_arg3) ? _arg2 : 0x545454);
            this.outerSlot.transform.colorTransform = _local5;
        }
    }

    protected function alignBitmapInBox():void {
        this.itemSprite.x = 0;
        this.itemSprite.y = 0;
        this.itemBitmap.x = ((100 - this.itemBitmap.width) * 0.5);
        this.itemBitmap.y = ((46 - this.itemBitmap.height) * 0.5);
    }

    public function setIcon(_arg1:Sprite):void {
        ((this.icon) && (removeChild(this.icon)));
        this.icon = _arg1;
        addChild(this.icon);
        this.alignIcon();
    }

    public function getIcon():Sprite {
        return (this.icon);
    }

    protected function alignIcon():void {
        this.icon.x = ((100 - this.icon.width) * 0.5);
        this.icon.y = ((46 - this.icon.height) * 0.5);
    }

    protected function onRemovedFromStage(_arg1:Event):void {
        removeEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        this.subtitleField.textChanged.remove(this.positionSubtitleText);
        this.titleField.textChanged.remove(this.positionSubtitleText);
    }

    private function addChildren():void {
        this.itemSprite.addChild(this.itemBitmap);
        addChild(this.bg);
        addChild(this.innerSlot);
        addChild(this.outerSlot);
        addChild(this.titleField);
        addChild(this.subtitleField);
        addChild(this.itemSprite);
    }

    private function positionSubtitleText():void {
        this.subtitleField.y = ((this.titleField.y + this.titleField.height) - 1);
    }


}
}
