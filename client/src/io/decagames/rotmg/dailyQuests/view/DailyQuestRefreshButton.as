package io.decagames.rotmg.dailyQuests.view
{
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class DailyQuestRefreshButton extends SliceScalingButton
   {
       
      
      public function DailyQuestRefreshButton()
      {
         super(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button",32));
         this.createRefreshIcon();
      }
      
      private function createRefreshIcon() : void
      {
         var _loc1_:SliceScalingBitmap = TextureParser.instance.getSliceScalingBitmap("UI","refresh_icon",10);
         _loc1_.x = 7;
         _loc1_.y = 8;
         addChild(_loc1_);
      }
   }
}
