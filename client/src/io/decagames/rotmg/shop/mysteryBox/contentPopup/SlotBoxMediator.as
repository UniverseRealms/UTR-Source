package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import com.company.assembleegameclient.ui.tooltip.TextToolTip;
   import kabam.rotmg.core.signals.HideTooltipsSignal;
   import kabam.rotmg.core.signals.ShowTooltipSignal;
   import kabam.rotmg.tooltips.HoverTooltipDelegate;
   import kabam.rotmg.ui.model.HUDModel;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class SlotBoxMediator extends Mediator
   {
       
      
      [Inject]
      public var view:SlotBox;
      
      [Inject]
      public var hud:HUDModel;
      
      [Inject]
      public var showTooltipSignal:ShowTooltipSignal;
      
      [Inject]
      public var hideTooltipSignal:HideTooltipsSignal;
      
      private var toolTip:TextToolTip = null;
      
      private var hoverTooltipDelegate:HoverTooltipDelegate;
      
      public function SlotBoxMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         if(this.view.slotType == "VAULT_SLOT")
         {
            this.toolTip = new TextToolTip(3552822,10197915,"Vault.chest","Vault.chestDescription",200);
            this.hoverTooltipDelegate = new HoverTooltipDelegate();
            this.hoverTooltipDelegate.setShowToolTipSignal(this.showTooltipSignal);
            this.hoverTooltipDelegate.setHideToolTipsSignal(this.hideTooltipSignal);
            this.hoverTooltipDelegate.setDisplayObject(this.view);
            this.hoverTooltipDelegate.tooltip = this.toolTip;
         }
      }
      
      override public function destroy() : void
      {
      }
   }
}
