package kabam.rotmg.potionStorage.UI
{
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import flash.events.MouseEvent;
import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;
import kabam.rotmg.game.signals.AddTextLineSignal;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.UsePotion;

import org.swiftsuspenders.Injector;
import robotlegs.bender.bundles.mvcs.Mediator;

public class PotionStorageMediator extends Mediator
{


    [Inject]
    public var injector:Injector;

    [Inject]
    public var closeDialogs:CloseDialogsSignal;

    [Inject]
    public var socketServer:SocketServer;

    [Inject]
    public var messages:MessageProvider;

    [Inject]
    public var view:PotionStorage;

    [Inject]
    public var openNoModalDialog:OpenDialogNoModalSignal;

    [Inject]
    public var addTextLine:AddTextLineSignal;

    public function PotionStorageMediator()
    {
        super();
    }

    override public function initialize() : void
    {
        this.view.close.add(this.onCancel);
        this.view.ItemButton1.addEventListener(MouseEvent.CLICK,this.onConsume);
        this.view.ItemButton2.addEventListener(MouseEvent.CLICK,this.onConsume2);
        this.view.ItemButton3.addEventListener(MouseEvent.CLICK,this.onConsume3);
        this.view.ItemButton4.addEventListener(MouseEvent.CLICK,this.onConsume4);
        this.view.ItemButton5.addEventListener(MouseEvent.CLICK,this.onConsume5);
        this.view.ItemButton6.addEventListener(MouseEvent.CLICK,this.onConsume6);
        this.view.ItemButton7.addEventListener(MouseEvent.CLICK,this.onConsume7);
        this.view.ItemButton8.addEventListener(MouseEvent.CLICK,this.onConsume8);
        this.view.ItemButton9.addEventListener(MouseEvent.CLICK,this.onConsume9);
        this.view.ItemButton10.addEventListener(MouseEvent.CLICK,this.onConsume10);
    }

    protected function onConsume(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 1;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume2(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 2;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume3(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 3;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume4(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 4;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume5(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 5;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume6(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 6;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume7(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 7;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume8(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 8;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume9(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 9;
        this.socketServer.sendMessage(usepotion);
    }

    protected function onConsume10(me:MouseEvent) : void
    {
        var usepotion:UsePotion = null;
        usepotion = this.messages.require(GameServerConnection.USE_POTION) as UsePotion;
        usepotion.itemId_ = 10;
        this.socketServer.sendMessage(usepotion);
    }

    override public function destroy() : void
    {
        this.view.close.remove(this.onCancel);
    }

    private function onCancel() : void
    {
        SoundEffectLibrary.play("button_click");
        this.closeDialogs.dispatch();
    }
}
}
