package io.decagames.rotmg.ui.labels
{
   import flash.text.TextField;
   import flash.text.TextFieldAutoSize;
   
   public class UILabel extends TextField
   {
      
      public static var DEBUG:Boolean = false;
       
      
      private var chromeFixMargin:int = 2;
      
      public function UILabel()
      {
         super();
         if(DEBUG)
         {
            this.debugDraw();
         }
         this.embedFonts = true;
         this.selectable = false;
         this.autoSize = TextFieldAutoSize.LEFT;
      }
      
      private function debugDraw() : void
      {
         this.border = true;
         this.borderColor = 16711680;
      }
      
      public function setTextWidth(param1:Number) : void
      {
         this.width = param1;
      }
      
      public function setAutoSize(param1:String) : void
      {
         this.autoSize = param1;
      }
      
      public function setMultiLine(param1:Boolean) : void
      {
         multiline = param1;
         wordWrap = param1;
      }
      
      override public function set y(param1:Number) : void
      {
         super.y = param1;
      }
      
      override public function get textWidth() : Number
      {
         return super.textWidth + 4;
      }
   }
}
