package io.decagames.rotmg.dailyQuests.view.slot
{
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import flash.display.Bitmap;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.display.Sprite;
   import flash.geom.Matrix;
   import io.decagames.rotmg.utils.colors.GreyScale;
   import kabam.rotmg.core.StaticInjectorContext;
   import kabam.rotmg.text.view.BitmapTextFactory;
   import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
   
   public class DailyQuestItemSlot extends Sprite
   {
      
      public static const SELECTED_BORDER_SIZE:int = 2;
      
      public static const SLOT_SIZE:int = 40;
       
      
      private var _itemID:int;
      
      private var _type:String;
      
      private var bitmapFactory:BitmapTextFactory;
      
      private var imageContainer:Sprite;
      
      private var _isSlotsSelectable:Boolean;
      
      public var _selected:Boolean;
      
      private var backgroundShape:Shape;
      
      private var hasItem:Boolean;
      
      private var imageBitmap:Bitmap;
      
      public function DailyQuestItemSlot(param1:int, param2:String, param3:Boolean = false, param4:Boolean = false)
      {
         super();
         this._itemID = param1;
         this._type = param2;
         this._isSlotsSelectable = param4;
         this.hasItem = param3;
         this.imageBitmap = new Bitmap();
         this.imageContainer = new Sprite();
         addChild(this.imageContainer);
         this.imageContainer.x = Math.round(40 / 2);
         this.imageContainer.y = Math.round(40 / 2);
         this.createBackground();
         this.renderItem();
      }
      
      private function createBackground() : void
      {
         if(!this.backgroundShape)
         {
            this.backgroundShape = new Shape();
            this.imageContainer.addChild(this.backgroundShape);
         }
         this.backgroundShape.graphics.clear();
         if(this.isSlotsSelectable)
         {
            if(this._selected)
            {
               this.backgroundShape.graphics.beginFill(14846006,1);
               this.backgroundShape.graphics.drawRect(-2,-2,40 + 2 * 2,40 + 2 * 2);
               this.backgroundShape.graphics.beginFill(14846006,1);
            }
            else
            {
               this.backgroundShape.graphics.beginFill(4539717,1);
            }
         }
         else
         {
            this.backgroundShape.graphics.beginFill(!!this.hasItem?uint(1286144):uint(4539717),1);
         }
         this.backgroundShape.graphics.drawRect(0,0,40,40);
         this.backgroundShape.x = -Math.round((40 + 2 * 2) / 2);
         this.backgroundShape.y = -Math.round((40 + 2 * 2) / 2);
      }
      
      private function renderItem() : void
      {
         var _loc2_:BitmapData = null;
         var _loc4_:Matrix = null;
         if(this.imageBitmap.bitmapData)
         {
            this.imageBitmap.bitmapData.dispose();
         }
         var _loc3_:BitmapData = ObjectLibrary.getRedrawnTextureFromType(this._itemID,40 * 2,true);
         _loc3_ = _loc3_.clone();
         var _loc1_:XML = ObjectLibrary.xmlLibrary_[this._itemID];
         this.bitmapFactory = StaticInjectorContext.getInjector().getInstance(BitmapTextFactory);
         if(_loc1_ && _loc1_.hasOwnProperty("Quantity") && this.bitmapFactory)
         {
            _loc2_ = this.bitmapFactory.make(new StaticStringBuilder(String(_loc1_.Quantity)),12,16777215,false,new Matrix(),true);
            _loc4_ = new Matrix();
            _loc4_.translate(8,7);
            _loc3_.draw(_loc2_,_loc4_);
         }
         this.imageBitmap.bitmapData = _loc3_;
         if(this.isSlotsSelectable && !this._selected)
         {
            GreyScale.setGreyScale(_loc3_);
         }
         if(!this.imageBitmap.parent)
         {
            this.imageBitmap.x = -Math.round(this.imageBitmap.width / 2);
            this.imageBitmap.y = -Math.round(this.imageBitmap.height / 2);
            this.imageContainer.addChild(this.imageBitmap);
         }
      }
      
      public function set selected(param1:Boolean) : void
      {
         this._selected = param1;
         this.createBackground();
         this.renderItem();
      }
      
      public function dispose() : void
      {
         if(this.imageBitmap && this.imageBitmap.bitmapData)
         {
            this.imageBitmap.bitmapData.dispose();
         }
      }
      
      public function get itemID() : int
      {
         return this._itemID;
      }
      
      public function get type() : String
      {
         return this._type;
      }
      
      public function get isSlotsSelectable() : Boolean
      {
         return this._isSlotsSelectable;
      }
      
      public function get selected() : Boolean
      {
         return this._selected;
      }
   }
}
