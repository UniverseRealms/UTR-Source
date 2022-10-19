package io.decagames.rotmg.shop.mysteryBox.rollModal.elements
{
   import flash.display.DisplayObject;
   import flash.display.Sprite;
   import flash.events.Event;
   import flash.utils.getTimer;
   import io.decagames.rotmg.utils.colors.RGB;
   import io.decagames.rotmg.utils.colors.RandomColorGenerator;
   import io.decagames.rotmg.utils.colors.Tint;
   import kabam.rotmg.assets.EmbeddedAssets;
   
   public class Spinner extends Sprite
   {
       
      
      public const graphic:DisplayObject = new EmbeddedAssets.StarburstSpinner();
      
      private var _degreesPerSecond:int;
      
      private var secondsElapsed:Number;
      
      private var previousSeconds:Number;
      
      private var startColor:uint;
      
      private var endColor:uint;
      
      private var direction:Boolean;
      
      private var previousProgress:Number = 0;
      
      private var multicolor:Boolean;
      
      private var rStart:Number = -1;
      
      private var gStart:Number = -1;
      
      private var bStart:Number = -1;
      
      private var rFinal:Number = -1;
      
      private var gFinal:Number = -1;
      
      private var bFinal:Number = -1;
      
      public function Spinner(param1:int, param2:Boolean = false)
      {
         super();
         this._degreesPerSecond = param1;
         this.multicolor = param2;
         this.secondsElapsed = 0;
         this.setupStartAndFinalColors();
         this.addGraphic();
         this.applyColor(0);
         addEventListener("enterFrame",this.onEnterFrame);
         addEventListener("removedFromStage",this.onRemoved);
      }
      
      private function addGraphic() : void
      {
         addChild(this.graphic);
         this.graphic.x = -1 * width / 2;
         this.graphic.y = -1 * height / 2;
      }
      
      private function onRemoved(param1:Event) : void
      {
         removeEventListener("removedFromStage",this.onRemoved);
         removeEventListener("enterFrame",this.onEnterFrame);
      }
      
      public function pause() : void
      {
         removeEventListener("enterFrame",this.onEnterFrame);
         this.previousSeconds = 0;
      }
      
      public function resume() : void
      {
         addEventListener("enterFrame",this.onEnterFrame);
      }
      
      private function onEnterFrame(param1:Event) : void
      {
         this.updateTimeElapsed();
         var _loc2_:Number = this._degreesPerSecond * this.secondsElapsed % 360;
         rotation = _loc2_;
         this.applyColor(_loc2_ / 360);
      }
      
      private function applyColor(param1:Number) : void
      {
         if(!this.multicolor)
         {
            return;
         }
         if(param1 < this.previousProgress)
         {
            this.direction = !this.direction;
         }
         this.previousProgress = param1;
         if(this.direction)
         {
            param1 = 1 - param1;
         }
         var _loc2_:uint = this.getColorByProgress(param1);
         Tint.add(this.graphic,_loc2_,1);
      }
      
      private function getColorByProgress(param1:Number) : uint
      {
         var _loc2_:Number = this.rStart + (this.rFinal - this.rStart) * param1;
         var _loc3_:Number = this.gStart + (this.gFinal - this.gStart) * param1;
         var _loc4_:Number = this.bStart + (this.bFinal - this.bStart) * param1;
         return RGB.fromRGB(_loc2_,_loc3_,_loc4_);
      }
      
      private function setupStartAndFinalColors() : void
      {
         var _loc3_:RandomColorGenerator = new RandomColorGenerator();
         var _loc1_:Array = _loc3_.randomColor();
         var _loc2_:Array = _loc3_.randomColor();
         this.rStart = _loc1_[0];
         this.gStart = _loc1_[1];
         this.bStart = _loc1_[2];
         this.rFinal = _loc2_[0];
         this.gFinal = _loc2_[1];
         this.bFinal = _loc2_[2];
      }
      
      private function updateTimeElapsed() : void
      {
         var _loc1_:Number = getTimer() / 1000;
         if(this.previousSeconds)
         {
            this.secondsElapsed = this.secondsElapsed + (_loc1_ - this.previousSeconds);
         }
         this.previousSeconds = _loc1_;
      }
      
      public function get degreesPerSecond() : int
      {
         return this._degreesPerSecond;
      }
   }
}
