package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class BuySkill extends OutgoingMessage {

    public var skillId_:int;

    public function BuySkill(val:uint, func:Function) {
        super(val, func);
    }

    override public function writeToOutput(iDataOutput:IDataOutput):void {
        iDataOutput.writeInt(this.skillId_);
    }

    override public function toString():String {
        return (formatToString("BUY_SKILL", "skillId_"));
    }


}
}
