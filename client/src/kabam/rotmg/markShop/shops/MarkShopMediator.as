package kabam.rotmg.markShop.shops {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.MarkRequest;
import kabam.rotmg.ui.signals.UpdateMarkTabSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class MarkShopMediator extends Mediator{

    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;
    [Inject]
    public var view:MarkShop;
    [Inject]
    private var gs:GameSprite;
    [Inject]
    public var updateMark:UpdateMarkTabSignal;

    override public function initialize():void {
        this.view.makeShopUI();
        this.view.buyButton.addEventListener(MouseEvent.CLICK, this.selectMark);
    }

    override public function destroy():void {
        super.destroy();
    }

    private function selectMark(e:MouseEvent) :void {
        SoundEffectLibrary.play("button_click");
        var data:MarkRequest = (this.messages.require(GameServerConnection.MARKREQUEST) as MarkRequest);
        data.slot_ = 0;
        data.id_ = this.view.selectedMark;
        data.type_ = 1;
        this.socketServer.sendMessage(data);
    }
}
}
