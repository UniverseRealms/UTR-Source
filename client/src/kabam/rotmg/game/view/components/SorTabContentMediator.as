package kabam.rotmg.game.view.components {
import flash.events.MouseEvent;

import kabam.lib.net.api.MessageProvider;
import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.outgoing.QoLAction;

public class SorTabContentMediator {
    [Inject]
    public var view:SorTabContent;
    [Inject]
    public var socketServer:SocketServer;
    [Inject]
    public var messages:MessageProvider;


    public function initialize():void {
        this.view.sorDisplay.addEventListener(MouseEvent.CLICK, this.onSorClick);
        this.view.airDisplay.addEventListener(MouseEvent.CLICK, this.onAirClick);
        this.view.waterDisplay.addEventListener(MouseEvent.CLICK, this.onWaterClick);
        this.view.earthDisplay.addEventListener(MouseEvent.CLICK, this.onEarthClick);
        this.view.fireDisplay.addEventListener(MouseEvent.CLICK, this.onFireClick);
    }

    protected function onSorClick(e:MouseEvent):void {
        var pkt:QoLAction;
        pkt = (this.messages.require(GameServerConnection.QOLACTION) as QoLAction);
        pkt.actionId_ = 1;
        this.socketServer.sendMessage(pkt);
    }

    protected function onAirClick(e:MouseEvent):void {
        var pkt:QoLAction;
        pkt = (this.messages.require(GameServerConnection.QOLACTION) as QoLAction);
        pkt.actionId_ = 2;
        this.socketServer.sendMessage(pkt);
    }

    protected function onWaterClick(e:MouseEvent):void {
        var pkt:QoLAction;
        pkt = (this.messages.require(GameServerConnection.QOLACTION) as QoLAction);
        pkt.actionId_ = 3;
        this.socketServer.sendMessage(pkt);
    }

    protected function onEarthClick(e:MouseEvent):void {
        var pkt:QoLAction;
        pkt = (this.messages.require(GameServerConnection.QOLACTION) as QoLAction);
        pkt.actionId_ = 4;
        this.socketServer.sendMessage(pkt);
    }

    protected function onFireClick(e:MouseEvent):void {
        var pkt:QoLAction;
        pkt = (this.messages.require(GameServerConnection.QOLACTION) as QoLAction);
        pkt.actionId_ = 5;
        this.socketServer.sendMessage(pkt);
    }
}
}
