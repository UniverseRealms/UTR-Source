package io.decagames.rotmg.dailyQuests.view.popup
{
   import flash.display.Bitmap;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import kabam.rotmg.assets.services.IconFactory;
   
   public class BuyQuestRefreshButton extends SliceScalingButton
   {
       
      
      private var _price:int;
      
      public function BuyQuestRefreshButton(param1:int)
      {
         super(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button",32));
         this._price = param1;
         this.init();
      }
      
      private function init() : void
      {
         var _loc1_:Bitmap = null;
         _loc1_ = null;
         this.width = 100;
         this.setLabelMargin(-10,0);
         this.setLabel(this._price.toString(),DefaultLabelFormat.defaultButtonLabel);
         _loc1_ = new Bitmap(IconFactory.makeCoin());
         _loc1_.x = this.width - 2 * _loc1_.width;
         _loc1_.y = (this.height - _loc1_.height) / 2;
         addChild(_loc1_);
      }
   }
}
