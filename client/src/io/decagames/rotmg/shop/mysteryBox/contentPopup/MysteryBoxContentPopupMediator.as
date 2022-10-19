package io.decagames.rotmg.shop.mysteryBox.contentPopup
{
   import flash.utils.Dictionary;
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.gird.UIGrid;
   import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class MysteryBoxContentPopupMediator extends Mediator
   {
       
      
      [Inject]
      public var view:MysteryBoxContentPopup;
      
      [Inject]
      public var closePopupSignal:ClosePopupSignal;
      
      private var closeButton:SliceScalingButton;
      
      private var contentGrids:Vector.<UIGrid>;
      
      private var jackpotsNumber:int = 0;
      
      private var jackpotsHeight:int = 0;
      
      private var jackpotUI:JackpotContainer;
      
      public function MysteryBoxContentPopupMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.closeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
         this.closeButton.clickSignal.addOnce(this.onClose);
         this.view.header.addButton(this.closeButton,"right_button");
         this.addJackpots(this.view.info.jackpots);
         this.addContentList(this.view.info.contents,this.view.info.jackpots);
      }
      
      private function addJackpots(param1:String) : void
      {
         var _loc3_:String = null;
         var _loc7_:Array = null;
         var _loc8_:Array = null;
         var _loc4_:Array = null;
         var _loc5_:String = null;
         var _loc9_:int = 0;
         var _loc11_:UIGrid = null;
         var _loc10_:UIItemContainer = null;
         var _loc6_:int = 0;
         var _loc2_:Array = param1.split("|");
         var _loc17_:int = 0;
         var _loc16_:* = _loc2_;
         for each(_loc3_ in _loc2_)
         {
            _loc7_ = _loc3_.split(",");
            _loc8_ = [];
            _loc4_ = [];
            var _loc13_:int = 0;
            var _loc12_:* = _loc7_;
            for each(_loc5_ in _loc7_)
            {
               _loc9_ = _loc8_.indexOf(_loc5_);
               if(_loc9_ == -1)
               {
                  _loc8_.push(_loc5_);
                  _loc4_.push(1);
               }
               else
               {
                  _loc4_[_loc9_] = _loc4_[_loc9_] + 1;
               }
            }
            if(param1.length > 0)
            {
               _loc11_ = new UIGrid(220,5,4);
               _loc11_.centerLastRow = true;
               var _loc15_:int = 0;
               var _loc14_:* = _loc8_;
               for each(_loc5_ in _loc8_)
               {
                  _loc10_ = new UIItemContainer(int(_loc5_),4737096,0,40);
                  _loc10_.showTooltip = true;
                  _loc11_.addGridElement(_loc10_);
                  _loc6_ = _loc4_[_loc8_.indexOf(_loc5_)];
                  if(_loc6_ > 1)
                  {
                     _loc10_.showQuantityLabel(_loc6_);
                  }
               }
               this.jackpotUI = new JackpotContainer();
               this.jackpotUI.x = 10;
               this.jackpotUI.y = 55 + this.jackpotsHeight - 22;
               if(this.jackpotsNumber == 0)
               {
                  this.jackpotUI.diamondBackground();
               }
               else if(this.jackpotsNumber == 1)
               {
                  this.jackpotUI.goldBackground();
               }
               else if(this.jackpotsNumber == 2)
               {
                  this.jackpotUI.silverBackground();
               }
               this.jackpotUI.addGrid(_loc11_);
               this.view.addChild(this.jackpotUI);
               this.jackpotsHeight = this.jackpotsHeight + (this.jackpotUI.height + 5);
               this.jackpotsNumber++;
            }
         }
      }
      
      private function addContentList(param1:String, param2:String) : void
      {
         var _loc10_:String = null;
         var _loc16_:int = 0;
         var _loc17_:int = 0;
         var _loc8_:Array = null;
         var _loc14_:Array = null;
         var _loc12_:Array = null;
         var _loc4_:String = null;
         var _loc3_:Boolean = false;
         var _loc7_:String = null;
         var _loc6_:Array = null;
         var _loc18_:UIGrid = null;
         var _loc20_:Array = null;
         var _loc19_:Vector.<ItemBox> = null;
         var _loc24_:Dictionary = null;
         var _loc23_:String = null;
         var _loc26_:Array = null;
         var _loc25_:String = null;
         var _loc22_:ItemsSetBox = null;
         var _loc21_:ItemBox = null;
         var _loc5_:Array = param1.split("|");
         var _loc13_:Array = param2.split("|");
         var _loc15_:Array = [];
         var _loc9_:int = 0;
         var _loc33_:int = 0;
         var _loc32_:* = _loc5_;
         for each(_loc10_ in _loc5_)
         {
            _loc14_ = [];
            _loc12_ = _loc10_.split(";");
            var _loc31_:* = 0;
            var _loc30_:* = _loc12_;
            for each(_loc4_ in _loc12_)
            {
               _loc3_ = false;
               var _loc29_:* = 0;
               var _loc28_:* = _loc13_;
               for each(_loc7_ in _loc13_)
               {
                  if(_loc7_ == _loc4_)
                  {
                     _loc3_ = true;
                     break;
                  }
               }
               if(!_loc3_)
               {
                  _loc6_ = _loc4_.split(",");
                  _loc14_.push(_loc6_);
               }
            }
            _loc15_[_loc9_] = _loc14_;
            _loc9_++;
         }
         _loc16_ = 475;
         _loc17_ = 30;
         if(this.jackpotsNumber > 0)
         {
            _loc16_ = _loc16_ - (this.jackpotsHeight + 10);
            _loc17_ = _loc17_ + (this.jackpotsHeight + 10);
         }
         this.contentGrids = new Vector.<UIGrid>(0);
         var _loc27_:int = 5;
         var _loc11_:Number = (260 - _loc27_ * (_loc15_.length - 1)) / _loc15_.length;
         var _loc41_:int = 0;
         var _loc40_:* = _loc15_;
         for each(_loc8_ in _loc15_)
         {
            _loc18_ = new UIGrid(_loc11_,1,5);
            var _loc39_:int = 0;
            var _loc38_:* = _loc8_;
            for each(_loc20_ in _loc8_)
            {
               _loc19_ = new Vector.<ItemBox>();
               _loc24_ = new Dictionary();
               var _loc35_:int = 0;
               var _loc34_:* = _loc20_;
               for each(_loc23_ in _loc20_)
               {
                  if(_loc24_[_loc23_])
                  {
                     _loc29_ = _loc24_;
                     _loc28_ = _loc23_;
                     _loc31_ = Number(_loc29_[_loc28_]) + 1;
                     _loc29_[_loc28_] = _loc31_;
                  }
                  else
                  {
                     _loc24_[_loc23_] = 1;
                  }
               }
               _loc26_ = [];
               var _loc37_:int = 0;
               var _loc36_:* = _loc20_;
               for each(_loc25_ in _loc20_)
               {
                  if(_loc26_.indexOf(_loc25_) == -1)
                  {
                     _loc21_ = new ItemBox(_loc25_,_loc24_[_loc25_],_loc15_.length == 1,"",false);
                     _loc21_.clearBackground();
                     _loc19_.push(_loc21_);
                     _loc26_.push(_loc25_);
                  }
               }
               _loc22_ = new ItemsSetBox(_loc19_);
               _loc18_.addGridElement(_loc22_);
            }
            _loc18_.y = _loc17_;
            _loc18_.x = 10 + _loc11_ * this.contentGrids.length + _loc27_ * this.contentGrids.length;
            this.view.addChild(_loc18_);
            this.contentGrids.push(_loc18_);
         }
      }
      
      override public function destroy() : void
      {
         var _loc1_:UIGrid = null;
         this.closeButton.dispose();
         var _loc3_:int = 0;
         var _loc2_:* = this.contentGrids;
         for each(_loc1_ in this.contentGrids)
         {
            _loc1_.dispose();
         }
         this.contentGrids = null;
      }
      
      private function onClose(param1:BaseButton) : void
      {
         this.closePopupSignal.dispatch(this.view);
      }
   }
}
