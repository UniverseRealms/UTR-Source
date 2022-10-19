﻿package com.company.assembleegameclient.ui.tooltip
{
public class TooltipHelper
{

    public static const BETTER_COLOR:uint = 65280;

    public static const WORSE_COLOR:uint = 16711680;

    public static const NO_DIFF_COLOR:uint = 16777103;

    public static const WIS_BONUS_COLOR:uint = 4219875;

    public static const UNTIERED_COLOR:uint = 9055202;

    public static const SET_COLOR:uint = 16750848;

    public static const HEROIC_COLOR:uint = 0x10d000;

    public static const ANCESTRAL_COLOR:uint = 0x5080f0;

    public static const SACRED_COLOR:uint = 0xff9000;

    public static const ETERNAL_COLOR:uint = 0x351c75;

    public static const DIVINE_COLOR:uint = 0xf04040;

    public static const AWAKENING_COLOR:uint = 14423100;

    public static const QUEST_COLOR:uint = 6711039;

    public function TooltipHelper()
    {
        super();
    }

    public static function wrapInFontTag(_arg_1:String, _arg_2:String) : String
    {
        return "<font color=\"" + _arg_2 + "\">" + _arg_1 + "</font>";
    }

    public static function getOpenTag(_arg_1:uint) : String
    {
        return "<font color=\"#" + _arg_1.toString(16) + "\">";
    }

    public static function getCloseTag() : String
    {
        return "</font>";
    }

    public static function getFormattedRangeString(_arg_1:Number) : String
    {
        var _local_2:Number = _arg_1 - int(_arg_1);
        return int(_local_2 * 10) == 0?int(_arg_1).toString():_arg_1.toFixed(1);
    }

    public static function compareAndGetPlural(_arg_1:Number, _arg_2:Number, _arg_3:String, _arg_4:Boolean = true, _arg_5:Boolean = true) : String
    {
        return wrapInFontTag(getPlural(_arg_1,_arg_3),"#" + getTextColor((!!_arg_4?_arg_1 - _arg_2:_arg_2 - _arg_1) * int(_arg_5)).toString(16));
    }

    public static function compare(_arg_1:Number, _arg_2:Number, _arg_3:Boolean = true, _arg_4:String = "", _arg_5:Boolean = false, _arg_6:Boolean = true) : String
    {
        return wrapInFontTag((!!_arg_5?Math.abs(_arg_1):_arg_1) + _arg_4,"#" + getTextColor((!!_arg_3?_arg_1 - _arg_2:_arg_2 - _arg_1) * int(_arg_6)).toString(16));
    }

    public static function getPlural(_arg_1:Number, _arg_2:String) : String
    {
        var _local_3:String = _arg_1 + " " + _arg_2;
        if(_arg_1 != 1)
        {
            return _local_3 + "s";
        }
        return _local_3;
    }

    public static function getTextColor(_arg_1:Number) : uint
    {
        if(_arg_1 < 0)
        {
            return WORSE_COLOR;
        }
        if(_arg_1 > 0)
        {
            return BETTER_COLOR;
        }
        return NO_DIFF_COLOR;
    }
}
}
