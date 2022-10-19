package kabam.rotmg.pets.view.components.slot {
import com.company.util.MoreColorUtil;

import flash.events.MouseEvent;
import flash.filters.ColorMatrixFilter;

import kabam.rotmg.text.model.TextKey;

import org.osflash.signals.Signal;

public class PetFeedFuseSlot extends FeedFuseSlot {

    public const openPetPicker:Signal = new Signal();

    public var processing:Boolean = false;
    private var grayscaleMatrix:ColorMatrixFilter;

    public function PetFeedFuseSlot() {
        this.grayscaleMatrix = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);
        super();
        addEventListener(MouseEvent.CLICK, this.onOpenPetPicker);
        this.updateTitle();
    }

    public function updateTitle():void {
        if (((!(icon)) || (contains(icon)))) {
            setTitle(TextKey.PETORFOODSLOT_FUSE_PET_TITLE, {});
        }
    }

    private function onOpenPetPicker(_arg1:MouseEvent):void {
        if (!this.processing) {
            this.openPetPicker.dispatch();
        }
    }


}
}
