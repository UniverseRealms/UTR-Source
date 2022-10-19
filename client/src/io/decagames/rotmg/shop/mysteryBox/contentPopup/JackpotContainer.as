package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import flash.display.Sprite;
   import io.decagames.rotmg.ui.gird.UIGrid;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class JackpotContainer extends Sprite
   {
       
      
      private const MAX_COLUMNS:int = 5;
      
      private const ROW_HEIGHT:int = 45;
      
      private var background:SliceScalingBitmap;
      
      private var grid:UIGrid;
      
      public function JackpotContainer()
      {
         super();
      }
      
      public function diamondBackground() : void
      {
         this.background = TextureParser.instance.getSliceScalingBitmap("UI","mystery_box_jackpot_diamond");
         addChild(this.background);
      }
      
      public function goldBackground() : void
      {
         this.background = TextureParser.instance.getSliceScalingBitmap("UI","mystery_box_jackpot_gold");
         addChild(this.background);
      }
      
      public function silverBackground() : void
      {
         this.background = TextureParser.instance.getSliceScalingBitmap("UI","mystery_box_jackpot_silver");
         addChild(this.background);
      }
      
      public function addGrid(param1:UIGrid) : void
      {
         this.background.height = 80 + Math.floor(param1.numberOfElements / 5) * 45;
         this.grid = param1;
         param1.y = 30;
         if(param1.numberOfElements <= 5)
         {
            param1.x = Math.round((this.background.width - (param1.numberOfElements * 40 + (param1.numberOfElements - 1) * 4)) / 2);
         }
         else
         {
            param1.x = Math.round((this.background.width - 216) / 2);
         }
         addChild(param1);
      }
      
      public function dispose() : void
      {
         this.background.dispose();
         this.grid.dispose();
      }
   }
}
