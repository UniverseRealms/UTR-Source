package kabam.rotmg.game.view {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.FameUtil;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.TimeUtil;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.assets.services.IconFactory;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.SignalWaiter;

import org.osflash.signals.Signal;

public class CreditDisplay extends Sprite {

    private static const FONT_SIZE:int = 18;
    public static const IMAGE_NAME:String = "lofiObj3";
    public static const IMAGE_ID:int = 225;
    public static const waiter:SignalWaiter = new SignalWaiter();

    private var creditsText_:TextFieldDisplayConcrete;
    private var fameText_:TextFieldDisplayConcrete;
    private var onraneText:TextFieldDisplayConcrete;
    //private var kantosText:TextFieldDisplayConcrete;
    private var coinIcon_:Bitmap;
    private var fameIcon_:Bitmap;
    private var onraneIcon:Bitmap;
    //private var kantosIcon:Bitmap;
    private var credits_:int = -1;
    private var fame_:int = -1;
    private var onrane_:int = -1;
    //private var kantos_:int = -1;
    private var fortune_:int = -1;
    private var displayFame_:Boolean = true;
    private var gs:GameSprite;
    public var openAccountDialog:Signal;

    public function CreditDisplay(_arg_1:GameSprite = null, _arg_2:Boolean = true, _arg_3:Boolean = false, _arg_4:Number = 0) {
        this.openAccountDialog = new Signal();
        super();
        this.displayFame_ = _arg_2;
        this.gs = _arg_1;
        this.creditsText_ = this.makeTextField();
        waiter.push(this.creditsText_.textChanged);
        addChild(this.creditsText_);
        var _local_5:BitmapData = AssetLibrary.getImageFromSet(IMAGE_NAME, IMAGE_ID);
        _local_5 = TextureRedrawer.redraw(_local_5, 40, true, 0);
        this.coinIcon_ = new Bitmap(_local_5);
        addChild(this.coinIcon_);
        this.onraneText = this.makeTextField();
        waiter.push(this.onraneText.textChanged);
        addChild(this.onraneText);
        this.onraneIcon = new Bitmap(IconFactory.makeOnrane());
        addChild(this.onraneIcon);
        if (this.displayFame_) {
            this.fameText_ = this.makeTextField();
            waiter.push(this.fameText_.textChanged);
            addChild(this.fameText_);
            this.fameIcon_ = new Bitmap(FameUtil.getFameIcon());
            addChild(this.fameIcon_);
        }
        this.draw(0, 0, 0, 0, 0);
        mouseEnabled = true;
        doubleClickEnabled = true;
        addEventListener(MouseEvent.DOUBLE_CLICK, this.onDoubleClick, false, 0, true);
        waiter.complete.add(this.onAlignHorizontal);
    }

    private function onAlignHorizontal():void {
        this.coinIcon_.x = -(this.coinIcon_.width);
        this.creditsText_.x = ((this.coinIcon_.x - this.creditsText_.width) + 8);
        this.creditsText_.y = ((this.coinIcon_.y + (this.coinIcon_.height / 2)) - (this.creditsText_.height / 2));

        this.onraneIcon.x = (this.coinIcon_.x + 10);
        this.onraneIcon.y = (this.coinIcon_.y + 30);
        this.onraneText.x = ((this.onraneIcon.x - this.onraneText.width) - 2);
        this.onraneText.y = ((this.onraneIcon.y + (this.onraneIcon.height / 2)) - (this.onraneText.height / 2));

        if (this.displayFame_) {
            this.fameIcon_.x = (this.creditsText_.x - this.fameIcon_.width);
            this.fameText_.x = ((this.fameIcon_.x - this.fameText_.width) + 8);
            this.fameText_.y = ((this.fameIcon_.y + (this.fameIcon_.height / 2)) - (this.fameText_.height / 2));

            //this.kantosIcon.x = (this.fameIcon_.x + 10);
            //this.kantosIcon.y = (this.fameIcon_.y + 30);
            //this.kantosText.x = ((this.kantosIcon.x - this.kantosText.width) - 2);
            //this.kantosText.y = ((this.kantosIcon.y + (this.kantosIcon.height / 2)) - (this.kantosText.height / 2));
        }
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

    public function draw(_arg_1:int, _arg_2:int, _arg_3:int, _arg_4:int, _arg_5:int = 0):void {
        if (_arg_1 == this.credits_ && this.displayFame_ && _arg_2 == this.fame_ && _arg_3 == this.onrane_) {
            return;
        }
        this.credits_ = _arg_1;
        this.onrane_ = _arg_3;
        this.onraneText.setStringBuilder(new StaticStringBuilder(this.onrane_.toString()));
        //this.kantos_ = _arg_5;
        //this.kantosText.setStringBuilder(new StaticStringBuilder(this.kantos_.toString()));
        this.creditsText_.setStringBuilder(new StaticStringBuilder(this.credits_.toString()));
        if (this.displayFame_) {
            this.fame_ = _arg_2;
            this.fameText_.setStringBuilder(new StaticStringBuilder(this.fame_.toString()));
        }
        if (waiter.isEmpty()) {
            this.onAlignHorizontal();
        }
    }
}
}
