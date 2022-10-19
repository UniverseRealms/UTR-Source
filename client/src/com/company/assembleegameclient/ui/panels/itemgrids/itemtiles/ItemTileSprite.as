package com.company.assembleegameclient.ui.panels.itemgrids.itemtiles {
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.TimerEvent;
import flash.filters.ColorMatrixFilter;
import flash.geom.Matrix;
import flash.utils.Timer;
import flash.utils.setInterval;

import kabam.rotmg.constants.ItemConstants;
import kabam.rotmg.text.view.BitmapTextFactory;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

public class ItemTileSprite extends Sprite {
    protected static const DIM_FILTER:Array = [new ColorMatrixFilter([0.4, 0, 0, 0, 0, 0, 0.4, 0, 0, 0, 0, 0, 0.4, 0, 0, 0, 0, 0, 1, 0])];
    private static const IDENTITY_MATRIX:Matrix = new Matrix();
    private static const DOSE_MATRIX:Matrix = function ():Matrix {
        var _local1:* = new Matrix();
        _local1.translate(10, 5);
        return (_local1);
    }();

    public var itemId:int;
    public var itemBitmap:Bitmap;
    private var bitmapFactory:BitmapTextFactory;

    public function ItemTileSprite() {
        this.itemBitmap = new Bitmap();
        addChild(this.itemBitmap);
        this.itemId = -1;
        this.spriteFile = null;
        this.first = -1;
        this.last = -1;
        this.next = -1;
    }

    public function setDim(_arg1:Boolean):void {
        filters = ((_arg1) ? DIM_FILTER : null);
    }

    public function setType(_arg1:int):void {
        this.itemId = _arg1;
        this.drawTile();
    }

    public function drawTile():void {
        var _local2:BitmapData;
        var _local3:XML;
        var _local4:BitmapData;
        var _local1:int = this.itemId;
        if (_local1 != ItemConstants.NO_ITEM) {
            if ((((_local1 >= 0x9000)) && ((_local1 < 0xF000)))) {
                _local1 = 36863;
            }
            _local2 = ObjectLibrary.getRedrawnTextureFromType(_local1, this.iconSize, true);
            _local3 = ObjectLibrary.xmlLibrary_[_local1];
            if (((((_local3) && (_local3.hasOwnProperty("Doses") || _local3.hasOwnProperty("@successChance")))) && (this.bitmapFactory))) {
                _local2 = _local2.clone();
                if (_local3.hasOwnProperty("@successChance")) {
                    var _successChance:String = _local3.attribute("successChance");
                    var _successChanceColor:uint;
                    if (_successChance == "100%")
                        _successChanceColor = 0xFFFFFF;
                    else if (_successChance == "75%")
                        _successChanceColor = 0xFFFF00;
                    else
                        _successChanceColor = 0xFF0000;
                    _local4 = this.bitmapFactory.make(new StaticStringBuilder(String(_successChance)), 12, _successChanceColor, true, IDENTITY_MATRIX, false);
                } else
                    _local4 = this.bitmapFactory.make(new StaticStringBuilder(String(_local3.Doses)), 12, 0xFFFFFF, false, IDENTITY_MATRIX, false);
                _local2.draw(_local4, DOSE_MATRIX);
            }
            if (((((_local3) && (_local3.hasOwnProperty("Quantity")))) && (this.bitmapFactory))) {
                _local2 = _local2.clone();
                _local4 = this.bitmapFactory.make(new StaticStringBuilder(String(_local3.Quantity)), 12, 0xFFFFFF, false, IDENTITY_MATRIX, false);
                _local2.draw(_local4, DOSE_MATRIX);
            }

            var spriteFile:String = null;
            var spriteArray:Array = null;
            var spritePeriod:Number = -1;
            var first:Number = -1;
            var last:Number = -1;
            var next:Number = -1;
            var makeAnimation:Function;
            var hasPeriod:Boolean = _local3.hasOwnProperty("@spritePeriod");
            var hasFile:Boolean = _local3.hasOwnProperty("@spriteFile");
            var hasArray:Boolean = _local3.hasOwnProperty("@spriteArray");
            var hasAnimatedSprites:Boolean = hasPeriod && hasFile && hasArray;

            if (hasPeriod)
                spritePeriod = 1000 / _local3.attribute("spritePeriod");

            if (hasFile)
                spriteFile = _local3.attribute("spriteFile");

            if (hasArray) {
                spriteArray = String(_local3.attribute("spriteArray")).split('-');
                first = Parameters.parse(spriteArray[0]);
                last = Parameters.parse(spriteArray[1]);
            }

            this.itemBitmap.bitmapData = _local2;
            this.itemBitmap.x = this.itemBitmap.y = -(this.itemBitmap.width / 2);

            if (hasAnimatedSprites && spritePeriod != -1 && spriteFile != null && spriteArray != null && first != -1 && last != -1) {
                this.spriteFile = spriteFile;
                this.first = first;
                this.last = last;
                this.next = this.first;
                var animatedTimer:Timer = new Timer(spritePeriod);
                animatedTimer.addEventListener(TimerEvent.TIMER, this.makeAnimation);
                animatedTimer.start();
            } else {
                this.spriteFile = null;
                this.first = this.last = this.next = -1;
            }

            visible = true;
        }
        else {
            this.itemId = -1;
            this.spriteFile = null;
            this.first = -1;
            this.last = -1;
            this.next = -1;

            visible = false;
        }
    }

    public function setBitmapFactory(_arg1:BitmapTextFactory):void {
        this.bitmapFactory = _arg1;
    }

    private var iconSize:Number = 80;
    private var spriteFile:String;
    private var first:Number;
    private var last:Number;
    private var next:Number;

    private function makeAnimation(event:TimerEvent = null):void {
        if (this.spriteFile == null)
            return;

        var size:int = this.iconSize;
        var bitmapData:BitmapData = AssetLibrary.getImageFromSet(this.spriteFile, this.next);

        if (Parameters.itemTypes16.indexOf(this.itemId) != -1 || bitmapData.height == 16)
            size = (size * 0.5);

        bitmapData = TextureRedrawer.redraw(bitmapData, size, true, 0, true, 5);

        this.itemBitmap.bitmapData = bitmapData;

        this.next++;

        if (this.next > this.last)
            this.next = this.first;
    }
}
}