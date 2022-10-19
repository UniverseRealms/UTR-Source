package kabam.rotmg.skilltree.ui
{
import com.company.assembleegameclient.ui.tooltip.SkillTreeToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;

import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.game.view.components.SkillTabContent;

import org.osflash.signals.Signal;

public class SkillTreeButton extends Sprite
   {

      public static const WIDTH:int = 40;
      
      public static const HEIGHT:int = 40;
      
      public static const BORDER:int = 4;
       

      
      public var buttonId:int;
      public var buttonType:int;
      public var SSButton:SliceScalingButton;
      public var purchased:Boolean;
      public var desc:String;
      public var unlocks:Vector.<int> = new Vector.<int>();
      public var locks:Vector.<int> = new Vector.<int>();
      public var previousId:int;
      public function SkillTreeButton(id:int, buttonType:int,
                                      TextureName:String, posX:int, posY:int,Unlocks:Vector.<int>, Locks:Vector.<int>, PreviousId:int = 0, Desc:String = "~ Add a description to this skill ~", bought:Boolean = false)
      {
         super();
         this.buttonId = id;
         this.buttonType = buttonType;
         this.SSButton = addButton(TextureName, posX, posY);
         this.desc = Desc;
         this.purchased = bought;
         this.unlocks = Unlocks;
         this.locks = Locks;
         this.previousId = PreviousId;
         addChild(this.SSButton);
      }

      public function addButton(buttonName:String, posX:int, posY:int) : SliceScalingButton
      {
         var button:SliceScalingButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", buttonName));
         button.x = posX;
         button.y = posY;
         return button;
      }
   }
}
