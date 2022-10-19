package io.decagames.rotmg.shop.packages.contentPopup
{
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.modal.ModalPopup;
   import kabam.rotmg.packages.model.PackageInfo;
   
   public class PackageBoxContentPopup extends ModalPopup
   {
       
      
      private var _info:PackageInfo;
      
      private var _infoLabel:UILabel;
      
      public function PackageBoxContentPopup(param1:PackageInfo)
      {
         this._info = param1;
         super(280, 0, param1.title, DefaultLabelFormat.defaultSmallPopupTitle);
         this._infoLabel = new UILabel();
         DefaultLabelFormat.mysteryBoxContentInfo(this._infoLabel);
         this._infoLabel.multiline = true;
         this._infoLabel.wordWrap = true;
         this._infoLabel.width = 255;
         this._infoLabel.text = param1.description != ""?param1.description:"The package contains all the following items:";
         this._infoLabel.x = 10;
         addChild(this._infoLabel);
      }
      
      public function get info() : PackageInfo
      {
         return this._info;
      }
      
      public function get infoLabel() : UILabel
      {
         return this._infoLabel;
      }
   }
}
