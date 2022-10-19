package com.company.assembleegameclient.ui.tooltip.slotcomparisons {
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class ShieldComparison extends SlotComparison {

    private var projectileComparison:GeneralProjectileComparison;

    public function ShieldComparison() {
        this.projectileComparison = new GeneralProjectileComparison();
        super();
    }

    override protected function compareSlots(_arg1:XML, _arg2:XML):void {
        var _local3:String;
        this.projectileComparison.compare(_arg1, _arg2);
        comparisonStringBuilder = this.projectileComparison.comparisonStringBuilder;
        for (_local3 in this.projectileComparison.processedTags) {
            processedTags[_local3] = this.projectileComparison.processedTags[_local3];
        }
    }
}
}
