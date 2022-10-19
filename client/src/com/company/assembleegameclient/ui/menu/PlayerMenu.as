package com.company.assembleegameclient.ui.menu {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.GameObjectListItem;
import com.company.assembleegameclient.util.GuildUtil;
import com.company.util.AssetLibrary;

import flash.events.Event;
import flash.events.MouseEvent;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.chat.control.ShowChatInputSignal;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.reportmenu.ui.ReportWindow;
import kabam.rotmg.text.model.TextKey;

public class PlayerMenu extends Menu {
    public var gs_:GameSprite;
    public var playerName_:String;
    public var player_:Player;
    public var playerPanel_:GameObjectListItem;
    public var Background:SliceScalingBitmap;
    public var quitButton:SliceScalingButton;
    public var sendButton:SliceScalingButton;
    public var msg:String;


    public function PlayerMenu() {
        super(0x363636, 0xFFFFFF);
    }

    public function initDifferentServer(gs:GameSprite, name:String,
                                        isGuild:Boolean = false, isOther:Boolean = false):void {
        var option:MenuOption;
        this.gs_ = gs;
        this.playerName_ = name;
        this.player_ = null;
        this.yOffset -= 25;
        option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 21)
                , 0xFFFFFF, TextKey.PLAYERMENU_PM);
        option.addEventListener(MouseEvent.CLICK, this.onPrivateMessage);
        addOption(option);
        if (isGuild) {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 21)
                    , 0xFFFFFF, TextKey.PLAYERMENU_GUILDCHAT);
            option.addEventListener(MouseEvent.CLICK, this.onGuildMessage);
            addOption(option);
        }
        if (isOther) {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 7)
                    , 0xFFFFFF, TextKey.PLAYERMENU_TRADE);
            option.addEventListener(MouseEvent.CLICK, this.onTradeMessage);
            addOption(option);
        }
    }

    public function init(gs:GameSprite, plr:Player):void {
        var option:MenuOption;
        this.gs_ = gs;
        this.playerName_ = plr.name_;
        this.player_ = plr;
        this.playerPanel_ = new GameObjectListItem(0xB3B3B3, true, this.player_, false, true);
        this.yOffset += 7;
        addChild(this.playerPanel_);

        if ((this.gs_.map.allowPlayerTeleport() && this.player_.isTeleportEligible(this.player_))) {
            option = new TeleportMenuOption(this.gs_.map.player_);
            option.addEventListener(MouseEvent.CLICK, this.onTeleport);
            addOption(option);
        }

        if (this.gs_.map.player_.guildRank_ >= GuildUtil.OFFICER
                && plr.guildName_ == null || plr.guildName_.length == 0) {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 10)
                    , 0xFFFFFF, TextKey.PLAYERMENU_INVITE);
            option.addEventListener(MouseEvent.CLICK, this.onInvite);
            addOption(option);
        }
        var pId:int = this.player_.partyId_;
        var cId:int = this.gs_.map.player_.partyId_;
        if(!(pId != cId && pId != -1))
        {
            if(!(pId == cId && pId == -1))
            {
                if(pId == -1 && cId != -1)
                {
                    option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig",13),16777215,"Party");
                    option.addEventListener(MouseEvent.CLICK,this.onPartyAdd);
                    addOption(option);
                }
                else if(pId == cId)
                {
                    option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig",14),16777215,"Party");
                    option.addEventListener(MouseEvent.CLICK,this.onPartyKick);
                    addOption(option);
                }
            }
            else
            {
                option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig",13),16777215,"Party");
                option.addEventListener(MouseEvent.CLICK,this.onPartyMake);
                option.addEventListener(MouseEvent.CLICK,this.onPartyAdd);
                addOption(option);
            }
        }
        else
        {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig",14),16777215,"Party",false);
            option.alpha = 0.5;
            option.mouseEnabled = false;
            addOption(option);
        }

        if (!this.player_.starred_) {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterface2", 5)
                    , 0xFFFFFF, TextKey.PLAYERMENU_LOCK);
            option.addEventListener(MouseEvent.CLICK, this.onLock);
            addOption(option);
        } else {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterface2", 6)
                    , 0xFFFFFF, TextKey.PLAYERMENU_UNLOCK);
            option.addEventListener(MouseEvent.CLICK, this.onUnlock);
            addOption(option);
        }

        option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 7)
                , 0xFFFFFF, TextKey.PLAYERMENU_TRADE);
        option.addEventListener(MouseEvent.CLICK, this.onTrade);
        addOption(option);

        option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 10)
                , 0xFFFFFF, "Gamble");
        option.addEventListener(MouseEvent.CLICK, this.onGamble);
        addOption(option);

        option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 21)
                , 0xFFFFFF, TextKey.PLAYERMENU_PM);
        option.addEventListener(MouseEvent.CLICK, this.onPrivateMessage);
        addOption(option);

        if (this.player_.isFellowGuild_) {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 21)
                    , 0xFFFFFF, TextKey.PLAYERMENU_GUILDCHAT);
            option.addEventListener(MouseEvent.CLICK, this.onGuildMessage);
            addOption(option);
        }

        if (!this.player_.ignored_) {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 8)
                    , 0xFFFFFF, TextKey.FRIEND_BLOCK_BUTTON);
            option.addEventListener(MouseEvent.CLICK, this.onIgnore);
            addOption(option);
        } else {
            option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 9)
                    , 0xFFFFFF, TextKey.PLAYERMENU_UNIGNORE);
            option.addEventListener(MouseEvent.CLICK, this.onUnignore);
            addOption(option);
        }
        option = new MenuOption(AssetLibrary.getImageFromSet("lofiInterfaceBig", 9) //Report Button
                , 0xFFFFFF, TextKey.PLAYERMENU_REPORT);
        option.addEventListener(MouseEvent.CLICK, this.createReportMenu);
        addOption(option);
    }

    private function createReportMenu(e:Event):void{
        stage.addChild(new ReportWindow(playerName_));
    }


    private function onPrivateMessage(e:Event):void {
        var signal:ShowChatInputSignal = StaticInjectorContext.getInjector().getInstance(ShowChatInputSignal);
        signal.dispatch(true, (("/tell " + this.playerName_) + " "));
        remove();
    }

    private function onTradeMessage(e:Event):void {
        var signal:ShowChatInputSignal = StaticInjectorContext.getInjector().getInstance(ShowChatInputSignal);
        signal.dispatch(true, ("/trade " + this.playerName_));
        remove();
    }

    private function onGuildMessage(e:Event):void {
        var signal:ShowChatInputSignal = StaticInjectorContext.getInjector().getInstance(ShowChatInputSignal);
        signal.dispatch(true, "/g ");
        remove();
    }

    private function onPartyAdd(_arg1:Event):void {
        this.gs_.gsc_.playerText("/pinv " + this.playerName_);
        remove();
    }

    private function onPartyKick(_arg1:Event):void {
        this.gs_.gsc_.playerText("/pkick " + this.playerName_);
        remove();
    }

    private function onPartyMake(_arg1:Event):void {
        this.gs_.gsc_.playerText("/pmake");
        remove();
    }
    private function onTeleport(e:Event):void {
        this.gs_.map.player_.teleportTo(this.player_);
        remove();
    }

    private function onInvite(e:Event):void {
        this.gs_.gsc_.guildInvite(this.playerName_);
        remove();
    }

    private function onLock(_arg1:Event):void {
        this.gs_.map.party_.lockPlayer(this.player_);
        remove();
    }

    private function onUnlock(e:Event):void {
        this.gs_.map.party_.unlockPlayer(this.player_);
        remove();
    }

    private function onTrade(e:Event):void {
        this.gs_.gsc_.requestTrade(this.playerName_);
        remove();
    }

    private function onGamble(e:Event):void {
        this.gs_.gsc_.requestGamble(this.playerName_, 1000);
        remove();
    }

    private function onIgnore(e:Event):void {
        this.gs_.map.party_.ignorePlayer(this.player_);
        remove();
    }

    private function onUnignore(e:Event):void {
        this.gs_.map.party_.unignorePlayer(this.player_);
        remove();
    }
}
}
