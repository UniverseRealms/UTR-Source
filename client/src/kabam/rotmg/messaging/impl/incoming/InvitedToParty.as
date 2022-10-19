package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class InvitedToParty extends IncomingMessage {

    public var name_:String;
    public var partyId_:int;

    public function InvitedToParty(_arg1:uint, _arg2:Function) {
        super(_arg1, _arg2);
    }

    override public function parseFromInput(_arg1:IDataInput):void {
        this.name_ = _arg1.readUTF();
        this.partyId_ = _arg1.readInt();
    }

    override public function toString():String {
        return (formatToString("INVITEDTOPARTY", "name_", "partyId_"));
    }


}
}
