package kabam.rotmg.messaging.impl.incoming
{
import com.company.assembleegameclient.util.FreeList;

import flash.utils.IDataInput;


import kabam.rotmg.messaging.impl.data.SkillTreeData;

import kabam.rotmg.messaging.impl.incoming.IncomingMessage;

public class SkillTreeResult  extends IncomingMessage
{
    public var results_:Vector.<SkillTreeData>;

    public function SkillTreeResult(id:uint, callback:Function)
    {
        this.results_ = new Vector.<SkillTreeData>();
        super(id,callback);
    }

    override public function parseFromInput(data:IDataInput) : void
    {
        var i:int = 0;
        var len:int = data.readShort();
        for(i = len; i < this.results_.length; i++)
        {
            FreeList.deleteObject(this.results_[i]);
        }
        this.results_.length = Math.min(len,this.results_.length);
        while(this.results_.length < len)
        {
            this.results_.push(FreeList.newObject(SkillTreeData) as SkillTreeData);
        }
        for(i = 0; i < len; i++)
        {
            this.results_[i].parseFromInput(data);
            trace(this.results_[i].unlockedSkillsId_);
        }

    }

    override public function toString():String {
        return (formatToString("SKILL_TREE", "results_"));
    }


}
}
