package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.Sprite;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.gird.UIGridElement;
   import io.decagames.rotmg.ui.labels.UILabel;
   import kabam.rotmg.text.view.stringBuilder.LineBuilder;
   
   public class ItemBox extends UIGridElement
   {
       
      
      private var _itemId:String;
      
      private var bitmapName:String;
      
      private var isLastElement:Boolean;
      
      private var amount:int;
      
      private var _itemBackground:Sprite;
      
      private var showFullName:Boolean;
      
      private var isBackgroundCleared:Boolean;
      
      private var label:UILabel;
      
      private var targetWidth:int = 260;
      
      private var itemSize:int = 40;
      
      private var itemMargin:int = 2;
      
      private var imageBitmap:Bitmap;
      
      public function ItemBox(param1:String, param2:int, param3:Boolean, param4:String = "", param5:Boolean = false)
      {
         super();
         this._itemId = param1;
         this.bitmapName = param4;
         this.isLastElement = param5;
         this.amount = param2;
         this.showFullName = param3;
         this.label = new UILabel();
         this.label.multiline = true;
         this.label.autoSize = "left";
         this.label.wordWrap = true;
         DefaultLabelFormat.mysteryBoxContentItemName(this.label);
         this.drawBackground(param4,param5,260);
         this.drawElement(param1,param2);
         this.resizeLabel();
      }
      
      private function drawBackground(param1:String, param2:Boolean, param3:int) : void
      {
         if(param1 == "")
         {
            this.graphics.clear();
            this.graphics.beginFill(2960685);
            this.graphics.drawRect(0,0,param3,this.itemSize + 2 * this.itemMargin);
            this.graphics.endFill();
         }
      }
      
      public function clearBackground() : void
      {
         this.isBackgroundCleared = true;
         this.graphics.clear();
      }
      
      private function drawElement(param1:String, param2:int) : void
      {
         this._itemBackground = new Sprite();
         this._itemBackground.graphics.clear();
         this._itemBackground.graphics.beginFill(16777215,0);
         this._itemBackground.graphics.drawRect(0,0,this.itemSize,this.itemSize);
         this._itemBackground.graphics.endFill();
         addChild(this._itemBackground);
         this._itemBackground.x = 10;
         this._itemBackground.y = 4;
         var _loc3_:BitmapData = ObjectLibrary.getRedrawnTextureFromType(int(param1),this._itemBackground.width * 2,true,false);
         this.imageBitmap = new Bitmap(_loc3_);
         this.imageBitmap.x = -Math.round((this.imageBitmap.width - this.itemSize) / 2);
         this.imageBitmap.y = -Math.round((this.imageBitmap.height - this.itemSize) / 2);
         this._itemBackground.addChild(this.imageBitmap);
         if(this.showFullName)
         {
            this.label.text = param2 + "x " + LineBuilder.getLocalizedStringFromKey(ObjectLibrary.typeToDisplayId_[param1]);
            this.label.x = 55;
         }
         else
         {
            this.label.text = param2 + "x";
            this.label.x = 10;
            this._itemBackground.x = this._itemBackground.x + (this.label.x + 10);
         }
         addChild(this.label);
      }
      
      override public function get height() : Number
      {
         return this.itemSize + 2 * this.itemMargin;
      }
      
      private function resizeLabel() : void
      {
         this.label.width = this.targetWidth - (this.itemSize + 2 * this.itemMargin) - 16;
         this.label.y = (this.height - this.label.textHeight - 4) / 2;
      }
      
      override public function resize(param1:int, param2:int = -1) : void
      {
         if(!this.isBackgroundCleared)
         {
            this.drawBackground(this.bitmapName,this.isLastElement,param1);
         }
         this.targetWidth = param1;
         this.resizeLabel();
      }
      
      override public function dispose() : void
      {
         if(this.imageBitmap)
         {
            this.imageBitmap.bitmapData.dispose();
         }
         super.dispose();
      }
      
      public function get itemId() : String
      {
         return this._itemId;
      }
      
      public function get itemBackground() : Sprite
      {
         return this._itemBackground;
      }
   }
}
