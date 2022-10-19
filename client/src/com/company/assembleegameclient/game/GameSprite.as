package com.company.assembleegameclient.game {
import com.company.assembleegameclient.game.events.MoneyChangedEvent;
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.IInteractiveObject;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.objects.Projectile;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.GuildText;
import com.company.assembleegameclient.ui.RankText;
import com.company.assembleegameclient.ui.menu.PlayerMenu;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.CachingColorTransformer;
import com.company.util.MoreColorUtil;
import com.company.util.MoreObjectUtil;
import com.company.util.PointUtil;

import flash.display.DisplayObject;
import flash.display.Sprite;
import flash.display.StageScaleMode;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.ColorMatrixFilter;
import flash.net.LocalConnection;
import flash.utils.ByteArray;
import flash.utils.getTimer;

import kabam.lib.loopedprocs.LoopedCallback;
import kabam.lib.loopedprocs.LoopedProcess;
import kabam.rotmg.chat.view.Chat;
import kabam.rotmg.constants.GeneralConstants;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.core.model.MapModel;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.core.view.Layers;
import kabam.rotmg.dailyLogin.signal.ShowDailyCalendarPopupSignal;
import kabam.rotmg.dialogs.control.AddPopupToStartupQueueSignal;
import kabam.rotmg.dialogs.control.FlushPopupStartupQueueSignal;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.dialogs.model.DialogsModel;

import kabam.rotmg.game.view.AlertStatusDisplay;
import kabam.rotmg.game.view.DiscordButtonDisplay;
import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.game.view.LootboxModalButton;
import kabam.rotmg.game.view.RaidLauncherButton;
import kabam.rotmg.maploading.signals.HideMapLoadingSignal;
import kabam.rotmg.maploading.signals.MapLoadedSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.GameServerConnectionConcrete;
import kabam.rotmg.messaging.impl.incoming.MapInfo;
import kabam.rotmg.packages.services.PackageModel;
import kabam.rotmg.promotions.model.BeginnersPackageModel;
import kabam.rotmg.promotions.signals.ShowBeginnersPackageSignal;
import kabam.rotmg.servers.api.Server;
import kabam.rotmg.stage3D.Renderer;
import kabam.rotmg.ui.UIUtils;
import kabam.rotmg.ui.signals.ShowHideKeyUISignal;
import kabam.rotmg.ui.view.HUDView;

import org.osflash.signals.Signal;

public class GameSprite extends Sprite {
    protected static const PAUSED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);

    public const monitor:Signal = new Signal(String, int);

    public const modelInitialized:Signal = new Signal();
    public const drawCharacterWindow:Signal = new Signal(Player);

    public var chatBox_:Chat;
    public var idleWatcher_:IdleWatcher;
    public var rankText_:RankText;
    public var guildText_:GuildText;
    public var creditDisplay_:CreditDisplay;
    public var alertStatusDisplay:AlertStatusDisplay;
    public var raidLauncherButton:RaidLauncherButton;
    public var discordButton:DiscordButtonDisplay;
    public var lootBoxButton:LootboxModalButton;
    private var send:Boolean = false;

    public var mapModel:MapModel;
    public var beginnersPackageModel:BeginnersPackageModel;
    public var dialogsModel:DialogsModel;
    public var showBeginnersPackage:ShowBeginnersPackageSignal;
    public var openDailyCalendarPopupSignal:ShowDailyCalendarPopupSignal;
    public var openDialog:OpenDialogSignal;
    public var showPackage:Signal;
    public var packageModel:PackageModel;
    public var addToQueueSignal:AddPopupToStartupQueueSignal;
    public var flushQueueSignal:FlushPopupStartupQueueSignal;
    private var focus:GameObject;
    private var isGameStarted:Boolean;
    private var displaysPosY:uint = 4;
    private var currentPackage:DisplayObject;
    public var chatPlayerMenu:PlayerMenu;

    public const closed:Signal = new Signal();

    public var isEditor:Boolean;
    public var mui_:MapUserInput;
    public var lastUpdate_:int;
    public var moveRecords_:MoveRecords;
    public var map:Map;
    public var model:PlayerModel;
    public var hudView:HUDView;
    public var camera_:Camera;
    public var gsc_:GameServerConnection;


    public function GameSprite(_arg1:Server, _arg2:int, _arg3:Boolean, _arg4:int, _arg5:int, _arg6:ByteArray, _arg7:PlayerModel, _arg8:String, _arg9:Boolean) {
        this.moveRecords_ = new MoveRecords();
        this.camera_ = new Camera();
        this.showPackage = new Signal();
        this.currentPackage = new Sprite();
        super();
        this.model = _arg7;
        map = new Map(this);
        addChild(map);
        gsc_ = new GameServerConnectionConcrete(this, _arg1, _arg2, _arg3, _arg4, _arg5, _arg6, _arg8, _arg9);
        mui_ = new MapUserInput(this);
        this.chatBox_ = new Chat();
        this.chatBox_.list.addEventListener(MouseEvent.MOUSE_DOWN, this.onChatDown);
        this.chatBox_.list.addEventListener(MouseEvent.MOUSE_UP, this.onChatUp);
        addChild(this.chatBox_);
        this.idleWatcher_ = new IdleWatcher();
    }

    public static function dispatchMapLoaded(_arg1:MapInfo):void {
        var _local2:MapLoadedSignal = StaticInjectorContext.getInjector().getInstance(MapLoadedSignal);
        ((_local2) && (_local2.dispatch(_arg1)));
    }

    private static function hidePreloader():void {
        var _local1:HideMapLoadingSignal = StaticInjectorContext.getInjector().getInstance(HideMapLoadingSignal);
        ((_local1) && (_local1.dispatch()));
    }


    public function onChatDown(_arg1:MouseEvent):void {
        if (this.chatPlayerMenu != null) {
            this.removeChatPlayerMenu();
        }
        mui_.onMouseDown(_arg1);
    }

    public function onChatUp(_arg1:MouseEvent):void {
        mui_.onMouseUp(_arg1);
    }

    public function setFocus(_arg1:GameObject):void {
        _arg1 = ((_arg1) || (map.player_));
        this.focus = _arg1;
    }

    public function addChatPlayerMenu(_arg1:Player, _arg2:Number, _arg3:Number, _arg4:String = null, _arg5:Boolean = false, _arg6:Boolean = false):void {
        this.removeChatPlayerMenu();
        this.chatPlayerMenu = new PlayerMenu();
        if (_arg4 == null) {
            this.chatPlayerMenu.init(this, _arg1);
        }
        else {
            if (_arg6) {
                this.chatPlayerMenu.initDifferentServer(this, _arg4, _arg5, _arg6);
            }
            else {
                if ((((_arg4.length > 0)) && ((((((_arg4.charAt(0) == "#")) || ((_arg4.charAt(0) == "*")))) || ((_arg4.charAt(0) == "@")))))) {
                    return;
                }
                this.chatPlayerMenu.initDifferentServer(this, _arg4, _arg5);
            }
        }
        addChild(this.chatPlayerMenu);
        this.chatPlayerMenu.x = _arg2;
        this.chatPlayerMenu.y = (_arg3 - this.chatPlayerMenu.height);
    }

    public function removeChatPlayerMenu():void {
        if (this.chatPlayerMenu != null && this.chatPlayerMenu.parent != null) {
            removeChild(this.chatPlayerMenu);
            this.chatPlayerMenu = null;
        }
    }

    public function applyMapInfo(mInfo:MapInfo):void {
        map.setProps(mInfo.width_, mInfo.height_, mInfo.name_, mInfo.allowPlayerTeleport_);
        dispatchMapLoaded(mInfo);
    }

    public function hudModelInitialized():void {
        hudView = new HUDView();
        hudView.x = 600;
        addChild(hudView);
    }

    public function initialize():void {
        map.initialize();
        this.modelInitialized.dispatch();
        if (this.evalIsNotInCombatMapArea()) {
            this.showSafeAreaDisplays();
        }
        if (map.name_ == Map.NEXUS) {
            this.flushQueueSignal.dispatch();
        }
        if (this.evalIsNotInCombatMapArea()) {
            this.creditDisplay_ = new CreditDisplay(this, true, true);
        }
        else {
            this.creditDisplay_ = new CreditDisplay(this);
        }
        this.creditDisplay_.x = 594;
        this.creditDisplay_.y = 0;
        addChild(this.creditDisplay_);
        Parameters.save();
        hidePreloader();
        stage.dispatchEvent(new Event(Event.RESIZE));
        this.parent.parent.setChildIndex((this.parent.parent as Layers).top, 2);
    }

    private function showSafeAreaDisplays():void {
        this.showRankText();
        this.showGuildText();
        this.showAlertStatusDisplay();
        this.addDiscordButton();
        this.showRaidLauncher();
        this.showLootboxButton();
    }

    private function addDiscordButton():void {
        this.discordButton = new DiscordButtonDisplay(this);
        this.discordButton.x = 6;
        this.discordButton.y = (this.displaysPosY + 2);
        this.displaysPosY = (this.displaysPosY + UIUtils.NOTIFICATION_SPACE);
        addChild(this.discordButton);
    }

    private function showAlertStatusDisplay():void {
        this.alertStatusDisplay = new AlertStatusDisplay();
        this.alertStatusDisplay.x = 6;
        this.alertStatusDisplay.y = (this.displaysPosY + 2);
        this.displaysPosY = (this.displaysPosY + UIUtils.NOTIFICATION_SPACE);
        addChild(this.alertStatusDisplay);
    }

    private function showRaidLauncher():void {
        this.raidLauncherButton = new RaidLauncherButton();
        this.raidLauncherButton.x = 6;
        this.raidLauncherButton.y = (this.displaysPosY + 2);
        this.displaysPosY = (this.displaysPosY + UIUtils.NOTIFICATION_SPACE);
        addChild(this.raidLauncherButton);
    }


    private function showLootboxButton():void {
        this.lootBoxButton = new LootboxModalButton();
        this.lootBoxButton.x = this.raidLauncherButton.x + 32;
        this.lootBoxButton.y = this.raidLauncherButton.y;
        addChild(this.lootBoxButton);
    }

    private function showGuildText():void {
        this.guildText_ = new GuildText("", -1);
        this.guildText_.x = 64;
        this.guildText_.y = 6;
        addChild(this.guildText_);
    }

    private function showRankText():void {
        this.rankText_ = new RankText(-1, true, false);
        this.rankText_.x = 8;
        this.rankText_.y = this.displaysPosY;
        this.displaysPosY = (this.displaysPosY + UIUtils.NOTIFICATION_SPACE);
        addChild(this.rankText_);
    }

    private function updateNearestInteractive() : void
    {
        if(!this.map || !this.map.player_) return;
        var dist:Number = NaN;
        var go:GameObject = null;
        var iObj:IInteractiveObject = null;
        var player:Player = this.map.player_;
        var minDist:Number = GeneralConstants.MAXIMUM_INTERACTION_DISTANCE;
        var closestInteractive:IInteractiveObject = null;
        var playerX:Number = player.x_;
        var playerY:Number = player.y_;
        for each(go in this.map.goDict_)
        {
            iObj = go as IInteractiveObject;
            if(iObj)
            {
                if(Math.abs(playerX - go.x_) < GeneralConstants.MAXIMUM_INTERACTION_DISTANCE || Math.abs(playerY - go.y_) < GeneralConstants.MAXIMUM_INTERACTION_DISTANCE)
                {
                    dist = PointUtil.distanceXY(go.x_,go.y_,playerX,playerY);
                    if(dist < GeneralConstants.MAXIMUM_INTERACTION_DISTANCE && dist < minDist)
                    {
                        minDist = dist;
                        closestInteractive = iObj;
                    }
                }
            }
        }
        this.mapModel.currentInteractiveTarget = closestInteractive;
    }

    public function onScreenResize(event:Event):void
    {
        var scaleW:Number = (800 / stage.stageWidth);
        var scaleH:Number = (600 / stage.stageHeight);
        var mscale:Number = Parameters.data_["mscale"];

        if (this.map != null)
        {
            this.map.scaleX = scaleW * mscale;
            this.map.scaleY = scaleH * mscale;
        }

        if (this.hudView != null)
        {
            this.hudView.scaleX = scaleW / scaleH;
            this.hudView.scaleY = 1;
            this.hudView.y = 0;
            this.hudView.x = (800 - (200 * this.hudView.scaleX));
            if (this.creditDisplay_ != null)
            {
                this.creditDisplay_.x = (this.hudView.x - (6 * this.creditDisplay_.scaleX));
            }
        }
        if (this.chatBox_ != null)
        {
            this.chatBox_.scaleX = scaleW;
            this.chatBox_.scaleY = scaleH;
            this.chatBox_.y = 300 + 300 * (1 - this.chatBox_.scaleY);
        }
        if (this.rankText_ != null)
        {
            this.rankText_.scaleX = scaleW;
            this.rankText_.scaleY = scaleH;
            this.rankText_.x = 8 * this.rankText_.scaleX;
            this.rankText_.y = 2 * this.rankText_.scaleY;
        }
        if (this.guildText_ != null)
        {
            this.guildText_.scaleX = scaleW;
            this.guildText_.scaleY = scaleH;
            this.guildText_.x = 64 * this.guildText_.scaleX;
            this.guildText_.y = 2 * this.guildText_.scaleY;
        }
        if (this.creditDisplay_ != null)
        {
            this.creditDisplay_.scaleX = scaleW;
            this.creditDisplay_.scaleY = scaleH;
        }

        if (this.discordButton != null) {
            this.discordButton.scaleX = scaleW;
            this.discordButton.scaleY = scaleH;
            this.discordButton.x = 6 * this.discordButton.scaleX;
            this.discordButton.y = 62 * this.discordButton.scaleY;
        }

        if (this.alertStatusDisplay != null) {
            this.alertStatusDisplay.scaleX = scaleW;
            this.alertStatusDisplay.scaleY = scaleH;
            this.alertStatusDisplay.x = 6 * this.alertStatusDisplay.scaleX;
            this.alertStatusDisplay.y = 34 * this.alertStatusDisplay.scaleY;
        }

        if (this.lootBoxButton != null) {
            this.lootBoxButton.scaleX = scaleW;
            this.lootBoxButton.scaleY = scaleH;
            this.lootBoxButton.x = 38 * this.lootBoxButton.scaleX;
            this.lootBoxButton.y = 90 * this.lootBoxButton.scaleY;
        }

        if (this.raidLauncherButton != null) {
            this.raidLauncherButton.scaleX = scaleW;
            this.raidLauncherButton.scaleY = scaleH;
            this.raidLauncherButton.x = 6 * this.raidLauncherButton.scaleX;
            this.raidLauncherButton.y = 90 * this.raidLauncherButton.scaleY;
        }
    }

    public function connect():void
    {
        if (!this.isGameStarted)
        {
            this.isGameStarted = true;
            Renderer.inGame = true;
            gsc_.connect();
            lastUpdate_ = getTimer();
            this.idleWatcher_.start(this);
            stage.addEventListener(MoneyChangedEvent.MONEY_CHANGED, this.onMoneyChanged);
            stage.addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
            this.parent.parent.setChildIndex((this.parent.parent as Layers).top, 0);
            if (Parameters.data_.mscale == undefined)
            {
                Parameters.data_.mscale = "1.0";
            }
            if (Parameters.data_.uiscale == undefined)
            {
                Parameters.data_.uiscale = true;
            }
            Parameters.save();
            stage.scaleMode = StageScaleMode.NO_SCALE;
            stage.addEventListener(Event.RESIZE, this.onScreenResize);
            stage.dispatchEvent(new Event(Event.RESIZE));
            LoopedProcess.addProcess(new LoopedCallback(100, this.updateNearestInteractive));
        }
    }

    public function disconnect():void
    {
        if (this.isGameStarted)
        {
            this.isGameStarted = false;
            Renderer.inGame = false;
            this.idleWatcher_.stop();
            stage.removeEventListener(MoneyChangedEvent.MONEY_CHANGED, this.onMoneyChanged);
            stage.removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
            stage.removeEventListener(Event.RESIZE, this.onScreenResize);
            stage.scaleMode = StageScaleMode.EXACT_FIT;
            stage.dispatchEvent(new Event(Event.RESIZE));
            LoopedProcess.destroyAll();
            contains(map) && removeChild(map);
            map.dispose();
            CachingColorTransformer.clear();
            TextureRedrawer.clearCache();
            Projectile.dispose();
            gsc_.disconnect();
        }
    }

    private function onMoneyChanged(event:Event):void {
        gsc_.checkCredits();
    }

    public function evalIsNotInCombatMapArea():Boolean {
        return map.name_ == Map.NEXUS || map.name_ == Map.VAULT || map.name_ == Map.GUILD_HALL || map.name_ == Map.CLOTH_BAZAAR || map.name_ == Map.TAVERN;
    }

    private function onEnterFrame(event:Event):void {
        var time:int = getTimer();
        var dt:int = time - lastUpdate_;
        if (this.idleWatcher_.update(dt)) {
            closed.dispatch();
            return;
        }
        LoopedProcess.runProcesses(time);
        map.update(time, dt);
        camera_.update(dt);
        var player:Player = map.player_;
        if (this.focus) {
            camera_.configureCamera(this.focus);
            map.draw(camera_, time);
        }
        if (player != null) {
            this.creditDisplay_.draw(player.credits_, player.fame_, player.onrane_, player.tokens_, player.kantos_);
            this.drawCharacterWindow.dispatch(player);
            if(this.evalIsNotInCombatMapArea()) {
                this.rankText_.draw(player.numStars_, player.rank_, player.admin_);
                this.guildText_.draw(player.guildName_, player.guildRank_);
                this.guildText_.x = this.rankText_.width + 16;
            }
            if (player.isPaused()) {
                hudView.filters = [PAUSED_FILTER];
                map.mouseEnabled = false;
                map.mouseChildren = false;
                hudView.mouseEnabled = false;
                hudView.mouseChildren = false;
            }
            else {
                if (hudView.filters.length > 0) {
                    hudView.filters = [];
                    map.mouseEnabled = true;
                    map.mouseChildren = true;
                    hudView.mouseEnabled = true;
                    hudView.mouseChildren = true;
                }
            }
            moveRecords_.addRecord(time, player.x_, player.y_);
        }
        lastUpdate_ = time;
    }
}
}