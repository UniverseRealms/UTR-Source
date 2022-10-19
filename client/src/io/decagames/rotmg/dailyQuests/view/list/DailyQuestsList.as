package io.decagames.rotmg.dailyQuests.view.list
{
   import flash.display.Sprite;
   import io.decagames.rotmg.ui.buttons.BaseButton;
   import io.decagames.rotmg.ui.scroll.UIScrollbar;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.tabs.TabButton;
   import io.decagames.rotmg.ui.tabs.UITab;
   import io.decagames.rotmg.ui.tabs.UITabs;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class DailyQuestsList extends Sprite
   {
      
      public static const QUEST_TAB_LABEL:String = "Quests";
      
      public static const EVENT_TAB_LABEL:String = "Events";
      
      public static const SCROLL_BAR_HEIGHT:int = 345;
       
      
      private var questLinesPosition:int = 0;
      
      private var eventLinesPosition:int = 0;
      
      private var questsContainer:Sprite;
      
      private var eventsContainer:Sprite;
      
      private var _tabs:UITabs;
      
      private var eventsTab:TabButton;
      
      private var contentTabs:SliceScalingBitmap;
      
      private var contentInset:SliceScalingBitmap;
      
      private var _dailyQuestElements:Vector.<DailyQuestListElement>;
      
      private var _eventQuestElements:Vector.<DailyQuestListElement>;
      
      public function DailyQuestsList()
      {
         super();
         this.init();
      }
      
      private function init() : void
      {
         this.createContentTabs();
         this.createContentInset();
         this.createTabs();
      }
      
      private function createTabs() : void
      {
         this._tabs = new UITabs(230,true);
         this._tabs.addTab(this.createQuestsTab(),true);
         this._tabs.addTab(this.createEventsTab());
         this._tabs.y = 1;
         addChild(this._tabs);
      }
      
      private function createContentInset() : void
      {
         this.contentInset = TextureParser.instance.getSliceScalingBitmap("UI","popup_content_inset",230);
         this.contentInset.height = 360;
         this.contentInset.y = 35;
         addChild(this.contentInset);
      }
      
      private function createContentTabs() : void
      {
         this.contentTabs = TextureParser.instance.getSliceScalingBitmap("UI","tab_inset_content_background",230);
         this.contentTabs.height = 45;
         addChild(this.contentTabs);
      }
      
      private function createQuestsTab() : UITab
      {
         var _loc4_:Sprite = null;
         var _loc3_:UITab = new UITab("Quests");
         var _loc1_:Sprite = new Sprite();
         this.questsContainer = new Sprite();
         this.questsContainer.x = this.contentInset.x;
         this.questsContainer.y = 10;
         _loc1_.addChild(this.questsContainer);
         var scrollBar:UIScrollbar = new UIScrollbar(345);
         scrollBar.mouseRollSpeedFactor = 1;
         scrollBar.scrollObject = _loc3_;
         scrollBar.content = this.questsContainer;
         _loc1_.addChild(scrollBar);
         scrollBar.x = this.contentInset.x + this.contentInset.width - 25;
         scrollBar.y = 7;
         _loc4_ = new Sprite();
         _loc4_.graphics.beginFill(0);
         _loc4_.graphics.drawRect(0,0,230,345);
         _loc4_.x = this.questsContainer.x;
         _loc4_.y = this.questsContainer.y;
         this.questsContainer.mask = _loc4_;
         _loc1_.addChild(_loc4_);
         _loc3_.addContent(_loc1_);
         return _loc3_;
      }
      
      private function createEventsTab() : UITab
      {
         var _loc3_:UITab = null;
         var _loc1_:Sprite = null;
         var _loc2_:UIScrollbar = null;
         _loc3_ = null;
         _loc3_ = new UITab("Events");
         _loc1_ = new Sprite();
         this.eventsContainer = new Sprite();
         this.eventsContainer.x = this.contentInset.x;
         this.eventsContainer.y = 10;
         _loc1_.addChild(this.eventsContainer);
         _loc2_ = new UIScrollbar(345);
         _loc2_.mouseRollSpeedFactor = 1;
         _loc2_.scrollObject = _loc3_;
         _loc2_.content = this.eventsContainer;
         _loc1_.addChild(_loc2_);
         _loc2_.x = this.contentInset.x + this.contentInset.width - 25;
         _loc2_.y = 7;
         var _loc4_:Sprite = new Sprite();
         _loc4_.graphics.beginFill(0);
         _loc4_.graphics.drawRect(0,0,230,345);
         _loc4_.x = this.eventsContainer.x;
         _loc4_.y = this.eventsContainer.y;
         this.eventsContainer.mask = _loc4_;
         _loc1_.addChild(_loc4_);
         _loc3_.addContent(_loc1_);
         return _loc3_;
      }
      
      public function addIndicator(param1:Boolean) : void
      {
         this.eventsTab = this._tabs.getTabButtonByLabel("Events");
         if(this.eventsTab)
         {
            this.eventsTab.showIndicator = param1;
            this.eventsTab.clickSignal.add(this.onEventsClick);
         }
      }
      
      private function onEventsClick(param1:BaseButton) : void
      {
         if(TabButton(param1).hasIndicator)
         {
            TabButton(param1).showIndicator = false;
         }
      }
      
      public function addQuestToList(param1:DailyQuestListElement) : void
      {
         if(!this._dailyQuestElements)
         {
            this._dailyQuestElements = new Vector.<DailyQuestListElement>(0);
         }
         param1.x = 10;
         param1.y = this.questLinesPosition * 35;
         this.questsContainer.addChild(param1);
         this.questLinesPosition++;
         this._dailyQuestElements.push(param1);
      }
      
      public function addEventToList(param1:DailyQuestListElement) : void
      {
         if(!this._eventQuestElements)
         {
            this._eventQuestElements = new Vector.<DailyQuestListElement>(0);
         }
         param1.x = 10;
         param1.y = this.eventLinesPosition * 35;
         this.eventsContainer.addChild(param1);
         this.eventLinesPosition++;
         this._eventQuestElements.push(param1);
      }
      
      public function get list() : Sprite
      {
         return this.questsContainer;
      }
      
      public function get tabs() : UITabs
      {
         return this._tabs;
      }
      
      public function clearQuestLists() : void
      {
         var _loc1_:DailyQuestListElement = null;
         while(this.questsContainer.numChildren > 0)
         {
            _loc1_ = this.questsContainer.removeChildAt(0) as DailyQuestListElement;
            _loc1_ = null;
         }
         this.questLinesPosition = 0;
         while(this.eventsContainer.numChildren > 0)
         {
            _loc1_ = this.eventsContainer.removeChildAt(0) as DailyQuestListElement;
            _loc1_ = null;
         }
         this.eventLinesPosition = 0;
      }
      
      public function getCurrentlySelected(param1:String) : DailyQuestListElement
      {
         var _loc6_:int = 0;
         var _loc5_:* = undefined;
         var _loc8_:int = 0;
         var _loc7_:* = undefined;
         var _loc2_:* = null;
         var _loc3_:DailyQuestListElement = null;
         var _loc4_:DailyQuestListElement = null;
         if(param1 == "Quests")
         {
            _loc6_ = 0;
            _loc5_ = this._dailyQuestElements;
            for each(_loc3_ in this._dailyQuestElements)
            {
               if(_loc3_.isSelected)
               {
                  _loc2_ = _loc3_;
                  break;
               }
            }
         }
         else if(param1 == "Events")
         {
            _loc8_ = 0;
            _loc7_ = this._eventQuestElements;
            for each(_loc4_ in this._eventQuestElements)
            {
               if(_loc4_.isSelected)
               {
                  _loc2_ = _loc4_;
                  break;
               }
            }
         }
         return _loc2_;
      }
   }
}
