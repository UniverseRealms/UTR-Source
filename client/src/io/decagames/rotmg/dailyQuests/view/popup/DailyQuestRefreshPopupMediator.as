package io.decagames.rotmg.dailyQuests.view.popup
{
   import io.decagames.rotmg.dailyQuests.signal.CloseRefreshPopupSignal;
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import kabam.rotmg.ui.model.HUDModel;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestRefreshPopupMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestRefreshPopup;
      
      [Inject]
      public var closeRefreshPopupSignal:CloseRefreshPopupSignal;
      
      [Inject]
      public var hudModel:HUDModel;
      
      private var closeButton:SliceScalingButton;
      
      public function DailyQuestRefreshPopupMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.closeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
         this.closeButton.clickSignal.addOnce(this.onClose);
         this.view.header.addButton(this.closeButton,"right_button");
         this.view.buyQuestRefreshButton.clickSignal.add(this.onBuyRefresh);
      }
      
      private function onBuyRefresh(param1:BaseButton) : void
      {
         this.closeRefreshPopupSignal.dispatch();
      }
      
      override public function destroy() : void
      {
         this.closeButton.clickSignal.remove(this.onClose);
      }
      
      private function onClose(param1:BaseButton) : void
      {
         this.closeRefreshPopupSignal.dispatch();
      }
   }
}
