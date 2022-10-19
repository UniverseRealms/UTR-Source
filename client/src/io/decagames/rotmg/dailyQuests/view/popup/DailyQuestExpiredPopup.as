package io.decagames.rotmg.dailyQuests.view.popup
{
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.modal.ModalPopup;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class DailyQuestExpiredPopup extends ModalPopup
   {
       
      
      private const TITLE:String = "Event Quest Expired";
      
      private const TEXT:String = "Sorry, this Quest has expired.";
      
      private const WIDTH:int = 300;
      
      private const HEIGHT:int = 100;
      
      private var _okButton:SliceScalingButton;
      
      public function DailyQuestExpiredPopup()
      {
         super(300, 100, "Event Quest Expired");
         this.init();
      }
      
      private function init() : void
      {
         var _loc1_:UILabel = null;
         _loc1_ = new UILabel();
         _loc1_.width = 250;
         _loc1_.multiline = true;
         _loc1_.wordWrap = true;
         _loc1_.text = "Sorry, this Quest has expired.";
         DefaultLabelFormat.defaultSmallPopupTitle(_loc1_,"center");
         _loc1_.x = (300 - _loc1_.width) / 2;
         _loc1_.y = 10;
         addChild(_loc1_);
         this._okButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
         this._okButton.setLabel("OK",DefaultLabelFormat.questButtonCompleteLabel);
         this._okButton.width = 149;
         this._okButton.x = (300 - this._okButton.width) / 2;
         this._okButton.y = (100 - this._okButton.height) / 2 + 10;
         addChild(this._okButton);
      }
      
      public function get okButton() : SliceScalingButton
      {
         return this._okButton;
      }
   }
}
