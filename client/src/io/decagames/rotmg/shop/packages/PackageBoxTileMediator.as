package io.decagames.rotmg.shop.packages
{
   import com.company.assembleegameclient.objects.Player;
   import flash.events.MouseEvent;
   import io.decagames.rotmg.seasonalEvent.data.SeasonalEventModel;
   import io.decagames.rotmg.shop.PurchaseInProgressModal;
   import io.decagames.rotmg.shop.genericBox.BoxUtils;
   import io.decagames.rotmg.shop.packages.contentPopup.PackageBoxContentPopup;
   import io.decagames.rotmg.supportCampaign.data.SupporterCampaignModel;
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.popups.modal.error.ErrorModal;
   import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
   import io.decagames.rotmg.ui.popups.signals.ShowPopupSignal;
   import kabam.rotmg.account.core.Account;
   import kabam.rotmg.appengine.api.AppEngineClient;
   import kabam.rotmg.core.model.PlayerModel;
   import kabam.rotmg.dialogs.control.OpenDialogSignal;
   import kabam.rotmg.game.model.GameModel;
   import kabam.rotmg.packages.model.PackageInfo;
   import kabam.rotmg.text.view.stringBuilder.LineBuilder;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class PackageBoxTileMediator extends Mediator
   {
       
      
      [Inject]
      public var view:PackageBoxTile;
      
      [Inject]
      public var gameModel:GameModel;
      
      [Inject]
      public var playerModel:PlayerModel;
      
      [Inject]
      public var showPopupSignal:ShowPopupSignal;
      
      [Inject]
      public var openDialogSignal:OpenDialogSignal;
      
      [Inject]
      public var client:AppEngineClient;
      
      [Inject]
      public var account:Account;
      
      [Inject]
      public var closePopupSignal:ClosePopupSignal;
      
      [Inject]
      public var supportCampaignModel:SupporterCampaignModel;
      
      [Inject]
      public var seasonalEventModel:SeasonalEventModel;
      
      private var inProgressModal:PurchaseInProgressModal;
      
      public function PackageBoxTileMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.view.spinner.valueWasChanged.add(this.changeAmountHandler);
         this.view.buyButton.clickSignal.add(this.onBuyHandler);
         if(this.view.infoButton)
         {
            this.view.infoButton.clickSignal.add(this.onInfoClick);
         }
         if(this.view.clickMask)
         {
            this.view.clickMask.addEventListener("click",this.onBoxClickHandler);
         }
      }
      
      private function onBoxClickHandler(param1:MouseEvent) : void
      {
         this.onInfoClick(null);
      }
      
      private function changeAmountHandler(param1:int) : void
      {
         if(this.view.boxInfo.isOnSale())
         {
            this.view.buyButton.price = param1 * int(this.view.boxInfo.saleAmount);
         }
         else
         {
            this.view.buyButton.price = param1 * int(this.view.boxInfo.priceAmount);
         }
      }
      
      private function onBuyHandler(param1:BaseButton) : void
      {
         var _loc2_:Boolean = BoxUtils.moneyCheckPass(this.view.boxInfo,this.view.spinner.value,this.gameModel,this.playerModel,this.showPopupSignal);
         if(_loc2_)
         {
            this.inProgressModal = new PurchaseInProgressModal();
            this.showPopupSignal.dispatch(this.inProgressModal);
            this.sendPurchaseRequest();
         }
      }
      
      private function sendPurchaseRequest() : void
      {
         var _loc1_:Object = this.account.getCredentials();
         _loc1_.isChallenger = this.seasonalEventModel.isChallenger;
         _loc1_.boxId = this.view.boxInfo.id;
         if(this.view.boxInfo.isOnSale())
         {
            _loc1_.quantity = this.view.spinner.value;
            _loc1_.price = this.view.boxInfo.saleAmount;
            _loc1_.currency = this.view.boxInfo.saleCurrency;
         }
         else
         {
            _loc1_.quantity = this.view.spinner.value;
            _loc1_.price = this.view.boxInfo.priceAmount;
            _loc1_.currency = this.view.boxInfo.priceCurrency;
         }
         this.client.sendRequest("/account/purchasePackage",_loc1_);
         this.client.complete.addOnce(this.onRollRequestComplete);
      }
      
      private function onRollRequestComplete(param1:Boolean, param2:*) : void
      {
         var _loc3_:XML = null;
         var _loc6_:Player = null;
         var _loc7_:String = null;
         var _loc4_:Array = null;
         var _loc5_:int = 0;
         var _loc8_:Array = null;
         var _loc10_:int = 0;
         var _loc9_:Array = null;
         if(param1)
         {
            _loc3_ = new XML(param2);
            if(_loc3_.hasOwnProperty("CampaignProgress"))
            {
               this.supportCampaignModel.parseUpdateData(_loc3_.CampaignProgress);
            }
            if(_loc3_.hasOwnProperty("Left") && this.view.boxInfo.unitsLeft != -1)
            {
               this.view.boxInfo.unitsLeft = int(_loc3_.Left);
            }
            if(_loc3_.hasOwnProperty("PurchaseLeft") && this.view.boxInfo.purchaseLeft != -1)
            {
               this.view.boxInfo.purchaseLeft = int(_loc3_.PurchaseLeft);
            }
            _loc6_ = this.gameModel.player;
            if(_loc6_ != null)
            {
               if(_loc3_.hasOwnProperty("Gold"))
               {
                  _loc6_.setCredits(int(_loc3_.Gold));
               }
               else if(_loc3_.hasOwnProperty("Fame"))
               {
                  _loc6_.setFame(int(_loc3_.Fame));
               }
            }
            else if(this.playerModel != null)
            {
               if(_loc3_.hasOwnProperty("Gold"))
               {
                  this.playerModel.setCredits(int(_loc3_.Gold));
               }
               else if(_loc3_.hasOwnProperty("Fame"))
               {
                  this.playerModel.setFame(int(_loc3_.Fame));
               }
            }
            this.closePopupSignal.dispatch(this.inProgressModal);
            this.showPopupSignal.dispatch(new PurchaseCompleteModal(PackageInfo(this.view.boxInfo).purchaseType));
         }
         else
         {
            _loc7_ = "MysteryBoxRollModal.pleaseTryAgainString";
            if(LineBuilder.getLocalizedStringFromKey(param2) != "")
            {
               _loc7_ = param2;
            }
            if(param2.indexOf("MysteryBoxError.soldOut") >= 0)
            {
               _loc4_ = param2.split("|");
               if(_loc4_.length == 2)
               {
                  _loc5_ = _loc4_[1];
                  this.view.boxInfo.unitsLeft = _loc5_;
                  if(_loc5_ == 0)
                  {
                     _loc7_ = "MysteryBoxError.soldOutAll";
                  }
                  else
                  {
                     _loc7_ = LineBuilder.getLocalizedStringFromKey("MysteryBoxError.soldOutLeft",{
                        "left":this.view.boxInfo.unitsLeft,
                        "box":(this.view.boxInfo.unitsLeft == 1?LineBuilder.getLocalizedStringFromKey("MysteryBoxError.box"):LineBuilder.getLocalizedStringFromKey("MysteryBoxError.boxes"))
                     });
                  }
               }
            }
            if(param2.indexOf("MysteryBoxError.maxPurchase") >= 0)
            {
               _loc8_ = param2.split("|");
               if(_loc8_.length == 2)
               {
                  _loc10_ = _loc8_[1];
                  if(_loc10_ == 0)
                  {
                     _loc7_ = "MysteryBoxError.maxPurchase";
                  }
                  else
                  {
                     _loc7_ = LineBuilder.getLocalizedStringFromKey("MysteryBoxError.maxPurchaseLeft",{"left":_loc10_});
                  }
               }
            }
            if(param2.indexOf("blockedForUser") >= 0)
            {
               _loc9_ = param2.split("|");
               if(_loc9_.length == 2)
               {
                  _loc7_ = LineBuilder.getLocalizedStringFromKey("MysteryBoxError.blockedForUser",{"date":_loc9_[1]});
               }
            }
            this.showErrorMessage(_loc7_);
         }
      }
      
      private function showErrorMessage(param1:String) : void
      {
         this.closePopupSignal.dispatch(this.inProgressModal);
         this.showPopupSignal.dispatch(new ErrorModal(300,LineBuilder.getLocalizedStringFromKey("MysteryBoxRollModal.purchaseFailedString",{}),LineBuilder.getLocalizedStringFromKey(param1,{}).replace("box","package")));
      }
      
      private function onInfoClick(param1:BaseButton) : void
      {
         this.showPopupSignal.dispatch(new PackageBoxContentPopup(PackageInfo(this.view.boxInfo)));
      }
      
      override public function destroy() : void
      {
         this.view.spinner.valueWasChanged.remove(this.changeAmountHandler);
         this.view.buyButton.clickSignal.remove(this.onBuyHandler);
         if(this.view.infoButton)
         {
            this.view.infoButton.clickSignal.remove(this.onInfoClick);
         }
         if(this.view.clickMask)
         {
            this.view.clickMask.removeEventListener("click",this.onBoxClickHandler);
         }
      }
   }
}
