package com.company.assembleegameclient.ui.panels.mediators {
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.objects.Container;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.OneWayContainer;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.panels.itemgrids.ContainerGrid;
import com.company.assembleegameclient.ui.panels.itemgrids.InventoryGrid;
import com.company.assembleegameclient.ui.panels.itemgrids.ItemGrid;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.InteractiveItemTile;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.ItemTile;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.ItemTileEvent;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.assembleegameclient.util.DisplayHierarchy;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.SocketServer;

import kabam.rotmg.chat.model.ChatMessage;
import kabam.rotmg.constants.ItemConstants;
import kabam.rotmg.core.model.MapModel;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.game.model.PotionInventoryModel;
import kabam.rotmg.game.signals.AddTextLineSignal;
import kabam.rotmg.game.view.CooldownTimer;
import kabam.rotmg.game.view.components.TabStripView;
import kabam.rotmg.market.ui.MarketCreateOfferScreen;
import kabam.rotmg.market.ui.MarketInventorySlot;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.pets.view.components.slot.FoodFeedFuseSlot;
import kabam.rotmg.skilltree.ui.SkillTreeWindow;
import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.ui.model.TabStripModel;

import robotlegs.bender.bundles.mvcs.Mediator;

public class SkillTreeMediator extends Mediator {

    [Inject]
    public var view:SkillTreeWindow;
    [Inject]
    public var mapModel:MapModel;
    [Inject]
    public var playerModel:PlayerModel;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var showToolTip:ShowTooltipSignal;


    override public function initialize():void {
        this.view.addToolTip.add(this.onAddToolTip);
        this.view.initalize();
    }

    private function onAddToolTip(_arg1:ToolTip):void {
        this.showToolTip.dispatch(_arg1);
    }

    override public function destroy():void {
        super.destroy();
    }

}
}
