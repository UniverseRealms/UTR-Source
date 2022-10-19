package kabam.rotmg.ui.signals {
import com.hurlant.util.asn1.parser.boolean;

import org.osflash.signals.Signal;

public class ShowHideKeyUISignal extends Signal {

    public static var instance:ShowHideKeyUISignal;

    public function ShowHideKeyUISignal() {
        instance = this
    }

}
}
