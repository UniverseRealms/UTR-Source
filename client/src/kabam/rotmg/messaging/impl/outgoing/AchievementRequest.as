package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class AchievementRequest extends OutgoingMessage {

    public var Name:int;

    public function AchievementRequest(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(this.Name);
    }

    override public function toString():String {
        return (formatToString("ACHIEVEMENT", "Name"));
    }


}
}
