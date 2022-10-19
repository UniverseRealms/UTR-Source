package io.decagames.rotmg.unity.popup
{
   import flash.text.TextFieldAutoSize;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.modal.ModalPopup;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class UnitySignupConfirmation extends ModalPopup
   {
       
      
      private const WIDTH:int = 330;
      
      private const HEIGHT:int = 100;
      
      private var _okButton:SliceScalingButton;
      
      private var _infoText:UILabel;
      
      private var _message:String;
      
      public function UnitySignupConfirmation(param1:String)
      {
         super(this.WIDTH, this.HEIGHT, UnitySignUpConstants.TITLE, DefaultLabelFormat.defaultSmallPopupTitle);
         this._message = param1;
         this.init();
      }
      
      private function init() : void
      {
         this.createLabel();
         this.createButton();
      }
      
      private function createButton() : void
      {
         var _loc1_:SliceScalingBitmap = null;
         _loc1_ = new TextureParser().getSliceScalingBitmap("UI","main_button_decoration",186);
         addChild(_loc1_);
         _loc1_.x = Math.round((this.WIDTH - _loc1_.width) / 2);
         _loc1_.y = this._infoText.y + this._infoText.height + 14;
         this._okButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
         this._okButton.setLabel("OK",DefaultLabelFormat.questButtonCompleteLabel);
         this._okButton.width = 130;
         this._okButton.x = Math.round((this.WIDTH - 130) / 2);
         this._okButton.y = this._infoText.y + this._infoText.height + 20;
         addChild(this._okButton);
      }
      
      private function createLabel() : void
      {
         this._infoText = new UILabel();
         DefaultLabelFormat.createLabelFormat(this._infoText,14,16777215,TextFieldAutoSize.CENTER);
         this._infoText.width = this.WIDTH;
         this._infoText.multiline = true;
         this._infoText.wordWrap = true;
         this._infoText.text = this._message;
         this._infoText.y = 10;
         addChild(this._infoText);
      }
      
      public function get okButton() : SliceScalingButton
      {
         return this._okButton;
      }
      
      public function get message() : String
      {
         return this._message;
      }
   }
}
