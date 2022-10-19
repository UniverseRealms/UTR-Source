package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class BountyRequest extends OutgoingMessage {

    public var BountyId:int;

    public function BountyRequest(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.BountyId);
    }

    override public function toString():String {
        return (formatToString("BOUNTYREQUEST", "BountyId"));
    }


}
}
