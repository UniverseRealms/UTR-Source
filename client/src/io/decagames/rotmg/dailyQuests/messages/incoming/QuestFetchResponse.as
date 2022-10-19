package io.decagames.rotmg.dailyQuests.messages.incoming
{
   import flash.utils.IDataInput;
   import io.decagames.rotmg.dailyQuests.messages.data.QuestData;
   import kabam.rotmg.messaging.impl.incoming.IncomingMessage;
   
   public class QuestFetchResponse extends IncomingMessage
   {
       
      
      public var quests:Vector.<QuestData>;
      
      public var nextRefreshPrice:int;
      
      public function QuestFetchResponse(param1:uint, param2:Function)
      {
         super(param1,param2);
         this.nextRefreshPrice = -1;
      }
      
      override public function parseFromInput(param1:IDataInput) : void
      {
         this.quests = new Vector.<QuestData>();
         var _loc2_:int = param1.readShort();
         var _loc3_:int = 0;
         while(_loc3_ < _loc2_)
         {
            this.quests[_loc3_] = new QuestData();
            this.quests[_loc3_].parseFromInput(param1);
            _loc3_++;
         }
         this.nextRefreshPrice = param1.readShort();
      }
      
      override public function toString() : String
      {
         return formatToString("QUESTFETCHRESPONSE");
      }
   }
}
