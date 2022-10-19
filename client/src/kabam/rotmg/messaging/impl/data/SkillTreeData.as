package kabam.rotmg.messaging.impl.data
{
   import flash.utils.IDataInput;
   
   public class SkillTreeData
   {
      public var unlockedSkillsId_:int;

      public function SkillTreeData()
      {
         super();
      }
      
      public function parseFromInput(data:IDataInput) : void
      {
         this.unlockedSkillsId_ = data.readInt();

      }
   }
}
