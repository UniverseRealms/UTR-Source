package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.TextButton;
import com.company.ui.SimpleText;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.Message;

import kabam.lib.net.impl.SocketServer;

import kabam.rotmg.core.signals.SetScreenSignal;

import kabam.rotmg.game.view.SorDisplay;
import kabam.rotmg.game.view.SorDisplayFire;
import kabam.rotmg.game.view.SorDisplayWater;
import kabam.rotmg.game.view.SorDisplayEarth;
import kabam.rotmg.game.view.SorDisplayAir;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.UpdateTree;
import kabam.rotmg.skilltree.ui.SkillTreeWindow;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.model.TabStripModel;

import org.osmf.net.httpstreaming.f4f.Box;

public class SkillTabContent extends Sprite {



    private var title:SimpleText;
    public var DecaUi:SliceScalingBitmap;
    public var ItemButton1:SliceScalingButton;
    public var quitButton:SliceScalingButton;
    public var DisableButtonUI:SliceScalingBitmap;
    public var SkillTreeUI:SkillTreeWindow;
    public var Open:Boolean;
    public var player:Player;
    public var gs_:GameSprite;
    private var socket:SocketServer;
    private var message:MessageProvider;
    public function SkillTabContent(plr:Player, gs:GameSprite, ss:SocketServer, msg:MessageProvider) {
        this.player = plr;
        this.gs_ = gs;
        this.socket = ss;
        this.message = msg;
        this.DecaUi = TextureParser.instance.getSliceScalingBitmap("UI", "shop_box_button_background");
        this.DecaUi.width = 120;
        this.DecaUi.x = 36;
        this.DecaUi.y = 46;
        addChild(this.DecaUi);

        this.ItemButton1 = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "generic_green_button"));
        this.ItemButton1.x = 50;
        this.ItemButton1.y = 50;
        this.ItemButton1.width = 92;
        this.ItemButton1.addEventListener(MouseEvent.CLICK, this.onClick);
        this.ItemButton1.setLabel("Open",DefaultLabelFormat.defaultModalTitle);
        addChild(this.ItemButton1);
        this.title = new SimpleText(20,11776947,false,0,0);
        this.title.text = "Skill Tree";
        this.title.setBold(true);
        this.title.x = 43;
        this.title.y = 10;
        addChild(this.title);
        super();
    }

    protected function onRequestUpdate() : void
    {
        var updateTree:UpdateTree = null;
        updateTree = this.message.require(GameServerConnection.UPDATE_TREE) as UpdateTree;
        this.socket.sendMessage(updateTree);
    }

    public function onClick(event:MouseEvent) : void {


        if(!this.Open) {
            this.SkillTreeUI = new SkillTreeWindow(this, this.gs_, this.socket, this.message);
            this.Open = true;
            this.ItemButton1.setLabel("Close", DefaultLabelFormat.defaultModalTitle);
            this.SkillTreeUI.initalize();
            //onRequestUpdate();
            stage.addChild(this.SkillTreeUI);
        }
        else
        {
            this.ItemButton1.setLabel("Open",DefaultLabelFormat.defaultModalTitle);
            this.Open = false;
            Close();
        }
    }
    public function Close() : void {
        stage.removeChild(this.SkillTreeUI);
    }
}
}
