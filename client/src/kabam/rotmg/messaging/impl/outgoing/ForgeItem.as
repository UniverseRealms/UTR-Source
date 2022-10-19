package kabam.rotmg.messaging.impl.outgoing {
import flash.utils.IDataOutput;

import kabam.rotmg.messaging.impl.data.SlotObjectData;

public class ForgeItem extends OutgoingMessage {

    public function ForgeItem(_arg_1:uint, _arg_2:Function) {
        super(_arg_1, _arg_2);
    }

    public var top:SlotObjectData;
    public var bottom:SlotObjectData;


    override public function writeToOutput(_arg1:IDataOutput):void {
        this.top.writeToOutput(_arg1);
        this.bottom.writeToOutput(_arg1);

    }

    override public function toString():String {
        return formatToString("FORGEITEM", "top", "bottom");
    }
}
}
