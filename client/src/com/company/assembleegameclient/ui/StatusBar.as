﻿package com.company.assembleegameclient.ui {
import com.company.assembleegameclient.parameters.Parameters;

import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import org.osflash.signals.Signal;

public class StatusBar extends Sprite {
    public static var barTextSignal:Signal = new Signal(Boolean);

    public var w_:int;
    public var h_:int;
    public var color_:uint;
    public var backColor_:uint;
    public var pulseBackColor:uint;
    public var textColor_:uint;
    public var val_:int = 0;
    public var valDUB_:Number = 0;
    public var max_:int = -1;
    public var boost_:int = -1;
    public var maxMax_:int = -1;
    public var postMax_:int = -1;
    private var labelText_:TextFieldDisplayConcrete;
    private var labelTextStringBuilder_:LineBuilder;
    private var valueText_:TextFieldDisplayConcrete;
    private var valueTextStringBuilder_:StaticStringBuilder;
    private var boostText_:TextFieldDisplayConcrete;
    private var multiplierText:TextFieldDisplayConcrete;
    public var multiplierIcon:Sprite;
    private var colorSprite:Sprite;
    private var defaultForegroundColor:Number;
    private var defaultBackgroundColor:Number;
    public var mouseOver_:Boolean = false;
    private var isPulsing:Boolean = false;
    private var repetitions:int;
    private var direction:int = -1;
    private var speed:Number = 0.1;

    public function StatusBar(w:int, h:int, color:uint, backColor:uint, label:String = null) {
        this.colorSprite = new Sprite();
        super();
        addChild(this.colorSprite);
        this.w_ = w;
        this.h_ = h;
        this.defaultForegroundColor = (this.color_ = color);
        this.defaultBackgroundColor = (this.backColor_ = backColor);
        this.textColor_ = 0xFFFFFF;
        if (label != null && label.length != 0) {
            this.labelText_ = new TextFieldDisplayConcrete().setSize(13).setColor(this.textColor_);
            this.labelText_.setBold(true);
            this.labelTextStringBuilder_ = new LineBuilder().setParams(label);
            this.labelText_.setStringBuilder(this.labelTextStringBuilder_);
            this.centerVertically(this.labelText_);
            this.labelText_.filters = [new DropShadowFilter(0, 0, 0)];
            addChild(this.labelText_);
        }
        this.valueText_ = new TextFieldDisplayConcrete().setSize(13).setColor(0xFFFFFF);
        this.valueText_.setBold(true);
        this.valueText_.filters = [new DropShadowFilter(0, 0, 0)];
        this.centerVertically(this.valueText_);
        this.valueTextStringBuilder_ = new StaticStringBuilder();
        this.boostText_ = new TextFieldDisplayConcrete().setSize(14).setColor(this.textColor_);
        this.boostText_.setBold(true);
        this.boostText_.alpha = 0.6;
        this.centerVertically(this.boostText_);
        this.boostText_.filters = [new DropShadowFilter(0, 0, 0)];
        this.multiplierIcon = new Sprite();
        this.multiplierIcon.x = this.w_ - 25;
        this.multiplierIcon.y = -3;
        this.multiplierIcon.graphics.beginFill(0xFF00FF, 0);
        this.multiplierIcon.graphics.drawRect(0, 0, 20, 20);
        this.multiplierIcon.addEventListener(MouseEvent.MOUSE_OVER, this.onMultiplierOver);
        this.multiplierIcon.addEventListener(MouseEvent.MOUSE_OUT, this.onMultiplierOut);
        this.multiplierText = new TextFieldDisplayConcrete().setSize(14).setColor(9493531);
        this.multiplierText.setBold(true);
        this.multiplierText.setStringBuilder(new StaticStringBuilder("x4"));
        this.multiplierText.filters = [new DropShadowFilter(0, 0, 0)];
        this.multiplierIcon.addChild(this.multiplierText);
        if (!Parameters.data_.toggleBarText) {
            addEventListener(MouseEvent.ROLL_OVER, this.onMouseOver);
            addEventListener(MouseEvent.ROLL_OUT, this.onMouseOut);
        }
        barTextSignal.add(this.setBarText);
    }

    public function centerVertically(textField:TextFieldDisplayConcrete):void {
        textField.setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
        textField.y = this.h_ / 2 + 1;
    }

    private function onMultiplierOver(e:MouseEvent):void {
        dispatchEvent(new Event("MULTIPLIER_OVER"));
    }

    private function onMultiplierOut(e:MouseEvent):void {
        dispatchEvent(new Event("MULTIPLIER_OUT"));
    }

    public function draw(val:int, max:int, boost:int, maxMax:int = -1, postMax:int = -1):void {
        if (max > 0) {
            val = Math.min(max, Math.max(0, val));
        }
        if (val == this.val_
                && max == this.max_
                && boost == this.boost_
                && maxMax == this.maxMax_
                && postMax == this.postMax_) {
            return;
        }
        this.val_ = val;
        this.max_ = max;
        this.boost_ = boost;
        this.maxMax_ = maxMax;
        this.postMax_ = postMax;
        this.internalDraw();
    }

    public function draw2(val:int, max:int, boost:int, maxMax:int = -1, postMax:int = -1):void {
        if (val == this.val_
                && max == this.max_
                && boost == this.boost_
                && maxMax == this.maxMax_
                && postMax == this.postMax_) {
            return;
        }
        this.val_ = val;
        this.max_ = max;
        this.boost_ = boost;
        this.maxMax_ = maxMax;
        this.postMax_ = postMax;
        this.valDUB_ = val / 10.0;
        this.internalDraw2();
    }

    public function setLabelText(label:String, tokens:Object = null):void {
        this.labelTextStringBuilder_.setParams(label, tokens);
        this.labelText_.setStringBuilder(this.labelTextStringBuilder_);
    }

    private function setTextColor(color:uint):void {
        this.textColor_ = color;
        if (this.boostText_ != null) {
            this.boostText_.setColor(this.textColor_);
        }
        this.valueText_.setColor(this.textColor_);
    }

    public function setBarText(barTextShown:Boolean):void {
        this.mouseOver_ = false;
        if (barTextShown) {
            removeEventListener(MouseEvent.ROLL_OVER, this.onMouseOver);
            removeEventListener(MouseEvent.ROLL_OUT, this.onMouseOut);
        }
        else {
            addEventListener(MouseEvent.ROLL_OVER, this.onMouseOver);
            addEventListener(MouseEvent.ROLL_OUT, this.onMouseOut);
        }
        this.internalDraw();
    }

    private function internalDraw():void {
        graphics.clear();
        this.colorSprite.graphics.clear();
        var color:uint = 0xFFFFFF;

        if (this.postMax_ >= 50) {
            color = 4286945;
        } else if (this.maxMax_ > 0 && this.max_ - this.boost_ >= this.maxMax_) {
            color = 0xFCDF00;
        } else if (this.boost_ > 0) {
            color = 6206769;
        }

        if (this.textColor_ != color) {
            this.setTextColor(color);
        }
        graphics.beginFill(this.backColor_);
        graphics.drawRect(0, 0, this.w_, this.h_);
        graphics.endFill();
        if (this.isPulsing) {
            this.colorSprite.graphics.beginFill(this.pulseBackColor);
            this.colorSprite.graphics.drawRect(0, 0, this.w_, this.h_);
        }
        this.colorSprite.graphics.beginFill(this.color_);
        if (this.max_ > 0) {
            this.colorSprite.graphics.drawRect(0, 0, (this.w_ * (this.val_ / this.max_)), this.h_);
        }
        else {
            this.colorSprite.graphics.drawRect(0, 0, this.w_, this.h_);
        }
        this.colorSprite.graphics.endFill();
        if (contains(this.valueText_)) {
            removeChild(this.valueText_);
        }
        if (contains(this.boostText_)) {
            removeChild(this.boostText_);
        }
        if (Parameters.data_.toggleBarText || this.mouseOver_ && this.h_ > 4) {
            this.drawWithMouseOver();
        }
    }

    private function internalDraw2():void {
        graphics.clear();
        this.colorSprite.graphics.clear();
        var color:uint = 0xFFFFFF;

        if (this.postMax_ >= 50) {
            color = 4286945;
        } else if (this.maxMax_ > 0 && this.max_ - this.boost_ >= this.maxMax_) {
            color = 0xFCDF00;
        } else if (this.boost_ > 0) {
            color = 6206769;
        }

        if (this.textColor_ != color) {
            this.setTextColor(color);
        }
        graphics.beginFill(this.backColor_);
        graphics.drawRect(0, 0, this.w_, this.h_);
        graphics.endFill();
        if (this.isPulsing) {
            this.colorSprite.graphics.beginFill(this.pulseBackColor);
            this.colorSprite.graphics.drawRect(0, 0, this.w_, this.h_);
        }
        this.colorSprite.graphics.beginFill(this.color_);
        if (this.max_ > 0) {
            this.colorSprite.graphics.drawRect(0, 0, (this.w_ * (this.valDUB_ / this.max_)), this.h_);
        }
        else {
            this.colorSprite.graphics.drawRect(0, 0, this.w_, this.h_);
        }
        this.colorSprite.graphics.endFill();
        if (contains(this.valueText_)) {
            removeChild(this.valueText_);
        }
        if (contains(this.boostText_)) {
            removeChild(this.boostText_);
        }
        if (Parameters.data_.toggleBarText || this.mouseOver_ && this.h_ > 4) {
            this.drawWithMouseOver2();
        }
    }

    public function drawWithMouseOver():void {
        if (this.max_ > 0) {
            this.valueText_.setStringBuilder(this.valueTextStringBuilder_.setString(this.val_ + "/" + this.max_));
        }
        else {
            this.valueText_.setStringBuilder(this.valueTextStringBuilder_.setString("" + this.val_));
        }
        if (!contains(this.valueText_)) {
            this.valueText_.mouseEnabled = false;
            this.valueText_.mouseChildren = false;
            addChild(this.valueText_);
        }
        if (this.boost_ != 0) {
            this.boostText_.setStringBuilder(
                    this.valueTextStringBuilder_.setString(" (" + (this.boost_ > 0 ? "+" : "") + this.boost_.toString() + ")"));
            if (!contains(this.boostText_)) {
                this.boostText_.mouseEnabled = false;
                this.boostText_.mouseChildren = false;
                addChild(this.boostText_);
            }
            this.valueText_.x = this.w_ / 2 - (this.valueText_.width + this.boostText_.width) / 2;
            this.boostText_.x = this.valueText_.x + this.valueText_.width;
        }
        else {
            this.valueText_.x = this.w_ / 2 - this.valueText_.width / 2;
            if (contains(this.boostText_)) {
                removeChild(this.boostText_);
            }
        }
    }

    public function drawWithMouseOver2():void {
        if(valDUB_ == -0.1)
            valDUB_ =  0.0;
        if (this.max_ > 0) {
            this.valueText_.setStringBuilder(this.valueTextStringBuilder_.setString(this.valDUB_ + "/" + this.max_));
        }
        else {
            this.valueText_.setStringBuilder(this.valueTextStringBuilder_.setString("" + this.valDUB_));
        }
        if (!contains(this.valueText_)) {
            this.valueText_.mouseEnabled = false;
            this.valueText_.mouseChildren = false;
            addChild(this.valueText_);
        }
        if (this.boost_ != 0) {
            this.boostText_.setStringBuilder(
                    this.valueTextStringBuilder_.setString(" (" + (this.boost_ > 0 ? "+" : "") + this.boost_.toString() + ")"));
            if (!contains(this.boostText_)) {
                this.boostText_.mouseEnabled = false;
                this.boostText_.mouseChildren = false;
                addChild(this.boostText_);
            }
            this.valueText_.x = this.w_ / 2 - (this.valueText_.width + this.boostText_.width) / 2;
            this.boostText_.x = this.valueText_.x + this.valueText_.width;
        }
        else {
            this.valueText_.x = this.w_ / 2 - this.valueText_.width / 2;
            if (contains(this.boostText_)) {
                removeChild(this.boostText_);
            }
        }
    }

    public function showMultiplierText():void {
        this.multiplierIcon.mouseEnabled = false;
        this.multiplierIcon.mouseChildren = false;
        addChild(this.multiplierIcon);
        this.startPulse(3, 9493531, 0xFFFFFF);
    }

    public function hideMultiplierText():void {
        if (this.multiplierIcon.parent) {
            removeChild(this.multiplierIcon);
        }
    }

    public function startPulse(reps:Number, color:Number, pulseColor:Number):void {
        this.isPulsing = true;
        this.color_ = color;
        this.pulseBackColor = pulseColor;
        this.repetitions = reps;
        this.internalDraw();
        addEventListener(Event.ENTER_FRAME, this.onPulse);
    }

    private function onPulse(e:Event):void {
        if (this.colorSprite.alpha > 1 || this.colorSprite.alpha < 0) {
            this.direction = (this.direction * -1);
            if (this.colorSprite.alpha > 1) {
                this.repetitions--;
                if (!this.repetitions) {
                    this.isPulsing = false;
                    this.color_ = this.defaultForegroundColor;
                    this.backColor_ = this.defaultBackgroundColor;
                    this.colorSprite.alpha = 1;
                    this.internalDraw();
                    removeEventListener(Event.ENTER_FRAME, this.onPulse);
                }
            }
        }
        this.colorSprite.alpha = (this.colorSprite.alpha + (this.speed * this.direction));
    }

    private function onMouseOver(e:MouseEvent):void {
        this.mouseOver_ = true;
        this.internalDraw();
    }

    private function onMouseOut(e:MouseEvent):void {
        this.mouseOver_ = false;
        this.internalDraw();
    }
}
}
