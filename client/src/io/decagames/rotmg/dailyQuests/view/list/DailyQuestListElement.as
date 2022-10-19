package io.decagames.rotmg.dailyQuests.view.list
{
   import flash.display.Bitmap;
   import flash.display.Sprite;
   import flash.filters.DropShadowFilter;
   import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
   import io.decagames.rotmg.ui.labels.UILabel;
   import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
   import io.decagames.rotmg.ui.texture.TextureParser;
   
   public class DailyQuestListElement extends Sprite
   {
       
      
      private var _id:String;
      
      private var _questName:String;
      
      private var _completed:Boolean;
      
      private var selectedBorder:Sprite;
      
      private var _isSelected:Boolean;
      
      private var ready:Boolean;
      
      private var background:SliceScalingBitmap;
      
      private var icon:Bitmap;
      
      private var questNameTextfield:UILabel;
      
      private var _category:int;
      
      public function DailyQuestListElement(param1:String, param2:String, param3:Boolean, param4:Boolean, param5:int)
      {
         super();
         this._id = param1;
         this._questName = param2;
         this._completed = param3;
         this.ready = param4;
         this._category = param5;
         this.setElements();
      }
      
      public function dispose() : void
      {
         this.background.dispose();
         this.icon = null;
         this.selectedBorder.removeChildren();
      }
      
      private function setElements() : void
      {
         this.selectedBorder = new Sprite();
         this.background = TextureParser.instance.getSliceScalingBitmap("UI","daily_quest_list_element_grey",190);
         this.icon = TextureParser.instance.getTexture("UI","daily_quest_list_element_available_icon");
         this.background.height = 30;
         this.icon.x = 5;
         this.icon.y = 5;
         this.questNameTextfield = new UILabel();
         DefaultLabelFormat.questNameListLabel(this.questNameTextfield,this._category == 3?uint(2201331):uint(uint(this._completed || this._isSelected?16777215:13619151)));
         this.questNameTextfield.filters = [new DropShadowFilter(1,90,0,1,2,2),new DropShadowFilter(0,90,0,0.4,4,4,1,3)];
         this.questNameTextfield.text = this._questName;
         this.questNameTextfield.x = 24;
         this.questNameTextfield.y = 7;
         addChild(this.background);
         addChild(this.icon);
         addChild(this.questNameTextfield);
         this.draw();
      }
      
      private function draw() : void
      {
         removeChild(this.icon);
         removeChild(this.background);
         if(this._completed)
         {
            this.icon = TextureParser.instance.getTexture("UI","daily_quest_list_element_complete_icon");
         }
         else if(this.ready)
         {
            this.icon = TextureParser.instance.getTexture("UI","daily_quest_list_element_ready_icon");
         }
         else
         {
            this.icon = TextureParser.instance.getTexture("UI","daily_quest_list_element_available_icon");
         }
         this.icon.x = 5;
         this.icon.y = 5;
         if(this._isSelected)
         {
            this.background = TextureParser.instance.getSliceScalingBitmap("UI","daily_quest_list_element_orange",190);
         }
         else if(this._completed)
         {
            this.background = TextureParser.instance.getSliceScalingBitmap("UI","daily_quest_list_element_green",190);
         }
         else
         {
            this.background = TextureParser.instance.getSliceScalingBitmap("UI","daily_quest_list_element_grey",190);
         }
         DefaultLabelFormat.questNameListLabel(this.questNameTextfield,this._category == 3?uint(2201331):uint(uint(this._completed || this._isSelected?16777215:13619151)));
         this.questNameTextfield.alpha = this._completed || this._isSelected?Number(1):Number(0.5);
         this.background.height = 30;
         addChild(this.background);
         addChild(this.icon);
         addChild(this.questNameTextfield);
      }
      
      public function get id() : String
      {
         return this._id;
      }
      
      public function get category() : int
      {
         return this._category;
      }
      
      public function set isSelected(param1:Boolean) : void
      {
         this._isSelected = param1;
         this.draw();
      }
      
      public function get isSelected() : Boolean
      {
         return this._isSelected;
      }
   }
}
