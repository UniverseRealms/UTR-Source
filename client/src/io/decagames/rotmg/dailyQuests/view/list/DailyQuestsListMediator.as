package io.decagames.rotmg.dailyQuests.view.list
{
   import io.decagames.rotmg.dailyQuests.model.DailyQuest;
   import io.decagames.rotmg.dailyQuests.model.DailyQuestsModel;
   import io.decagames.rotmg.dailyQuests.signal.ShowQuestInfoSignal;
   import io.decagames.rotmg.dailyQuests.view.info.DailyQuestInfo;
   import kabam.rotmg.ui.model.HUDModel;
   import kabam.rotmg.ui.signals.UpdateQuestSignal;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestsListMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestsList;
      
      [Inject]
      public var model:DailyQuestsModel;
      
      [Inject]
      public var hud:HUDModel;
      
      [Inject]
      public var updateQuestSignal:UpdateQuestSignal;
      
      [Inject]
      public var showInfoSignal:ShowQuestInfoSignal;
      
      private var hasEvent:Boolean;
      
      public function DailyQuestsListMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.onQuestsUpdate("UpdateQuestSignal.QuestListLoaded");
         this.updateQuestSignal.add(this.onQuestsUpdate);
         this.view.tabs.tabSelectedSignal.add(this.onTabSelected);
      }
      
      private function onTabSelected(param1:String) : void
      {
         var _loc2_:DailyQuestListElement = this.view.getCurrentlySelected(param1);
         if(_loc2_)
         {
            this.showInfoSignal.dispatch(_loc2_.id,_loc2_.category,param1);
         }
         else
         {
            this.showInfoSignal.dispatch("",-1,param1);
         }
      }
      
      private function onQuestsUpdate(param1:String) : void
      {
         this.view.clearQuestLists();
         var _loc2_:Vector.<int> = !!this.hud.gameSprite.map.player_?this.hud.gameSprite.map.player_.equipment_.slice(4 - 1,4 + 8 * 2):new Vector.<int>();
         this.view.tabs.buttonsRenderedSignal.addOnce(this.onAddedHandler);
         this.addDailyQuests(_loc2_);
         this.addEventQuests(_loc2_);
      }
      
      private function addEventQuests(param1:Vector.<int>) : void
      {
         var _loc5_:DailyQuest = null;
         var _loc6_:* = false;
         var _loc4_:DailyQuestListElement = null;
         var _loc2_:Boolean = true;
         var _loc3_:Date = new Date();
         var _loc8_:int = 0;
         var _loc7_:* = this.model.eventQuestsList;
         for each(_loc5_ in this.model.eventQuestsList)
         {
            _loc6_ = false;
            if(!(_loc5_.completed || _loc6_))
            {
               _loc4_ = new DailyQuestListElement(_loc5_.id,_loc5_.name,_loc5_.completed,DailyQuestInfo.hasAllItems(_loc5_.requirements,param1),_loc5_.category);
               if(_loc2_)
               {
                  _loc4_.isSelected = true;
               }
               _loc2_ = false;
               this.view.addEventToList(_loc4_);
               this.hasEvent = true;
            }
         }
      }
      
      private function addDailyQuests(param1:Vector.<int>) : void
      {
         var _loc3_:DailyQuest = null;
         var _loc4_:DailyQuestListElement = null;
         var _loc2_:Boolean = true;
         var _loc6_:int = 0;
         var _loc5_:* = this.model.dailyQuestsList;
         for each(_loc3_ in this.model.dailyQuestsList)
         {
            if(!_loc3_.completed)
            {
               _loc4_ = new DailyQuestListElement(_loc3_.id,_loc3_.name,_loc3_.completed,DailyQuestInfo.hasAllItems(_loc3_.requirements,param1),_loc3_.category);
               if(_loc2_)
               {
                  _loc4_.isSelected = true;
               }
               _loc2_ = false;
               this.view.addQuestToList(_loc4_);
            }
         }
         this.onTabSelected("Quests");
      }
      
      private function onAddedHandler() : void
      {
         if(this.hasEvent)
         {
            this.view.addIndicator(this.hasEvent);
         }
      }
      
      override public function destroy() : void
      {
         this.view.tabs.buttonsRenderedSignal.remove(this.onAddedHandler);
      }
   }
}
