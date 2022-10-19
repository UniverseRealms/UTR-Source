package kabam.rotmg.skilltree.signal {
import kabam.rotmg.messaging.impl.incoming.SkillTreeResult;


import org.osflash.signals.Signal;

public class SkillTreeSignal extends Signal
{
    public static var instance:SkillTreeSignal;

    public function SkillTreeSignal()
    {
        super(SkillTreeResult);
        instance = this;
    }
}
}
