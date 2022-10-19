package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import io.decagames.rotmg.ui.gird.UIGridElement;
   
   public class ItemsSetBox extends UIGridElement
   {
       
      
      private var items:Vector.<ItemBox>;
      
      public function ItemsSetBox(param1:Vector.<ItemBox>)
      {
         var _loc3_:* = null;
         super();
         this.items = param1;
         var _loc2_:int = 0;
         var _loc5_:int = 0;
         var _loc4_:* = param1;
         for each(_loc3_ in param1)
         {
            _loc3_.y = _loc2_;
            addChild(_loc3_);
            _loc2_ = _loc2_ + _loc3_.height;
         }
         this.drawBackground(260);
      }
      
      private function drawBackground(param1:int) : void
      {
         this.graphics.clear();
         this.graphics.lineStyle(1,10915138);
         this.graphics.beginFill(2960685);
         this.graphics.drawRect(0,0,param1,this.items.length * 48);
         this.graphics.endFill();
      }
      
      override public function get height() : Number
      {
         return this.items.length * 48;
      }
      
      override public function resize(param1:int, param2:int = -1) : void
      {
         this.drawBackground(param1);
      }
      
      override public function dispose() : void
      {
         var _loc1_:* = null;
         var _loc3_:int = 0;
         var _loc2_:* = this.items;
         for each(_loc1_ in this.items)
         {
            _loc1_.dispose();
         }
         this.items = null;
         super.dispose();
      }
   }
}
