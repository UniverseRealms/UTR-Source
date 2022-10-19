package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import com.company.assembleegameclient.objects.Player;
   import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
   import flash.events.MouseEvent;
   import kabam.rotmg.core.signals.ShowTooltipSignal;
   import kabam.rotmg.ui.model.HUDModel;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class ItemBoxMediator extends Mediator
   {
       
      
      [Inject]
      public var view:ItemBox;
      
      [Inject]
      public var hud:HUDModel;
      
      [Inject]
      public var showTooltipSignal:ShowTooltipSignal;
      
      private var tooltip:EquipmentToolTip;
      
      public function ItemBoxMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         var _loc2_:Player = this.hud.gameSprite && this.hud.gameSprite.map?this.hud.gameSprite.map.player_:null;
         var _loc1_:int = ObjectLibrary.idToType_[int(this.view.itemId)];
         this.tooltip = new EquipmentToolTip(int(this.view.itemId),_loc2_,_loc1_,"CURRENT_PLAYER");
         this.view.itemBackground.addEventListener("rollOver",this.onRollOverHandler);
      }
      
      private function onRollOverHandler(param1:MouseEvent) : void
      {
         this.tooltip.attachToTarget(this.view);
         this.showTooltipSignal.dispatch(this.tooltip);
      }
      
      override public function destroy() : void
      {
         this.view.itemBackground.removeEventListener("rollOver",this.onRollOverHandler);
         this.tooltip.detachFromTarget();
      }
   }
}
