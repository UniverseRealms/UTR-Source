package io.decagames.rotmg.shop.mysteryBox
{
   import flash.geom.Point;
   import io.decagames.rotmg.shop.genericBox.GenericBoxTile;
   import io.decagames.rotmg.shop.mysteryBox.contentPopup.UIItemContainer;
   import io.decagames.rotmg.ui.gird.UIGrid;
   import kabam.rotmg.mysterybox.model.MysteryBoxInfo;
   
   public class MysteryBoxTile extends GenericBoxTile
   {
       
      
      private var displayedItemsGrid:UIGrid;
      
      private var maxResultHeight:int = 75;
      
      private var maxResultWidth:int;
      
      private var resultElementWidth:int;
      
      private var gridConfig:Point;
      
      public function MysteryBoxTile(param1:MysteryBoxInfo)
      {
         buyButtonBitmapBackground = "shop_box_button_background";
         super(param1);
      }
      
      private function prepareResultGrid(param1:int) : void
      {
         this.maxResultWidth = 160;
         this.gridConfig = this.calculateGrid(param1);
         this.resultElementWidth = this.calculateElementSize(this.gridConfig);
         this.displayedItemsGrid = new UIGrid(this.resultElementWidth * this.gridConfig.x,this.gridConfig.x,0);
         this.displayedItemsGrid.x = 20 + Math.round((this.maxResultWidth - this.resultElementWidth * this.gridConfig.x) / 2);
         this.displayedItemsGrid.y = Math.round(42 + (this.maxResultHeight - this.resultElementWidth * this.gridConfig.y) / 2);
         this.displayedItemsGrid.centerLastRow = true;
         addChild(this.displayedItemsGrid);
      }
      
      private function calculateGrid(param1:int) : Point
      {
         var _loc6_:int = 0;
         var _loc4_:int = 0;
         var _loc2_:Point = new Point(11,4);
         var _loc3_:* = -2147483648;
         if(param1 >= _loc2_.x * _loc2_.y)
         {
            return _loc2_;
         }
         var _loc5_:int = 11;
         while(_loc5_ >= 1)
         {
            _loc6_ = 4;
            while(_loc6_ >= 1)
            {
               if(_loc5_ * _loc6_ >= param1 && (_loc5_ - 1) * (_loc6_ - 1) < param1)
               {
                  _loc4_ = this.calculateElementSize(new Point(_loc5_,_loc6_));
                  if(_loc4_ != -1)
                  {
                     if(_loc4_ > _loc3_)
                     {
                        _loc3_ = _loc4_;
                        _loc2_ = new Point(_loc5_,_loc6_);
                     }
                     else if(_loc4_ == _loc3_)
                     {
                        if(_loc2_.x * _loc2_.y - param1 > _loc5_ * _loc6_ - param1)
                        {
                           _loc3_ = _loc4_;
                           _loc2_ = new Point(_loc5_,_loc6_);
                        }
                     }
                  }
               }
               _loc6_--;
            }
            _loc5_--;
         }
         return _loc2_;
      }
      
      private function calculateElementSize(param1:Point) : int
      {
         var _loc2_:int = Math.floor(this.maxResultHeight / param1.y);
         if(_loc2_ * param1.x > this.maxResultWidth)
         {
            _loc2_ = Math.floor(this.maxResultWidth / param1.x);
         }
         if(_loc2_ * param1.y > this.maxResultHeight)
         {
            return -1;
         }
         return _loc2_;
      }
      
      override protected function createBoxBackground() : void
      {
         var _loc1_:int = 0;
         var _loc4_:UIItemContainer = null;
         var _loc3_:Array = MysteryBoxInfo(_boxInfo).displayedItems.split(",");
         if(_loc3_.length == 0 || MysteryBoxInfo(_boxInfo).displayedItems == "")
         {
            return;
         }
         if(_infoButton)
         {
            _infoButton.alpha = 0;
         }
         switch(int(_loc3_.length) - 1)
         {
            case 0:
               break;
            case 1:
               _loc1_ = 50;
            default:
               _loc1_ = 50;
         }
         this.prepareResultGrid(_loc3_.length);
         var _loc2_:int = 0;
         while(_loc2_ < _loc3_.length)
         {
            _loc4_ = new UIItemContainer(_loc3_[_loc2_],0,0,this.resultElementWidth);
            this.displayedItemsGrid.addGridElement(_loc4_);
            _loc2_++;
         }
      }
      
      override public function resize(param1:int, param2:int = -1) : void
      {
         background.width = param1;
         backgroundTitle.width = param1;
         backgroundButton.width = param1;
         background.height = 184;
         backgroundTitle.y = 2;
         titleLabel.x = Math.round((param1 - titleLabel.textWidth) / 2);
         titleLabel.y = 6;
         backgroundButton.y = 133;
         _buyButton.y = backgroundButton.y + 4;
         _buyButton.x = param1 - 110;
         _infoButton.x = 130;
         _infoButton.y = 45;
         if(this.displayedItemsGrid)
         {
            this.displayedItemsGrid.x = 10 + Math.round((this.maxResultWidth - this.resultElementWidth * this.gridConfig.x) / 2);
         }
         updateSaleLabel();
         updateClickMask(param1);
         updateStartTimeString(param1);
         updateTimeEndString(param1);
      }
      
      override public function dispose() : void
      {
         if(this.displayedItemsGrid)
         {
            this.displayedItemsGrid.dispose();
         }
         super.dispose();
      }
   }
}
