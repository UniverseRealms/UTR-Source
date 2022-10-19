package kabam.rotmg.chat.control {
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.parameters.Parameters;

import flash.display.DisplayObject;

import flash.display.StageScaleMode;
import flash.events.Event;

import kabam.rotmg.account.core.Account;
import kabam.rotmg.appengine.api.AppEngineClient;
import kabam.rotmg.build.api.BuildData;
import kabam.rotmg.chat.model.ChatMessage;
import kabam.rotmg.dailyLogin.model.DailyLoginModel;
import kabam.rotmg.game.signals.AddTextLineSignal;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.ui.model.HUDModel;

public class ParseChatMessageCommand {

    [Inject]
    public var data:String;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var addTextLine:AddTextLineSignal;
    [Inject]
    public var client:AppEngineClient;
    [Inject]
    public var account:Account;
    [Inject]
    public var buildData:BuildData;
    [Inject]
    public var dailyLoginModel:DailyLoginModel;


    public function execute():void {
        var Commands:Array;
        if (this.scaleCommands(this.data))
        {
            return;
        }
        if (this.data.charAt(0) == "/"){
            Commands = this.data.substr(1, this.data.length).split(" ");
            switch (Commands[0]){
                case "help":
                    this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, TextKey.HELP_COMMAND));
                    return;
            };
        };
        switch (this.data) {
            case "/c":
            case "/class":
            case "/classes":
                var classCount:Object = {};
                var loops:uint = 0;
                var go:GameObject = null;
                var classList:String = "";
                var goType:* = null;
                for each (go in this.hudModel.gameSprite.map.goDict_) {
                    if (go.props_.isPlayer_) {
                        classCount[go.objectType_] = (classCount[go.objectType_] != undefined ? classCount[go.objectType_] + 1 : 1);
                        loops++;
                    }
                }

                for (goType in classCount) {
                    classList += " " + ObjectLibrary.typeToDisplayId_[goType] + ": " + classCount[goType];
                }
                this.addTextLine.dispatch(ChatMessage.make("","Classes online (" + loops + "):" + classList));
                break;
            default:
                this.hudModel.gameSprite.gsc_.playerText(this.data);
        }
    }
    public function scaleCommands(_arg_1:String):Boolean
    {
        _arg_1 = _arg_1.toLowerCase();
        var _local_2:DisplayObject = WebMain.STAGE;
        if (_arg_1 == "/fs")
        {
            if (_local_2.stage.scaleMode == StageScaleMode.EXACT_FIT)
            {
                _local_2.stage.scaleMode = StageScaleMode.NO_SCALE;
                Parameters.data_.stageScale = StageScaleMode.NO_SCALE;
                this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, "Fullscreen: On"));
            }
            else
            {
                _local_2.stage.scaleMode = StageScaleMode.EXACT_FIT;
                Parameters.data_.stageScale = StageScaleMode.EXACT_FIT;
                this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, "Fullscreen: Off"));
            }
            Parameters.save();
            _local_2.dispatchEvent(new Event(Event.RESIZE));
            return (true);
        }
        if (_arg_1 == "/mscale")
        {
            this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, (("Map Scale: " + Parameters.data_.mscale) + " - Usage: /mscale <any decimal number>.")));
            return (true);
        }
        var _local_3:Array = _arg_1.match("^/mscale (\\d*\\.*\\d+)$");
        if (_local_3 != null)
        {
            Parameters.data_.mscale = _local_3[1];
            Parameters.save();
            _local_2.dispatchEvent(new Event(Event.RESIZE));
            this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, ("Map Scale: " + _local_3[1])));
            return (true);
        }
        if (_arg_1 == "/scaleui")
        {
            Parameters.data_.uiscale = (!(Parameters.data_.uiscale));
            Parameters.save();
            _local_2.dispatchEvent(new Event(Event.RESIZE));
            this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, ("Scale UI: " + Parameters.data_.uiscale)));
            return (true);
        }
        return (false);
    }


}
}
