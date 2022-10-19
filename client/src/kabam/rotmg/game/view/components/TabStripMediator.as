package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.ImageFactory;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.icons.IconButtonFactory;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.SocketServer;

import kabam.rotmg.assets.services.IconFactory;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.ui.model.TabStripModel;
import kabam.rotmg.ui.signals.UpdateBackpackTabSignal;
import kabam.rotmg.ui.signals.UpdateHUDSignal;
import kabam.rotmg.ui.signals.UpdateMarkTabSignal;
import kabam.rotmg.ui.signals.UpdateMarksSignal;
import kabam.rotmg.ui.view.StatsDockedSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class TabStripMediator extends Mediator {

    [Inject]
    public var view:TabStripView;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var tabStripModel:TabStripModel;
    [Inject]
    public var updateHUD:UpdateHUDSignal;
    [Inject]
    public var updateBackpack:UpdateBackpackTabSignal;
    [Inject]
    public var updateMark:UpdateMarkTabSignal;
    [Inject]
    public var updateMarksSignal:UpdateMarksSignal;
    [Inject]
    public var iconFactory:IconFactory;
    [Inject]
    public var imageFactory:ImageFactory;
    [Inject]
    public var iconButtonFactory:IconButtonFactory;
    [Inject]
    public var statsUndocked:StatsUndockedSignal;
    [Inject]
    public var statsDocked:StatsDockedSignal;
    [Inject]
    public var statsTabHotKeyInput:StatsTabHotKeyInputSignal;
    [Inject]
    public var openDialog:OpenDialogSignal;
    private var doShowStats:Boolean = true;
    private var markTab:MarksTabContent;

    [Inject]
    public var socketServer:SocketServer;

    [Inject]
    public var messages:MessageProvider;


    override public function initialize():void {
        this.view.imageFactory = this.imageFactory;
        this.view.iconButtonFactory = this.iconButtonFactory;
        this.view.tabSelected.add(this.onTabSelected);
        this.updateHUD.addOnce(this.addTabs);
        this.statsUndocked.add(this.onStatsUndocked);
        this.statsDocked.add(this.onStatsDocked);
        this.statsTabHotKeyInput.add(this.onTabHotkey);
    }

    private function onStatsUndocked(_arg1:StatsView):void {
        this.doShowStats = false;

        this.clearTabs();
        this.addTabs(this.hudModel.gameSprite.map.player_);
    }

    private function onStatsDocked():void {
        this.doShowStats = true;
        this.clearTabs();
        this.addTabs(this.hudModel.gameSprite.map.player_);
        this.view.setSelectedTab(1);
    }

    private function onTabHotkey():void {
        var _local1:int = (this.view.currentTabIndex + 1);
        _local1 = (_local1 % this.view.tabs.length);
        this.view.setSelectedTab(_local1);
    }

    override public function destroy():void {
        this.view.tabSelected.remove(this.onTabSelected);
        this.updateBackpack.remove(this.onUpdateBackPack);
        this.updateMark.remove(this.onUpdateMark);
        this.updateMarksSignal.remove(this.onUpdateMarkTab);
    }

    private function addTabs(_arg1:Player):void {
        if (!_arg1) return;

        this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.INVENTORY_ICON_ID), new InventoryTabContent(_arg1));
        if (_arg1.hasBackpack_)
            this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.BACKPACK_ICON_ID), new BackpackTabContent(_arg1));
        else
            this.updateBackpack.add(this.onUpdateBackPack);

        if (this.doShowStats)
            this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.STATS_ICON_ID), new StatsTabContent(this.view.HEIGHT));

        if (_arg1.marksEnabled_) {
            this.markTab = new MarksTabContent(_arg1);
            this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.MARKS_ICON_ID), this.markTab);
            this.updateMarksSignal.add(this.onUpdateMarkTab)
        } else
            this.updateMark.add(this.onUpdateMark);

        this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.SOR_ICON_ID), new SorTabContent(_arg1));
        //if(_arg1.skillPoints_ > 0) //if player has gotten any skill points skill tree will unlock

        this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.SKILLTREE_ICON_ID), new SkillTabContent(_arg1, this.hudModel.gameSprite, socketServer, messages));
    }

    private function clearTabs():void {
        this.view.clearTabs();
    }

    private function onTabSelected(_arg1:String):void {
        this.tabStripModel.currentSelection = _arg1;
    }

    private function onUpdateBackPack(_arg1:Boolean):void {
        var _local2:Player;
        if (_arg1) {
            _local2 = this.hudModel.gameSprite.map.player_;
            this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.BACKPACK_ICON_ID), new BackpackTabContent(_local2));
            this.updateBackpack.remove(this.onUpdateBackPack);
        }
    }

    private function onUpdateMarkTab(a:int, b:int, c:int, d:int, m:int):void {
        this.markTab.updateMarksUI(a,b,c,d, m);
        if(this.markTab.nodeShop)
            this.markTab.nodeShop.updateShop(a,b,c,d);
        if(this.markTab.markShop)
            this.markTab.markShop.updateShop(m);
    }

    private function onUpdateMark(_arg1:Boolean):void {
        var _local2:Player = this.hudModel.gameSprite.map.player_;
        if (_arg1) {
            this.markTab = new MarksTabContent(_local2);
            this.view.addTab(this.iconFactory.makeIconBitmap(TabConstants.MARKS_ICON_ID), this.markTab);
            this.updateMark.remove(this.onUpdateMark);
        }
    }
}
}