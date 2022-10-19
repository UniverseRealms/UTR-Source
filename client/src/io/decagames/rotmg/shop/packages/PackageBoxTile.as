package io.decagames.rotmg.shop.packages
{
   import flash.display.Loader;
   import flash.display.Sprite;
   import io.decagames.rotmg.shop.genericBox.GenericBoxTile;
   import io.decagames.rotmg.shop.genericBox.data.GenericBoxInfo;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import kabam.rotmg.packages.model.PackageInfo;
   
   public class PackageBoxTile extends GenericBoxTile
   {
       
      
      private var imageMask:SliceScalingBitmap;
      
      public function PackageBoxTile(param1:GenericBoxInfo, param2:Boolean = false)
      {
         buyButtonBitmapBackground = "buy_button_background";
         backgroundContainer = new Sprite();
         super(param1,param2);
      }
      
      override protected function createBoxBackground() : void
      {
         addChild(backgroundContainer);
         this.resizeBackgroundImage();
      }
      
      private function resizeBackgroundImage() : void
      {
         var _loc1_:Loader = null;
         if(_isPopup)
         {
            _loc1_ = PackageInfo(_boxInfo).popupLoader;
         }
         else
         {
            _loc1_ = PackageInfo(_boxInfo).loader;
         }
         if(_loc1_ && _loc1_.parent != backgroundContainer)
         {
            backgroundContainer.addChild(_loc1_);
            backgroundContainer.cacheAsBitmap = true;
            this.imageMask = background.clone();
            addChild(this.imageMask);
            this.imageMask.cacheAsBitmap = true;
            backgroundContainer.mask = this.imageMask;
         }
         if(this.imageMask)
         {
            this.imageMask.width = background.width - 6;
            this.imageMask.height = background.height - 6;
            this.imageMask.x = background.x + 3;
            this.imageMask.y = background.y + 3;
            this.imageMask.cacheAsBitmap = true;
         }
      }
      
      override public function dispose() : void
      {
         this.imageMask.dispose();
         super.dispose();
      }
      
      override public function resize(param1:int, param2:int = -1) : void
      {
         background.width = param1;
         if(backgroundTitle)
         {
            backgroundTitle.width = param1;
            backgroundTitle.y = 2;
         }
         backgroundButton.width = 158;
         if(param2 == -1)
         {
            background.height = 184;
         }
         else
         {
            background.height = param2;
         }
         titleLabel.x = Math.round((param1 - titleLabel.textWidth) / 2);
         titleLabel.y = 6;
         backgroundButton.y = background.height - 51;
         backgroundButton.x = Math.round((param1 - backgroundButton.width) / 2);
         _buyButton.y = backgroundButton.y + 4;
         _buyButton.x = backgroundButton.x + backgroundButton.width - _buyButton.width - 6;
         if(_infoButton)
         {
            _infoButton.x = background.width - _infoButton.width - 3;
            _infoButton.y = 2;
         }
         _spinner.x = backgroundButton.x + 34;
         _spinner.y = background.height - 53;
         this.resizeBackgroundImage();
         updateSaleLabel();
         updateClickMask(param1);
         updateStartTimeString(param1);
         updateTimeEndString(param1);
      }
   }
}
