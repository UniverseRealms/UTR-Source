package io.decagames.rotmg.dailyQuests.view.popup
{
   import flash.events.MouseEvent;
   import io.decagames.rotmg.dailyQuests.signal.CloseRedeemPopupSignal;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestRedeemPopupMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestRedeemPopup;
      
      [Inject]
      public var closeRedeem:CloseRedeemPopupSignal;
      
      public function DailyQuestRedeemPopupMediator()
      {
         super();
      }
      
      override public function destroy() : void
      {
         this.view.thanksButton.removeEventListener("click",this.onThanksClickedHandler);
      }
      
      override public function initialize() : void
      {
         this.view.thanksButton.addEventListener("click",this.onThanksClickedHandler);
      }
      
      private function onThanksClickedHandler(param1:MouseEvent) : void
      {
         this.view.thanksButton.removeEventListener("click",this.onThanksClickedHandler);
         this.closeRedeem.dispatch();
      }
   }
}
