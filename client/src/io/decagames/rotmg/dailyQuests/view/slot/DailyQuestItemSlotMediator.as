package io.decagames.rotmg.dailyQuests.view.slot
{
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import com.company.assembleegameclient.objects.Player;
   import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
   import flash.events.MouseEvent;
   import io.decagames.rotmg.dailyQuests.model.DailyQuestsModel;
   import io.decagames.rotmg.dailyQuests.signal.SelectedItemSlotsSignal;
   import io.decagames.rotmg.dailyQuests.signal.UnselectAllSlotsSignal;
   import kabam.rotmg.core.signals.ShowTooltipSignal;
   import kabam.rotmg.ui.model.HUDModel;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestItemSlotMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestItemSlot;
      
      [Inject]
      public var hud:HUDModel;
      
      [Inject]
      public var showTooltipSignal:ShowTooltipSignal;
      
      [Inject]
      public var selectedItemSlotsSignal:SelectedItemSlotsSignal;
      
      [Inject]
      public var unselectAllSignal:UnselectAllSlotsSignal;
      
      [Inject]
      public var model:DailyQuestsModel;
      
      private var tooltip:EquipmentToolTip;
      
      public function DailyQuestItemSlotMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         var _loc2_:Player = this.hud.gameSprite && this.hud.gameSprite.map?this.hud.gameSprite.map.player_:null;
         var _loc1_:int = ObjectLibrary.idToType_[this.view.itemID];
         this.tooltip = new EquipmentToolTip(this.view.itemID,this.view.type == "requirement"?null:_loc2_,_loc1_,"CURRENT_PLAYER");
         this.view.addEventListener("rollOver",this.onRollOverHandler);
         if(this.view.isSlotsSelectable)
         {
            this.unselectAllSignal.add(this.unselectHandler);
            this.view.addEventListener("click",this.onSlotSelected);
         }
      }
      
      private function unselectHandler(param1:int) : void
      {
         if(this.view.itemID != param1)
         {
            this.view.selected = false;
         }
      }
      
      override public function destroy() : void
      {
         this.view.removeEventListener("rollOver",this.onRollOverHandler);
         if(this.view.isSlotsSelectable)
         {
            this.unselectAllSignal.remove(this.unselectHandler);
            this.view.removeEventListener("click",this.onSlotSelected);
         }
         this.view.dispose();
      }
      
      private function onSlotSelected(param1:MouseEvent) : void
      {
         this.view.selected = !this.view.selected;
         this.unselectAllSignal.dispatch(this.view.itemID);
         if(this.view.selected)
         {
            this.model.selectedItem = this.view.itemID;
         }
         else
         {
            this.model.selectedItem = -1;
         }
         this.selectedItemSlotsSignal.dispatch(this.model.selectedItem);
      }
      
      private function onRollOverHandler(param1:MouseEvent) : void
      {
         this.tooltip.attachToTarget(this.view);
         this.showTooltipSignal.dispatch(this.tooltip);
      }
   }
}
