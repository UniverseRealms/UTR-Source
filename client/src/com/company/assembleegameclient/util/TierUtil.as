package com.company.assembleegameclient.util
{
import com.company.assembleegameclient.misc.DefaultLabelFormat;
import com.company.assembleegameclient.misc.UILabel;
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;

public class TierUtil
   {
       
      
      public function TierUtil()
      {
         super();
      }
      
      public static function getTierTag(_arg_1:XML, _arg_2:int = 12) : UILabel
      {
         var _local_9:UILabel = null;
         var _local_10:Number = NaN;
         var _local_11:String = null;
         var _local_3:* = isPet(_arg_1) == false;
         var _local_4:* = _arg_1.hasOwnProperty("Consumable") == false;
         var potion:* = _arg_1.hasOwnProperty("Potion") == false;
         var InvUse:* = _arg_1.hasOwnProperty("InvUse") == false;
         var PetUse:* = _arg_1.hasOwnProperty("PetUse") == false;
         var _local_6:* = _arg_1.hasOwnProperty("Treasure") == false;
         var _local_7:* = _arg_1.hasOwnProperty("PetFood") == false;
         var _local_8:Boolean = _arg_1.hasOwnProperty("Tier");
         if(_local_3 && _local_4 && _local_6 && _local_7 && potion && InvUse && PetUse)
         {
            _local_9 = new UILabel();
            if(_local_8)
            {
               _local_10 = 16777215;
               _local_11 = "T" + _arg_1.Tier;
            }
            else if(_arg_1.hasOwnProperty("@setType"))
            {
               _local_10 = TooltipHelper.SET_COLOR;
               _local_11 = "ST";
            }
            else if (_arg_1.hasOwnProperty("Heroic"))
            {
               _local_10 = TooltipHelper.HEROIC_COLOR;
               _local_11 = "H";
            }
            else if (_arg_1.hasOwnProperty("Ancestral"))
            {
               _local_10 = TooltipHelper.ANCESTRAL_COLOR;
               _local_11 = "A";
            }
            else if (_arg_1.hasOwnProperty("Sacred"))
            {
               _local_10 = TooltipHelper.SACRED_COLOR;
               _local_11 = "S";
            }
            else if (_arg_1.hasOwnProperty("Eternal"))
            {
               _local_10 = TooltipHelper.ETERNAL_COLOR;
               _local_11 = "E";
            }
            else if (_arg_1.hasOwnProperty("Divine"))
            {
                _local_10 = TooltipHelper.DIVINE_COLOR;
                _local_11 = "D";
            }
            else if (_arg_1.hasOwnProperty("RT"))
            {
                _local_10 = TooltipHelper.ETERNAL_COLOR;
                _local_11 = "R";
            }
            else if (_arg_1.hasOwnProperty("Awakening"))
            {
                _local_10 = TooltipHelper.AWAKENING_COLOR;
                _local_11 = "A";
            }
            else if (_arg_1.hasOwnProperty("QuestItem"))
            {
                _local_10 = TooltipHelper.QUEST_COLOR;
                _local_11 = "QT";
            }
            else
            {
               _local_10 = TooltipHelper.UNTIERED_COLOR;
               _local_11 = "UT";
            }
            _local_9.text = _local_11;
            DefaultLabelFormat.tierLevelLabel(_local_9,_arg_2,_local_10);
            return _local_9;
         }
         return null;
      }
      
      public static function isPet(itemDataXML:XML) : Boolean
      {
         var activateTags:XMLList = null;
         activateTags = itemDataXML.Activate.(text() == "PermaPet");
         return activateTags.length() >= 1;
      }
   }
}
