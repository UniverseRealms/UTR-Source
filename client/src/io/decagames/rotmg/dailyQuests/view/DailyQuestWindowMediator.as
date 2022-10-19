package io.decagames.rotmg.dailyQuests.view
{
   import com.company.assembleegameclient.ui.tooltip.TextToolTip;
   import flash.events.Event;
   import io.decagames.rotmg.dailyQuests.model.DailyQuestsModel;
   import io.decagames.rotmg.dailyQuests.signal.CloseExpirePopupSignal;
   import io.decagames.rotmg.dailyQuests.signal.CloseRedeemPopupSignal;
   import io.decagames.rotmg.dailyQuests.signal.CloseRefreshPopupSignal;
   import io.decagames.rotmg.dailyQuests.signal.LockQuestScreenSignal;
   import io.decagames.rotmg.dailyQuests.signal.QuestRedeemCompleteSignal;
   import io.decagames.rotmg.dailyQuests.view.popup.DailyQuestRedeemPopup;
   import io.decagames.rotmg.dailyQuests.view.popup.DailyQuestRefreshPopup;
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.popups.modal.error.ErrorModal;
   import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
   import io.decagames.rotmg.ui.popups.signals.ShowPopupSignal;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import kabam.rotmg.core.signals.HideTooltipsSignal;
   import kabam.rotmg.core.signals.ShowTooltipSignal;
   import kabam.rotmg.dailyLogin.model.DailyLoginModel;
   import kabam.rotmg.messaging.impl.incoming.QuestRedeemResponse;
   import kabam.rotmg.tooltips.HoverTooltipDelegate;
   import kabam.rotmg.ui.model.HUDModel;
   import kabam.rotmg.ui.signals.UpdateQuestSignal;
   import kabam.rotmg.ui.view.NotEnoughGoldDialog;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class DailyQuestWindowMediator extends Mediator
   {
       
      
      [Inject]
      public var view:DailyQuestWindow;
      
      [Inject]
      public var lockScreen:LockQuestScreenSignal;
      
      [Inject]
      public var redeemCompleteSignal:QuestRedeemCompleteSignal;
      
      [Inject]
      public var dailyQuestsModel:DailyQuestsModel;
      
      [Inject]
      public var closeRedeem:CloseRedeemPopupSignal;
      
      [Inject]
      public var closeExpire:CloseExpirePopupSignal;
      
      [Inject]
      public var dailyLoginModel:DailyLoginModel;
      
      [Inject]
      public var showTooltipSignal:ShowTooltipSignal;
      
      [Inject]
      public var hideTooltipsSignal:HideTooltipsSignal;
      
      [Inject]
      public var closePopupSignal:ClosePopupSignal;
      
      [Inject]
      public var showPopupSignal:ShowPopupSignal;
      
      [Inject]
      public var closeRefreshPopupSignal:CloseRefreshPopupSignal;
      
      [Inject]
      public var hudModel:HUDModel;
      
      [Inject]
      public var updateQuestSignal:UpdateQuestSignal;
      
      private var toolTip:TextToolTip;
      
      private var hoverTooltipDelegate:HoverTooltipDelegate;
      
      private var closeButton:SliceScalingButton;
      
      private var infoButton:SliceScalingButton;
      
      private var contentBackground:SliceScalingBitmap;
      
      private var redeemPopup:DailyQuestRedeemPopup;
      
      private var dailyQuestRefreshPopup:DailyQuestRefreshPopup;
      
      private var refreshTooltipDelegate:HoverTooltipDelegate;
      
      public function DailyQuestWindowMediator()
      {
         this.hoverTooltipDelegate = new HoverTooltipDelegate();
         super();
      }
      
      override public function initialize() : void
      {
         this.closeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
         this.infoButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","info_button"));
         this.view.header.setTitle("The Tinkerer",450,DefaultLabelFormat.defaultPopupTitle);
         this.closeButton.clickSignal.addOnce(this.onClose);
         this.view.header.addButton(this.closeButton,"right_button");
         this.view.header.addButton(this.infoButton,"left_button");
         this.view.refreshButton.clickSignal.add(this.onRefreshClick);
         this.view.questList.tabs.tabSelectedSignal.add(this.onTabSelected);
         this.contentBackground = TextureParser.instance.getSliceScalingBitmap("UI","tab_cointainer_background_filled");
         this.contentBackground.width = 580;
         this.contentBackground.height = 445;
         this.view.contentContainer.addChildAt(this.contentBackground,0);
         this.lockScreen.add(this.onLockScreen);
         this.redeemCompleteSignal.add(this.onRedeemComplete);
         this.closeRedeem.add(this.onRedeemClose);
         this.closeExpire.add(this.onExpireClose);
         this.closeRefreshPopupSignal.add(this.onRefreshPopupClosed);
         this.updateQuestSignal.add(this.onQuestsUpdate);
         this.view.addEventListener("enterFrame",this.updateTimeHandler);
         this.setToolTipTitle("The Tinkerer","Complete the quests to earn great rewards!\n\nYou can select a quest from the list to display the quest requirements. Bring the items back to me to complete the quest and rewards will be sent directly to your Gift Chest.\n\nItems will be directly consumed from your inventory or backpack when you press \"Complete!\".\n\nYou can complete each quest only once per day, refresh to get new Quests up to 2 times per day in addition to the Tinkerer offering you new quests everyday!");
         this.setRefreshQuestsState();
      }
      
      private function setRefreshQuestsState() : void
      {
         this.destroyRefreshTooltip();
         if(this.dailyQuestsModel.dailyQuestsList.length == 0)
         {
            this.view.refreshButton.clickSignal.remove(this.onRefreshClick);
            this.setRefreshButtonState(true);
         }
         else if(this.dailyQuestsModel.nextRefreshPrice != -1)
         {
            this.view.refreshButton.clickSignal.remove(this.onRefreshClick);
            this.view.refreshButton.clickSignal.add(this.onRefreshClick);
            this.setRefreshButtonState(false);
         }
         else
         {
            this.view.refreshButton.clickSignal.remove(this.onRefreshClick);
            this.setRefreshButtonState(true);
         }
      }
      
      private function setRefreshButtonState(param1:Boolean) : void
      {
         this.view.refreshButton.disabled = param1;
      }
      
      private function onQuestsUpdate(param1:String) : void
      {
         this.setRefreshQuestsState();
      }
      
      private function onTabSelected(param1:String) : void
      {
         if(param1 == "Events")
         {
            this.view.refreshButton.visible = false;
         }
         else
         {
            this.view.refreshButton.visible = true;
            this.setRefreshQuestsState();
         }
      }
      
      private function onRefreshClick(param1:BaseButton) : void
      {
         if(!this.hasEnoughGold(this.dailyQuestsModel.nextRefreshPrice))
         {
            this.showPopupSignal.dispatch(new NotEnoughGoldDialog());
         }
         else
         {
            this.dailyQuestRefreshPopup = new DailyQuestRefreshPopup(this.dailyQuestsModel.nextRefreshPrice);
            this.view.hideFade();
            this.view.showFade(1381653);
            this.showPopupSignal.dispatch(this.dailyQuestRefreshPopup);
         }
      }
      
      private function setToolTipTitle(param1:String, param2:String) : void
      {
         this.toolTip = new TextToolTip(3552822,10197915,param1,param2,300,null);
         this.hoverTooltipDelegate.setDisplayObject(this.infoButton);
         this.hoverTooltipDelegate.setHideToolTipsSignal(this.hideTooltipsSignal);
         this.hoverTooltipDelegate.setShowToolTipSignal(this.showTooltipSignal);
         this.hoverTooltipDelegate.tooltip = this.toolTip;
      }
      
      private function updateTimeHandler(param1:Event) : void
      {
      }
      
      override public function destroy() : void
      {
         this.lockScreen.remove(this.onLockScreen);
         this.redeemCompleteSignal.remove(this.onRedeemComplete);
         this.closeRedeem.remove(this.onRedeemClose);
         this.closeExpire.remove(this.onExpireClose);
         this.view.removeEventListener("enterFrame",this.updateTimeHandler);
         this.dailyQuestsModel.isPopupOpened = false;
         this.toolTip = null;
         this.hoverTooltipDelegate.removeDisplayObject();
         this.hoverTooltipDelegate = null;
         this.view.refreshButton.clickSignal.remove(this.onRefreshClick);
         this.destroyRefreshTooltip();
      }
      
      private function destroyRefreshTooltip() : void
      {
         this.refreshTooltipDelegate && this.refreshTooltipDelegate.removeDisplayObject();
         this.refreshTooltipDelegate = null;
      }
      
      private function onRedeemComplete(param1:QuestRedeemResponse) : void
      {
         var _loc3_:String = null;
         var _loc2_:int = 0;
         var _loc4_:* = param1;
         if(_loc4_.ok)
         {
            _loc3_ = this.dailyQuestsModel.currentQuest.id;
            this.redeemPopup = new DailyQuestRedeemPopup(this.dailyQuestsModel.getQuestById(_loc3_),this.dailyQuestsModel.selectedItem);
            this.dailyQuestsModel.markAsCompleted(this.dailyQuestsModel.currentQuest.id);
            _loc2_ = this.dailyQuestsModel.numberOfCompletedQuests;
            if(!this.dailyQuestsModel.currentQuest.repeatable)
            {
               this.dailyQuestsModel.currentQuest.completed = true;
            }
            _loc2_++;
            this.view.hideFade();
            this.view.showFade(1381653,_loc2_ == this.dailyQuestsModel.numberOfActiveQuests);
            this.showPopupSignal.dispatch(this.redeemPopup);
         }
         else
         {
            this.view.hideFade();
            this.showPopupSignal.dispatch(new ErrorModal(300,"Quest Error",_loc4_.message));
            this.dailyQuestsModel.removeQuestFromlist(this.dailyQuestsModel.getQuestById(this.dailyQuestsModel.currentQuest.id));
            this.onRedeemClose();
         }
      }
      
      private function onLockScreen() : void
      {
         this.view.showFade();
      }
      
      private function onRedeemClose() : void
      {
         this.reloadQuestList();
         this.closePopupSignal.dispatch(this.redeemPopup);
      }
      
      private function onExpireClose() : void
      {
         this.reloadQuestList();
      }
      
      private function reloadQuestList() : void
      {
         this.view.hideFade();
         this.view.questList.tabs.tabSelectedSignal.remove(this.onTabSelected);
         this.view.renderList();
         this.view.questList.tabs.tabSelectedSignal.add(this.onTabSelected);
         this.onTabSelected("Quests");
      }
      
      private function onRefreshPopupClosed() : void
      {
         this.view.hideFade();
         this.closePopupSignal.dispatch(this.dailyQuestRefreshPopup);
      }
      
      private function onClose(param1:BaseButton) : void
      {
         this.closePopupSignal.dispatch(this.view);
      }
      
      private function hasEnoughGold(param1:int) : Boolean
      {
         return param1 <= this.hudModel.gameSprite.map.player_.credits_;
      }
   }
}
