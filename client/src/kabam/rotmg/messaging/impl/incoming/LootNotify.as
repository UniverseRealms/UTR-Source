package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class LootNotify extends IncomingMessage {

    public var bagType_:int;
    public var text:String;

    public function LootNotify(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function parseFromInput(_arg1:IDataInput):void {
        this.bagType_ = _arg1.readByte();
        this.text = _arg1.readUTF();
    }

    override public function toString():String {
        return (formatToString("LootNotify", "bagType_", "text"));
    }


}
}
