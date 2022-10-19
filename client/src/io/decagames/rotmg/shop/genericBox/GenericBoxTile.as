package io.decagames.rotmg.shop.genericBox
{
   import flash.display.Sprite;
   import flash.geom.ColorTransform;
   import io.decagames.rotmg.shop.ShopBoxTag;
   import io.decagames.rotmg.shop.ShopBuyButton;
   import io.decagames.rotmg.shop.genericBox.data.GenericBoxInfo;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.gird.UIGridElement;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.spinner.FixedNumbersSpinner;
   import io.decagames.rotmg.ui.texture.TextureParser;
   import io.decagames.rotmg.utils.colors.Tint;
   import org.osflash.signals.Signal;
   
   public class GenericBoxTile extends UIGridElement
   {
       
      
      protected var background:SliceScalingBitmap;
      
      protected var backgroundTitle:SliceScalingBitmap;
      
      protected var buyButtonBitmapBackground:String;
      
      protected var backgroundButton:SliceScalingBitmap;
      
      protected var titleLabel:UILabel;
      
      protected var _buyButton:ShopBuyButton;
      
      protected var _infoButton:SliceScalingButton;
      
      protected var tags:Vector.<ShopBoxTag>;
      
      protected var _spinner:FixedNumbersSpinner;
      
      protected var _boxInfo:GenericBoxInfo;
      
      protected var _endTimeLabel:UILabel;
      
      protected var _startTimeLabel:UILabel;
      
      protected var originalPriceLabel:SalePriceTag;
      
      protected var _isPopup:Boolean;
      
      protected var _clickMask:Sprite;
      
      protected var boxHeight:int = 184;
      
      private var _isAvailable:Boolean = true;
      
      private var clickMaskAlpha:Number = 0;
      
      private var tagContainer:Sprite;
      
      protected var backgroundContainer:Sprite;
      
      public const selfRemoveSignal:Signal = new Signal();
      
      public function GenericBoxTile(param1:GenericBoxInfo, param2:Boolean = false)
      {
         super();
         this._boxInfo = param1;
         this._isPopup = param2;
         this.background = TextureParser.instance.getSliceScalingBitmap("UI","shop_box_background",10);
         this.tagContainer = new Sprite();
         if(!param2)
         {
            this.backgroundTitle = TextureParser.instance.getSliceScalingBitmap("UI","shop_title_background",10);
            this._infoButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","tab_info_button"));
         }
         if(this.buyButtonBitmapBackground)
         {
            this.backgroundButton = TextureParser.instance.getSliceScalingBitmap("UI",this.buyButtonBitmapBackground,10);
         }
         this._spinner = new FixedNumbersSpinner(TextureParser.instance.getSliceScalingBitmap("UI","spinner_up_arrow"),0,new <int>[1,2,3,5,10],"x");
         this._spinner.y = 131;
         this._spinner.x = 43;
         this.titleLabel = new UILabel();
         this.titleLabel.text = param1.title;
         DefaultLabelFormat.shopBoxTitle(this.titleLabel);
         if(param1.isOnSale())
         {
            this._buyButton = new ShopBuyButton(param1.saleAmount,param1.saleCurrency);
         }
         else
         {
            this._buyButton = new ShopBuyButton(param1.priceAmount,param1.priceCurrency);
         }
         this._buyButton.width = 95;
         if(param1.unitsLeft == 0)
         {
            this._buyButton.soldOut = true;
         }
         this._buyButton.showCampaignTooltip = true;
         this.tags = new Vector.<ShopBoxTag>(0);
         addChild(this.background);
         this.createBoxBackground();
         if(this.backgroundTitle)
         {
            addChild(this.backgroundTitle);
         }
         this._clickMask = new Sprite();
         this._clickMask.graphics.beginFill(16711680,this.clickMaskAlpha);
         this._clickMask.graphics.drawRect(0,0,95,this.boxHeight);
         this._clickMask.graphics.endFill();
         addChild(this._clickMask);
         if(this.backgroundButton)
         {
            addChild(this.backgroundButton);
         }
         addChild(this.titleLabel);
         if(param1.isOnSale())
         {
            this.originalPriceLabel = new SalePriceTag(param1.priceAmount,param1.priceCurrency);
            addChild(this.originalPriceLabel);
         }
         addChild(this._buyButton);
         addChild(this._spinner);
         if(!param2)
         {
            addChild(this._infoButton);
         }
         addChild(this.tagContainer);
         this.createBoxTags();
         this.createEndTime();
         this.createStartTime();
         this.updateStartTimeString(this.background.width);
         this.updateTimeEndString(this.background.width);
         param1.updateSignal.add(this.updateBox);
      }
      
      private function updateBox() : void
      {
         var _loc1_:ShopBoxTag = this.getTagByName("LEFT_TAG");
         if(_loc1_)
         {
            _loc1_.updateLabel(this.getLeftUnits() + " LEFT!");
         }
         if(this.boxInfo.unitsLeft == 0 || this.boxInfo.purchaseLeft == 0)
         {
            this.triggerSelfRemove();
         }
      }
      
      private function createEndTime() : void
      {
         this._endTimeLabel = new UILabel();
         this._endTimeLabel.y = 28;
         this._endTimeLabel.visible = false;
         addChild(this._endTimeLabel);
         if(this._isPopup)
         {
            DefaultLabelFormat.popupEndsIn(this._endTimeLabel);
         }
         else
         {
            DefaultLabelFormat.mysteryBoxEndsIn(this._endTimeLabel);
         }
      }
      
      private function createStartTime() : void
      {
         this._startTimeLabel = new UILabel();
         this._startTimeLabel.y = 28;
         this._startTimeLabel.visible = false;
         addChild(this._startTimeLabel);
         if(this._isPopup)
         {
            DefaultLabelFormat.popupStartsIn(this._startTimeLabel);
         }
         else
         {
            DefaultLabelFormat.mysteryBoxStartsIn(this._startTimeLabel);
         }
      }
      
      private function getTagByName(param1:String) : ShopBoxTag
      {
         var _loc2_:ShopBoxTag = null;
         var _loc4_:int = 0;
         var _loc3_:* = this.tags;
         for each(_loc2_ in this.tags)
         {
            if(_loc2_.tagName == param1)
            {
               return _loc2_;
            }
         }
         return null;
      }
      
      private function createBoxTags() : void
      {
         var _loc1_:String = null;
         this.clearTagContainer();
         if(this._boxInfo.isNew())
         {
            this.addTag(new ShopBoxTag("NEW_TAG","shop_blue_tag","NEW",this._isPopup));
         }
         var _loc2_:Array = this._boxInfo.tags.split(",");
         var _loc5_:int = 0;
         var _loc4_:* = _loc2_;
         for each(_loc1_ in _loc2_)
         {
            var _loc3_:* = _loc1_;
            switch(_loc3_)
            {
               case "best_seller":
                  this.addTag(new ShopBoxTag("BEST_TAG","shop_green_tag","BEST",this._isPopup));
                  continue;
               case "hot":
                  this.addTag(new ShopBoxTag("HOT_TAG","shop_orange_tag","HOT",this._isPopup));
                  continue;
               default:
                  continue;
            }
         }
         if(this._boxInfo.isOnSale())
         {
            this.addTag(new ShopBoxTag("PROMOTION_TAG","shop_red_tag",this.calculateBoxPromotionPercent(this._boxInfo) + "% OFF",this._isPopup));
         }
         if(this._boxInfo.unitsLeft != -1 || this._boxInfo.purchaseLeft != -1)
         {
            this.addTag(new ShopBoxTag("LEFT_TAG","shop_purple_tag",this.getLeftUnits() + " LEFT!",this._isPopup));
         }
      }
      
      private function getLeftUnits() : int
      {
         var _loc2_:int = this._boxInfo.unitsLeft == -1?2147483647:int(this._boxInfo.unitsLeft);
         var _loc1_:int = this._boxInfo.purchaseLeft == -1?2147483647:int(this._boxInfo.purchaseLeft);
         return Math.min(_loc2_,_loc1_);
      }
      
      private function clearTagContainer() : void
      {
         var _loc2_:ShopBoxTag = null;
         if(!this.tagContainer)
         {
            return;
         }
         while(this.tagContainer.numChildren > 0)
         {
            this.tagContainer.removeChildAt(0);
         }
         var _loc3_:int = this.tags.length;
         var _loc1_:int = _loc3_ - 1;
         while(_loc1_ >= 0)
         {
            _loc2_ = this.tags.pop();
            _loc2_.dispose();
            _loc2_ = null;
            _loc1_--;
         }
      }
      
      private function calculateBoxPromotionPercent(param1:GenericBoxInfo) : int
      {
         return (param1.priceAmount - param1.saleAmount) / param1.priceAmount * 100;
      }
      
      protected function createBoxBackground() : void
      {
      }
      
      protected function updateTimeEndString(param1:int) : void
      {
         if(!this._isAvailable)
         {
            return;
         }
         var _loc2_:String = this.boxInfo.getEndTimeString();
         var _loc3_:String = this.boxInfo.getStartTimeString();
         if(_loc3_ == "" && _loc2_)
         {
            this._endTimeLabel.text = _loc2_;
            this._endTimeLabel.x = (param1 - this._endTimeLabel.width) / 2;
            this._endTimeLabel.visible = true;
         }
         else
         {
            this._endTimeLabel.text = "";
         }
      }
      
      protected function updateStartTimeString(param1:int) : void
      {
         var _loc2_:String = this.boxInfo.getStartTimeString();
         if(_loc2_)
         {
            this._startTimeLabel.text = _loc2_;
            this._startTimeLabel.x = (param1 - this._startTimeLabel.width) / 2;
            this.isAvailable = false;
            this._startTimeLabel.visible = true;
         }
         else
         {
            this.isAvailable = true;
            this._startTimeLabel.text = "";
            this._startTimeLabel.visible = false;
         }
      }
      
      private function set isAvailable(param1:Boolean) : void
      {
         var _loc2_:* = NaN;
         if(this._isAvailable == param1)
         {
            return;
         }
         if(param1)
         {
            this.createBoxTags();
            this._buyButton.disabled = false;
            this.background.transform.colorTransform = new ColorTransform();
            if(!this._isPopup)
            {
               this.backgroundTitle.transform.colorTransform = new ColorTransform();
               if(this._infoButton.alpha != 0)
               {
                  this._infoButton.transform.colorTransform = new ColorTransform();
               }
            }
            this._spinner.transform.colorTransform = new ColorTransform();
            this.titleLabel.transform.colorTransform = new ColorTransform();
            this._buyButton.transform.colorTransform = new ColorTransform();
            if(this.backgroundContainer)
            {
               this.backgroundContainer.transform.colorTransform = new ColorTransform();
            }
            if(this.buyButtonBitmapBackground)
            {
               this.backgroundButton.transform.colorTransform = new ColorTransform();
            }
         }
         else
         {
            _loc2_ = 0.3;
            Tint.add(this.background,0,_loc2_);
            if(!this._isPopup)
            {
               Tint.add(this.backgroundTitle,0,_loc2_);
               if(this._infoButton.alpha != 0)
               {
                  Tint.add(this._infoButton,0,_loc2_);
               }
            }
            Tint.add(this._spinner,0,_loc2_);
            Tint.add(this.titleLabel,0,_loc2_);
            Tint.add(this._buyButton,0,_loc2_);
            if(this.backgroundContainer)
            {
               Tint.add(this.backgroundContainer,0,_loc2_);
            }
            this._buyButton.disabled = true;
            if(this.buyButtonBitmapBackground)
            {
               Tint.add(this.backgroundButton,0,_loc2_);
            }
         }
         this._isAvailable = param1;
      }
      
      override public function get height() : Number
      {
         return this.background.height;
      }
      
      override public function resize(param1:int, param2:int = -1) : void
      {
         this.background.width = param1;
         this.backgroundTitle.width = param1;
         this.backgroundButton.width = param1;
         this.background.height = this.boxHeight;
         this.backgroundTitle.y = 2;
         this.titleLabel.x = Math.round((param1 - this.titleLabel.textWidth) / 2);
         this.titleLabel.y = 6;
         if(this.backgroundButton)
         {
            this.backgroundButton.y = 133;
            this._buyButton.y = this.backgroundButton.y + 4;
            this._buyButton.x = param1 - 110;
         }
         else
         {
            this._buyButton.y = 137;
            this._buyButton.x = param1 - 110;
         }
         if(this._infoButton)
         {
            this._infoButton.x = 130;
            this._infoButton.y = 45;
         }
         this.updateSaleLabel();
         this.updateClickMask(param1);
         this.updateStartTimeString(param1);
         this.updateTimeEndString(param1);
      }
      
      protected function updateClickMask(param1:int) : void
      {
         var _loc2_:int = 0;
         if(!this._isPopup)
         {
            this.backgroundTitle = TextureParser.instance.getSliceScalingBitmap("UI","shop_title_background",10);
            _loc2_ = this.backgroundTitle.y + this.backgroundTitle.height + 2;
            this._clickMask.y = _loc2_;
         }
         if(this.backgroundButton)
         {
            this.boxHeight = this.boxHeight - (this.boxHeight - this.backgroundButton.y + 4);
         }
         this._clickMask.graphics.clear();
         this._clickMask.graphics.beginFill(16711680,this.clickMaskAlpha);
         this._clickMask.graphics.drawRect(0,0,param1,this.boxHeight - _loc2_);
         this._clickMask.graphics.endFill();
      }
      
      protected function updateSaleLabel() : void
      {
         if(this.originalPriceLabel)
         {
            this.originalPriceLabel.y = this._buyButton.y - 23;
            this.originalPriceLabel.x = this._buyButton.x + this._buyButton.width - this.originalPriceLabel.width - 13;
         }
      }
      
      override public function update() : void
      {
         this.updateStartTimeString(this.background.width);
         this.updateTimeEndString(this.background.width);
         if(!this._isPopup && (this._startTimeLabel.text != "" || this._endTimeLabel.text != ""))
         {
            this.tagContainer.y = 10;
         }
         else
         {
            this.tagContainer.y = 0;
         }
         if(this._isAvailable && this._endTimeLabel.text == "" && this.boxInfo.endTime)
         {
            this.triggerSelfRemove();
         }
      }
      
      private function triggerSelfRemove() : void
      {
         this.selfRemoveSignal.dispatch();
         this.selfRemoveSignal.removeAll();
      }
      
      public function addTag(param1:ShopBoxTag) : void
      {
         this.tagContainer.addChild(param1);
         param1.y = 33 + this.tags.length * param1.height;
         param1.x = -5;
         this.tags.push(param1);
      }
      
      public function get spinner() : FixedNumbersSpinner
      {
         return this._spinner;
      }
      
      public function get buyButton() : ShopBuyButton
      {
         return this._buyButton;
      }
      
      public function get boxInfo() : GenericBoxInfo
      {
         return this._boxInfo;
      }
      
      override public function dispose() : void
      {
         var _loc1_:ShopBoxTag = null;
         this.boxInfo.updateSignal.remove(this.updateBox);
         this.selfRemoveSignal.removeAll();
         this.background.dispose();
         if(this.backgroundTitle)
         {
            this.backgroundTitle.dispose();
         }
         this.backgroundButton.dispose();
         this._buyButton.dispose();
         if(this._infoButton)
         {
            this._infoButton.dispose();
         }
         this._spinner.dispose();
         if(this.originalPriceLabel)
         {
            this.originalPriceLabel.dispose();
         }
         var _loc3_:int = 0;
         var _loc2_:* = this.tags;
         for each(_loc1_ in this.tags)
         {
            _loc1_.dispose();
         }
         this.tags = null;
         super.dispose();
      }
      
      public function get infoButton() : SliceScalingButton
      {
         return this._infoButton;
      }
      
      public function get isPopup() : Boolean
      {
         return this._isPopup;
      }
      
      public function get clickMask() : Sprite
      {
         return this._clickMask;
      }
   }
}
