package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class ClientOptionChanged extends OutgoingMessage {

    public var type_:int;
    public var value_:Boolean;
    public var ALLY_DAMAGE:int;
    public var ALLY_PROJECTILES:int;

    public function Escape(_arg1:uint, _arg2:Function) {
        ALLY_DAMAGE = 0;
        ALLY_PROJECTILES = 1;
        super(_arg1, _arg2);
    }

    override public function writeToOutput(_arg1:IDataOutput):void {
        _arg1.writeInt(type_);
        _arg1.writeBoolean(value_);
    }

    override public function toString():String {
        return (formatToString("ClientOptionChanged"));
    }


}
}
