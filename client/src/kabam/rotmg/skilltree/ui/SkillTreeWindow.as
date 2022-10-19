package kabam.rotmg.skilltree.ui
{

import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;

import com.company.assembleegameclient.ui.tooltip.SkillTreeToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.ui.SimpleText;

import flash.display.Sprite;

import flash.events.Event;
import flash.events.MouseEvent;

import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.SocketServer;

import kabam.rotmg.appengine.api.AppEngineClient;

import kabam.rotmg.game.view.components.SkillTabContent;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.data.SkillTreeData;
import kabam.rotmg.messaging.impl.incoming.SkillTreeResult;
import kabam.rotmg.messaging.impl.outgoing.BuySkill;
import kabam.rotmg.messaging.impl.outgoing.UpdateTree;


import kabam.rotmg.skilltree.data.SkillTreeBranchData;
import kabam.rotmg.skilltree.signal.SkillTreeSignal;


import org.osflash.signals.Signal;

import starling.display.Button;

public class SkillTreeWindow extends Sprite
{


   public var Background:SliceScalingBitmap;
   public var quitButton:SkillTreeButton;
   public var starting_node:SliceScalingBitmap;


   public const skillTreeSignal:SkillTreeSignal = new SkillTreeSignal();


   public const addToolTip:Signal = new Signal(ToolTip);
   private var tooltipFocusTile:SkillTreeButton;
   private var tooltip:ToolTip;

   private var Content:SkillTabContent;

   private var BoughtGoldSkills:int = 0;
   private var BoughtSilverSkills:int = 0;
   private var BoughtBronzeSkills:int = 0;
   private var UsedSkillPoints:int = 0;
   private var SkillPoints:int = 0;
   private var BoughtSkills:Vector.<SkillTreeData> = new Vector.<SkillTreeData>();
   private var SkillPointText:SimpleText;
   private var socket:SocketServer;
   private var message:MessageProvider;
   private var gs_:GameSprite;

   private var LockList:Vector.<int> = new Vector.<int>();
   private var UnlockList:Vector.<int> = new Vector.<int>();

   private var Buttons:Vector.<SkillTreeButton> = new Vector.<SkillTreeButton>();

   private var LoadingText:SimpleText;

   public function SkillTreeWindow(content:SkillTabContent, gs:GameSprite, ss:SocketServer, msg:MessageProvider)
   {
      this.gs_ = gs;
      this.Content = content;
      this.socket = ss;
      this.message = msg;

      GetData(this.Content.player);
      this.load();
   }



   public function load():void
   {
      var updateTree:UpdateTree = null;
      updateTree = this.message.require(GameServerConnection.UPDATE_TREE) as UpdateTree;
      this.socket.sendMessage(updateTree);
   }

   public function initalize():void
   {

      this.skillTreeSignal.add(this.onSkillTree);
      this.Background = TextureParser.instance.getSliceScalingBitmap("UI", "tab_cointainer_background");
      this.Background.width = 575;
      this.Background.height = 480;
      this.Background.x = 10;
      this.Background.y = 120;
      addChild(this.Background);
      this.starting_node = TextureParser.instance.getSliceScalingBitmap("UI", "tabs_tile_decor");
      this.starting_node.x = this.Background.bitmapData.width / 2 + this.starting_node.bitmapData.width / 2;
      this.starting_node.y = 110 + this.Background.height / 2 + this.starting_node.bitmapData.width / 2;
      addChild(this.starting_node);
      this.SkillPointText = new SimpleText(16, 0x9B9B9B, false, 100, 0, false);
      this.SkillPointText.text = (this.SkillPoints - this.UsedSkillPoints).toString() + "/" + this.SkillPoints;
      this.SkillPointText.x = this.Background.bitmapData.width - this.Background.bitmapData.width / 4;
      this.SkillPointText.y = 80;
      this.SkillPointText.updateMetrics();
      addChild(this.SkillPointText);
      this.quitButton = new SkillTreeButton(0, -1,"close_button",560,110, null, null);
      this.quitButton.addEventListener(MouseEvent.CLICK, this.onClose);
      addChild(this.quitButton);
      this.LoadingText = new SimpleText(40, 4935506, false, 200);
      this.LoadingText.text = "Loading";
      this.LoadingText.x = this.Background.width/2;
      this.LoadingText.y = this.starting_node.y - 50;
      this.LoadingText.updateMetrics();
      addChild(this.LoadingText);
   }

   private function onSkillTree(result:SkillTreeResult) : void
   {
      this.BoughtSkills = result.results_;
      SetData(this.Content.player);
      removeChild(this.LoadingText);


      CreateTree();
   }

   private function CreateTree(): void
   {
      var skill:SkillTreeButton = null;

      for (var i:int = 0; i < ObjectLibrary.SkillLibrary_.length; i++)
      {
         var xml:XML = ObjectLibrary.SkillLibrary_[i]; //Gets the XML file
         if(xml.@id == 0)
            continue;


         //Add new types
         //Offensive, Defensive, Support and Misc



         skill = new SkillTreeButton(int(xml.@id), int(xml.@type), FindBitMap(int(xml.@type)), int(xml.X), int(xml.Y),
                 getUnlocks(xml), getLocks(xml), xml.PreviousId); //adds all the skill nodes

         Buttons.push(skill);

         if(CheckIfBought(skill))//checks if skill is boughtUIAssets
         {
            skill.SSButton.changeBitmap(FindBitMap(skill.buttonType, true));
            skill.purchased = true;
         }

         skill.x = this.starting_node.x + skill.x;
         skill.y = this.starting_node.y + skill.y;

         skill.addEventListener(MouseEvent.CLICK, this.onButtonClick);
         skill.addEventListener(MouseEvent.ROLL_OVER, this.onButtonHover);
         if(xml.Desc != null) {
            skill.desc = xml.Desc;
         }

         addChild(skill);
      }
      update();
   }

   private static function getUnlocks(xml:XML): Vector.<int> {
      var Unlocks:Vector.<int> = new Vector.<int>();
      for each(var unlock:int in xml.Unlocks) {
         Unlocks.push(unlock);
      }
      return Unlocks;
   }

   private static function getLocks(xml:XML): Vector.<int> {
      var Locks:Vector.<int> = new Vector.<int>();
      for each(var lock:int in xml.Locks) {
         Locks.push(lock);
      }
      return Locks;
   }

   private function update():void
   {
      for each(var button:SkillTreeButton in Buttons)
      {
         if(button.buttonId > 5)
            LockButton(button.buttonId);

         if(CheckIfBought(button))
         {
            button.purchased = true;
            for each(var unlock:int in button.unlocks)
            {
               //if(!IsLocked(button))
               UnlockButton(unlock);
            }
            for each(var lock:int in button.locks)
            {
               LockButton(lock);
            }

         }
      }
   }

   private function IsLocked(button:SkillTreeButton) : Boolean
   {
      return button.SSButton.disabled;
   }

   private function UnlockButton(buttonId:int) : void
   {
      Buttons[buttonId].SSButton.disabled = false;
      //Buttons[buttonId].addEventListener(MouseEvent.CLICK, this.onButtonClick);
   }

   private function LockButton(buttonId:int) : void
   {
      Buttons[buttonId].SSButton.disabled = true;
      //Buttons[buttonId].removeEventListener(MouseEvent.CLICK, this.onButtonClick);
   }

   private static function FindBitMap(skillType:int, closed:Boolean = false) : String
   { //Finds the right texture string depending on the type of skill
      switch (skillType) {
         case 0:
            if(closed)
               return "skilltree_bronze_closed";
            else
               return "skilltree_bronze_open";

         case 1:
               if(closed)
                  return "skilltree_silver_closed";
               else
                  return "skilltree_silver_open";
         case 2:
               if(closed)
                  return "skilltree_gold_closed";
               else
                  return "skilltree_gold_open";
      }
      return "skilltree_bronze_open";
   }

   private function CheckIfBought(skill:SkillTreeButton) : Boolean
   {
      return this.BoughtSkills[skill.buttonId].unlockedSkillsId_ == 1;
   }

   private function GetData(plr:Player) : void
   {
      this.SkillPoints = plr.skillPoints_;
      this.UsedSkillPoints = plr.usedSkillPoints_;
      this.BoughtSkills = plr.boughtSkills_;
   }

   private function SetData(plr:Player) : void
   {
      plr.skillPoints_ = SkillPoints;
      plr.usedSkillPoints_ = UsedSkillPoints;
      plr.boughtSkills_ = BoughtSkills;
   }

   protected function onBuy(id:int) : void
   {
      var buySkill:BuySkill = null;
      buySkill = this.message.require(GameServerConnection.BUY_SKILL) as BuySkill;
      buySkill.skillId_ = id;
      this.socket.sendMessage(buySkill);
   }

   private function onButtonClick(e:Event) : void
   {
      var skillData:SkillTreeButton = e.currentTarget as SkillTreeButton;

      if(skillData == null)
         return;



      if(CanBuySkill(skillData)) {
         onBuy(skillData.buttonId);
         load();
         update();
         //implement buy packet to server
         switch (skillData.buttonType) {
            case 0:
               skillData.SSButton.changeBitmap("skilltree_bronze_closed");
               break;
            case 1:
               skillData.SSButton.changeBitmap("skilltree_silver_closed");
               break;
            case 2:
               this.BoughtGoldSkills++;
               skillData.SSButton.changeBitmap("skilltree_gold_closed");
               break;
         }
      }
   }


   public function GenerateBranch(branchId:int, skillInBranch:Vector.<int>, branchesToLock:Vector.<int>) : SkillTreeBranchData
   {
      var branch:SkillTreeBranchData = new SkillTreeBranchData();
      branch.CurrentBranchId = branchId;
      branch.SkillIdsInBranch.push(skillInBranch);
      branch.BranchIdsToClose.push(branchesToLock);
      return branch;
   }

   private function CanBuySkill(skill:SkillTreeButton) : Boolean{ //client side check //also there is server side check
      var skillPoints:int = this.Content.player.skillPoints_ - this.Content.player.usedSkillPoints_;
      if(skillPoints > 0) {
         if(!IsLocked(skill))// if its not locked than can buy
         //if player has 3 gold skills than can't buy
         if(this.BoughtGoldSkills > 2 && skill.buttonType == 2) {
            return false;
         }
         //if player has skill points than he can buy
         return true;
      }
      //if no skill points than he can't buy
      return false;
   }

   private function onButtonHover(e:MouseEvent):void {
      if (!stage) {
         return;
      }

      var button:SkillTreeButton = (e.currentTarget as SkillTreeButton);
      this.tooltip = new SkillTreeToolTip(button, 12);
      this.tooltip.attachToTarget(button);
      this.tooltipFocusTile = button;
      stage.addChild(this.tooltip);
   }


   private function onClose(param1:Event) : void
   {
      this.quitButton.removeEventListener(MouseEvent.CLICK, this.onClose);
      this.Content.ItemButton1.setLabel("Open",DefaultLabelFormat.defaultModalTitle);
      this.Content.Open = false;
      stage.removeChild(this);
   }


}
}
//TODO:
//Need to get skill tree from server!
//Add data to XML file about which skills do what
//Impliment that on server side
//Try to fix any exploits
//Make it look nice