package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.filters.DropShadowFilter;
   import flash.text.TextFormat;
   import io.decagames.rotmg.ui.gird.UIGridElement;
   import io.decagames.rotmg.ui.labels.UILabel;
   import kabam.rotmg.text.model.FontModel;
   
   public class UIItemContainer extends UIGridElement
   {
       
      
      private var _itemId:int;
      
      private var size:int;
      
      private var background:uint;
      
      private var backgroundAlpha:Number;
      
      private var _imageBitmap:Bitmap;
      
      private var _quantity:int = 1;
      
      private var _showTooltip:Boolean;
      
      public function UIItemContainer(param1:int, param2:uint, param3:Number = 0.0, param4:int = 40)
      {
         super();
         this._itemId = param1;
         this.size = param4;
         this.background = param2;
         this.backgroundAlpha = param3;
         this.graphics.clear();
         this.graphics.beginFill(param2,param3);
         this.graphics.drawRect(0,0,param4,param4);
         this.graphics.endFill();
         var _loc5_:BitmapData = ObjectLibrary.getRedrawnTextureFromType(int(param1),param4 * 2,true,false);
         this._imageBitmap = new Bitmap(_loc5_);
         this._imageBitmap.x = -Math.round((this._imageBitmap.width - param4) / 2);
         this._imageBitmap.y = -Math.round((this._imageBitmap.height - param4) / 2);
         this.addChild(this._imageBitmap);
      }
      
      override public function dispose() : void
      {
         this._imageBitmap.bitmapData.dispose();
         super.dispose();
      }
      
      public function showQuantityLabel(param1:int) : void
      {
         var _loc2_:* = null;
         this._quantity = param1;
         _loc2_ = new UILabel();
         var _loc3_:TextFormat = new TextFormat();
         _loc3_.color = 16777215;
         _loc3_.bold = true;
         _loc3_.font = FontModel.DEFAULT_FONT_NAME;
         _loc3_.size = this.size * 0.35;
         _loc2_.defaultTextFormat = _loc3_;
         _loc2_.text = param1 + "x";
         _loc2_.y = this.size - _loc2_.textHeight - this.size * 0.1;
         _loc2_.x = this.size - _loc2_.textWidth - this.size * 0.1;
         _loc2_.filters = [new DropShadowFilter(1,90,0,0.5,4,4)];
         addChild(_loc2_);
      }
      
      public function clone() : UIItemContainer
      {
         var _loc1_:UIItemContainer = new UIItemContainer(this._itemId,this.background,this.backgroundAlpha,this.size);
         if(this._quantity > 1)
         {
            _loc1_.showQuantityLabel(this._quantity);
         }
         return _loc1_;
      }
      
      public function get itemId() : int
      {
         return this._itemId;
      }
      
      override public function get width() : Number
      {
         return this.size;
      }
      
      override public function get height() : Number
      {
         return this.size;
      }
      
      public function get imageBitmap() : Bitmap
      {
         return this._imageBitmap;
      }
      
      public function get showTooltip() : Boolean
      {
         return this._showTooltip;
      }
      
      public function set showTooltip(param1:Boolean) : void
      {
         this._showTooltip = param1;
      }
      
      public function get quantity() : int
      {
         return this._quantity;
      }
   }
}
