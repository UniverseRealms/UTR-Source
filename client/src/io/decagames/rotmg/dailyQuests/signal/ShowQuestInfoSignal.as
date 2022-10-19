package io.decagames.rotmg.dailyQuests.signal
{
   import org.osflash.signals.Signal;
   
   public class ShowQuestInfoSignal extends Signal
   {
       
      
      public function ShowQuestInfoSignal()
      {
         super(String,int,String);
      }
   }
}
