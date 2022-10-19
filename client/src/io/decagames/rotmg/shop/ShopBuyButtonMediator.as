package io.decagames.rotmg.shop
{
   import io.decagames.rotmg.supportCampaign.data.SupporterCampaignModel;
   import io.decagames.rotmg.supportCampaign.tooltips.PointsTooltip;
   import kabam.rotmg.core.signals.HideTooltipsSignal;
   import kabam.rotmg.core.signals.ShowTooltipSignal;
   import kabam.rotmg.tooltips.HoverTooltipDelegate;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class ShopBuyButtonMediator extends Mediator
   {
       
      
      [Inject]
      public var view:ShopBuyButton;
      
      [Inject]
      public var showTooltipSignal:ShowTooltipSignal;
      
      [Inject]
      public var hideTooltipSignal:HideTooltipsSignal;
      
      [Inject]
      public var model:SupporterCampaignModel;
      
      private var toolTip:PointsTooltip = null;
      
      private var hoverTooltipDelegate:HoverTooltipDelegate;
      
      public function ShopBuyButtonMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         if(this.model.hasValidData && this.view.showCampaignTooltip && this.model.isStarted && !this.model.isEnded && this.view.currency == 0 && this.model.shopPurchasePointsRatio != 0)
         {
            this.toolTip = new PointsTooltip(this.view,3552822,10197915,200);
            this.hoverTooltipDelegate = new HoverTooltipDelegate();
            this.hoverTooltipDelegate.setShowToolTipSignal(this.showTooltipSignal);
            this.hoverTooltipDelegate.setHideToolTipsSignal(this.hideTooltipSignal);
            this.hoverTooltipDelegate.setDisplayObject(this.view);
            this.hoverTooltipDelegate.tooltip = this.toolTip;
         }
      }
      
      override public function destroy() : void
      {
         if(this.view.showCampaignTooltip)
         {
            this.hoverTooltipDelegate = null;
            this.toolTip = null;
         }
      }
   }
}
