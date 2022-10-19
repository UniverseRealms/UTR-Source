﻿package com.company.assembleegameclient.ui.tooltip {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.map.partyoverlay.PlayerArrow;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.options.Options;
import com.company.util.GraphicsUtil;

import flash.display.CapsStyle;
import flash.display.DisplayObject;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.GraphicsStroke;
import flash.display.IGraphicsData;
import flash.display.JointStyle;
import flash.display.LineScaleMode;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.memMarket.MemMarket;
import kabam.rotmg.memMarket.content.MemMarketItem;
import kabam.rotmg.memMarket.tabs.MemMarketBuyTab;
import kabam.rotmg.memMarket.tabs.MemMarketSellTab;

import kabam.rotmg.tooltips.view.TooltipsView;
import kabam.rotmg.ui.view.SignalWaiter;

public class ToolTip extends Sprite {

    protected const waiter:SignalWaiter = new SignalWaiter();

    private var background_:uint;
    private var backgroundAlpha_:Number;
    private var outline_:uint;
    private var outlineAlpha_:Number;
    private var followMouse_:Boolean;
    private var forcePositionLeft_:Boolean = false;
    private var forcePositionRight_:Boolean = false;
    public var contentWidth_:int;
    public var contentHeight_:int;
    private var targetObj:DisplayObject;
    private var backgroundFill_:GraphicsSolidFill = new GraphicsSolidFill(0, 1);
    private var outlineFill_:GraphicsSolidFill = new GraphicsSolidFill(0xFFFFFF, 1);
    private var lineStyle_:GraphicsStroke = new GraphicsStroke(1, false, LineScaleMode.NORMAL, CapsStyle.NONE, JointStyle.ROUND, 3, outlineFill_);
    private var path_:GraphicsPath = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());

    private const graphicsData_:Vector.<IGraphicsData> = new <IGraphicsData>[lineStyle_, backgroundFill_, path_, GraphicsUtil.END_FILL, GraphicsUtil.END_STROKE];

    public function ToolTip(background:uint, backgroundAlpha:Number, outline:uint, outlineAlpha:Number, followMouse:Boolean = true) {
        super();
        this.background_ = background;
        this.backgroundAlpha_ = backgroundAlpha;
        this.outline_ = outline;
        this.outlineAlpha_ = outlineAlpha;
        this.followMouse_ = followMouse;
        mouseEnabled = false;
        mouseChildren = false;
        filters = [new DropShadowFilter(0, 0, 0, 1, 16, 16)];
        addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        this.waiter.complete.add(this.alignUIAndDraw);
    }

    private function alignUIAndDraw():void {
        this.alignUI();
        this.draw();
        this.position();
    }

    protected function alignUI():void {
    }

    public function attachToTarget(_arg1:DisplayObject):void {
        if (_arg1) {
            this.targetObj = _arg1;
            this.targetObj.addEventListener(MouseEvent.ROLL_OUT, this.onLeaveTarget);
        }
    }

    public function detachFromTarget():void {
        if (this.targetObj) {
            this.targetObj.removeEventListener(MouseEvent.ROLL_OUT, this.onLeaveTarget);
            if (parent) {
                parent.removeChild(this);
            }
            this.targetObj = null;
        }
    }

    public function forcePostionLeft():void {
        this.forcePositionLeft_ = true;
        this.forcePositionRight_ = false;
    }

    public function forcePostionRight():void {
        this.forcePositionRight_ = true;
        this.forcePositionLeft_ = false;
    }

    private function onLeaveTarget(_arg1:MouseEvent):void {
        this.detachFromTarget();
    }

    private function onAddedToStage(_arg1:Event):void {
        if (this.waiter.isEmpty()) {
            this.draw();
        }
        if (this.followMouse_) {
            this.position();
            addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        }
    }

    private function onRemovedFromStage(_arg1:Event):void {
        if (this.followMouse_) {
            removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        }
    }

    private function onEnterFrame(_arg1:Event):void {
        this.position();
    }

    protected function position():void
    {
        var mousePosX:Number;
        var mousePosY:Number;
        var scaleW:Number = (800 / stage.stageWidth);
        var scaleH:Number = (600 / stage.stageHeight);
        if ((this.parent is Options))
        {
            mousePosX = ((((stage.mouseX + (stage.stageWidth / 2)) - 400) / stage.stageWidth) * 800);
            mousePosY = ((((stage.mouseY + (stage.stageHeight / 2)) - 300) / stage.stageHeight) * 600);
        }
        else
        {
            mousePosX = (((stage.stageWidth - 800) / 2) + stage.mouseX);
            mousePosY = (((stage.stageHeight - 600) / 2) + stage.mouseY);
            if ((this.parent is TooltipsView || (this is PlayerGroupToolTip && !(this.parent is PlayerArrow))))
            {
                if (Parameters.data_.uiscale)
                {
                    this.parent.scaleX = (scaleW / scaleH);
                    this.parent.scaleY = 1;
                    mousePosX = (mousePosX * scaleH);
                    mousePosY = (mousePosY * scaleH);
                }
                else
                {
                    this.parent.scaleX = scaleW;
                    this.parent.scaleY = scaleH;
                }
            }
        }
        if (stage == null)
        {
            return;
        }
        if (((stage.mouseX + (0.5 * stage.stageWidth)) - 400) < (stage.stageWidth / 2))
        {
            x = (mousePosX + 12);
        }
        else
        {
            x = ((mousePosX - width) - 1);
        }
        if (x < 12)
        {
            x = 12;
        }
        if (((stage.mouseY + (0.5 * stage.stageHeight)) - 300) < (stage.stageHeight / 3))
        {
            y = (mousePosY + 12);
        }
        else
        {
            y = ((mousePosY - height) - 1);
        }
        if (y < 12)
        {
            y = 12;
        }
    }

    public function draw():void {
        this.backgroundFill_.color = this.background_;
        this.backgroundFill_.alpha = this.backgroundAlpha_;
        this.outlineFill_.color = this.outline_;
        this.outlineFill_.alpha = this.outlineAlpha_;
        graphics.clear();
        this.contentWidth_ = width;
        this.contentHeight_ = height;
        GraphicsUtil.clearPath(this.path_);
        GraphicsUtil.drawCutEdgeRect(-6, -6, (this.contentWidth_ + 12), (this.contentHeight_ + 12), 4, [1, 1, 1, 1], this.path_);
        graphics.drawGraphicsData(this.graphicsData_);
    }


}
}
