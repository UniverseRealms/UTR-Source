package kabam.rotmg.game.commands {
import com.company.assembleegameclient.appengine.SavedCharacter;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;

import flash.utils.ByteArray;

import kabam.lib.net.impl.SocketServerModel;
import kabam.lib.tasks.TaskMonitor;
import kabam.rotmg.account.core.services.GetCharListTask;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.core.signals.SetScreenSignal;
import kabam.rotmg.game.model.GameInitData;
import kabam.rotmg.servers.api.Server;
import kabam.rotmg.servers.api.ServerModel;

public class PlayGameCommand {
    public static const RECONNECT_DELAY:int = 100;

    [Inject]
    public var setScreen:SetScreenSignal;
    [Inject]
    public var data:GameInitData;
    [Inject]
    public var model:PlayerModel;
    [Inject]
    public var servers:ServerModel;
    [Inject]
    public var task:GetCharListTask;
    [Inject]
    public var monitor:TaskMonitor;
    [Inject]
    public var socketServerModel:SocketServerModel;


    public function execute():void {
        if (!this.data.isNewGame) {
            this.socketServerModel.connectDelayMS = PlayGameCommand.RECONNECT_DELAY;
        }
        this.recordCharacterUseInSharedObject();
        this.makeGameView();
    }

    private function recordCharacterUseInSharedObject():void {
        Parameters.data_.charIdUseMap[this.data.charId] = new Date().getTime();
        Parameters.save();
    }

    private function makeGameView():void {
        var _local1:Server = ((this.data.server) || (this.servers.getServer()));
        var _local2:int = ((this.data.isNewGame) ? Parameters.NEXUS_GAMEID : this.data.gameId);
        var _local3:Boolean = this.data.createCharacter;
        var _local4:int = this.data.charId;
        var _local5:int = ((this.data.isNewGame) ? -1 : this.data.keyTime);
        var _local6:ByteArray = this.data.key;
        this.model.currentCharId = _local4;
        this.setScreen.dispatch(new GameSprite(_local1, _local2, _local3, _local4, _local5, _local6, this.model, null, this.data.isFromArena));
    }
}
}
