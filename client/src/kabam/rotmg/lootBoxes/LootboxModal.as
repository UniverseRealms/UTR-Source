package kabam.rotmg.lootBoxes {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.util.Currency;
import com.company.util.MoreColorUtil;

import flash.display.DisplayObject;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.filters.GlowFilter;
import flash.geom.ColorTransform;
import flash.text.TextFieldAutoSize;
import flash.text.TextFormatAlign;

import kabam.rotmg.account.core.view.EmptyFrame;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.dialogs.control.FlushPopupStartupQueueSignal;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.game.view.LootboxesDisplay;
import kabam.rotmg.pets.view.components.PopupWindowBackground;
import kabam.rotmg.text.model.FontModel;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.util.components.LegacyBuyButton;

public class LootboxModal extends EmptyFrame {

    public static const MODAL_WIDTH:int = 490;
    public static const MODAL_HEIGHT:int = 540;
    private static const OVER_COLOR_TRANSFORM:ColorTransform = new ColorTransform(1, (220 / 0xFF), (133 / 0xFF));
    private static const DROP_SHADOW_FILTER:DropShadowFilter = new DropShadowFilter(0, 0, 0);
    private static const GLOW_FILTER:GlowFilter = new GlowFilter(0xFF0000, 1, 11, 5);
    private static const filterWithGlow:Array = [DROP_SHADOW_FILTER, GLOW_FILTER];
    private static const filterNoGlow:Array = [DROP_SHADOW_FILTER];

    public static var backgroundImageEmbed:Class = LootboxBackground_ImageEmbed;
    public static var modalWidth:int = MODAL_WIDTH;//440
    public static var modalHeight:int = MODAL_HEIGHT;//400

    private var fontModel:FontModel;
    private var triggeredOnStartup:Boolean;
    public var lootboxDisplay_:LootboxesDisplay;
    private var Lootbox_Image1:BronzeLootbox_ImageEmbed;
    private var Lootbox_Image2:SilverLootbox_ImageEmbed;
    private var Lootbox_Image3:GoldLootbox_ImageEmbed;
    private var Lootbox_Image4:EliteLootbox_ImageEmbed;
    private var Lootbox_Image5:PremiumLootbox_ImageEmbed;

    private var Lootbox_Image6:LockerLootbox_ImageEmbed;
    private var Lootbox_Image7:EventLootbox_ImageEmbed;

    private var Lootbox1Title:TextFieldDisplayConcrete;
    private var Lootbox2Title:TextFieldDisplayConcrete;
    private var Lootbox3Title:TextFieldDisplayConcrete;
    private var Lootbox4Title:TextFieldDisplayConcrete;
    private var Lootbox5Title:TextFieldDisplayConcrete;

    private var Lootbox6Title:TextFieldDisplayConcrete;
    private var Lootbox7Title:TextFieldDisplayConcrete;

    public var Lootbox1Amount:LegacyBuyButton;
    public var Lootbox2Amount:LegacyBuyButton;
    public var Lootbox3Amount:DeprecatedTextButton;
    public var Lootbox4Amount:DeprecatedTextButton;
    public var Lootbox5Amount:LegacyBuyButton;

    public var Lootbox6Amount:LegacyBuyButton;
    public var Lootbox7Amount:DeprecatedTextButton;

    public function LootboxModal(_arg1:Boolean = false) {
        this.triggeredOnStartup = _arg1;
        this.fontModel = StaticInjectorContext.getInjector().getInstance(FontModel);
        modalWidth = MODAL_WIDTH;
        modalHeight = MODAL_HEIGHT;
        super(modalWidth, modalHeight);
        this.setCloseButton(true);
        this.setTitle("Lootboxes", true);
        addEventListener(Event.ADDED_TO_STAGE, this.onAdded);
        addEventListener(Event.REMOVED_FROM_STAGE, this.destroy);
        closeButton.clicked.add(this.onCloseButtonClicked);
    }

    public static function getText(_arg1:String, _arg2:int, _arg3:int, _arg4:Boolean):TextFieldDisplayConcrete {
        var _local5:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(18).setColor(0xFFFFFF).setTextWidth(((LootboxModal.modalWidth - (TEXT_MARGIN * 2)) - 10));
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

    private function onAdded(_arg1:Event) : void {
    }


    private function destroy(_arg1:Event):void {
        removeEventListener(Event.ADDED_TO_STAGE, this.onAdded);
        removeEventListener(Event.REMOVED_FROM_STAGE, this.destroy);
    }

    private function onArrowHover(_arg1:MouseEvent):void {
        _arg1.currentTarget.transform.colorTransform = OVER_COLOR_TRANSFORM;
    }

    private function onArrowHoverOut(_arg1:MouseEvent):void {
        _arg1.currentTarget.transform.colorTransform = MoreColorUtil.identity;
    }



    override protected function makeModalBackground():Sprite {
        var _local1:Sprite = new Sprite();
        var _local2:DisplayObject = new backgroundImageEmbed();
        _local2.width = (modalWidth + 1);
        _local2.height = (modalHeight - 25);
        _local2.y = 27;
        _local2.alpha = 1.00;
        this.Lootbox_Image1 = new BronzeLootbox_ImageEmbed();
        this.Lootbox_Image1.y = 100;
        this.Lootbox_Image1.x = 14;
        Lootbox_Image1.width = 21 * 4; // the original size of the lootboxes are 19 x 16, I added 2 to each to allow one pixel and then x 4 to get the perfect size.
        Lootbox_Image1.height = 18 * 4;
        Lootbox_Image1.filters = [new DropShadowFilter(0, 0, 0xFF7700, 1, 12, 12, 1.5)]; // Bronze LootBox
        this.Lootbox_Image2 = new SilverLootbox_ImageEmbed();
        this.Lootbox_Image2.y = 100;
        this.Lootbox_Image2.x = 109;
        Lootbox_Image2.width = 21 * 4;
        Lootbox_Image2.height = 18 * 4;
        Lootbox_Image2.filters = [new DropShadowFilter(0, 0, 0xC0CBDC, 1, 12, 12, 1.5)]; // Silver LootBox
        this.Lootbox_Image3 = new GoldLootbox_ImageEmbed();
        this.Lootbox_Image3.y = 100;
        this.Lootbox_Image3.x = 204;
        Lootbox_Image3.width = 21 * 4;
        Lootbox_Image3.height = 18 * 4;
        Lootbox_Image3.filters = [new DropShadowFilter(0, 0, 0xF7AA22, 1, 12, 12, 1.5)]; // Gold LootBox
        this.Lootbox_Image4 = new EliteLootbox_ImageEmbed();
        this.Lootbox_Image4.y = 100;
        this.Lootbox_Image4.x = 299;
        Lootbox_Image4.width = 21 * 4;
        Lootbox_Image4.height = 18 * 4;
        Lootbox_Image4.filters = [new DropShadowFilter(0, 0, 0xA80013, 1, 12, 12, 1.5)]; // Elite LootBox
        this.Lootbox_Image5 = new PremiumLootbox_ImageEmbed();
        this.Lootbox_Image5.y = 100;
        this.Lootbox_Image5.x = 394;
        Lootbox_Image5.width = 21 * 4;
        Lootbox_Image5.height = 18 * 4;
        Lootbox_Image5.filters = [new DropShadowFilter(0, 0, 0x00FFF6, 1, 12, 12, 1.5)]; // Premium LootBox
        this.Lootbox_Image6 = new LockerLootbox_ImageEmbed();
        this.Lootbox_Image6.y = 260;
        this.Lootbox_Image6.x = 14;
        Lootbox_Image6.width = 21 * 4;
        Lootbox_Image6.height = 18 * 4;
        Lootbox_Image6.filters = [new DropShadowFilter(0, 0, 0xFF0044, 1, 12, 12, 1.5)]; // Locker LootBox
        this.Lootbox_Image7 = new EventLootbox_ImageEmbed();
        this.Lootbox_Image7.y = 260;
        this.Lootbox_Image7.x = 109;
        Lootbox_Image7.width = 21 * 4;
        Lootbox_Image7.height = 18 * 4;
        Lootbox_Image7.filters = [new DropShadowFilter(0, 0, 0x00FF21, 1, 12, 12, 1.5)]; // Event LootBox

        this.Lootbox1Title = new TextFieldDisplayConcrete().setSize(10).setColor(0xD36200).setBold(true).setTextWidth(20);
        this.Lootbox1Title.setStringBuilder(new StaticStringBuilder().setString(("Fame Lootbox")));
        this.Lootbox1Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox1Title.x = this.Lootbox_Image1.x + 2;
        this.Lootbox1Title.y = this.Lootbox_Image1.y - 20;

        this.Lootbox1Amount = new LegacyBuyButton("", 12, 2000, Currency.FAME);
        this.Lootbox1Amount.x = this.Lootbox_Image1.x + 15;
        this.Lootbox1Amount.y = this.Lootbox_Image1.y + 80;
        this.Lootbox1Amount.setEnabled(true);

        this.Lootbox2Title = new TextFieldDisplayConcrete().setSize(10).setColor(0xC0CBDC).setBold(true).setTextWidth(20);
        this.Lootbox2Title.setStringBuilder(new StaticStringBuilder().setString(("Coin Lootbox")));
        this.Lootbox2Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox2Title.x = this.Lootbox_Image2.x + 5;
        this.Lootbox2Title.y = this.Lootbox_Image2.y - 20;

        this.Lootbox2Amount = new LegacyBuyButton("", 12, 1000, Currency.GOLD);
        this.Lootbox2Amount.x = this.Lootbox_Image2.x + 15;
        this.Lootbox2Amount.y = this.Lootbox_Image2.y + 80;
        this.Lootbox2Amount.setEnabled(true);


        this.Lootbox3Title = new TextFieldDisplayConcrete().setSize(10).setColor(0xF7AA22).setBold(true).setTextWidth(20);
        this.Lootbox3Title.setStringBuilder(new StaticStringBuilder().setString(("Gold Lootbox")));
        this.Lootbox3Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox3Title.x = this.Lootbox_Image3.x + 6;
        this.Lootbox3Title.y = this.Lootbox_Image3.y - 20;

        this.Lootbox3Amount = new DeprecatedTextButton(12, "Unbox");
        this.Lootbox3Amount.x = this.Lootbox_Image3.x + 15;
        this.Lootbox3Amount.y = this.Lootbox_Image3.y + 80;
        this.Lootbox3Amount.setEnabled(true);

        this.Lootbox4Title = new TextFieldDisplayConcrete().setSize(10).setColor(0xA80013).setBold(true).setTextWidth(20);
        this.Lootbox4Title.setStringBuilder(new StaticStringBuilder().setString(("Elite Lootbox")));
        this.Lootbox4Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox4Title.x = this.Lootbox_Image4.x + 8;
        this.Lootbox4Title.y = this.Lootbox_Image4.y - 20;

        this.Lootbox4Amount = new DeprecatedTextButton(12,  "Unbox");
        this.Lootbox4Amount.x = this.Lootbox_Image4.x + 23;
        this.Lootbox4Amount.y = this.Lootbox_Image4.y + 80;
        this.Lootbox4Amount.setEnabled(true);

        this.Lootbox5Title = new TextFieldDisplayConcrete().setSize(10).setColor(0x00FFF6).setBold(true).setTextWidth(20);
        this.Lootbox5Title.setStringBuilder(new StaticStringBuilder().setString(("Premium Lootbox")));
        this.Lootbox5Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox5Title.x = this.Lootbox_Image5.x - 3;
        this.Lootbox5Title.y = this.Lootbox_Image5.y - 20;

        this.Lootbox5Amount = new LegacyBuyButton("", 12, 5000, Currency.KANTOS);
        this.Lootbox5Amount.x = this.Lootbox_Image5.x + 15;
        this.Lootbox5Amount.y = this.Lootbox_Image5.y + 80;
        this.Lootbox5Amount.setEnabled(true);

        this.Lootbox6Title = new TextFieldDisplayConcrete().setSize(10).setColor(0xFF0044).setBold(true).setTextWidth(20);
        this.Lootbox6Title.setStringBuilder(new StaticStringBuilder().setString(("Legendary Box")));
        this.Lootbox6Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox6Title.x = this.Lootbox_Image6.x + 3;
        this.Lootbox6Title.y = this.Lootbox_Image6.y - 20;

        this.Lootbox6Amount = new LegacyBuyButton("", 12, 10000000, Currency.GOLD);
        this.Lootbox6Amount.x = this.Lootbox_Image6.x + 8;
        this.Lootbox6Amount.y = this.Lootbox_Image6.y + 80;
        this.Lootbox6Amount.setEnabled(true);

        this.Lootbox7Title = new TextFieldDisplayConcrete().setSize(10).setColor(0x00FF21).setBold(true).setTextWidth(20);
        this.Lootbox7Title.setStringBuilder(new StaticStringBuilder().setString(("Event Lootbox")));
        this.Lootbox7Title.filters = [new DropShadowFilter(0, 0, 0, 0.5, 12, 12)];
        this.Lootbox7Title.x = this.Lootbox_Image7.x + 5;
        this.Lootbox7Title.y = this.Lootbox_Image7.y - 20;

        this.Lootbox7Amount = new DeprecatedTextButton(12, "Unbox");
        this.Lootbox7Amount.x = this.Lootbox_Image7.x + 15;
        this.Lootbox7Amount.y = this.Lootbox_Image7.y + 80;
        this.Lootbox7Amount.setEnabled(true);

        var _local4:PopupWindowBackground = new PopupWindowBackground();
        _local4.draw(modalWidth, modalHeight, PopupWindowBackground.TYPE_TRANSPARENT_WITH_HEADER);
        _local1.addChild(_local2);
        _local1.addChild(_local4);
        _local1.addChild(this.Lootbox_Image1);
        _local1.addChild(this.Lootbox_Image2);
        _local1.addChild(this.Lootbox_Image3);
        _local1.addChild(this.Lootbox_Image4);
        _local1.addChild(this.Lootbox_Image5);
        _local1.addChild(this.Lootbox_Image6);
        _local1.addChild(this.Lootbox_Image7);
        _local1.addChild(this.Lootbox1Title);
        _local1.addChild(this.Lootbox2Title);
        _local1.addChild(this.Lootbox3Title);
        _local1.addChild(this.Lootbox4Title);
        _local1.addChild(this.Lootbox5Title);
        _local1.addChild(this.Lootbox6Title);
        _local1.addChild(this.Lootbox7Title);
        _local1.addChild(this.Lootbox1Amount);
        _local1.addChild(this.Lootbox2Amount);
        _local1.addChild(this.Lootbox3Amount);
        _local1.addChild(this.Lootbox4Amount);
        _local1.addChild(this.Lootbox5Amount);
        _local1.addChild(this.Lootbox6Amount);
        _local1.addChild(this.Lootbox7Amount);
        this.lootboxDisplay_ = new LootboxesDisplay(null);

        this.lootboxDisplay_.lootbox1Icon.x = Lootbox_Image1.x + 5000;
        this.lootboxDisplay_.lootbox1Icon.y = Lootbox_Image1.y + 5000;
        this.lootboxDisplay_.lootbox1Text.x = Lootbox_Image1.x + 5000;
        this.lootboxDisplay_.lootbox1Text.y = Lootbox_Image1.y + 5000;

        this.lootboxDisplay_.lootbox2Icon.x = Lootbox_Image2.x + 5000;
        this.lootboxDisplay_.lootbox2Icon.y = Lootbox_Image2.y + 5000;
        this.lootboxDisplay_.lootbox2Text.x = Lootbox_Image2.x + 5000;
        this.lootboxDisplay_.lootbox2Text.y = Lootbox_Image2.y + 5000;

        this.lootboxDisplay_.lootbox3Icon.x = Lootbox_Image3.x + 2;
        this.lootboxDisplay_.lootbox3Icon.y = Lootbox_Image3.y + 100;
        this.lootboxDisplay_.lootbox3Text.x = Lootbox_Image3.x + 33;
        this.lootboxDisplay_.lootbox3Text.y = Lootbox_Image3.y + 110;

        this.lootboxDisplay_.lootbox4Icon.x = Lootbox_Image4.x + 2;
        this.lootboxDisplay_.lootbox4Icon.y = Lootbox_Image4.y + 100;
        this.lootboxDisplay_.lootbox4Text.x = Lootbox_Image4.x + 33;
        this.lootboxDisplay_.lootbox4Text.y = Lootbox_Image4.y + 110;

        this.lootboxDisplay_.lootbox7Icon.x = Lootbox_Image7.x + 2;
        this.lootboxDisplay_.lootbox7Icon.y = Lootbox_Image7.y + 100;
        this.lootboxDisplay_.lootbox7Text.x = Lootbox_Image7.x + 33;
        this.lootboxDisplay_.lootbox7Text.y = Lootbox_Image7.y + 110;

        _local1.addChild(this.lootboxDisplay_);
        var _local_3:Player = StaticInjectorContext.getInjector().getInstance(GameModel).player;
        if (_local_3 != null) {
            this.lootboxDisplay_.draw(_local_3.bronzeLootbox_, _local_3.silverLootbox_, _local_3.goldLootbox_, _local_3.eliteLootbox_, _local_3.eventLootbox_);
        }
        //_local1.addChild(_local5);
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
