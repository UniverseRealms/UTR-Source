package io.decagames.rotmg.dailyQuests.view.popup
{
   import flash.events.MouseEvent;
   import io.decagames.rotmg.dailyQuests.signal.CloseExpirePopupSignal;
   import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestExpirePopupMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestExpiredPopup;
      
      [Inject]
      public var closeExpirePopupSignal:CloseExpirePopupSignal;
      
      [Inject]
      public var closePopupSignal:ClosePopupSignal;
      
      public function DailyQuestExpirePopupMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.view.okButton.addEventListener("click",this.onOK);
      }
      
      override public function destroy() : void
      {
         this.view.okButton.removeEventListener("click",this.onOK);
      }
      
      private function onOK(param1:MouseEvent) : void
      {
         this.view.okButton.removeEventListener("click",this.onOK);
         this.closeExpirePopupSignal.dispatch();
         this.closePopupSignal.dispatch(this.view);
      }
   }
}
