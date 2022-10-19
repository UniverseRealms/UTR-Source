package kabam.rotmg.potionStorage.UI
{
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.ui.SimpleText;

import flash.display.Sprite;

import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.popups.header.PopupHeader;
import io.decagames.rotmg.ui.popups.modal.ModalPopup;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.potionStorage.Items.Item1;
import kabam.rotmg.potionStorage.Items.Item10;
import kabam.rotmg.potionStorage.Items.Item2;
import kabam.rotmg.potionStorage.Items.Item3;
import kabam.rotmg.potionStorage.Items.Item4;
import kabam.rotmg.potionStorage.Items.Item5;
import kabam.rotmg.potionStorage.Items.Item6;
import kabam.rotmg.potionStorage.Items.Item7;
import kabam.rotmg.potionStorage.Items.Item8;
import kabam.rotmg.potionStorage.Items.Item9;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.ui.view.components.DarkLayer;

import org.osflash.signals.Signal;

public class PotionStorage extends Sprite
{
   private var gs_:AGameSprite;
   private var player:Player;
   public var quitButton:SliceScalingButton;
   public var ItemButton1:SliceScalingButton;
   public var ItemButton2:SliceScalingButton;
   public var ItemButton3:SliceScalingButton;
   public var ItemButton4:SliceScalingButton;
   public var ItemButton5:SliceScalingButton;
   public var ItemButton6:SliceScalingButton;
   public var ItemButton7:SliceScalingButton;
   public var ItemButton8:SliceScalingButton;
   public var ItemButton9:SliceScalingButton;
   public var ItemButton10:SliceScalingButton;
   public var ItemOne:Item1;
   public var ItemTwo:Item2
   public var ItemThree:Item3;
   public var ItemFour:Item4;
   public var ItemFive:Item5;
   public var ItemSix:Item6;
   public var ItemSeven:Item7;
   public var ItemEight:Item8;
   public var ItemNine:Item9;
   public var ItemTen:Item10;
   public const close:Signal = new Signal();

   private var backgroundAll:SliceScalingBitmap;
   private var nameTextBackground:SliceScalingBitmap;
   public var Title:TextFieldDisplayConcrete;
   public var DecaUi:SliceScalingBitmap;
   public var Background:SliceScalingBitmap;
   public var Box:SliceScalingBitmap;

   public function PotionStorage(arg1:AGameSprite)
   {
      this.gs_ = arg1;
      this.backgroundAll = TextureParser.instance.getSliceScalingBitmap("UI", "popup_header", 600);
      this.backgroundAll.y = 0;
      addChild(this.backgroundAll);
      this.nameTextBackground = TextureParser.instance.getSliceScalingBitmap("UI", "popup_header_title", 400);
      this.nameTextBackground.y = this.backgroundAll.y + 26;
      this.nameTextBackground.x = 105;
      addChild(this.nameTextBackground);
      this.Title = new TextFieldDisplayConcrete().setSize(30).setColor(0xFFFFFF).setBold(true);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Potion Storage"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = nameTextBackground.x + 200;
      this.Title.y = nameTextBackground.y + 20;
      addChild(this.Title);

      this.Background = TextureParser.instance.getSliceScalingBitmap("UI", "tab_cointainer_background");
      this.Background.width = 575;
      this.Background.height = 500;
      this.Background.x = 10;
      this.Background.y = 135;
      addChild(this.Background);

      this.quitButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "close_button"));
      this.quitButton.addEventListener(MouseEvent.CLICK,this.onClose);
      this.quitButton.x = 417;
      this.quitButton.y = 45;
      addChild(this.quitButton);

      Box1();
      Box2();
      Box3();
      Box4();
      Box5();
      Box6();
      Box7();
      Box8();
      Box9();
      Box10();
   }

   private function Box1() : void
   {
      var BoxX:int = 24;
      var BoxY:int = Background.y + 15;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton1 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton1.x = Box.x + 17;
      this.ItemButton1.y = Box.y + 92;
      this.ItemButton1.width = 92;
      this.ItemButton1.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton1);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Attack Potion x "));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemOne = new Item1();
      this.ItemOne.scaleX = 1.5;
      this.ItemOne.scaleY = 1.5;
      ItemOne.x = Box.x + 33;
      ItemOne.y = Box.y + 31;
      addChild(this.ItemOne);
   }

   private function Box2() : void
   {
      var BoxX:int = 164;
      var BoxY:int = Background.y + 15;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton2 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton2.x = Box.x + 17;
      this.ItemButton2.y = Box.y + 92;
      this.ItemButton2.width = 92;
      this.ItemButton2.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton2);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Defense Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemTwo = new Item2();
      this.ItemTwo.scaleX = 1.5;
      this.ItemTwo.scaleY = 1.5;
      ItemTwo.x = Box.x + 33;
      ItemTwo.y = Box.y + 31;
      addChild(this.ItemTwo);
   }

   private function Box3() : void
   {
      var BoxX:int = 305;
      var BoxY:int = Background.y + 15;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton3 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton3.x = Box.x + 17;
      this.ItemButton3.y = Box.y + 92;
      this.ItemButton3.width = 92;
      this.ItemButton3.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton3);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Speed Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemThree = new Item3();
      this.ItemThree.scaleX = 1.5;
      this.ItemThree.scaleY = 1.5;
      ItemThree.x = Box.x + 33;
      ItemThree.y = Box.y + 31;
      addChild(this.ItemThree);
   }

   private function Box4() : void
   {
      var BoxX:int = 445;
      var BoxY:int = Background.y + 15;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton4 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton4.x = Box.x + 17;
      this.ItemButton4.y = Box.y + 92;
      this.ItemButton4.width = 92;
      this.ItemButton4.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton4);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Dexterity Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemFour = new Item4();
      this.ItemFour.scaleX = 1.5;
      this.ItemFour.scaleY = 1.5;
      ItemFour.x = Box.x + 33;
      ItemFour.y = Box.y + 31;
      addChild(this.ItemFour);
   }

   private function Box5() : void
   {
      var BoxX:int = 24;
      var BoxY:int = Background.y + 163;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton5 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton5.x = Box.x + 17;
      this.ItemButton5.y = Box.y + 92;
      this.ItemButton5.width = 92;
      this.ItemButton5.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton5);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Vitality Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemFive = new Item5();
      this.ItemFive.scaleX = 1.5;
      this.ItemFive.scaleY = 1.5;
      ItemFive.x = Box.x + 33;
      ItemFive.y = Box.y + 31;
      addChild(this.ItemFive);
   }

   private function Box6() : void
   {
      var BoxX:int = 164;
      var BoxY:int = Background.y + 163;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton6 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton6.x = Box.x + 17;
      this.ItemButton6.y = Box.y + 92;
      this.ItemButton6.width = 92;
      this.ItemButton6.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton6);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Wisdom Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemSix = new Item6();
      this.ItemSix.scaleX = 1.5;
      this.ItemSix.scaleY = 1.5;
      ItemSix.x = Box.x + 33;
      ItemSix.y = Box.y + 31;
      addChild(this.ItemSix);
   }

   private function Box7() : void
   {
      var BoxX:int = 305;
      var BoxY:int = Background.y + 163;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton7 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton7.x = Box.x + 17;
      this.ItemButton7.y = Box.y + 92;
      this.ItemButton7.width = 92;
      this.ItemButton7.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton7);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Life Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemSeven = new Item7();
      this.ItemSeven.scaleX = 1.5;
      this.ItemSeven.scaleY = 1.5;
      ItemSeven.x = Box.x + 33;
      ItemSeven.y = Box.y + 31;
      addChild(this.ItemSeven);
   }

   private function Box8() : void
   {
      var BoxX:int = 445;
      var BoxY:int = Background.y + 163;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton8 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton8.x = Box.x + 17;
      this.ItemButton8.y = Box.y + 92;
      this.ItemButton8.width = 92;
      this.ItemButton8.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton8);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Mana Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemEight = new Item8();
      this.ItemEight.scaleX = 1.5;
      this.ItemEight.scaleY = 1.5;
      ItemEight.x = Box.x + 33;
      ItemEight.y = Box.y + 31;
      addChild(this.ItemEight);
   }

   private function Box9() : void
   {
      var BoxX:int = 164;
      var BoxY:int = Background.y + 311;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton9 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton9.x = Box.x + 17;
      this.ItemButton9.y = Box.y + 92;
      this.ItemButton9.width = 92;
      this.ItemButton9.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton9);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Restoration Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemNine = new Item9();
      this.ItemNine.scaleX = 1.5;
      this.ItemNine.scaleY = 1.5;
      ItemNine.x = Box.x + 33;
      ItemNine.y = Box.y + 31;
      addChild(this.ItemNine);
   }

   private function Box10() : void
   {
      var BoxX:int = 305;
      var BoxY:int = Background.y + 311;

      this.Box = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_background");
      this.Box.width = 125;
      this.Box.height = 135;
      this.Box.x = BoxX;
      this.Box.y = BoxY;
      addChild(this.Box);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_title_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 2;
      addChild(this.DecaUi);

      this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
      this.DecaUi.width = Box.width;
      this.DecaUi.x = Box.x;
      this.DecaUi.y = Box.y + 88;
      addChild(this.DecaUi);

      this.ItemButton10 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
      this.ItemButton10.x = Box.x + 17;
      this.ItemButton10.y = Box.y + 92;
      this.ItemButton10.width = 92;
      this.ItemButton10.setLabel("Consume",DefaultLabelFormat.defaultModalTitle);
      addChild(this.ItemButton10);

      this.Title = new TextFieldDisplayConcrete().setSize(12).setColor(0xFFFFFF).setBold(false);
      this.Title.setAutoSize(TextFieldAutoSize.CENTER);
      this.Title.setBold(true);
      this.Title.setStringBuilder(new LineBuilder().setParams("Luck Potion"));
      this.Title.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
      this.Title.x = Box.x + 60;
      this.Title.y = Box.y + 7;
      addChild(this.Title);

      this.ItemTen = new Item10();
      this.ItemTen.scaleX = 1.5;
      this.ItemTen.scaleY = 1.5;
      ItemTen.x = Box.x + 33;
      ItemTen.y = Box.y + 31;
      addChild(this.ItemTen);
   }

   private function onClose(param1:Event) : void
   {
      this.close.dispatch();
   }
}
}