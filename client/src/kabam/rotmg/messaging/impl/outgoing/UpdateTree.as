package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class UpdateTree extends OutgoingMessage {

    public function UpdateTree(val:uint, func:Function) {
        super(val, func);
    }

    override public function writeToOutput(iDataOutput:IDataOutput):void {
    }

    override public function toString():String {
        return (formatToString("UPDATE_TREE"));
    }


}
}
