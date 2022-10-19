package kabam.rotmg.markShop {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;

import kabam.rotmg.core.signals.HideTooltipsSignal;
import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.tooltips.HoverTooltipDelegate;
import kabam.rotmg.tooltips.TooltipAble;
import kabam.rotmg.ui.UIUtils;

public class NodeDisplay extends Sprite implements TooltipAble {

    public var hoverTooltipDelegate:HoverTooltipDelegate;
    private var bitmap:Bitmap;
    private var background:Sprite;
    private var nodeTexture:BitmapData;
    private var tooltip:TextToolTip;
    public var gs_:GameSprite;
    private var node_id:int;
    private var slot_id:int;

    public function NodeDisplay(node:int, bg:Boolean=true , slot:int=0) {
        this.hoverTooltipDelegate = new HoverTooltipDelegate();
        node_id = node;
        slot_id = slot;
        switch(node) {
            case 0:
                this.nodeTexture = getNodeTexture(15);
                this.tooltip = getNodeTT("No Node", "This does nothing!");
                break;
            case 1:
                this.nodeTexture = getNodeTexture(0);
                this.tooltip = getNodeTT("Node of Fortitude", "Raises your OVERALL defense by 5%.");
                break;
            case 2:
                this.nodeTexture = getNodeTexture(1);
                this.tooltip = getNodeTT("Node of Recovery", "Raises your OVERALL vitality by 5%.");
                break;
            case 3:
                this.nodeTexture = getNodeTexture(2);
                this.tooltip = getNodeTT("Node of Blood", "Raises your OVERALL attack by 5%.");
                break;
            case 4:
                this.nodeTexture = getNodeTexture(3);
                this.tooltip = getNodeTT("Node of Intelligence", "Raises your OVERALL wisdom by 5%.");
                break;
            case 5:
                this.nodeTexture = getNodeTexture(4);
                this.tooltip = getNodeTT("Node of Faith", "Raises your OVERALL protection by 5%.");
                break;
            case 6:
                this.nodeTexture = getNodeTexture(5);
                this.tooltip = getNodeTT("Node of Agility", "Raises your OVERALL dexterity by 5%.");
                break;
            case 7:
                this.nodeTexture = getNodeTexture(6);
                this.tooltip = getNodeTT("Node of Swiftness", "Raises your OVERALL speed by 5%.");
                break;
            case 8:
                this.nodeTexture = getNodeTexture(7);
                this.tooltip = getNodeTT("Node of Vigor", "Raises your OVERALL might by 5%.");
                break;
            case 9:
                this.nodeTexture = getNodeTexture(8);
                this.tooltip = getNodeTT("Node of Skill", "Raises your OVERALL luck by 5%.");
                break;
            case 10:
                this.nodeTexture = getNodeTexture(9);
                this.tooltip = getNodeTT("Node of Power", "Raises the time it takes for your surge to deplete.");
                break;
            case 11:
                this.nodeTexture = getNodeTexture(10);
                this.tooltip = getNodeTT("Node of Aid", "Raises your OVERALL restoration by 5%.");
                break;
            default:
                this.nodeTexture = getNodeTexture(15);
                this.tooltip = getNodeTT("Error, invalid node", "Please report this to a dev");
        }
        mouseChildren = false;
        if(bg)
            this.background = drawBackground();
        else
            this.background = new Sprite();
        this.bitmap = new Bitmap(this.nodeTexture);
        this.bitmap.x = -8;
        this.bitmap.y = -8;
        this.hoverTooltipDelegate.setDisplayObject(this);
        this.hoverTooltipDelegate.tooltip = this.tooltip;
        this.drawAsOpen();
    }

    public function getNodeId():int {
        return node_id;
    }

    public function getNodeSlot():int {
        return slot_id;
    }

    private static function getNodeTexture(id:int):BitmapData {
        return TextureRedrawer.redraw(AssetLibrary.getImageFromSet("marks10x10", id), 64, true, 0);
    }

    private static function getNodeTT(name:String, desc:String):TextToolTip {
        return new TextToolTip(0x363636, 0x000000, name, desc, 200);
    }

    private static function drawBackground():Sprite {
        var bg:Sprite = new Sprite();
        bg.graphics.beginFill(5526612);
        bg.graphics.drawRoundRect(0, 0, 40, 40, 6, 6);
        bg.graphics.endFill();
        return bg;
    }

    public function setShowToolTipSignal(_arg_1:ShowTooltipSignal):void {
        this.hoverTooltipDelegate.setShowToolTipSignal(_arg_1);
    }

    public function getShowToolTip():ShowTooltipSignal {
        return (this.hoverTooltipDelegate.getShowToolTip());
    }

    public function setHideToolTipsSignal(_arg_1:HideTooltipsSignal):void {
        this.hoverTooltipDelegate.setHideToolTipsSignal(_arg_1);
    }

    public function getHideToolTips():HideTooltipsSignal {
        return (this.hoverTooltipDelegate.getHideToolTips());
    }

    public function drawAsOpen():void {
        addChild(this.background);
        addChild(this.bitmap);
    }

    public function drawAsClosed():void {
        if (((this.background) && ((this.background.parent == this)))) {
            removeChild(this.background);
        }
        if (((this.bitmap) && ((this.bitmap.parent == this)))) {
            removeChild(this.bitmap);
        }
    }


}
}