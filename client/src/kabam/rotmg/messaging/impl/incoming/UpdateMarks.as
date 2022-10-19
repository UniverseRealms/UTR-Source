package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class UpdateMarks extends IncomingMessage {

    public var mark:int;
    public var node1:int;
    public var node2:int;
    public var node3:int;
    public var node4:int;

    public function UpdateMarks(param1:uint, param2:Function)
    {
        super(param1,param2);
    }

    override public function parseFromInput(param1:IDataInput) : void
    {
        this.mark = param1.readInt();
        this.node1 = param1.readInt();
        this.node2 = param1.readInt();
        this.node3 = param1.readInt();
        this.node4 = param1.readInt();
    }

    override public function toString() : String
    {
        return formatToString("UPDATEMARKS","mark","node1", "node2", "node3","node4");
    }
}
}