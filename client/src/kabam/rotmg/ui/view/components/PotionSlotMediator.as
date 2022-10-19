package kabam.rotmg.ui.view.components {
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.InteractiveItemTile;
import com.company.assembleegameclient.util.DisplayHierarchy;

import flash.display.DisplayObject;

import kabam.rotmg.constants.ItemConstants;
import kabam.rotmg.game.model.PotionInventoryModel;
import kabam.rotmg.game.model.UseBuyPotionVO;
import kabam.rotmg.game.signals.UseBuyPotionSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.pets.view.components.slot.FoodFeedFuseSlot;
import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.ui.model.PotionModel;
import kabam.rotmg.ui.signals.UpdateHUDSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class PotionSlotMediator extends Mediator
{
    [Inject]
    public var view:PotionSlotView;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var updateHUD:UpdateHUDSignal;
    [Inject]
    public var potionInventoryModel:PotionInventoryModel;
    [Inject]
    public var useBuyPotionSignal:UseBuyPotionSignal;
    private var blockingUpdate:Boolean = false;

    public function PotionSlotMediator() {
        super();
    }

    override public function initialize():void {
        this.updateHUD.addOnce(this.initializeData);
        this.view.drop.add(this.onDrop);
        this.view.buyUse.add(this.onBuyUse);
        this.updateHUD.add(this.update);
    }

    override public function destroy():void {
        this.view.drop.remove(this.onDrop);
        this.view.buyUse.remove(this.onBuyUse);
        this.updateHUD.remove(this.update);
    }

    private function initializeData(param1:Player):void {
        var _loc2_:PotionModel = this.potionInventoryModel.potionModels[this.view.position];
        var _loc3_:int = param1.getPotionCount(_loc2_.objectId);
        this.view.setData(_loc3_, _loc2_.currentCost(_loc3_), _loc2_.available, _loc2_.objectId);
    }

    private function update(param1:Player):void {
        var _loc2_:PotionModel = null;
        var _loc3_:int = 0;
        if ((this.view.objectType == PotionInventoryModel.HEALTH_POTION_ID || this.view.objectType == PotionInventoryModel.MAGIC_POTION_ID) && !this.blockingUpdate) {
            _loc2_ = this.potionInventoryModel.getPotionModel(this.view.objectType);
            _loc3_ = param1.getPotionCount(_loc2_.objectId);
            this.view.setData(_loc3_, _loc2_.currentCost(_loc3_), _loc2_.available);
        }
    }

    private function onDrop(param1:DisplayObject):void {
        var _loc2_:InteractiveItemTile = null;
        var _loc3_:Player = this.hudModel.gameSprite.map.player_;
        var _loc4_:* = DisplayHierarchy.getParentWithTypeArray(param1, InteractiveItemTile, Map, FoodFeedFuseSlot);
        if (_loc4_ is Map || Parameters.isGpuRender() && _loc4_ == null) {
            GameServerConnection.instance.invDrop(_loc3_, PotionInventoryModel.getPotionSlot(this.view.objectType), this.view.objectType);
        } else if (_loc4_ is InteractiveItemTile) {
            _loc2_ = _loc4_ as InteractiveItemTile;
            if (_loc2_.getItemId() == ItemConstants.NO_ITEM && _loc2_.ownerGrid.owner != _loc3_) {
                GameServerConnection.instance.invSwapPotion(_loc3_, _loc3_, PotionInventoryModel.getPotionSlot(this.view.objectType), this.view.objectType, _loc2_.ownerGrid.owner, _loc2_.tileId, ItemConstants.NO_ITEM);
            }
        }
    }

    private function onBuyUse():void {
        var _loc1_:UseBuyPotionVO = null;
        var _loc2_:PotionModel = this.potionInventoryModel.potionModels[this.view.position];
        if (_loc2_.available) {
            _loc1_ = new UseBuyPotionVO(_loc2_.objectId, UseBuyPotionVO.SHIFTCLICK);
            this.useBuyPotionSignal.dispatch(_loc1_);
        }
    }
}
}
