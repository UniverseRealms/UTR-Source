package io.decagames.rotmg.dailyQuests.view.popup
{
   import flash.display.Sprite;
   import io.decagames.rotmg.dailyQuests.model.DailyQuest;
   import io.decagames.rotmg.dailyQuests.utils.SlotsRendered;
   import io.decagames.rotmg.dailyQuests.view.slot.DailyQuestItemSlot;
   import io.decagames.rotmg.ui.buttons.SliceScalingButton;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.modal.ModalPopup;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class DailyQuestRedeemPopup extends ModalPopup
   {
       
      
      private var w_:int = 326;
      
      private var h_:int = 238;
      
      private var _thanksButton:SliceScalingButton;
      
      private var slots:Vector.<DailyQuestItemSlot>;
      
      private var slotContainerPosition:int = 15;
      
      public function DailyQuestRedeemPopup(param1:DailyQuest, param2:int = -1)
      {
         super(this.w_, this.h_, "Quest Complete");
         var _loc3_:SliceScalingBitmap = TextureParser.instance.getSliceScalingBitmap("UI","popup_content_inset",this.w_);
         _loc3_.height = 117;
         addChild(_loc3_);
         this.slots = new Vector.<DailyQuestItemSlot>();
         var _loc5_:SliceScalingBitmap = new TextureParser().getSliceScalingBitmap("UI","main_button_decoration",194);
         addChild(_loc5_);
         _loc5_.y = 179;
         _loc5_.x = Math.round((this.w_ - _loc5_.width) / 2);
         this._thanksButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
         this._thanksButton.setLabel("Thanks",DefaultLabelFormat.questButtonCompleteLabel);
         this._thanksButton.width = 149;
         addChild(this._thanksButton);
         this._thanksButton.x = Math.round((this.w_ - 149) / 2);
         this._thanksButton.y = 185;
         var _loc6_:Sprite = new Sprite();
         addChild(_loc6_);
         if(param1.itemOfChoice)
         {
            SlotsRendered.renderSlots(new <int>[param2],new Vector.<int>(),"reward",_loc6_,this.slotContainerPosition,4,this.w_,this.slots);
         }
         else
         {
            SlotsRendered.renderSlots(param1.rewards,new Vector.<int>(),"reward",_loc6_,this.slotContainerPosition,4,this.w_,this.slots);
         }
         var _loc4_:UILabel = new UILabel();
         DefaultLabelFormat.questRefreshLabel(_loc4_);
         _loc4_.width = this.w_;
         _loc4_.autoSize = "center";
         _loc4_.text = "Rewards are sent to the Gift Chest!";
         _loc4_.y = 140;
         addChild(_loc4_);
      }
      
      public function get thanksButton() : SliceScalingButton
      {
         return this._thanksButton;
      }
   }
}
