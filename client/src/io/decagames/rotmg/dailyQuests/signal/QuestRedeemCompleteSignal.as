package io.decagames.rotmg.dailyQuests.signal
{
   import kabam.rotmg.messaging.impl.incoming.QuestRedeemResponse;
   import org.osflash.signals.Signal;
   
   public class QuestRedeemCompleteSignal extends Signal
   {
       
      
      public function QuestRedeemCompleteSignal()
      {
         super(QuestRedeemResponse);
      }
   }
}
