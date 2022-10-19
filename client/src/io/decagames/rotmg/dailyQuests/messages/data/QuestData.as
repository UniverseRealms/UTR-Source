package io.decagames.rotmg.dailyQuests.messages.data
{
   import flash.utils.IDataInput;
   
   public class QuestData
   {
       
      
      public var id:String;
      
      public var name:String;
      
      public var description:String;
      
      public var expiration:String;
      
      public var requirements:Vector.<int>;
      
      public var rewards:Vector.<int>;
      
      public var completed:Boolean;
      
      public var itemOfChoice:Boolean;
      
      public var category:int;
      
      public var repeatable:Boolean;
      
      public var weight:int;
      
      public function QuestData()
      {
         this.requirements = new Vector.<int>();
         this.rewards = new Vector.<int>();
         super();
      }
      
      public function parseFromInput(param1:IDataInput) : void
      {
         this.id = param1.readUTF();
         this.name = param1.readUTF();
         this.description = param1.readUTF();
         this.expiration = param1.readUTF();
         this.weight = param1.readInt();
         this.category = param1.readInt();
         var _loc2_:int = param1.readShort();
         var _loc3_:int = 0;
         while(_loc3_ < _loc2_)
         {
            this.requirements.push(param1.readInt());
            _loc3_++;
         }
         _loc2_ = param1.readShort();
         _loc3_ = 0;
         while(_loc3_ < _loc2_)
         {
            this.rewards.push(param1.readInt());
            _loc3_++;
         }
         this.completed = param1.readBoolean();
         this.itemOfChoice = param1.readBoolean();
         this.repeatable = param1.readBoolean();
      }
   }
}
