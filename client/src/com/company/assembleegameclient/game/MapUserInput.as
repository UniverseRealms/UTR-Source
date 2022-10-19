package com.company.assembleegameclient.game {
import com.company.assembleegameclient.map.Square;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.options.Options;
import com.company.util.KeyCodes;

import flash.display.Stage;
import flash.display.StageDisplayState;
import flash.events.Event;
import flash.events.KeyboardEvent;
import flash.events.MouseEvent;
import flash.system.Capabilities;


import kabam.rotmg.application.setup.ApplicationSetup;
import kabam.rotmg.chat.model.ChatMessage;
import kabam.rotmg.constants.GeneralConstants;
import kabam.rotmg.constants.UseType;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.core.view.Layers;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.game.model.PotionInventoryModel;
import kabam.rotmg.game.model.UseBuyPotionVO;
import kabam.rotmg.game.signals.AddTextLineSignal;
import kabam.rotmg.game.signals.ExitGameSignal;
import kabam.rotmg.game.signals.GiftStatusUpdateSignal;
import kabam.rotmg.game.signals.SetTextBoxVisibilitySignal;
import kabam.rotmg.game.signals.UpdateAlertStatusDisplaySignal;
import kabam.rotmg.game.signals.UpdateLootboxButtonSignal;
import kabam.rotmg.game.signals.UpdateMarkShopButtonSignal;
import kabam.rotmg.game.signals.UseBuyPotionSignal;
import kabam.rotmg.game.view.components.StatsTabHotKeyInputSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.minimap.control.MiniMapZoomSignal;
import kabam.rotmg.ui.model.TabStripModel;

import net.hires.debug.Stats;

import org.swiftsuspenders.Injector;

public class MapUserInput {

    private static const MOUSE_DOWN_WAIT_PERIOD:uint = 175;

    private static var stats_:Stats = new Stats();
    private static var arrowWarning_:Boolean = false;

    public var gs_:GameSprite;
    private var moveLeft_:Boolean = false;
    private var moveRight_:Boolean = false;
    private var moveUp_:Boolean = false;
    private var moveDown_:Boolean = false;
    private var rotateLeft_:Boolean = false;
    private var rotateRight_:Boolean = false;
    private var mouseDown_:Boolean = false;
    private var autofire_:Boolean = false;
    private var currentString:String = "";
    private var specialKeyDown_:Boolean = false;
    private var enablePlayerInput_:Boolean = true;
    private var giftStatusUpdateSignal:GiftStatusUpdateSignal;
    private var addTextLine:AddTextLineSignal;
    private var setTextBoxVisibility:SetTextBoxVisibilitySignal;
    private var statsTabHotKeyInputSignal:StatsTabHotKeyInputSignal;
    private var miniMapZoom:MiniMapZoomSignal;
    private var markShopUpdateSignal:UpdateMarkShopButtonSignal;
    private var lootBoxUpdateSignal:UpdateLootboxButtonSignal;
    private var alertStatusUpdateSignal:UpdateAlertStatusDisplaySignal;
    private var useBuyPotionSignal:UseBuyPotionSignal;
    private var potionInventoryModel:PotionInventoryModel;
    private var openDialogSignal:OpenDialogSignal;
    private var closeDialogSignal:CloseDialogsSignal;
    private var tabStripModel:TabStripModel;
    public var layers:Layers;
    private var exitGame:ExitGameSignal;

    public function EndSpecial():void {
        this.specialKeyDown_ = false;
    }

    public function MapUserInput(_arg1:GameSprite) {
        this.gs_ = _arg1;
        this.gs_.addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        this.gs_.addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        var _local2:Injector = StaticInjectorContext.getInjector();
        this.giftStatusUpdateSignal = _local2.getInstance(GiftStatusUpdateSignal);
        this.alertStatusUpdateSignal = _local2.getInstance(UpdateAlertStatusDisplaySignal);
        this.markShopUpdateSignal = _local2.getInstance(UpdateMarkShopButtonSignal);
        this.lootBoxUpdateSignal = _local2.getInstance(UpdateLootboxButtonSignal);
        this.addTextLine = _local2.getInstance(AddTextLineSignal);
        this.setTextBoxVisibility = _local2.getInstance(SetTextBoxVisibilitySignal);
        this.miniMapZoom = _local2.getInstance(MiniMapZoomSignal);
        this.useBuyPotionSignal = _local2.getInstance(UseBuyPotionSignal);
        this.potionInventoryModel = _local2.getInstance(PotionInventoryModel);
        this.tabStripModel = _local2.getInstance(TabStripModel);
        this.layers = _local2.getInstance(Layers);
        this.statsTabHotKeyInputSignal = _local2.getInstance(StatsTabHotKeyInputSignal);
        this.exitGame = _local2.getInstance(ExitGameSignal);
        this.openDialogSignal = _local2.getInstance(OpenDialogSignal);
        this.closeDialogSignal = _local2.getInstance(CloseDialogsSignal);
        this.gs_.map.signalRenderSwitch.add(this.onRenderSwitch);
    }

    public function onRenderSwitch(hwa:Boolean):void {
        if (hwa) {
            this.gs_.map.removeEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            this.gs_.map.removeEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
            this.gs_.stage.addEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            this.gs_.stage.addEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
        } else {
            this.gs_.stage.removeEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            this.gs_.stage.removeEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
            this.gs_.map.addEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            this.gs_.map.addEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
        }
    }

    public function clearInput():void {
        this.moveLeft_ = false;
        this.moveRight_ = false;
        this.moveUp_ = false;
        this.moveDown_ = false;
        this.rotateLeft_ = false;
        this.rotateRight_ = false;
        this.mouseDown_ = false;
        this.autofire_ = false;
        this.setPlayerMovement();
    }

    public function setEnablePlayerInput(input:Boolean):void {
        if (this.enablePlayerInput_ != input) {
            this.enablePlayerInput_ = input;
            this.clearInput();
        }
    }

    private function onAddedToStage(_arg1:Event):void {
        var stage:Stage = this.gs_.stage;
        stage.addEventListener(Event.ACTIVATE, this.onActivate);
        stage.addEventListener(Event.DEACTIVATE, this.onDeactivate);
        stage.addEventListener(KeyboardEvent.KEY_DOWN, this.onKeyDown);
        stage.addEventListener(KeyboardEvent.KEY_UP, this.onKeyUp);
        stage.addEventListener(MouseEvent.MOUSE_WHEEL, this.onMouseWheel);
        if (Parameters.isGpuRender()) {
            stage.addEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            stage.addEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
        }
        else {
            this.gs_.map.addEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            this.gs_.map.addEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
        }
        stage.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        stage.addEventListener(MouseEvent.RIGHT_CLICK, this.disableRightClick);
    }

    private function onRemovedFromStage(_arg1:Event):void {
        var stage:Stage = this.gs_.stage;
        stage.removeEventListener(Event.ACTIVATE, this.onActivate);
        stage.removeEventListener(Event.DEACTIVATE, this.onDeactivate);
        stage.removeEventListener(KeyboardEvent.KEY_DOWN, this.onKeyDown);
        stage.removeEventListener(KeyboardEvent.KEY_UP, this.onKeyUp);
        stage.removeEventListener(MouseEvent.MOUSE_WHEEL, this.onMouseWheel);
        if (Parameters.isGpuRender()) {
            stage.removeEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            stage.removeEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
        }
        else {
            this.gs_.map.removeEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            this.gs_.map.removeEventListener(MouseEvent.MOUSE_UP, this.onMouseUp);
        }
        stage.removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        stage.removeEventListener(MouseEvent.RIGHT_CLICK, this.disableRightClick);
    }

    public function disableRightClick(event:MouseEvent):void {}

    private function onActivate(event:Event):void {}

    private function onDeactivate(event:Event):void {
        this.clearInput();
    }

    public function onMouseDown(event:MouseEvent):void {
        var player:Player = this.gs_.map.player_;
        var angle:Number = NaN;

        if(player == null || !this.enablePlayerInput_) return;
        if (Parameters.isGpuRender() && event.currentTarget != event.target && event.target != this.gs_.map && event.target != this.gs_ && event.currentTarget != this.gs_.chatBox_.list) return;

        angle = Math.atan2(this.gs_.map.mouseY, this.gs_.map.mouseX);
        player.attemptAttackAngle(angle, player.isUnstable());
        this.mouseDown_ = true;
    }

    public function onMouseUp(event:MouseEvent):void {
        var player:Player = this.gs_.map.player_;
        this.mouseDown_ = false;
        if (player == null) return;
        player.isShooting = false;
    }

    private function onMouseWheel(event:MouseEvent):void {
        if (event.delta > 0) {
            this.miniMapZoom.dispatch(MiniMapZoomSignal.IN);
        }
        else {
            this.miniMapZoom.dispatch(MiniMapZoomSignal.OUT);
        }
    }

    private function onEnterFrame(event:Event):void {
        var player:Player = this.gs_.map.player_;
        var angle:Number = NaN;
        if(player == null || !this.enablePlayerInput_) return;
        if(this.mouseDown_ || this.autofire_) {
            angle = Math.atan2(this.gs_.map.mouseY, this.gs_.map.mouseX);
            player.attemptAttackAngle(angle, player.isUnstable());
        }
    }

    public function onKeyDown(event:KeyboardEvent):void {
        var stage:Stage = this.gs_.stage;

        if(stage.focus != null) return;

        var player:Player = this.gs_.map.player_;

        var _local4:AddTextLineSignal;
        var _local5:ChatMessage;
        var _local6:GameObject;
        var _local7:Number;
        var _local8:Number;
        var _local9:Boolean;
        var _local10:Square;

        switch (event.keyCode) {
            case Parameters.data_.moveUp:
                this.moveUp_ = true;
                break;
            case Parameters.data_.moveDown:
                this.moveDown_ = true;
                break;
            case Parameters.data_.moveLeft:
                this.moveLeft_ = true;
                break;
            case Parameters.data_.moveRight:
                this.moveRight_ = true;
                break;
            case Parameters.data_.rotateLeft:
                if (!Parameters.data_.allowRotation) break;
                this.rotateLeft_ = true;
                break;
            case Parameters.data_.rotateRight:
                if (!Parameters.data_.allowRotation) break;
                this.rotateRight_ = true;
                break;
            case Parameters.data_.resetToDefaultCameraAngle:
                Parameters.data_.cameraAngle = Parameters.data_.defaultCameraAngle;
                Parameters.save();
                break;
            case Parameters.data_.useSpecial:
                _local6 = this.gs_.map.player_;
                if (_local6 == null) break;
                if (!this.specialKeyDown_) {
                    if (player.isUnstable()) {
                        _local7 = ((Math.random() * 600) - 300);
                        _local8 = ((Math.random() * 600) - 325);
                    }
                    else {
                        _local7 = this.gs_.map.mouseX;
                        _local8 = this.gs_.map.mouseY;
                    }
                    _local9 = player.useAltWeapon(_local7, _local8, UseType.START_USE);
                    if (_local9) {
                        this.specialKeyDown_ = true;
                    }
                }
                break;
            case Parameters.data_.autofireToggle:
                this.gs_.map.player_.isShooting = (this.autofire_ = !(this.autofire_));
                break;
            case Parameters.data_.toggleHPBar:
                Parameters.data_.HPBar = !(Parameters.data_.HPBar);
                break;
            case Parameters.data_.useInvSlot1:
                this.useItem(4);
                break;
            case Parameters.data_.useInvSlot2:
                this.useItem(5);
                break;
            case Parameters.data_.useInvSlot3:
                this.useItem(6);
                break;
            case Parameters.data_.useInvSlot4:
                this.useItem(7);
                break;
            case Parameters.data_.useInvSlot5:
                this.useItem(8);
                break;
            case Parameters.data_.useInvSlot6:
                this.useItem(9);
                break;
            case Parameters.data_.useInvSlot7:
                this.useItem(10);
                break;
            case Parameters.data_.useInvSlot8:
                this.useItem(11);
                break;
            case Parameters.data_.useHealthPotion:
                if (this.potionInventoryModel.getPotionModel(PotionInventoryModel.HEALTH_POTION_ID).available) {
                    this.useBuyPotionSignal.dispatch(new UseBuyPotionVO(PotionInventoryModel.HEALTH_POTION_ID, UseBuyPotionVO.CONTEXTBUY));
                }
                break;
            case Parameters.data_.GPURenderToggle:
                Parameters.data_.GPURender = !(Parameters.data_.GPURender);
                break;
            case Parameters.data_.useMagicPotion:
                if (this.potionInventoryModel.getPotionModel(PotionInventoryModel.MAGIC_POTION_ID).available) {
                    this.useBuyPotionSignal.dispatch(new UseBuyPotionVO(PotionInventoryModel.MAGIC_POTION_ID, UseBuyPotionVO.CONTEXTBUY));
                }
                break;
            case Parameters.data_.miniMapZoomOut:
                this.miniMapZoom.dispatch(MiniMapZoomSignal.OUT);
                break;
            case Parameters.data_.miniMapZoomIn:
                this.miniMapZoom.dispatch(MiniMapZoomSignal.IN);
                break;
            case Parameters.data_.togglePerformanceStats:
                this.togglePerformanceStats();
                break;
            case Parameters.data_.escapeToNexus:
                this.exitGame.dispatch();
                this.gs_.gsc_.escape();
                break;
            case Parameters.data_.options:
                this.clearInput();
                this.layers.overlay.addChild(new Options(this.gs_));
                break;
            case Parameters.data_.toggleCentering:
                Parameters.data_.centerOnPlayer = !(Parameters.data_.centerOnPlayer);
                Parameters.save();
                break;
            case Parameters.data_.toggleFullscreen:
                if (Capabilities.playerType == "Desktop") {
                    Parameters.data_.fullscreenMode = !(Parameters.data_.fullscreenMode);
                    Parameters.save();
                    stage.displayState = ((Parameters.data_.fullscreenMode) ? "fullScreenInteractive" : StageDisplayState.NORMAL);
                }
                break;
            case Parameters.data_.switchTabs:
                this.statsTabHotKeyInputSignal.dispatch();
                break;
            case Parameters.data_.partySummon:
                this.gs_.gsc_.playerText("/psummon");
                break;
            case Parameters.data_.partyAccept:
                this.gs_.gsc_.playerText("/accept");
                break;
        }
        this.setPlayerMovement();
    }

    private function onKeyUp(event:KeyboardEvent):void {
        switch (event.keyCode) {
            case Parameters.data_.moveUp:
                this.moveUp_ = false;
                break;
            case Parameters.data_.moveDown:
                this.moveDown_ = false;
                break;
            case Parameters.data_.moveLeft:
                this.moveLeft_ = false;
                break;
            case Parameters.data_.moveRight:
                this.moveRight_ = false;
                break;
            case Parameters.data_.rotateLeft:
                this.rotateLeft_ = false;
                break;
            case Parameters.data_.rotateRight:
                this.rotateRight_ = false;
                break;
            case Parameters.data_.useSpecial:
                if (this.specialKeyDown_) {
                    this.specialKeyDown_ = false;
                    this.gs_.map.player_.useAltWeapon(this.gs_.map.mouseX, this.gs_.map.mouseY, UseType.END_USE);
                }
                break;
        }
        this.setPlayerMovement();
    }

    private function setPlayerMovement():void {
        var player:Player = this.gs_.map.player_;
        if(player == null) return;
        if(!this.enablePlayerInput_) {
            player.setRelativeMovement(0, 0, 0);
            return;
        }
        player.setRelativeMovement(int(this.rotateRight_) - int(this.rotateLeft_), int(this.moveRight_) - int(this.moveLeft_), int(this.moveDown_) - int(this.moveUp_));
    }

    private function useItem(slot:int):void {
        if (this.tabStripModel.currentSelection == TabStripModel.BACKPACK) {
            slot = slot + GeneralConstants.NUM_INVENTORY_SLOTS;
        }
        var slotIndex:int =
                ObjectLibrary.getMatchingSlotIndex(this.gs_.map.player_.equipment_[slot], this.gs_.map.player_);
        if (slotIndex != -1) {
            GameServerConnection.instance.invSwap(
                    this.gs_.map.player_,
                    this.gs_.map.player_, slot,
                    this.gs_.map.player_.equipment_[slot],
                    this.gs_.map.player_, slotIndex,
                    this.gs_.map.player_.equipment_[slotIndex]);
        }
        else {
            GameServerConnection.instance.useItem_new(this.gs_.map.player_, slot);
        }
    }

    private function togglePerformanceStats():void {
        if (this.gs_.contains(stats_)) {
            this.gs_.removeChild(stats_);
            this.gs_.removeChild(this.gs_.gsc_.jitterWatcher_);
            this.gs_.gsc_.disableJitterWatcher();
        }
        else {
            this.gs_.addChild(stats_);
            this.gs_.gsc_.enableJitterWatcher();
            this.gs_.gsc_.jitterWatcher_.y = stats_.height;
            this.gs_.addChild(this.gs_.gsc_.jitterWatcher_);
        }
    }
}
}