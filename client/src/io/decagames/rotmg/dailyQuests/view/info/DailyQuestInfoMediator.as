package io.decagames.rotmg.dailyQuests.view.info
{
   import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.InventoryTile;
   import com.company.assembleegameclient.ui.tooltip.TextToolTip;
   import flash.events.MouseEvent;
   import io.decagames.rotmg.dailyQuests.model.DailyQuest;
   import io.decagames.rotmg.dailyQuests.model.DailyQuestsModel;
   import io.decagames.rotmg.dailyQuests.signal.LockQuestScreenSignal;
   import io.decagames.rotmg.dailyQuests.signal.SelectedItemSlotsSignal;
   import io.decagames.rotmg.dailyQuests.signal.ShowQuestInfoSignal;
   import io.decagames.rotmg.dailyQuests.view.popup.DailyQuestExpiredPopup;
   import io.decagames.rotmg.ui.popups.signals.ShowPopupSignal;
   import kabam.rotmg.core.signals.HideTooltipsSignal;
   import kabam.rotmg.core.signals.ShowTooltipSignal;
   import kabam.rotmg.dailyLogin.model.DailyLoginModel;
   import kabam.rotmg.game.view.components.BackpackTabContent;
   import kabam.rotmg.game.view.components.InventoryTabContent;
   import kabam.rotmg.messaging.impl.data.SlotObjectData;
   import kabam.rotmg.tooltips.HoverTooltipDelegate;
   import kabam.rotmg.ui.model.HUDModel;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestInfoMediator extends Mediator
   {
       
      
      [Inject]
      public var showInfoSignal:ShowQuestInfoSignal;
      
      [Inject]
      public var view:DailyQuestInfo;
      
      [Inject]
      public var model:DailyQuestsModel;
      
      [Inject]
      public var hud:HUDModel;
      
      [Inject]
      public var lockScreen:LockQuestScreenSignal;
      
      [Inject]
      public var selectedItemSlotsSignal:SelectedItemSlotsSignal;
      
      [Inject]
      public var showTooltipSignal:ShowTooltipSignal;
      
      [Inject]
      public var hideTooltipsSignal:HideTooltipsSignal;
      
      [Inject]
      public var dailyLoginModel:DailyLoginModel;
      
      [Inject]
      public var showPopupSignal:ShowPopupSignal;
      
      private var tooltip:TextToolTip;
      
      private var hoverTooltipDelegate:HoverTooltipDelegate;
      
      public function DailyQuestInfoMediator()
      {
         this.hoverTooltipDelegate = new HoverTooltipDelegate();
         super();
      }
      
      override public function initialize() : void
      {
         this.showInfoSignal.add(this.showQuestInfo);
         this.tooltip = new TextToolTip(3552822,10197915,"","You must select a reward first!",190,null);
         this.hoverTooltipDelegate.setHideToolTipsSignal(this.hideTooltipsSignal);
         this.hoverTooltipDelegate.setShowToolTipSignal(this.showTooltipSignal);
         this.hoverTooltipDelegate.tooltip = this.tooltip;
         this.view.completeButton.addEventListener("click",this.onCompleteButtonClickHandler);
         this.selectedItemSlotsSignal.add(this.itemSelectedHandler);
      }
      
      private function itemSelectedHandler(param1:int) : void
      {
         this.view.completeButton.disabled = !!this.model.currentQuest.completed?Boolean(true):Boolean(Boolean(this.model.selectedItem == -1?true:!DailyQuestInfo.hasAllItems(this.model.currentQuest.requirements,this.model.playerItemsFromInventory)));
         if(this.model.selectedItem == -1)
         {
            this.hoverTooltipDelegate.setDisplayObject(this.view.completeButton);
         }
         else
         {
            this.hoverTooltipDelegate.removeDisplayObject();
         }
      }
      
      override public function destroy() : void
      {
         this.view.completeButton.removeEventListener("click",this.onCompleteButtonClickHandler);
         this.showInfoSignal.remove(this.showQuestInfo);
         this.selectedItemSlotsSignal.remove(this.itemSelectedHandler);
      }
      
      private function showQuestInfo(param1:String, param2:int, param3:String) : void
      {
         this.view.eventQuestsCompleted();
      }
      
      private function setupQuestInfo(param1:String) : void
      {
         this.model.selectedItem = -1;
         this.view.dailyQuestsCompleted();
         this.model.currentQuest = this.model.getQuestById(param1);
         this.view.show(this.model.currentQuest,this.model.playerItemsFromInventory);
         if(!this.view.completeButton.completed && this.model.currentQuest.itemOfChoice)
         {
            this.view.completeButton.disabled = true;
            this.hoverTooltipDelegate.setDisplayObject(this.view.completeButton);
         }
      }
      
      private function tileToSlot(param1:InventoryTile) : SlotObjectData
      {
         var _loc2_:SlotObjectData = new SlotObjectData();
         _loc2_.objectId_ = param1.ownerGrid.owner.objectId_;
         _loc2_.objectType_ = param1.getItemId();
         _loc2_.slotId_ = param1.tileId;
         return _loc2_;
      }
      
      private function onCompleteButtonClickHandler(param1:MouseEvent) : void
      {
         if(this.checkIfQuestHasExpired())
         {
            this.showPopupSignal.dispatch(new DailyQuestExpiredPopup());
         }
         else
         {
            this.completeQuest();
         }
      }
      
      private function completeQuest() : void
      {
         var _loc11_:int = 0;
         var _loc10_:* = undefined;
         var _loc9_:int = 0;
         var _loc8_:* = undefined;
         var _loc3_:Vector.<SlotObjectData> = null;
         var _loc1_:BackpackTabContent = null;
         var _loc2_:InventoryTabContent = null;
         var _loc6_:Vector.<int> = null;
         var _loc7_:Vector.<InventoryTile> = null;
         var _loc4_:int = 0;
         var _loc5_:InventoryTile = null;
         if(!this.view.completeButton.disabled && !this.view.completeButton.completed)
         {
            _loc11_ = 0;
            _loc10_ = _loc6_;
            for each(_loc4_ in _loc6_)
            {
               _loc9_ = 0;
               _loc8_ = _loc7_;
               for each(_loc5_ in _loc7_)
               {
                  if(_loc5_.getItemId() == _loc4_)
                  {
                     _loc7_.splice(_loc7_.indexOf(_loc5_),1);
                     _loc3_.push(this.tileToSlot(_loc5_));
                     break;
                  }
               }
            }
            this.lockScreen.dispatch();
            if(!this.model.currentQuest.repeatable)
            {
               this.model.currentQuest.completed = true;
            }
            this.view.completeButton.completed = true;
            this.view.completeButton.disabled = true;
         }
      }
      
      private function checkIfQuestHasExpired() : Boolean
      {
         var _loc2_:* = false;
         var _loc3_:DailyQuest = this.model.currentQuest;
         var _loc1_:Date = new Date();
         return _loc2_;
      }
   }
}
