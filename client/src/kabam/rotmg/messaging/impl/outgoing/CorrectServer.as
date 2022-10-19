package kabam.rotmg.messaging.impl.outgoing {

import com.company.assembleegameclient.objects.Player;

import flash.utils.IDataOutput;

public class CorrectServer extends OutgoingMessage {

    public function CorrectServer(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(Player.correctServer);
    }

    override public function toString():String {
        return (formatToString(
                "CORRECTSERVER", "Player.correctServer"
        ));
    }

}
}//package _0A_g
