package com.company.assembleegameclient.ui.panels {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;

import flash.events.Event;
import flash.events.MouseEvent;
import flash.events.TimerEvent;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;
import flash.utils.Timer;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.SignalWaiter;

public class PartyInvitePanel extends Panel {

    private const waiter:SignalWaiter = new SignalWaiter();

    public var name_:String;
    private var title_:TextFieldDisplayConcrete;
    private var partyId_:int;
    private var rejectButton_:DeprecatedTextButton;
    private var acceptButton_:DeprecatedTextButton;
    private var timer_:Timer;

    public function PartyInvitePanel(_arg1:GameSprite, _arg2:String, _arg3:int) {
        super(_arg1);
        this.name_ = _arg2;
        this.partyId_ = _arg3;
        this.title_ = new TextFieldDisplayConcrete().setSize(16).setColor(0xFFFFFF).setTextWidth(WIDTH).setBold(true).setAutoSize(TextFieldAutoSize.CENTER).setHTML(true);
        this.title_.setStringBuilder(new LineBuilder().setParams("{playerName} wants to party with you!", {"playerName": _arg2}).setPrefix('<p align="center">').setPostfix("</p>"));
        this.title_.setWordWrap(true).setMultiLine(true);
        this.title_.setAutoSize(TextFieldAutoSize.CENTER);
        this.title_.filters = [new DropShadowFilter(0, 0, 0)];
        this.title_.y = 0;
        addChild(this.title_);
        this.rejectButton_ = new DeprecatedTextButton(16, TextKey.GUILD_REJECTION);
        this.rejectButton_.addEventListener(MouseEvent.CLICK, this.onRejectClick);
        this.waiter.push(this.rejectButton_.textChanged);
        addChild(this.rejectButton_);
        this.acceptButton_ = new DeprecatedTextButton(16, TextKey.GUILD_ACCEPT);
        this.acceptButton_.addEventListener(MouseEvent.CLICK, this.onAcceptClick);
        this.waiter.push(this.acceptButton_.textChanged);
        addChild(this.acceptButton_);
        this.timer_ = new Timer((20 * 1000), 1);
        this.timer_.start();
        this.timer_.addEventListener(TimerEvent.TIMER, this.onTimer);
        this.waiter.complete.addOnce(this.alignUI);
    }

    private function alignUI():void {
        this.rejectButton_.x = ((WIDTH / 4) - (this.rejectButton_.width / 2));
        this.rejectButton_.y = ((HEIGHT - this.rejectButton_.height) - 4);
        this.acceptButton_.x = (((3 * WIDTH) / 4) - (this.acceptButton_.width / 2));
        this.acceptButton_.y = ((HEIGHT - this.acceptButton_.height) - 4);
    }

    private function onTimer(_arg1:TimerEvent):void {
        dispatchEvent(new Event(Event.COMPLETE));
    }

    private function onRejectClick(_arg1:MouseEvent):void {
        dispatchEvent(new Event(Event.COMPLETE));
    }

    private function onAcceptClick(_arg1:MouseEvent):void {
        gs_.gsc_.playerText("/paccept " + partyId_);
        dispatchEvent(new Event(Event.COMPLETE));
    }


}
}
