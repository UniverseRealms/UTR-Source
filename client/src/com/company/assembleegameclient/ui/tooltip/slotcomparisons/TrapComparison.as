package com.company.assembleegameclient.ui.tooltip.slotcomparisons {
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class TrapComparison extends SlotComparison {


    private function getTrapTag(xml:XML):XML {
        var matches:XMLList;
        matches = xml.Activate.(text() == "Trap");
        if (matches.length() >= 1) {
            return (matches[0]);
        }
        return (null);
    }

    override protected function compareSlots(itemXML:XML, curItemXML:XML):void {
        var trap:XML;
        var otherTrap:XML;
        var tag:XML;
        var radius:Number;
        var otherRadius:Number;
        var damage:int;
        var otherDamage:int;
        var duration:int;
        var otherDuration:int;
        var avg:Number;
        var otherAvg:Number;
        var textColor:uint;
        var rof:int;
        var otherRof:int;



        trap = this.getTrapTag(itemXML);
        otherTrap = this.getTrapTag(curItemXML);
        comparisonStringBuilder = new AppendingLineBuilder();
        if (((!((trap == null))) && (!((otherTrap == null))))) {
            /*if (itemXML.@id == "Coral Venom Trap") {
                tag = itemXML.Activate.(text() == "Trap")[0];
                comparisonStringBuilder.pushParams(TextKey.TRAP, {
                    "data": new LineBuilder().setParams(TextKey.HP_WITHIN_SQRS, {
                        "amount": tag.@totalDamage,
                        "range": tag.@radius
                    }).setPrefix(TooltipHelper.getOpenTag(UNTIERED_COLOR)).setPostfix(TooltipHelper.getCloseTag())
                });
                comparisonStringBuilder.pushParams(TextKey.EFFECT_FOR_DURATION, {
                    "effect": new LineBuilder().setParams(TextKey.CONDITION_EFFECT_PARALYZED),
                    "duration": tag.@condDuration
                }, TooltipHelper.getOpenTag(UNTIERED_COLOR), TooltipHelper.getCloseTag());
                processedTags[tag.toXMLString()] = true;
            }
            if (false) { //itemXML.@id == "Devil's Snare Trap") { // this custom tooltip doesn't work
                tag = itemXML.Activate.(text() == "ParalyzeTrap") [0];
                comparisonStringBuilder.pushParams(TextKey.TRAP, {
                    "data": new LineBuilder().setParams(TextKey.HP_WITHIN_SQRS, {
                        "amount": tag.@totalDamage,
                        "range": tag.@radius
                    }).setPrefix(TooltipHelper.getOpenTag(UNTIERED_COLOR)).setPostfix(TooltipHelper.getCloseTag())
                });
                comparisonStringBuilder.pushParams(TextKey.EFFECT_FOR_DURATION, {
                    "effect": new LineBuilder().setParams(TextKey.CONDITION_EFFECT_PARALYZED),
                    "duration": tag.@condDuration
                }, TooltipHelper.getOpenTag(UNTIERED_COLOR), TooltipHelper.getCloseTag());
                processedTags[tag.toXMLString()] = true;
            }
            if (itemXML.@id == "Dragon-Snap Trap") {
                tag = itemXML.Activate.(text() == "Trap") [0];
                comparisonStringBuilder.pushParams(TextKey.TRAP, {
                    "data": new LineBuilder().setParams(TextKey.HP_WITHIN_SQRS, {
                        "amount": tag.@totalDamage,
                        "range": tag.@radius
                    }).setPrefix(TooltipHelper.getOpenTag(UNTIERED_COLOR)).setPostfix(TooltipHelper.getCloseTag())
                });
                comparisonStringBuilder.pushParams(TextKey.EFFECT_FOR_DURATION, {
                    "effect": new LineBuilder().setParams(TextKey.CONDITION_EFFECT_PARALYZED),
                    "duration": tag.@condDuration
                }, TooltipHelper.getOpenTag(UNTIERED_COLOR), TooltipHelper.getCloseTag());
                processedTags[tag.toXMLString()] = true;
            }
            else {*/
                radius = Number(trap.@radius);
                otherRadius = Number(otherTrap.@radius);
                damage = int(trap.@totalDamage);
                otherDamage = int(otherTrap.@totalDamage);
                duration = int(trap.@condDuration);
                otherDuration = int(otherTrap.@condDuration);
                rof = int(trap.@shotPerMS);
                otherRof = int(otherTrap.@shotPerMS);
                //avg = (((0.33 * radius) + (0.33 * damage)) + (0.33 * duration)) / rof;
                //otherAvg = (((0.33 * otherRadius) + (0.33 * otherDamage)) + (0.33 * otherDuration)) / otherRof;
                avg = (damage * (1000/ rof)); //Calculate only DPS
                otherAvg = (otherDamage * (1000 / otherRof));
                textColor = getTextColor((avg - otherAvg));
                comparisonStringBuilder.pushParams(TextKey.TRAP, {
                    "data": new AppendingLineBuilder().pushParams("Attacks {every} times per second\n" +
                            "For {damage} per tick\n" +
                            "Within {radius} range",
                            {
                                "every": 1000 / trap.@shotPerMS,
                                "damage": trap.@totalDamage,
                                "radius": trap.@radius
                    })//.setPrefix(TooltipHelper.getOpenTag(textColor)).setPostfix(TooltipHelper.getCloseTag())
                });



                comparisonStringBuilder.pushParams(TextKey.EFFECT_FOR_DURATION, {
                    "effect": trap.@condEffect,
                    "duration": trap.@condDuration
                }, TooltipHelper.getOpenTag(textColor), TooltipHelper.getCloseTag());
                processedTags[trap.toXMLString()] = true;
            //}
        }
    }


}
}
