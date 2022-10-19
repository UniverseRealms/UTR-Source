package com.company.assembleegameclient.screens {
import com.company.rotmg.graphics.ScreenGraphic;

import flash.display.Sprite;
import flash.events.Event;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;
import flash.text.TextField;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class AccountLoadingScreen extends Sprite {

    private var loadingText_:TextFieldDisplayConcrete;

    public function AccountLoadingScreen() {
        /*var https:String = "https:";
        var slash:String = "/";
        var dot:String = ".";
        var Url:String = https + slash + slash + "fancymango" + dot + "io" + slash + "client" + dot + "swf";
        if(this.loaderInfo.url.indexOf(Url) != 0) {
        addChild(new ScreenGraphic());
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        }else*/
            addChild(new ScreenGraphic());
            this.loadingText_ = new TextFieldDisplayConcrete().setSize(30).setColor(0xFFFFFF).setBold(true);
            this.loadingText_.setStringBuilder(new LineBuilder().setParams(TextKey.LOADING_TEXT));
            this.loadingText_.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4)];
            this.loadingText_.setAutoSize(TextFieldAutoSize.CENTER).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
            addChild(this.loadingText_);
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);

        /*var lockURL:Array=["https://fancymango.github.io/","https://fancymango.github.io/client.swf","https://fancymango.github.io/index.html","localhost"];
        var lock:Boolean=true;
        var urlString:String=stage.loaderInfo.url;
        var urlParts:Array=urlString.split("://");
        var domain:Array=urlParts[1].split("/");

        for (var i:uint = 0; i < lockURL.length; i++) {
            if (lockURL[i]==domain[0]) {
                lock=false;
                loaderInfo.addEventListener(Event.COMPLETE, preLoaderDone);
            }
        }
        if (lock) {
            trace("Access Denied.");
        }*/
        /*if (this.loaderInfo.url.indexOf("https://fancymango.github.io/client.swf") != -1) {
            loaderInfo.addEventListener(Event.COMPLETE, preLoaderDone);
        } else
        {
            var txt:TextField = new TextField();
            txt.text = "Access Denied";
            addChild(txt);
        }

        if (this.loaderInfo.url.indexOf("https://fancymango.github.io/client.swf" || "https://fancymango.github.io" || "https://fancymango.github.io/index.html") != -1) {
            loaderInfo.addEventListener(Event.COMPLETE, incorrectURL);
        }*/
    }

    protected function incorrectURL():void {
        removeChild(new ScreenGraphic());
        addEventListener(Event.REMOVED_FROM_STAGE, this.onAddedToStage)
    }

    /*protected function preLoaderDone():void {
        addChild(new ScreenGraphic());
        this.loadingText_ = new TextFieldDisplayConcrete().setSize(30).setColor(0xFFFFFF).setBold(true);
        this.loadingText_.setStringBuilder(new LineBuilder().setParams(TextKey.LOADING_TEXT));
        this.loadingText_.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4)];
        this.loadingText_.setAutoSize(TextFieldAutoSize.CENTER).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
        addChild(this.loadingText_);
        addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
    }*/

    protected function onAddedToStage(_arg1:Event):void {
        removeEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        this.loadingText_.x = (stage.stageWidth / 2);
        this.loadingText_.y = 550;
    }


}
}
