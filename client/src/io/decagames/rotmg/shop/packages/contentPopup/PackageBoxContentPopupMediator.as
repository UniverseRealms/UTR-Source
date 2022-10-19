package io.decagames.rotmg.shop.packages.contentPopup
{
   import flash.utils.Dictionary;
   import io.decagames.rotmg.shop.mysteryBox.contentPopup.ItemBox;
   import io.decagames.rotmg.shop.mysteryBox.contentPopup.SlotBox;
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.gird.UIGrid;
   import io.decagames.rotmg.ui.popups.signals.ClosePopupSignal;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import robotlegs.bender.bundles.mvcs.Mediator;
   
   public class PackageBoxContentPopupMediator extends Mediator
   {
       
      
      [Inject]
      public var view:PackageBoxContentPopup;
      
      [Inject]
      public var closePopupSignal:ClosePopupSignal;
      
      private var closeButton:SliceScalingButton;
      
      private var contentGrids:UIGrid;
      
      public function PackageBoxContentPopupMediator()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.closeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
         this.closeButton.clickSignal.addOnce(this.onClose);
         this.view.header.addButton(this.closeButton,"right_button");
         this.addContentList(this.view.info.contents,this.view.info.charSlot,this.view.info.vaultSlot,this.view.info.gold);
      }
      
      private function addContentList(param1:String, param2:int, param3:int, param4:int) : void
      {
         var _loc8_:Array = null;
         var _loc13_:Dictionary = null;
         var _loc14_:String = null;
         var _loc15_:Array = null;
         var _loc9_:String = null;
         var _loc6_:ItemBox = null;
         var _loc11_:SlotBox = null;
         var _loc10_:SlotBox = null;
         var _loc5_:SlotBox = null;
         var _loc12_:int = 5;
         var _loc7_:Number = 260 - _loc12_;
         this.contentGrids = new UIGrid(_loc7_,1,2);
         if(param1 != "")
         {
            _loc8_ = param1.split(",");
            _loc13_ = new Dictionary();
            var _loc20_:int = 0;
            var _loc19_:* = _loc8_;
            for each(_loc14_ in _loc8_)
            {
               if(_loc13_[_loc14_])
               {
                  var _loc16_:* = _loc13_;
                  var _loc17_:* = _loc14_;
                  var _loc18_:* = Number(_loc16_[_loc17_]) + 1;
                  _loc16_[_loc17_] = _loc18_;
               }
               else
               {
                  _loc13_[_loc14_] = 1;
               }
            }
            _loc15_ = [];
            var _loc22_:int = 0;
            var _loc21_:* = _loc8_;
            for each(_loc9_ in _loc8_)
            {
               if(_loc15_.indexOf(_loc9_) == -1)
               {
                  _loc6_ = new ItemBox(_loc9_,_loc13_[_loc9_],true,"",false);
                  this.contentGrids.addGridElement(_loc6_);
                  _loc15_.push(_loc9_);
               }
            }
         }
         if(param2 > 0)
         {
            _loc11_ = new SlotBox("CHAR_SLOT",param2,true,"",false);
            this.contentGrids.addGridElement(_loc11_);
         }
         if(param3 > 0)
         {
            _loc10_ = new SlotBox("VAULT_SLOT",param3,true,"",false);
            this.contentGrids.addGridElement(_loc10_);
         }
         if(param4 > 0)
         {
            _loc5_ = new SlotBox("GOLD_SLOT",param4,true,"",false);
            this.contentGrids.addGridElement(_loc5_);
         }
         this.contentGrids.y = this.view.infoLabel.textHeight + 8;
         this.contentGrids.x = 10;
         this.view.addChild(this.contentGrids);
      }
      
      override public function destroy() : void
      {
         this.closeButton.clickSignal.remove(this.onClose);
         this.closeButton.dispose();
         this.contentGrids.dispose();
         this.contentGrids = null;
      }
      
      private function onClose(param1:BaseButton) : void
      {
         this.closePopupSignal.dispatch(this.view);
      }
   }
}
