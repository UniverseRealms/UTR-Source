package io.decagames.rotmg.ui.popups.header
{
import flash.display.Sprite;
import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.labels.UILabel;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

public class PopupHeader extends Sprite
{

   public static const LEFT_BUTTON:String = "left_button";

   public static const RIGHT_BUTTON:String = "right_button";

   public static var TYPE_FULL:String = "full";

   public static var TYPE_MODAL:String = "modal";


   private var backgroundBitmap:SliceScalingBitmap;

   private var titleBackgroundBitmap:SliceScalingBitmap;

   private var _titleLabel:UILabel;

   private var buttonsContainers:Vector.<Sprite>;

   private var buttons:Vector.<SliceScalingButton>;

   private var _coinsField:CoinsField;

   private var headerWidth:int;

   private var headerType:String;

   public function PopupHeader(param1:int, param2:String)
   {
      super();
      this.headerWidth = param1;
      this.headerType = param2;
      if(param2 == TYPE_FULL)
      {
         this.backgroundBitmap = TextureParser.instance.getSliceScalingBitmap("UI","popup_header",param1);
         addChild(this.backgroundBitmap);
      }
      this.buttonsContainers = new Vector.<Sprite>();
      this.buttons = new Vector.<SliceScalingButton>();
   }

   public function setTitle(param1:String, param2:int, param3:Function = null) : void
   {
      if(!this.titleBackgroundBitmap)
      {
         if(this.headerType == TYPE_FULL)
         {
            this.titleBackgroundBitmap = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",param2);
            addChild(this.titleBackgroundBitmap);
            this.titleBackgroundBitmap.x = Math.round((this.headerWidth - param2) / 2);
            this.titleBackgroundBitmap.y = 29;
         }
         else
         {
            this.titleBackgroundBitmap = TextureParser.instance.getSliceScalingBitmap("UI","modal_header_title",param2);
            addChild(this.titleBackgroundBitmap);
            this.titleBackgroundBitmap.x = Math.round((this.headerWidth - param2) / 2);
         }
         this._titleLabel = new UILabel();
         if(param3 != null)
         {
            param3(this._titleLabel);
         }
         this._titleLabel.text = param1;
         addChild(this._titleLabel);
         this._titleLabel.x = this.titleBackgroundBitmap.x + (this.titleBackgroundBitmap.width - this._titleLabel.textWidth) / 2;
         if(this.headerType == TYPE_FULL)
         {
            this._titleLabel.y = this.titleBackgroundBitmap.height - this._titleLabel.height / 2 - 3;
         }
         else
         {
            this._titleLabel.y = this.titleBackgroundBitmap.y + (this.titleBackgroundBitmap.height - this._titleLabel.height) / 2;
         }
      }
   }

   public function addButton(param1:SliceScalingButton, param2:String) : void
   {
      var _loc3_:SliceScalingBitmap = null;
      var _loc4_:Sprite = new Sprite();
      if(this.headerType == TYPE_FULL)
      {
         _loc3_ = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_button_decor");
         _loc4_.addChild(_loc3_);
      }
      _loc4_.addChild(param1);
      addChild(_loc4_);
      this.buttonsContainers.push(_loc4_);
      this.buttons.push(param1);
      if(this.headerType == TYPE_FULL)
      {
         _loc3_.y = (this.backgroundBitmap.height - _loc3_.height) / 2;
         param1.y = _loc3_.y + 8;
      }
      else
      {
         param1.y = 5;
      }
      if(param2 == RIGHT_BUTTON)
      {
         if(this.headerType == TYPE_FULL)
         {
            _loc3_.x = this.headerWidth - _loc3_.width;
            param1.x = _loc3_.x + 6;
         }
         else
         {
            param1.x = this.titleBackgroundBitmap.x + this.titleBackgroundBitmap.width - param1.width - 3;
         }
      }
      else if(this.headerType == TYPE_FULL)
      {
         _loc3_.x = _loc3_.width;
         _loc3_.scaleX = -1;
         param1.x = 16;
      }
      else
      {
         param1.x = this.titleBackgroundBitmap.x + 3;
      }
   }

   public function showCoins(param1:int) : CoinsField
   {
      var _loc2_:Sprite = null;
      this._coinsField = new CoinsField(param1);
      this._coinsField.x = 44;
      addChild(this._coinsField);
      this.alignCurrency();
      for each(_loc2_ in this.buttonsContainers)
      {
         addChild(_loc2_);
      }
      return this._coinsField;
   }

   private function alignCurrency() : void
   {
      if(this._coinsField)
      {
         this._coinsField.y = 39;
      }
      else if(this._coinsField)
      {
         this._coinsField.y = 51;
      }
   }

   public function dispose() : void
   {
      var _loc1_:SliceScalingButton = null;
      if(this.backgroundBitmap)
      {
         this.backgroundBitmap.dispose();
      }
      this.titleBackgroundBitmap.dispose();
      if(this._coinsField)
      {
         this._coinsField.dispose();
      }
      for each(_loc1_ in this.buttons)
      {
         _loc1_.dispose();
      }
      this.buttonsContainers = null;
      this.buttons = null;
   }

   public function get titleLabel() : UILabel
   {
      return this._titleLabel;
   }

   public function get coinsField() : CoinsField
   {
      return this._coinsField;
   }
}
}
