package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

public class RequestToken extends OutgoingMessage {

    public var token_:String;
    public var isAir_:Boolean;

    public function RequestToken(_arg1:uint, _arg2:Function) {
        this.token_ = "";
        this.isAir_ = false;
        super (_arg1, _arg2);
    }

    override public function writeToOutput(data:IDataOutput):void {
        data.writeUTF(this.token_);
        data.writeBoolean(this.isAir_);
    }

    override public function toString():String {
        return (formatToString("REQUESTTOKEN", "token_", "isAir_"));
    }
}
}