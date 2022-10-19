package io.decagames.rotmg.shop.genericBox
{
   import com.company.assembleegameclient.objects.Player;
   import io.decagames.rotmg.shop.NotEnoughResources;
   import io.decagames.rotmg.shop.genericBox.data.GenericBoxInfo;
   import io.decagames.rotmg.ui.popups.signals.ShowPopupSignal;
   import kabam.rotmg.core.model.PlayerModel;
   import kabam.rotmg.game.model.GameModel;
   
   public class BoxUtils
   {
       
      
      public function BoxUtils()
      {
         super();
      }
      
      public static function moneyCheckPass(param1:GenericBoxInfo, param2:int, param3:GameModel, param4:PlayerModel, param5:ShowPopupSignal) : Boolean
      {
         var _loc6_:int = 0;
         var _loc7_:int = 0;
         if(param1.isOnSale() && param1.saleAmount > 0)
         {
            _loc6_ = param1.saleCurrency;
            _loc7_ = int(param1.saleAmount) * param2;
         }
         else
         {
            _loc6_ = param1.priceCurrency;
            _loc7_ = int(param1.priceAmount) * param2;
         }
         var _loc9_:Boolean = true;
         var _loc11_:int = 0;
         var _loc10_:int = 0;
         var _loc8_:Player = param3.player;
         if(_loc8_ != null)
         {
            _loc10_ = _loc8_.credits_;
            _loc11_ = _loc8_.fame_;
         }
         else if(param4 != null)
         {
            _loc10_ = param4.getCredits();
            _loc11_ = param4.getFame();
         }
         if(_loc6_ == 0 && _loc10_ < _loc7_)
         {
            param5.dispatch(new NotEnoughResources(300,0));
            _loc9_ = false;
         }
         else if(_loc6_ == 1 && _loc11_ < _loc7_)
         {
            param5.dispatch(new NotEnoughResources(300,1));
            _loc9_ = false;
         }
         return _loc9_;
      }
   }
}
