package io.decagames.rotmg.shop.mysteryBox.rollModal
{
   import com.company.assembleegameclient.map.ParticleModalMap;
   import com.gskinner.motion.GTween;
   import com.gskinner.motion.easing.Sine;
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import flash.utils.clearTimeout;
   import flash.utils.setTimeout;
   import io.decagames.rotmg.shop.ShopBuyButton;
   import io.decagames.rotmg.shop.mysteryBox.contentPopup.UIItemContainer;
   import io.decagames.rotmg.shop.mysteryBox.rollModal.elements.Spinner;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.gird.UIGrid;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.modal.ModalPopup;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.spinner.FixedNumbersSpinner;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import kabam.rotmg.mysterybox.model.MysteryBoxInfo;
   import org.osflash.signals.Signal;
   
   public class MysteryBoxRollModal extends ModalPopup
   {
       
      
      private var spinnersContainer:Sprite;
      
      private var _quantity:int;
      
      private var _info:MysteryBoxInfo;
      
      private var itemsBitmap:Vector.<Bitmap>;
      
      private var rollGrid:UIGrid;
      
      private var resultGrid:UIGrid;
      
      private const iconSize:Number = 80;
      
      private var maxInColumn:int = 5;
      
      private var elementMargin:int = 10;
      
      private var maxResultWidth:int;
      
      private var maxResultRows:int = 3;
      
      private var resultElementWidth:int;
      
      private var resultGridMargin:int = 10;
      
      private var spinnerTopMargin:int = 165;
      
      private var buyButtonBackground:SliceScalingBitmap;
      
      private var maxResultHeight:int = 135;
      
      private var buySectionContainer:Sprite;
      
      public var buyButton:ShopBuyButton;
      
      public var spinner:FixedNumbersSpinner;
      
      public var bigSpinner:Spinner;
      
      public var littleSpinner:Spinner;
      
      private var particleModalMap:ParticleModalMap;
      
      private var vaultInfo:UILabel;
      
      public var finishedShowingResult:Signal;
      
      private var exposeDuration:Number = 0.4;
      
      private var exposeScale:Number = 1.5;
      
      private var movingDuration:Number = 0.2;
      
      private var movingDelay:Number = 0.1;
      
      private var buyButtonAnimationDuration:Number = 0.4;
      
      public function MysteryBoxRollModal(param1:MysteryBoxInfo, param2:int = 1)
      {
         this.spinnersContainer = new Sprite();
         this.finishedShowingResult = new Signal();
         super(415, 530, param1.title);
         this._quantity = param2;
         this._info = param1;
         this.itemsBitmap = new Vector.<Bitmap>();
         this.createSpinners();
         this.vaultInfo = new UILabel();
         DefaultLabelFormat.mysteryBoxVaultInfo(this.vaultInfo);
         this.vaultInfo.text = "Rewards will be placed in your Vault!";
         this.vaultInfo.y = 2;
         this.vaultInfo.x = (contentWidth - this.vaultInfo.textWidth) / 2;
         addChild(this.vaultInfo);
         this.vaultInfo.alpha = 0;
         this.particleModalMap = new ParticleModalMap(2);
         addChild(this.particleModalMap);
         this.rollGrid = new UIGrid(this.maxInColumn * 80,this.maxInColumn,this.elementMargin);
         this.rollGrid.centerLastRow = true;
         this.addBuyButton();
      }
      
      public function totalAnimationTime(param1:int) : Number
      {
         return this.exposeDuration * 2 + this.movingDuration + (param1 - 1) * this.movingDelay;
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
      
      public function prepareResultGrid(param1:int) : void
      {
         this.maxResultWidth = contentWidth - 2 * this.resultGridMargin;
         var _loc2_:Point = this.calculateGrid(param1);
         this.resultElementWidth = this.calculateElementSize(_loc2_);
         var _loc3_:int = param1 / _loc2_.x > 4?int(this.resultElementWidth * _loc2_.y):-1;
         this.resultGrid = new UIGrid(this.resultElementWidth * _loc2_.x,_loc2_.x,0,_loc3_);
         this.resultGrid.x = this.resultGridMargin + Math.round((this.maxResultWidth - this.resultElementWidth * _loc2_.x) / 2);
         this.resultGrid.y = Math.round(330 + (this.maxResultHeight - this.resultElementWidth * _loc2_.y) / 2);
         addChild(this.resultGrid);
      }
      
      public function buyMore(param1:int) : void
      {
         this._quantity = param1;
         removeChild(this.resultGrid);
         this.hideBuyButton();
      }
      
      public function showBuyButton() : void
      {
         new GTween(this.buySectionContainer,this.buyButtonAnimationDuration,{"alpha":1},{"ease":Sine.easeIn});
      }
      
      private function hideBuyButton() : void
      {
         new GTween(this.buySectionContainer,this.buyButtonAnimationDuration,{"alpha":0},{"ease":Sine.easeOut});
      }
      
      private function addBuyButton() : void
      {
         this.buySectionContainer = new Sprite();
         this.buySectionContainer.alpha = 0;
         this.spinner = new FixedNumbersSpinner(TextureParser.instance.getSliceScalingBitmap("UI","spinner_up_arrow"),0,new <int>[1,2,3,5,10],"x");
         if(this.info.isOnSale())
         {
            this.buyButton = new ShopBuyButton(this.info.saleAmount,this.info.saleCurrency);
         }
         else
         {
            this.buyButton = new ShopBuyButton(this.info.priceAmount,this.info.priceCurrency);
         }
         this.buyButton.width = 95;
         this.buyButton.showCampaignTooltip = true;
         this.buyButtonBackground = TextureParser.instance.getSliceScalingBitmap("UI","buy_button_background",this.buyButton.width + 60);
         this.buySectionContainer.addChild(this.buyButtonBackground);
         this.buySectionContainer.addChild(this.spinner);
         this.buySectionContainer.addChild(this.buyButton);
         this.buySectionContainer.x = 100;
         this.buySectionContainer.y = contentHeight - 45;
         this.buyButton.x = this.buyButtonBackground.width - this.buyButton.width - 6;
         this.buyButton.y = 4;
         this.spinner.y = -2;
         this.spinner.x = 32;
         addChild(this.buySectionContainer);
         this.buySectionContainer.x = Math.round((contentWidth - this.buySectionContainer.width) / 2);
      }
      
      private function animateGridElement(param1:UIItemContainer, param2:Number, param3:Boolean) : void
      {
         param1 = param1;
         param2 = param2;
         param3 = param3;
         var resultGridElement:UIItemContainer = null;
         var timeout:uint = 0;
         var element:UIItemContainer = param1;
         var delay:Number = param2;
         var triggerEventOnEnd:Boolean = param3;
         resultGridElement = new UIItemContainer(element.itemId,0,0,this.resultElementWidth);
         resultGridElement.alpha = 0;
         if(element.quantity > 1)
         {
            resultGridElement.showQuantityLabel(element.quantity);
         }
         resultGridElement.showTooltip = false;
         this.resultGrid.addGridElement(resultGridElement);
         var scale:Number = this.resultElementWidth / 80;
         var newX:Number = Math.abs(resultGridElement.x) + (this.resultGrid.x - this.rollGrid.x);
         var newY:Number = Math.abs(element.y - resultGridElement.y) + (this.resultGrid.y - this.rollGrid.y);
         var toPoint:Point = new Point(newX,newY);
         var tween1:GTween = new GTween(element,this.movingDuration,{
            "x":toPoint.x,
            "y":toPoint.y,
            "scaleX":scale,
            "scaleY":scale
         },{"ease":Sine.easeIn});
         tween1.delay = delay;
         tween1.onComplete = function():void
         {
            element.alpha = 0;
            resultGridElement.alpha = 1;
            resultGridElement.showTooltip = true;
            if(triggerEventOnEnd)
            {
               finishedShowingResult.dispatch();
            }
         };
         timeout = setTimeout(function():void
         {
            particleModalMap.doLightning(rollGrid.x + element.x + element.width / 2,rollGrid.y + element.y + element.height / 2,resultGrid.x + resultGridElement.x + resultGridElement.width / 2,resultGrid.y + resultGridElement.y + resultGridElement.height / 2,115,15787660,movingDuration);
         },delay + 0.2);
      }
      
      public function displayResult(param1:Array) : void
      {
         param1 = param1;
         var elements:Vector.<UIItemContainer> = null;
         var items:Array = param1;
         elements = this.displayItems(items);
         var resetTween:GTween = new GTween(this.rollGrid,this.exposeDuration,{
            "scaleX":1,
            "scaleY":1,
            "x":this.rollGrid.x,
            "y":this.rollGrid.y
         },{"ease":Sine.easeIn});
         resetTween.beginning();
         resetTween.onComplete = function():void
         {
            var _loc2_:* = null;
            var _loc1_:int = 0;
            var _loc4_:int = 0;
            var _loc3_:* = elements;
            for each(_loc2_ in elements)
            {
               _loc1_ = elements.indexOf(_loc2_);
               animateGridElement(_loc2_,_loc1_ * movingDelay,_loc1_ == elements.length - 1);
            }
         };
         var blinkTween:GTween = new GTween(this.rollGrid,this.exposeDuration,{
            "x":this.rollGrid.x - this.rollGrid.width * (this.exposeScale - 1) / 2,
            "y":this.rollGrid.y - this.rollGrid.height * (this.exposeScale - 1) / 2,
            "scaleX":this.exposeScale,
            "scaleY":this.exposeScale
         },{"ease":Sine.easeOut});
         blinkTween.nextTween = resetTween;
      }
      
      public function displayItems(param1:Array) : Vector.<UIItemContainer>
      {
         var _loc3_:* = null;
         var _loc5_:* = undefined;
         var _loc6_:* = null;
         var _loc4_:int = 0;
         this.rollGrid.clearGrid();
         var _loc2_:Vector.<UIItemContainer> = new Vector.<UIItemContainer>();
         var _loc10_:int = 0;
         var _loc9_:* = param1;
         for each(_loc3_ in param1)
         {
            var _loc8_:int = 0;
            var _loc7_:* = _loc3_;
            for(_loc5_ in _loc3_)
            {
               _loc6_ = new UIItemContainer(int(_loc5_),0,0,80);
               _loc4_ = _loc3_[_loc5_];
               if(_loc4_ > 1)
               {
                  _loc6_.showQuantityLabel(_loc4_);
               }
               _loc6_.showTooltip = false;
               _loc2_.push(_loc6_);
               this.rollGrid.addGridElement(_loc6_);
            }
         }
         this.rollGrid.render();
         if(!this.rollGrid.parent)
         {
            addChild(this.rollGrid);
         }
         this.rollGrid.x = this.spinnersContainer.x - this.rollGrid.width / 2;
         this.rollGrid.y = this.spinnersContainer.y - this.rollGrid.height / 2;
         return _loc2_;
      }
      
      private function createSpinners() : void
      {
         this.bigSpinner = new Spinner(50,true);
         this.littleSpinner = new Spinner(this.bigSpinner.degreesPerSecond * 1.5,true);
         var _loc1_:* = 0.7;
         this.littleSpinner.width = this.bigSpinner.width * _loc1_;
         this.littleSpinner.height = this.bigSpinner.height * _loc1_;
         var _loc2_:* = 0.7;
         this.bigSpinner.alpha = _loc2_;
         this.littleSpinner.alpha = _loc2_;
         this.spinnersContainer.addChild(this.bigSpinner);
         this.spinnersContainer.addChild(this.littleSpinner);
         this.littleSpinner.pause();
         addChild(this.spinnersContainer);
         this.spinnersContainer.x = contentWidth / 2;
         this.spinnersContainer.y = this.spinnerTopMargin;
      }
      
      override public function dispose() : void
      {
         this.rollGrid.dispose();
         if(this.resultGrid)
         {
            this.resultGrid.dispose();
         }
         this.buyButtonBackground.dispose();
         this.buyButton.dispose();
         this.spinner.dispose();
         this.particleModalMap.dispose();
         this.finishedShowingResult.removeAll();
      }
      
      public function get quantity() : int
      {
         return this._quantity;
      }
      
      public function get info() : MysteryBoxInfo
      {
         return this._info;
      }
   }
}
