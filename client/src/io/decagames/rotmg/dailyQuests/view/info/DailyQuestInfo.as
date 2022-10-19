package io.decagames.rotmg.dailyQuests.view.info
{
   import flash.display.Sprite;
   import io.decagames.rotmg.dailyQuests.model.DailyQuest;
   import io.decagames.rotmg.dailyQuests.utils.SlotsRendered;
   import io.decagames.rotmg.dailyQuests.view.slot.DailyQuestItemSlot;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class DailyQuestInfo extends Sprite
   {
      
      public static var INFO_WIDTH:int = 328;
      
      public static const INFO_HEIGHT:int = 434;
       
      
      private var contentInset:SliceScalingBitmap;
      
      private var contentTitle:SliceScalingBitmap;
      
      private var contentButton:SliceScalingBitmap;
      
      private var contentDivider:SliceScalingBitmap;
      
      private var contentDividerTitle:SliceScalingBitmap;
      
      private var questName:UILabel;
      
      private var questDescription:UILabel;
      
      private var rewardsTitle:UILabel;
      
      private var rewardsChoice:UILabel;
      
      private var questAvailable:UILabel;
      
      private var refreshInfo:UILabel;
      
      private var slots:Vector.<DailyQuestItemSlot>;
      
      private var slotMargin:int = 4;
      
      private var requirementsTopMargin:int = 100;
      
      private var rewardsTopMargin:int = 255;
      
      private var requirementsContainer:Sprite;
      
      private var rewardsContainer:Sprite;
      
      private var _completeButton:DailyQuestCompleteButton;
      
      private var _playerEquipment:Vector.<int>;
      
      public function DailyQuestInfo()
      {
         super();
         this.init();
      }
      
      public static function hasAllItems(param1:Vector.<int>, param2:Vector.<int>) : Boolean
      {
         var _loc4_:int = 0;
         var _loc5_:int = 0;
         var _loc3_:Vector.<int> = param1.concat();
         var _loc7_:int = 0;
         var _loc6_:* = param2;
         for each(_loc4_ in param2)
         {
            _loc5_ = _loc3_.indexOf(_loc4_);
            if(_loc5_ >= 0)
            {
               _loc3_.splice(_loc5_,1);
            }
         }
         return _loc3_.length == 0;
      }
      
      private function init() : void
      {
         this.createContentInset();
         this.createContentTitle();
         this.createContentButton();
         this.createContentDivider();
         this.createContentDividerTitle();
         this.createQuestName();
         this.createContainers();
         this.createQuestDescription();
         this.createRewardsTitle();
         this.createRewardChoice();
         this.createQuestAvailable();
         this.createRefreshInfo();
         this.createCompleteButton();
      }
      
      private function createContentInset() : void
      {
         this.contentInset = TextureParser.instance.getSliceScalingBitmap("UI","popup_content_inset",325);
         this.contentInset.height = 425;
         this.contentInset.x = 0;
         this.contentInset.y = 0;
         addChild(this.contentInset);
      }
      
      private function createContentTitle() : void
      {
         this.contentTitle = TextureParser.instance.getSliceScalingBitmap("UI","content_title_decoration",325);
         this.contentTitle.x = 0;
         this.contentTitle.y = 0;
         addChild(this.contentTitle);
      }
      
      private function createContentButton() : void
      {
         this.contentButton = TextureParser.instance.getSliceScalingBitmap("UI","content_button_decoration",325);
         this.contentButton.x = 0;
         this.contentButton.y = 340;
         addChild(this.contentButton);
      }
      
      private function createContentDivider() : void
      {
         this.contentDivider = TextureParser.instance.getSliceScalingBitmap("UI","content_divider",305);
         this.contentDivider.x = 10;
         this.contentDivider.y = 203;
         addChild(this.contentDivider);
      }
      
      private function createContentDividerTitle() : void
      {
         this.contentDividerTitle = TextureParser.instance.getSliceScalingBitmap("UI","content_divider_title",145);
         this.contentDividerTitle.x = this.contentInset.width / 2 - this.contentDividerTitle.width / 2;
         this.contentDividerTitle.y = 193;
         addChild(this.contentDividerTitle);
      }
      
      private function createQuestName() : void
      {
         this.questName = new UILabel();
         DefaultLabelFormat.questNameLabel(this.questName);
         this.questName.width = INFO_WIDTH;
         this.questName.wordWrap = true;
         this.questName.x = 0;
         this.questName.y = 6;
         addChild(this.questName);
      }
      
      private function createContainers() : void
      {
         this.requirementsContainer = new Sprite();
         addChild(this.requirementsContainer);
         this.rewardsContainer = new Sprite();
         addChild(this.rewardsContainer);
      }
      
      private function createQuestDescription() : void
      {
         this.questDescription = new UILabel();
         DefaultLabelFormat.questDescriptionLabel(this.questDescription);
         this.questDescription.width = INFO_WIDTH - 30;
         this.questDescription.wordWrap = true;
         this.questDescription.multiline = true;
         this.questDescription.x = 15;
         this.questDescription.y = 44;
         addChild(this.questDescription);
      }
      
      private function createRewardsTitle() : void
      {
         this.rewardsTitle = new UILabel();
         DefaultLabelFormat.questRewardLabel(this.rewardsTitle);
         this.rewardsTitle.text = "Rewards";
         this.rewardsTitle.x = this.contentInset.width / 2 - this.rewardsTitle.width / 2;
         this.rewardsTitle.y = 200;
         addChild(this.rewardsTitle);
      }
      
      private function createRewardChoice() : void
      {
         this.rewardsChoice = new UILabel();
         DefaultLabelFormat.questChoiceLabel(this.rewardsChoice);
         this.rewardsChoice.text = "Choose one of the following Items";
         this.rewardsChoice.x = this.contentInset.width / 2 - this.rewardsChoice.width / 2;
         this.rewardsChoice.y = 230;
         addChild(this.rewardsChoice);
      }
      
      private function createQuestAvailable() : void
      {
         this.questAvailable = new UILabel();
         DefaultLabelFormat.createLabelFormat(this.questAvailable,12,10724259,"center",true);
         this.questAvailable.y = 280;
         addChild(this.questAvailable);
      }
      
      private function createRefreshInfo() : void
      {
         this.refreshInfo = new UILabel();
         DefaultLabelFormat.createLabelFormat(this.refreshInfo,12,65280,"center",true);
         this.refreshInfo.width = 170;
         this.refreshInfo.multiline = true;
         this.refreshInfo.wordWrap = true;
         this.refreshInfo.text = "You can also refresh Quests up to 2 times per day!";
         this.refreshInfo.x = (this.contentInset.width - this.refreshInfo.width) / 2;
         this.refreshInfo.y = this.questAvailable.y + 18;
         addChild(this.refreshInfo);
      }
      
      private function createCompleteButton() : void
      {
         this._completeButton = new DailyQuestCompleteButton();
         this._completeButton.x = 92;
         this._completeButton.y = 370;
      }
      
      public function dailyQuestsCompleted() : void
      {
         var _loc1_:DailyQuestItemSlot = null;
         this.questName.text = "Quests Completed!";
         this.questDescription.text = "Congratulation, you have completed all quests for today!";
         this.showQuestsCompleteInfo(true);
         var _loc3_:int = 0;
         var _loc2_:* = this.slots;
         for each(_loc1_ in this.slots)
         {
            _loc1_.parent.removeChild(_loc1_);
         }
         if(!this.slots)
         {
            this.slots = new Vector.<DailyQuestItemSlot>();
         }
         else
         {
            this.slots.length = 0;
         }
      }
      
      public function eventQuestsCompleted() : void
      {
         var _loc1_:DailyQuestItemSlot = null;
         this.questName.text = "No Event Quests!";
         this.questDescription.text = "There are no Event quests currently available. Come back later!";
         this.showQuestsCompleteInfo(false,false);
         var _loc3_:int = 0;
         var _loc2_:* = this.slots;
         for each(_loc1_ in this.slots)
         {
            _loc1_.parent.removeChild(_loc1_);
         }
         if(!this.slots)
         {
            this.slots = new Vector.<DailyQuestItemSlot>();
         }
         else
         {
            this.slots.length = 0;
         }
      }
      
      private function showQuestsCompleteInfo(param1:Boolean, param2:Boolean = true) : void
      {
         this.questAvailable.visible = param1;
         this.refreshInfo.visible = param1;
         this.rewardsChoice.visible = !!param2?Boolean(!param1):Boolean(Boolean(param1));
         this.rewardsTitle.visible = !!param2?Boolean(!param1):Boolean(Boolean(param1));
         this.contentDivider.visible = !!param2?Boolean(!param1):Boolean(Boolean(param1));
         this.contentDividerTitle.visible = !!param2?Boolean(!param1):Boolean(Boolean(param1));
         this._completeButton.visible = !!param2?Boolean(!param1):Boolean(Boolean(param1));
      }
      
      public function show(param1:DailyQuest, param2:Vector.<int>) : void
      {
         this._playerEquipment = param2.concat();
         this.showQuestsCompleteInfo(false);
         this.rewardsChoice.visible = param1.itemOfChoice;
         this.questName.text = param1.name;
         this.questDescription.text = param1.description;
         SlotsRendered.renderSlots(param1.requirements,this._playerEquipment,"requirement",this.requirementsContainer,this.requirementsTopMargin,this.slotMargin,INFO_WIDTH,this.slots);
         SlotsRendered.renderSlots(param1.rewards,this._playerEquipment,"reward",this.rewardsContainer,this.rewardsTopMargin,this.slotMargin,INFO_WIDTH,this.slots,param1.itemOfChoice);
         this._completeButton.disabled = !!param1.completed?Boolean(true):Boolean(!hasAllItems(param1.requirements,param2));
         this._completeButton.completed = param1.completed;
         addChild(this._completeButton);
      }
      
      public function get completeButton() : DailyQuestCompleteButton
      {
         return this._completeButton;
      }
      
      public function setQuestAvailableTime(param1:String) : void
      {
         this.questAvailable.text = param1;
         this.questAvailable.x = (this.contentInset.width - this.questAvailable.width) / 2;
      }
   }
}
