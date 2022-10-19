package io.decagames.rotmg.shop
{
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.popups.modal.TextModal;
   
   public class PreparingPurchaseTransactionModal extends TextModal
   {
       
      
      public function PreparingPurchaseTransactionModal()
      {
         super(300,"Payment","Preparing transaction...",new Vector.<BaseButton>());
      }
   }
}
