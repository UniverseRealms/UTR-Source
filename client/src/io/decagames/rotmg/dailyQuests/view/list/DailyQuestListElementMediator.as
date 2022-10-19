package io.decagames.rotmg.dailyQuests.view.list
{
   import flash.events.MouseEvent;
   import io.decagames.rotmg.dailyQuests.signal.ShowQuestInfoSignal;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestListElementMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestListElement;
      
      [Inject]
      public var showInfoSignal:ShowQuestInfoSignal;
      
      public function DailyQuestListElementMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.showInfoSignal.add(this.resetElement);
         this.view.addEventListener("click",this.onClickHandler);
      }
      
      private function resetElement(param1:String, param2:int, param3:String) : void
      {
         if(param1 == "" || param2 == -1)
         {
            return;
         }
         if(param1 != this.view.id)
         {
            if(param2 != 3 && this.view.category != 3)
            {
               this.view.isSelected = false;
            }
            else if(param2 == this.view.category)
            {
               this.view.isSelected = false;
            }
         }
      }
      
      override public function destroy() : void
      {
         this.view.removeEventListener("click",this.onClickHandler);
      }
      
      private function onClickHandler(param1:MouseEvent) : void
      {
         this.view.isSelected = true;
         var _loc2_:String = this.view.category == 3?"Events":"Quests";
         this.showInfoSignal.dispatch(this.view.id,this.view.category,_loc2_);
      }
   }
}
