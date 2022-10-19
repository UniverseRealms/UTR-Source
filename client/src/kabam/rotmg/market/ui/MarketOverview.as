package kabam.rotmg.market.ui
{
import com.company.assembleegameclient.screens.TitleMenuOption;

import flash.display.Sprite;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.account.core.view.EmptyFrame;
import kabam.rotmg.pets.view.components.MarketCloseButton;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

public class MarketOverview extends EmptyFrame
   {
       
      
      private var currentScreen:Sprite;
      
      private var myOffersButton:TitleMenuOption;
      
      private var createOfferButton:TitleMenuOption;
      
      public var closeButton_:MarketCloseButton;

       public static var MarketPlaceOpened:Boolean;
      
      public function MarketOverview()
      {
          var _loc2_:Sprite = new Sprite();
         super();
          addChild(_loc2_);
         var _loc1_:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(46).setBold(true).setStringBuilder(new StaticStringBuilder("UT Reborn")).setAutoSize(TextFieldAutoSize.CENTER).setTextWidth(600).setColor(16777215).setPosition(100,30);
          _loc1_.y -= 10;
          addChild(_loc1_);
         this.closeButton_ = new MarketCloseButton(1);
         this.closeButton_.x = 566;
         this.closeButton_.y = 30;

          MarketPlaceOpened = true;

         addChild(this.closeButton_);
         this.myOffersButton = this.createButton("My offers",10,this.myOffers,true);
          this.myOffersButton.y -= 70;
          this.myOffersButton.x += 10;
         this.createOfferButton = this.createButton("Create offer",110,this.createOffer);
          this.createOfferButton.y -= 70;
          this.createOfferButton.x += 575;
         this.myOffers();
      }
      
      private function createOffer() : void
      {
         var screen:MarketCreateOfferScreen = new MarketCreateOfferScreen();
         screen.removed.add(function():void
         {
            createOfferButton.activate();
             myOffersButton.deactivate();
         });
         this.setScreen(screen);
         this.createOfferButton.deactivate();
          this.myOffersButton.activate();
      }
      
      public function myOffers() : void
      {
         var screen:MarketMyOffersScreen = new MarketMyOffersScreen();
         screen.removed.add(function():void
         {
            myOffersButton.activate();
             createOfferButton.deactivate();
         });
         this.setScreen(screen);
          this.createOfferButton.activate();
         this.myOffersButton.deactivate();
      }
      
      public function setScreen(param1:Sprite) : void
      {
         if(this.currentScreen && contains(this.currentScreen))
         {
            removeChild(this.currentScreen);
         }
         this.currentScreen = param1;
         this.currentScreen.y = 100;
         addChild(param1);
      }
      
      override protected function makeModalBackground() : Sprite
      {
         x = 0;
         y = 0;
         var _loc1_:Sprite = new Sprite();
            //Left side
          _loc1_.graphics.beginFill(0x11091c, 0.9);
          _loc1_.graphics.moveTo(140, 60);
          _loc1_.graphics.lineTo(140, 160);
          _loc1_.graphics.lineTo(100, 110);
          _loc1_.graphics.endFill();
          _loc1_.graphics.lineStyle();
          _loc1_.graphics.beginFill(0x40214f, 0.9);
          _loc1_.graphics.moveTo(140, 80);
          _loc1_.graphics.lineTo(140, 140);
          _loc1_.graphics.lineTo(115, 110);
          _loc1_.graphics.endFill();
          _loc1_.graphics.lineStyle();

            //Right Side
          _loc1_.graphics.beginFill(0x11091c, 0.9);
          _loc1_.graphics.moveTo(660, 60);
          _loc1_.graphics.lineTo(660, 160);
          _loc1_.graphics.lineTo(700, 110);
          _loc1_.graphics.endFill();
          _loc1_.graphics.lineStyle();
          _loc1_.graphics.beginFill(0x40214f, 0.9);
          _loc1_.graphics.moveTo(660, 80);
          _loc1_.graphics.lineTo(660, 140);
          _loc1_.graphics.lineTo(685, 110);
          _loc1_.graphics.endFill();
          _loc1_.graphics.lineStyle();
         //
         _loc1_.graphics.beginFill(0,0.9);
         _loc1_.graphics.drawRect(140,60,520,480);
         _loc1_.graphics.endFill();
         _loc1_.graphics.lineStyle(3, 0x903da0);
         _loc1_.graphics.moveTo(140, 60);
          _loc1_.graphics.lineTo(660, 60);
         _loc1_.graphics.endFill();
          //Line underneath title
          _loc1_.graphics.beginFill(0,0.9);
          _loc1_.graphics.lineStyle(3, 0x903da0);
          _loc1_.graphics.moveTo(160, 60);
          _loc1_.graphics.lineTo(195, 20);
          _loc1_.graphics.lineTo(605, 20);
          _loc1_.graphics.lineTo(640, 60);
          _loc1_.graphics.endFill();
          //My Offers
          _loc1_.graphics.beginFill(0,0.9);
          _loc1_.graphics.lineStyle(3, 0x903da0);
          _loc1_.graphics.moveTo(1, 30);
          _loc1_.graphics.lineTo(120, 30);
          _loc1_.graphics.lineTo(140, 1);
          _loc1_.graphics.lineTo(1, 1);
          _loc1_.graphics.endFill();
          //Create offers
          _loc1_.graphics.beginFill(0,0.9);
          _loc1_.graphics.lineStyle(3, 0x903da0);
          _loc1_.graphics.moveTo(799, 30);
          _loc1_.graphics.lineTo(680, 30);
          _loc1_.graphics.lineTo(660, 1);
          _loc1_.graphics.lineTo(799, 1);
          _loc1_.graphics.endFill();
         return _loc1_;
      }
      
      private function createButton(param1:String, param2:Number, param3:Function, param4:Boolean = false) : TitleMenuOption
      {
         var _loc5_:TitleMenuOption = new TitleMenuOption(param1,18,false);
         _loc5_.setAutoSize(TextFieldAutoSize.LEFT);
         _loc5_.setVerticalAlign("middle");
         _loc5_.x = param2;
         _loc5_.y = 85;
         _loc5_.clicked.add(param3);
         if(param4)
         {
            _loc5_.deactivate();
         }
         addChild(_loc5_);
         return _loc5_;
      }
   }
}
