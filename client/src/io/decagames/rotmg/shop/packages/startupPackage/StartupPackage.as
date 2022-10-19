package io.decagames.rotmg.shop.packages.startupPackage
{
   import io.decagames.rotmg.shop.packages.PackageBoxTile;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.popups.BasePopup;
   import io.decagames.rotmg.ui.popups.header.PopupHeader;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import kabam.rotmg.packages.model.PackageInfo;
   
   public class StartupPackage extends BasePopup
   {
       
      
      public var closeButton:SliceScalingButton;
      
      public var infoButton:SliceScalingButton;
      
      private var header:PopupHeader;
      
      private var _info:PackageInfo;
      
      public function StartupPackage(param1:PackageInfo)
      {
         super(550,385);
         this._info = param1;
         showOnFullScreen = true;
         var _loc2_:PackageBoxTile = new PackageBoxTile(param1,true);
         addChild(_loc2_);
         _loc2_.resize(550,385);
         this.header = new PopupHeader(550,PopupHeader.TYPE_MODAL);
         this.header.setTitle(param1.title,popupWidth - 18,DefaultLabelFormat.defaultModalTitle);
         this.closeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
         this.infoButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","info_button"));
         this.header.addButton(this.closeButton,"right_button");
         this.header.addButton(this.infoButton,"left_button");
         this.header.y = -this.header.height / 2 + 4;
         addChild(this.header);
      }
      
      public function dispose() : void
      {
         this.closeButton.dispose();
         this.infoButton.dispose();
         this.header.dispose();
      }
      
      public function get info() : PackageInfo
      {
         return this._info;
      }
   }
}
