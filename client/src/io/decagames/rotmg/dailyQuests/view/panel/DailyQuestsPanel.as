package io.decagames.rotmg.dailyQuests.view.panel
{
   import com.company.assembleegameclient.game.GameSprite;
   import com.company.assembleegameclient.ui.DeprecatedTextButtonStatic;
   import com.company.assembleegameclient.ui.panels.Panel;
   import flash.display.Bitmap;
   import io.decagames.rotmg.dailyQuests.model.DailyQuestsModel;
   import kabam.rotmg.core.StaticInjectorContext;
   import kabam.rotmg.pets.util.PetsViewAssetFactory;
   import kabam.rotmg.text.view.TextFieldDisplayConcrete;
   import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
   
   public class DailyQuestsPanel extends Panel
   {
      
      private static var questDataExists:Boolean = false;
       
      
      private const titleText:TextFieldDisplayConcrete = PetsViewAssetFactory.returnTextfield(16777215,18,true);
      
      private var icon:Bitmap;
      
      private var title:String = "The Tinkerer";
      
      private var feedPetText:String = "See Quests";
      
      private var checkBackLaterText:String = "Check Back Later";
      
      private var objectType:int;
      
      var feedButton:DeprecatedTextButtonStatic;
      
      public function DailyQuestsPanel(param1:GameSprite)
      {
         super(param1);
         this.icon = PetsViewAssetFactory.returnBitmap(5972);
         this.icon.x = -4;
         this.icon.y = -8;
         addChild(this.icon);
         this.objectType = 5972;
         this.titleText.setStringBuilder(new StaticStringBuilder(this.title));
         this.titleText.x = 58;
         this.titleText.y = 28;
         addChild(this.titleText);
         if(this.hasQuests())
         {
            this.addSeeOffersButton();
         }
         else
         {
            this.addCheckBackLaterButton();
         }
      }
      
      public function addSeeOffersButton() : void
      {
         this.feedButton = new DeprecatedTextButtonStatic(16,this.feedPetText);
         this.feedButton.textChanged.addOnce(this.alignButton);
         addChild(this.feedButton);
      }
      
      public function addCheckBackLaterButton() : void
      {
         this.feedButton = new DeprecatedTextButtonStatic(16,this.checkBackLaterText);
         this.feedButton.textChanged.addOnce(this.alignButton);
         addChild(this.feedButton);
      }
      
      private function hasQuests() : Boolean
      {
         var _loc1_:DailyQuestsModel = StaticInjectorContext.getInjector().getInstance(DailyQuestsModel);
         return _loc1_.hasQuests();
      }
      
      private function alignButton() : void
      {
         this.feedButton.x = 188 / 2 - this.feedButton.width / 2;
         this.feedButton.y = 84 - this.feedButton.height - 4;
      }
   }
}
