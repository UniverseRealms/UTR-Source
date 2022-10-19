package io.decagames.rotmg.shop.packages
{
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.popups.modal.TextModal;
   import io.decagames.rotmg.ui.popups.modal.buttons.ClosePopupButton;
   
   public class PurchaseCompleteModal extends TextModal
   {
       
      
      public function PurchaseCompleteModal(param1:String)
      {
         var _loc2_:Vector.<BaseButton> = new Vector.<BaseButton>();
         _loc2_.push(new ClosePopupButton("OK"));
         var _loc3_:String = "";
         var _loc4_:* = param1;
         switch(_loc4_)
         {
            case "PURCHASE_TYPE_SLOTS_ONLY":
               _loc3_ = "Your purchase has been validated!";
               break;
            case "PURCHASE_TYPE_CONTENTS_ONLY":
               _loc3_ = "Your items have been sent to the Gift Chest!";
               break;
            case "PURCHASE_TYPE_MIXED":
               _loc3_ = "Your purchase has been validated! You will find your items in the Gift Chest.";
         }
         super(300,"Package Purchased",_loc3_,_loc2_);
      }
   }
}
