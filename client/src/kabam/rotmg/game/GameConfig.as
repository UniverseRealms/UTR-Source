﻿package kabam.rotmg.game {
import com.company.assembleegameclient.game.AlertStatusModel;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.game.GiftStatusModel;
import com.company.assembleegameclient.game.LootboxModel;
import com.company.assembleegameclient.game.MarkShopModel;
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.map.MapMediator;
import com.company.assembleegameclient.map.QueueStatusTextSignal;
import com.company.assembleegameclient.map.mapoverlay.MapOverlay;
import com.company.assembleegameclient.ui.TradeSlot;
import com.company.assembleegameclient.ui.TradeSlotMediator;
import com.company.assembleegameclient.ui.panels.InteractPanel;
import com.company.assembleegameclient.ui.panels.PartyPanel;
import com.company.assembleegameclient.ui.panels.PortalPanel;
import com.company.assembleegameclient.ui.panels.itemgrids.EquippedGrid;
import com.company.assembleegameclient.ui.panels.itemgrids.InventoryGrid;
import com.company.assembleegameclient.ui.panels.itemgrids.ItemGrid;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.ItemTileSprite;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.ItemTileSpriteMediator;
import com.company.assembleegameclient.ui.panels.mediators.EquippedGridMediator;
import com.company.assembleegameclient.ui.panels.mediators.InteractPanelMediator;
import com.company.assembleegameclient.ui.panels.mediators.InventoryGridMediator;
import com.company.assembleegameclient.ui.panels.mediators.ItemGridMediator;
import com.company.assembleegameclient.ui.panels.mediators.PartyPanelMediator;
import com.company.assembleegameclient.ui.panels.mediators.SkillTreeMediator;

import kabam.lib.net.impl.SocketServerModel;
import kabam.rotmg.application.setup.ApplicationSetup;
import kabam.rotmg.chat.ChatConfig;
import kabam.rotmg.core.signals.AppInitDataReceivedSignal;
import kabam.rotmg.game.commands.AlertStatusUpdateCommand;
import kabam.rotmg.game.commands.GiftStatusUpdateCommand;
import kabam.rotmg.game.commands.LootboxUpdateCommand;
import kabam.rotmg.game.commands.MarkShopUpdateCommand;
import kabam.rotmg.game.commands.ParsePotionDataCommand;
import kabam.rotmg.game.commands.PlayGameCommand;
import kabam.rotmg.game.commands.TextPanelMessageUpdateCommand;
import kabam.rotmg.game.commands.TransitionFromGameToMenuCommand;
import kabam.rotmg.game.commands.UseBuyPotionCommand;
import kabam.rotmg.game.focus.GameFocusConfig;
import kabam.rotmg.game.model.ChatFilter;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.game.model.TextPanelData;
import kabam.rotmg.game.signals.AddSpeechBalloonSignal;
import kabam.rotmg.game.signals.ExitGameSignal;
import kabam.rotmg.game.signals.GameClosedSignal;
import kabam.rotmg.game.signals.GiftStatusUpdateSignal;
import kabam.rotmg.game.signals.PlayGameSignal;
import kabam.rotmg.game.signals.SetTextBoxVisibilitySignal;
import kabam.rotmg.game.signals.SetWorldInteractionSignal;
import kabam.rotmg.game.signals.TextPanelMessageUpdateSignal;
import kabam.rotmg.game.signals.UpdateAlertStatusDisplaySignal;
import kabam.rotmg.game.signals.UpdateGiftStatusDisplaySignal;
import kabam.rotmg.game.signals.UpdateLootboxButtonSignal;
import kabam.rotmg.game.signals.UpdateMarkShopButtonSignal;
import kabam.rotmg.game.signals.UseBuyPotionSignal;
import kabam.rotmg.game.view.AlertStatusDisplay;
import kabam.rotmg.game.view.AlertStatusMediator;
import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.game.view.CreditDisplayMediator;
import kabam.rotmg.game.view.GameSpriteMediator;
import kabam.rotmg.game.view.GiftStatusDisplay;
import kabam.rotmg.game.view.GiftStatusDisplayMediator;
import kabam.rotmg.game.view.LootboxMediator;
import kabam.rotmg.game.view.LootboxModalButton;
import kabam.rotmg.game.view.LootboxesDisplay;
import kabam.rotmg.game.view.LootboxesDisplayMediator;
import kabam.rotmg.game.view.MapOverlayMediator;
import kabam.rotmg.game.view.MoneyChangerPanel;
import kabam.rotmg.game.view.MoneyChangerPanelMediator;
import kabam.rotmg.game.view.PortalPanelMediator;
import kabam.rotmg.game.view.SellableObjectPanel;
import kabam.rotmg.game.view.SellableObjectPanelMediator;
import kabam.rotmg.game.view.TextPanel;
import kabam.rotmg.game.view.TextPanelMediator;
import kabam.rotmg.game.view.components.SorTabContent;
import kabam.rotmg.game.view.components.SorTabContentMediator;
import kabam.rotmg.game.view.components.StatsMediator;
import kabam.rotmg.game.view.components.StatsView;
import kabam.rotmg.game.view.components.TabStripMediator;
import kabam.rotmg.game.view.components.TabStripView;
import kabam.rotmg.lootBoxes.LootboxModal;
import kabam.rotmg.lootBoxes.LootboxModalMediator;

import kabam.rotmg.markShop.shops.MarkShop;
import kabam.rotmg.markShop.shops.MarkShopMediator;
import kabam.rotmg.markShop.shops.NodeShop;
import kabam.rotmg.markShop.shops.NodeShopMediator;
import kabam.rotmg.raidLauncher.RaidLauncherMediator;
import kabam.rotmg.raidLauncher.RaidLauncherModal;
import kabam.rotmg.skilltree.ui.SkillTreeWindow;
import kabam.rotmg.sorForge.SorForgeMediator;
import kabam.rotmg.sorForge.SorForgeModal;
import kabam.rotmg.sorForge.SorForgerUI;
import kabam.rotmg.sorForge.SorForgerUIMediator;
import kabam.rotmg.sorForge.components.SorForgePanel;
import kabam.rotmg.sorForge.components.SorForgePanelMediator;
import kabam.rotmg.ui.model.TabStripModel;

import org.swiftsuspenders.Injector;

import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;
import robotlegs.bender.framework.api.IConfig;
import robotlegs.bender.framework.api.IContext;

public class GameConfig implements IConfig {

    [Inject]
    public var context:IContext;
    [Inject]
    public var injector:Injector;
    [Inject]
    public var mediatorMap:IMediatorMap;
    [Inject]
    public var commandMap:ISignalCommandMap;
    [Inject]
    public var setup:ApplicationSetup;


    private function generalGameConfiguration():void {
        this.injector.map(UpdateGiftStatusDisplaySignal).asSingleton();
        this.injector.map(SetWorldInteractionSignal).asSingleton();
        this.injector.map(SetTextBoxVisibilitySignal).asSingleton();
        this.injector.map(AddSpeechBalloonSignal).asSingleton();
        this.injector.map(ChatFilter).asSingleton();
        this.injector.map(GiftStatusModel).asSingleton();
        this.injector.map(AlertStatusModel).asSingleton();
        this.injector.map(MarkShopModel).asSingleton();
        this.injector.map(LootboxModel).asSingleton();
        this.injector.map(TabStripModel).asSingleton();
        this.injector.map(ExitGameSignal).asSingleton();
        this.injector.map(QueueStatusTextSignal).asSingleton();
        this.injector.map(SocketServerModel).asSingleton();
        this.makeTextPanelMappings();
        this.makeGiftStatusDisplayMappings();
        this.mediatorMap.map(SorForgePanel).toMediator(SorForgePanelMediator);
        this.mediatorMap.map(PortalPanel).toMediator(PortalPanelMediator);
        this.mediatorMap.map(PartyPanel).toMediator(PartyPanelMediator);
        this.mediatorMap.map(InteractPanel).toMediator(InteractPanelMediator);
        this.mediatorMap.map(ItemGrid).toMediator(ItemGridMediator);
        this.mediatorMap.map(SkillTreeWindow).toMediator(SkillTreeMediator);
        this.mediatorMap.map(InventoryGrid).toMediator(InventoryGridMediator);
        this.mediatorMap.map(ItemTileSprite).toMediator(ItemTileSpriteMediator);
        this.mediatorMap.map(TradeSlot).toMediator(TradeSlotMediator);
        this.mediatorMap.map(EquippedGrid).toMediator(EquippedGridMediator);
        this.mediatorMap.map(MapOverlay).toMediator(MapOverlayMediator);
        this.mediatorMap.map(LootboxModal).toMediator(LootboxModalMediator);
        this.mediatorMap.map(Map).toMediator(MapMediator);
        this.mediatorMap.map(StatsView).toMediator(StatsMediator);
        this.mediatorMap.map(TabStripView).toMediator(TabStripMediator);
        this.mediatorMap.map(RaidLauncherModal).toMediator(RaidLauncherMediator);
        this.mediatorMap.map(NodeShop).toMediator(NodeShopMediator);
        this.mediatorMap.map(MarkShop).toMediator(MarkShopMediator);
        this.mediatorMap.map(SorTabContent).toMediator(SorTabContentMediator);
        this.mediatorMap.map(SorForgeModal).toMediator(SorForgeMediator);
        this.mediatorMap.map(SorForgerUI).toMediator(SorForgerUIMediator);
        this.commandMap.map(AppInitDataReceivedSignal).toCommand(ParsePotionDataCommand);
        this.commandMap.map(GiftStatusUpdateSignal).toCommand(GiftStatusUpdateCommand);
        this.commandMap.map(UpdateAlertStatusDisplaySignal).toCommand(AlertStatusUpdateCommand);
        this.commandMap.map(UpdateMarkShopButtonSignal).toCommand(MarkShopUpdateCommand);
        this.commandMap.map(UpdateLootboxButtonSignal).toCommand(LootboxUpdateCommand);
        this.commandMap.map(UseBuyPotionSignal).toCommand(UseBuyPotionCommand);
        this.commandMap.map(GameClosedSignal).toCommand(TransitionFromGameToMenuCommand);
        this.commandMap.map(PlayGameSignal).toCommand(PlayGameCommand);
    }

    public function configure():void {
        this.context.configure(GameFocusConfig);
        this.injector.map(GameModel).asSingleton();
        this.generalGameConfiguration();
        this.context.configure(ChatConfig);
    }

    private function makeTextPanelMappings():void {
        this.injector.map(TextPanelData).asSingleton();
        this.commandMap.map(TextPanelMessageUpdateSignal, true).toCommand(TextPanelMessageUpdateCommand);
        this.mediatorMap.map(TextPanel).toMediator(TextPanelMediator);
    }

    private function makeGiftStatusDisplayMappings():void {
        this.mediatorMap.map(AlertStatusDisplay).toMediator(AlertStatusMediator);
        this.mediatorMap.map(GiftStatusDisplay).toMediator(GiftStatusDisplayMediator);
        this.mediatorMap.map(GameSprite).toMediator(GameSpriteMediator);
        this.mediatorMap.map(CreditDisplay).toMediator(CreditDisplayMediator);
        this.mediatorMap.map(LootboxesDisplay).toMediator(LootboxesDisplayMediator);
        this.mediatorMap.map(MoneyChangerPanel).toMediator(MoneyChangerPanelMediator);
        this.mediatorMap.map(LootboxModalButton).toMediator(LootboxMediator);
        this.mediatorMap.map(SellableObjectPanel).toMediator(SellableObjectPanelMediator);
    }
}
}
