﻿package kabam.rotmg.raidLauncher {
import com.company.assembleegameclient.account.ui.CheckBoxField;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;

import flash.display.DisplayObject;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;
import flash.text.TextFormatAlign;

import kabam.rotmg.account.core.view.EmptyFrame;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.dialogs.control.FlushPopupStartupQueueSignal;
import kabam.rotmg.pets.view.components.PopupWindowBackground;
import kabam.rotmg.text.model.FontModel;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

public class RaidLauncherModal extends EmptyFrame {
    public static const MODAL_WIDTH:int = 330;
    public static const MODAL_HEIGHT:int = 500;

    public static var backgroundImageEmbed:Class = RaidLauncher_backgroundImageEmbed;
    public static var raid1launchFlagEmbed:Class = Raid1_launchFlag;
    public static var raid2launchFlagEmbed:Class = Raid2_launchFlag;
    public static var raid3launchFlagEmbed:Class = Raid3_launchFlag;
    public static var raid4launchFlagEmbed:Class = Raid4_launchFlag;
    public static var modalWidth:int = MODAL_WIDTH;
    public static var modalHeight:int = MODAL_HEIGHT;

    public var launchButton:DeprecatedTextButton;
    public var ultraCheckbox:CheckBoxField;
    public var launchButton2:DeprecatedTextButton;
    public var ultraCheckbox2:CheckBoxField;
    public var launchButton3:DeprecatedTextButton;
    public var ultraCheckbox3:CheckBoxField;
    public var launchButton4:DeprecatedTextButton;
    public var ultraCheckbox4:CheckBoxField;
    private var fontModel:FontModel;
    private var triggeredOnStartup:Boolean;

    public function RaidLauncherModal(_arg1:Boolean = false) {
        this.triggeredOnStartup = _arg1;
        this.fontModel = StaticInjectorContext.getInjector().getInstance(FontModel);
        modalWidth = MODAL_WIDTH;
        modalHeight = MODAL_HEIGHT;
        super(modalWidth, modalHeight);
        this.setCloseButton(true);
        this.setTitle("Choose a raid to launch", true);
        closeButton.clicked.add(this.onCloseButtonClicked);
    }

    public static function getText(_arg1:String, _arg2:int, _arg3:int, _arg4:Boolean):TextFieldDisplayConcrete {
        var _local5:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(18).setColor(0xFFFFFF).setTextWidth(((RaidLauncherModal.modalWidth - (TEXT_MARGIN * 2)) - 10));
        _local5.setBold(true);
        if (_arg4) {
            _local5.setStringBuilder(new StaticStringBuilder(_arg1));
        }
        else {
            _local5.setStringBuilder(new LineBuilder().setParams(_arg1));
        }
        _local5.setWordWrap(true);
        _local5.setMultiLine(true);
        _local5.setAutoSize(TextFieldAutoSize.CENTER);
        _local5.setHorizontalAlign(TextFormatAlign.CENTER);
        _local5.filters = [new DropShadowFilter(0, 0, 0)];
        _local5.x = _arg2;
        _local5.y = _arg3;
        return (_local5);
    }

    public function onCloseButtonClicked() : void {
        var _local1:FlushPopupStartupQueueSignal = StaticInjectorContext.getInjector().getInstance(FlushPopupStartupQueueSignal);
        closeButton.clicked.remove(this.onCloseButtonClicked);
        if (this.triggeredOnStartup) {
            _local1.dispatch();
        }
    }

    public function onClick(_arg1:MouseEvent):void {
    }

    override protected function makeModalBackground():Sprite {
        var _local1:Sprite = new Sprite();
        var _local2:DisplayObject = new backgroundImageEmbed();
        _local2.width = (modalWidth + 1);
        _local2.height = (modalHeight - 25);
        _local2.y = 27;
        _local2.alpha = 1.00;
        var _local3:DisplayObject = new raid1launchFlagEmbed();
        _local3.width = 330;
        _local3.height = 80;
        _local3.y = 30;
        _local3.alpha = 1.00;
        var _local5:DisplayObject = new raid2launchFlagEmbed();
        _local5.width = 330;
        _local5.height = 80;
        _local5.y = 150;
        _local5.alpha = 1;
        var _local6:DisplayObject = new raid3launchFlagEmbed();
        _local6.width = 330;
        _local6.height = 80;
        _local6.y = 270;
        _local6.alpha = 1;
        var _local7:DisplayObject = new raid4launchFlagEmbed();
        _local7.width = 330;
        _local7.height = 80;
        _local7.y = 390;
        _local7.alpha = 1;
        var _local4:PopupWindowBackground = new PopupWindowBackground();
        _local4.draw(modalWidth, modalHeight, PopupWindowBackground.TYPE_TRANSPARENT_WITH_HEADER);
        _local1.addChild(_local2);
        _local1.addChild(_local3);
        _local1.addChild(_local4);
        _local1.addChild(_local5);
        _local1.addChild(_local6);
        _local1.addChild(_local7);
        this.launchButton = new DeprecatedTextButton(12, "Launch");
        this.launchButton.y = 118;
        this.launchButton.x = this.launchButton.x + 10;
        this.launchButton.setEnabled(true);
        _local1.addChild(this.launchButton);
        this.ultraCheckbox = new CheckBoxField("Ultra", true);

        this.launchButton2 = new DeprecatedTextButton(12, "Launch");
        this.launchButton2.y = 236;
        this.launchButton2.x = this.launchButton2.x + 10;
        this.launchButton2.setEnabled(true);
        _local1.addChild(this.launchButton2);
        this.ultraCheckbox2 = new CheckBoxField("Ultra", true);

        this.launchButton3 = new DeprecatedTextButton(12, "Launch");
        this.launchButton3.y = 354;
        this.launchButton3.x = this.launchButton3.x + 10;
        this.launchButton3.setEnabled(true);
        _local1.addChild(this.launchButton3);
        this.ultraCheckbox3 = new CheckBoxField("Ultra", true);

        this.launchButton4 = new DeprecatedTextButton(12, "Launch");
        this.launchButton4.y = 472;
        this.launchButton4.x = this.launchButton4.x + 10;
        this.launchButton4.setEnabled(true);
        _local1.addChild(this.launchButton4);
        this.ultraCheckbox4 = new CheckBoxField("Ultra", true);
        return (_local1);
    }

    private function onMouseClick(e:MouseEvent):void {
        SoundEffectLibrary.play("button_click");
    }

    override public function onCloseClick(_arg1:MouseEvent):void {
        SoundEffectLibrary.play("button_click");
    }
}
}
