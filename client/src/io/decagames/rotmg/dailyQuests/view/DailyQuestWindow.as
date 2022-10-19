package io.decagames.rotmg.dailyQuests.view
{
   import com.company.assembleegameclient.map.ParticleModalMap;
   import flash.display.Sprite;
   import io.decagames.rotmg.dailyQuests.view.info.DailyQuestInfo;
   import io.decagames.rotmg.dailyQuests.view.list.DailyQuestsList;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.popups.UIPopup;
   
   public class DailyQuestWindow extends UIPopup
   {
      
      public static const MODAL_WIDTH:int = 600;
      
      public static const MODAL_FULL_WIDTH:int = 800;
      
      public static const MODAL_HEIGHT:int = 600;
       
      
      private var fade:Sprite;
      
      private var _closeButton:Sprite;
      
      private var _infoButton:Sprite;
      
      private var _refreshButton:DailyQuestRefreshButton;
      
      private var _questList:DailyQuestsList;
      
      private var questInfo:DailyQuestInfo;
      
      private var questRefreshText:UILabel;
      
      private var completedCounter:UILabel;
      
      private var completedTxt:UILabel;
      
      private var particleLayer:ParticleModalMap;
      
      private var _contentContainer:Sprite;
      
      public function DailyQuestWindow()
      {
         super(600,600);
         this.init();
      }
      
      private function init() : void
      {
         this.createContainer();
         this.createQuestRefreshText();
         this.createRefreshButton();
         this.renderQuestInfo();
         this.renderList();
      }
      
      private function createContainer() : void
      {
         this._contentContainer = new Sprite();
         this._contentContainer.y = 120;
         this._contentContainer.x = 10;
         addChild(this._contentContainer);
      }
      
      private function createQuestRefreshText() : void
      {
         this.questRefreshText = new UILabel();
         DefaultLabelFormat.questRefreshLabel(this.questRefreshText);
         this.questRefreshText.x = -10;
         this.questRefreshText.y = 11;
         this.questRefreshText.width = 230;
         this.questRefreshText.wordWrap = true;
         this._contentContainer.addChild(this.questRefreshText);
      }
      
      private function createRefreshButton() : void
      {
         this._refreshButton = new DailyQuestRefreshButton();
         this._refreshButton.x = this.questRefreshText.x + this.questRefreshText.width - 16;
         this._refreshButton.y = 3;
         this._contentContainer.addChild(this._refreshButton);
      }
      
      public function setCompletedCounter(param1:int, param2:int) : void
      {
         if(param1 == param2)
         {
            DefaultLabelFormat.questCompletedLabel(this.completedTxt,true,false);
            DefaultLabelFormat.questCompletedLabel(this.completedCounter,true,true);
         }
         this.completedCounter.text = param1 + "/" + param2;
         this.completedCounter.x = this.completedCounter.x - this.completedCounter.textWidth;
      }
      
      public function setQuestRefreshHeader(param1:String) : void
      {
         this.questRefreshText.text = param1;
      }
      
      public function renderQuestInfo() : void
      {
         if(this.questInfo && this.questInfo.parent)
         {
            this.questInfo.parent.removeChild(this.questInfo);
         }
         this.questInfo = new DailyQuestInfo();
         this.questInfo.x = 257;
         this.questInfo.y = 130;
         addChild(this.questInfo);
      }
      
      public function renderList() : void
      {
         if(this._questList && this._questList.parent)
         {
            removeChild(this._questList);
         }
         this._questList = new DailyQuestsList();
         this._questList.x = 20;
         this._questList.y = 160;
         addChild(this._questList);
      }
      
      public function showFade(param1:int = 1381653, param2:Boolean = false) : void
      {
         if(param2)
         {
            this.particleLayer = new ParticleModalMap(1);
            addChild(this.particleLayer);
         }
         else
         {
            this.fade = new Sprite();
            this.fade.graphics.clear();
            this.fade.graphics.beginFill(param1,0.8);
            this.fade.graphics.drawRect(0,0,800,600);
            addChild(this.fade);
         }
      }
      
      public function hideFade() : void
      {
         if(this.fade && this.fade.parent)
         {
            removeChild(this.fade);
         }
         if(this.particleLayer && this.particleLayer.parent)
         {
            removeChild(this.particleLayer);
         }
      }
      
      public function get closeButton() : Sprite
      {
         return this._closeButton;
      }
      
      public function get infoButton() : Sprite
      {
         return this._infoButton;
      }
      
      public function get contentContainer() : Sprite
      {
         return this._contentContainer;
      }
      
      public function get refreshButton() : DailyQuestRefreshButton
      {
         return this._refreshButton;
      }
      
      public function get questList() : DailyQuestsList
      {
         return this._questList;
      }
   }
}
