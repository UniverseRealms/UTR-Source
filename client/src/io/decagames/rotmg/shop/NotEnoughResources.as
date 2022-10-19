package io.decagames.rotmg.shop
{
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.popups.modal.TextModal;
   import io.decagames.rotmg.ui.popups.modal.buttons.BuyGoldButton;
   import io.decagames.rotmg.ui.popups.modal.buttons.ClosePopupButton;
   
   public class NotEnoughResources extends TextModal
   {
       
      
      public function NotEnoughResources(param1:int, param2:int)
      {
         var _loc3_:String = param2 == 0?"gold":"fame";
         var _loc4_:String = param2 == 0?"You do not have enough Gold for this item. Would you like to buy Gold?":"You do not have enough Fame for this item. You gain Fame when your character dies after having accomplished great things.";
         var _loc5_:Vector.<BaseButton> = new Vector.<BaseButton>();
         _loc5_.push(new ClosePopupButton("Cancel"));
         if(param2 == 0)
         {
            _loc5_.push(new BuyGoldButton());
         }
         super(param1,"Not enough " + _loc3_,_loc4_,_loc5_);
      }
   }
}
