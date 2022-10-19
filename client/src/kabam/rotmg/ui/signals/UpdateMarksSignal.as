package kabam.rotmg.ui.signals {
import org.osflash.signals.Signal;

public class UpdateMarksSignal extends Signal {

    public function UpdateMarksSignal() {
        super(int, int, int, int, int);
    }
}
}
