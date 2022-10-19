package io.decagames.rotmg.dailyQuests.utils
{
   import flash.display.Sprite;
   import io.decagames.rotmg.dailyQuests.view.slot.DailyQuestItemSlot;
   
   public class SlotsRendered
   {
       
      
      public function SlotsRendered()
      {
         super();
      }
      
      public static function renderSlots(param1:Vector.<int>, param2:Vector.<int>, param3:String, param4:Sprite, param5:int, param6:int, param7:int, param8:Vector.<DailyQuestItemSlot>, param9:Boolean = false) : void
      {
         var _loc21_:int = 0;
         var _loc12_:* = null;
         var _loc20_:int = 0;
         var _loc23_:int = 0;
         _loc21_ = 0;
         var _loc16_:int = 0;
         var _loc14_:* = null;
         var _loc22_:int = 0;
         var _loc17_:int = 4;
         var _loc15_:int = 0;
         var _loc19_:int = 0;
         var _loc18_:int = 0;
         var _loc11_:Boolean = false;
         var _loc10_:Sprite = new Sprite();
         var _loc13_:Sprite = new Sprite();
         _loc12_ = _loc10_;
         param4.addChild(_loc10_);
         param4.addChild(_loc13_);
         _loc13_.y = 40 + param6;
         var _loc25_:int = 0;
         var _loc24_:* = param1;
         for each(_loc20_ in param1)
         {
            if(!_loc11_)
            {
               _loc19_++;
            }
            else
            {
               _loc18_++;
            }
            _loc16_ = param2.indexOf(_loc20_);
            if(_loc16_ >= 0)
            {
               param2.splice(_loc16_,1);
            }
            _loc14_ = new DailyQuestItemSlot(_loc20_,param3,param3 == "reward"?Boolean(false):Boolean(_loc16_ >= 0),param9);
            _loc14_.x = _loc22_ * (40 + param6);
            _loc12_.addChild(_loc14_);
            param8.push(_loc14_);
            _loc22_++;
            if(_loc22_ >= _loc17_)
            {
               _loc12_ = _loc13_;
               _loc22_ = 0;
               _loc11_ = true;
            }
         }
         _loc23_ = _loc19_ * 40 + (_loc19_ - 1) * param6;
         _loc21_ = _loc18_ * 40 + (_loc18_ - 1) * param6;
         param4.y = param5;
         if(!_loc11_)
         {
            param4.y = param4.y + Math.round(40 / 2 + param6 / 2);
         }
         _loc10_.x = Math.round((param7 - _loc23_) / 2);
         _loc13_.x = Math.round((param7 - _loc21_) / 2);
      }
   }
}
