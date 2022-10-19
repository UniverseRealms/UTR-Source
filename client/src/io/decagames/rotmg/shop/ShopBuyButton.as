package io.decagames.rotmg.shop
{
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import kabam.rotmg.assets.services.IconFactory;
   
   public class ShopBuyButton extends SliceScalingButton {


      private var _priceLabel:UILabel;

      private var coinBitmap:Bitmap;

      private var _price:int;

      private var _soldOut:Boolean;

      private var _currency:int;

      private var _showCampaignTooltip:Boolean;

      public function ShopBuyButton(param1:int, param2:int = 0) {
         super(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
         this._price = param1;
         this._priceLabel = new UILabel();
         this._priceLabel.text = param1.toString();
         this._priceLabel.y = 7;
         this._currency = param2;
         var _loc3_:BitmapData = param2 == 0 ? IconFactory.makeCoin() : IconFactory.makeFame();
         this.coinBitmap = new Bitmap(_loc3_);
         this.coinBitmap.y = Math.round(this.coinBitmap.height / 2) - 3;
         DefaultLabelFormat.priceButtonLabel(this._priceLabel);
         addChild(this._priceLabel);
         addChild(this.coinBitmap);
      }

      override public function dispose():void {
         this.coinBitmap.bitmapData.dispose();
         super.dispose();
      }

      override public function set width(param1:Number):void {
         super.width = param1;
         this.updateLabelPosition();
      }
      
      private function updateLabelPosition() : void
      {
         if(this.coinBitmap.parent)
         {
            this._priceLabel.x = bitmap.width - 38 - this._priceLabel.textWidth;
         }
         else
         {
            this._priceLabel.x = (bitmap.width - this._priceLabel.textWidth) / 2;
         }
         this.coinBitmap.x = bitmap.width - this.coinBitmap.width - 15;
      }
      
      public function set price(param1:int) : void
      {
         this._price = param1;
         if(!this._soldOut)
         {
            this.priceLabel.text = param1.toString();
            this.updateLabelPosition();
         }
      }
      
      public function get priceLabel() : UILabel
      {
         return this._priceLabel;
      }
      
      public function get soldOut() : Boolean
      {
         return this._soldOut;
      }
      
      public function set soldOut(param1:Boolean) : void
      {
         this._soldOut = param1;
         disabled = param1;
         if(param1)
         {
            this._priceLabel.text = "Sold out";
            if(this.coinBitmap && this.coinBitmap.parent)
            {
               removeChild(this.coinBitmap);
            }
         }
         else
         {
            this._priceLabel.text = this._price.toString();
            addChild(this.coinBitmap);
         }
         this.updateLabelPosition();
      }
      
      public function get price() : int
      {
         return this._price;
      }
      
      public function get currency() : int
      {
         return this._currency;
      }
      
      public function get showCampaignTooltip() : Boolean
      {
         return this._showCampaignTooltip;
      }
      
      public function set showCampaignTooltip(param1:Boolean) : void
      {
         this._showCampaignTooltip = param1;
      }
   }
}
