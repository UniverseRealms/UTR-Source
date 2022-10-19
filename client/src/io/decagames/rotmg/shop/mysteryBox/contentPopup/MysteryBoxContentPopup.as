package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.modal.ModalPopup;
   import kabam.rotmg.mysterybox.model.MysteryBoxInfo;
   
   public class MysteryBoxContentPopup extends ModalPopup
   {
       
      
      private var _info:MysteryBoxInfo;
      
      public function MysteryBoxContentPopup(param1:MysteryBoxInfo)
      {
         this._info = param1;
         super(280, 0, param1.title, DefaultLabelFormat.defaultSmallPopupTitle);
         var _loc2_:UILabel = new UILabel();
         DefaultLabelFormat.mysteryBoxContentInfo(_loc2_);
         _loc2_.multiline = true;
         switch(int(param1.rolls) - 1)
         {
            case 0:
               _loc2_.text = "You will win one\nof the rewards listed below!";
               break;
            case 1:
               _loc2_.text = "You will win two\nof the rewards listed below!";
               break;
            case 2:
               _loc2_.text = "You will win three\nof the rewards listed below!";
         }
         _loc2_.x = (280 - _loc2_.textWidth) / 2;
         addChild(_loc2_);
      }
      
      public function get info() : MysteryBoxInfo
      {
         return this._info;
      }
   }
}
