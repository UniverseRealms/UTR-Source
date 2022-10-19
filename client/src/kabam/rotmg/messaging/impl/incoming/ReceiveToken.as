package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class ReceiveToken extends IncomingMessage {
    public function ReceiveToken(_arg1:uint, _arg2:Function) {
        this.token_ = "";
        super (_arg1, _arg2);
    }

    public var token_:String;

    override public function parseFromInput(data:IDataInput):void {
        this.token_ = data.readUTF();
    }

    override public function toString():String {
        return (formatToString("RECEIVETOKEN", "token_"));
    }
}
}