package io.decagames.rotmg.shop
{
   import flash.display.Sprite;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class ShopBoxTag extends Sprite
   {
      
      public static const NEW_TAG:String = "NEW_TAG";
      
      public static const HOT_TAG:String = "HOT_TAG";
      
      public static const BEST_TAG:String = "BEST_TAG";
      
      public static const LEFT_TAG:String = "LEFT_TAG";
      
      public static const PROMOTION_TAG:String = "PROMOTION_TAG";
      
      public static const BLUE_TAG:String = "shop_blue_tag";
      
      public static const ORANGE_TAG:String = "shop_orange_tag";
      
      public static const GREEN_TAG:String = "shop_green_tag";
      
      public static const PURPLE_TAG:String = "shop_purple_tag";
      
      public static const RED_TAG:String = "shop_red_tag";
       
      
      private var background:SliceScalingBitmap;
      
      private var _color:String;
      
      private var _label:UILabel;
      
      private var _name:String;
      
      public function ShopBoxTag(param1:String, param2:String, param3:String, param4:Boolean = false)
      {
         super();
         this._color = param2;
         this._name = param1;
         this.background = TextureParser.instance.getSliceScalingBitmap("UI",param2);
         this.background.scaleType = SliceScalingBitmap.SCALE_TYPE_9;
         addChild(this.background);
         this._label = new UILabel();
         this._label.autoSize = "left";
         this._label.text = param3;
         this._label.x = 4;
         if(param4)
         {
            DefaultLabelFormat.popupTag(this._label);
         }
         else
         {
            DefaultLabelFormat.shopTag(this._label);
         }
         addChild(this._label);
         this.background.width = this._label.textWidth + 8;
         this.background.height = this._label.textHeight + 8;
      }
      
      public function updateLabel(param1:String) : void
      {
         this._label.text = param1;
         this.background.width = this._label.textWidth + 8;
         this.background.height = this._label.textHeight + 8;
      }
      
      public function dispose() : void
      {
         this.background.dispose();
      }
      
      public function get color() : String
      {
         return this._color;
      }
      
      public function get tagName() : String
      {
         return this._name;
      }
   }
}
