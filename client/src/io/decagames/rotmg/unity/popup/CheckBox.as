package io.decagames.rotmg.unity.popup
{
   import flash.display.CapsStyle;
   import flash.display.Graphics;
   import flash.display.JointStyle;
   import flash.display.LineScaleMode;
   import flash.display.Sprite;
   import flash.events.MouseEvent;
   import flash.filters.DropShadowFilter;
   import kabam.rotmg.text.view.TextFieldDisplayConcrete;
   import kabam.rotmg.text.view.stringBuilder.LineBuilder;
   import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
   import kabam.rotmg.text.view.stringBuilder.StringBuilder;
   
   public class CheckBox extends Sprite
   {
       
      
      public var checkBox_:Sprite;
      
      public var text_:TextFieldDisplayConcrete;
      
      public var errorText_:TextFieldDisplayConcrete;
      
      private var hasError:Boolean;
      
      private var _checked:Boolean;
      
      private var _text:String;
      
      private var _fontSize:int;
      
      private var _boxSize:int;
      
      public function CheckBox(param1:String, param2:Boolean, param3:uint = 16, param4:int = 20)
      {
         super();
         this._text = param1;
         this._checked = param2;
         this._fontSize = param3;
         this._boxSize = param4;
         this.init();
      }
      
      private function init() : void
      {
         this.createCheckbox();
         this.createText();
         this.createErrorText();
      }
      
      private function createErrorText() : void
      {
         this.errorText_ = new TextFieldDisplayConcrete().setSize(12).setColor(16549442);
         this.errorText_.filters = [new DropShadowFilter(0,0,0)];
         addChild(this.errorText_);
      }
      
      private function createText() : void
      {
         this.text_ = new TextFieldDisplayConcrete().setSize(this._fontSize).setColor(11776947);
         this.text_.setTextWidth(243);
         this.text_.x = this.checkBox_.x + this._boxSize + 8;
         this.text_.setBold(true);
         this.text_.setMultiLine(true);
         this.text_.setWordWrap(true);
         this.text_.setHTML(true);
         this.text_.setStringBuilder(new LineBuilder().setParams(this._text));
         this.text_.mouseEnabled = true;
         this.text_.filters = [new DropShadowFilter(0,0,0)];
         this.text_.textChanged.addOnce(this.onTextChanged);
         addChild(this.text_);
      }
      
      private function createCheckbox() : void
      {
         this.checkBox_ = new Sprite();
         this.checkBox_.x = 2;
         this.checkBox_.y = 2;
         this.redrawCheckBox();
         this.checkBox_.addEventListener(MouseEvent.CLICK,this.onClick);
         addChild(this.checkBox_);
      }
      
      public function setTextStringBuilder(param1:StringBuilder) : void
      {
         this.text_.setStringBuilder(param1);
      }
      
      private function onTextChanged() : void
      {
         this.errorText_.x = this.text_.x;
         this.errorText_.y = this.text_.y + 20;
      }
      
      private function onClick(param1:MouseEvent) : void
      {
         this.errorText_.setStringBuilder(new StaticStringBuilder(""));
         this._checked = !this._checked;
         this.redrawCheckBox();
      }
      
      public function setErrorHighlight(param1:Boolean) : void
      {
         this.hasError = param1;
         this.redrawCheckBox();
      }
      
      private function redrawCheckBox() : void
      {
         var _loc2_:Number = NaN;
         var _loc1_:Graphics = this.checkBox_.graphics;
         _loc1_.clear();
         _loc1_.beginFill(3355443,1);
         _loc1_.drawRect(0,0,this._boxSize,this._boxSize);
         _loc1_.endFill();
         if(this._checked)
         {
            _loc1_.lineStyle(2,11776947,1,false,LineScaleMode.NORMAL,CapsStyle.ROUND,JointStyle.ROUND);
            _loc1_.moveTo(2,2);
            _loc1_.lineTo(this._boxSize - 2,this._boxSize - 2);
            _loc1_.moveTo(2,this._boxSize - 2);
            _loc1_.lineTo(this._boxSize - 2,2);
            _loc1_.lineStyle();
            this.hasError = false;
         }
         if(this.hasError)
         {
            _loc2_ = 16549442;
         }
         else
         {
            _loc2_ = 4539717;
         }
         _loc1_.lineStyle(2,_loc2_,1,false,LineScaleMode.NORMAL,CapsStyle.ROUND,JointStyle.ROUND);
         _loc1_.drawRect(0,0,this._boxSize,this._boxSize);
         _loc1_.lineStyle();
      }
      
      public function isChecked() : Boolean
      {
         return this._checked;
      }
      
      public function setChecked() : void
      {
         this._checked = true;
         this.redrawCheckBox();
      }
      
      public function setUnchecked() : void
      {
         this._checked = false;
         this.redrawCheckBox();
      }
      
      public function setError(param1:String) : void
      {
         this.errorText_.setStringBuilder(new LineBuilder().setParams(param1));
      }
   }
}
