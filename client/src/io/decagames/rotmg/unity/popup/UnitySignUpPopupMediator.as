package io.decagames.rotmg.unity.popup
{
   import com.company.util.EmailValidator;
   import flash.events.MouseEvent;
   import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
   import io.decagames.rotmg.ui.popups.signals.ShowPopupSignal;
   import kabam.rotmg.account.core.Account;
   import kabam.rotmg.appengine.api.AppEngineClient;
   import robotlegs.bender.bundles.mvcs.Mediator;
   import robotlegs.bender.framework.api.ILogger;
   
   public class UnitySignUpPopupMediator extends Mediator
   {
       
      
      [Inject]
      public var view:UnitySignUpPopup;
      
      [Inject]
      public var showPopupSignal:ShowPopupSignal;
      
      [Inject]
      public var closePopupSignal:ClosePopupSignal;
      
      [Inject]
      public var client:AppEngineClient;
      
      [Inject]
      public var account:Account;
      
      [Inject]
      public var logger:ILogger;
      
      private var steamSignupConfirmation:UnitySignupConfirmation;
      
      public function UnitySignUpPopupMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.view.submitButton.addEventListener(MouseEvent.CLICK,this.onOK);
         this.view.cancelButton.addEventListener(MouseEvent.CLICK,this.onCancel);
         this.view.emailInputField.setSelection(0,this.view.emailInputField.text.length);
         this.view.stage.focus = this.view.emailInputField;
      }
      
      override public function destroy() : void
      {
         this.view.submitButton.removeEventListener(MouseEvent.CLICK,this.onOK);
         this.view.cancelButton.removeEventListener(MouseEvent.CLICK,this.onCancel);
      }
      
      private function onOK(param1:MouseEvent) : void
      {
         this.view.submitButton.removeEventListener(MouseEvent.CLICK,this.onOK);
         this.view.errorText.text = "";
         var _loc2_:Boolean = this.view.checkBox.isChecked();
         var _loc3_:String = this.view.emailInputField.text;
         if(!this.checkEmail(_loc3_))
         {
            this.view.emailInputField.setSelection(0,this.view.emailInputField.text.length);
            this.view.errorText.text = "Invalid email address - please check.";
            this.view.submitButton.addEventListener(MouseEvent.CLICK,this.onOK);
            this.view.stage.focus = this.view.emailInputField;
         }
         else if(!_loc2_)
         {
            this.view.errorText.text = "Please check the checkbox allowing us to contact you.";
            this.view.submitButton.addEventListener(MouseEvent.CLICK,this.onOK);
         }
         else
         {
            this.submit();
         }
      }
      
      private function submit() : void
      {
         this.logger.info("SteamUnityTask start");
         var _loc1_:Object = this.account.getCredentials();
         _loc1_.email = this.view.emailInputField.text;
         _loc1_.notifyMe = !!this.view.checkBox.isChecked()?"1":"0";
         this.client.complete.addOnce(this.onComplete);
         this.client.sendRequest("/account/signupDecaEmail",_loc1_);
      }
      
      private function onComplete(param1:Boolean, param2:*) : void
      {
         if(param1)
         {
            this.closePopUp();
            this.showSignUpResult("Thank you for signing up!");
         }
         else
         {
            this.logger.error(param2);
            this.showSignUpResult(param2);
         }
      }
      
      private function checkEmail(param1:String) : Boolean
      {
         return EmailValidator.isValidEmail(param1);
      }
      
      private function onCancel(param1:MouseEvent) : void
      {
         this.view.cancelButton.removeEventListener(MouseEvent.CLICK,this.onCancel);
         this.closePopUp();
      }
      
      private function closePopUp() : void
      {
         this.closePopupSignal.dispatch(this.view);
      }
      
      private function showSignUpResult(param1:String) : void
      {
         this.steamSignupConfirmation = new UnitySignupConfirmation(param1);
         this.steamSignupConfirmation.okButton.addEventListener(MouseEvent.CLICK,this.onSignUpResultClose);
         this.showPopupSignal.dispatch(this.steamSignupConfirmation);
      }
      
      private function onSignUpResultClose(param1:MouseEvent) : void
      {
         this.steamSignupConfirmation.okButton.removeEventListener(MouseEvent.CLICK,this.onSignUpResultClose);
         this.closePopupSignal.dispatch(this.steamSignupConfirmation);
      }
   }
}
