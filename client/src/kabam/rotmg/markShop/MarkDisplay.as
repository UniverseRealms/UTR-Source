package kabam.rotmg.markShop {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.core.signals.HideTooltipsSignal;
import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.game.view.components.MarksTabContent;
import kabam.rotmg.tooltips.HoverTooltipDelegate;
import kabam.rotmg.tooltips.TooltipAble;
import kabam.rotmg.ui.UIUtils;

public class MarkDisplay extends Sprite implements TooltipAble {

    public var hoverTooltipDelegate:HoverTooltipDelegate;
    private var bitmap:Bitmap;
    private var background:Sprite;
    private var nodeTexture:BitmapData;
    private var tooltip:TextToolTip;
    public var gs_:GameSprite;
    private var mark_id:int;

    public function MarkDisplay(mark:int, bg:Boolean=true) {
        this.hoverTooltipDelegate = new HoverTooltipDelegate();
        mark_id = mark;
        switch(mark){
            case 0:
                this.nodeTexture = getMarkTexture(15);
                this.tooltip = getMarkTT("No Mark", "This does nothing!");
                break;
            case 1:
                this.nodeTexture = getMarkTexture(16);
                this.tooltip = getMarkTT("Sorcery", "Raises your OVERALL magic by 25%");
                break;
            case 2:
                this.nodeTexture = getMarkTexture(17);
                this.tooltip = getMarkTT("Lifecharged", "Raises your OVERALL health by 25%");
                break;
            case 3:
                this.nodeTexture = getMarkTexture(18);
                this.tooltip = getMarkTT("Crush", "Do more initial damage to enemies");
                break;
            case 4:
                this.nodeTexture = getMarkTexture(19);
                this.tooltip = getMarkTT("Energy Eye", "You gain 2 extra surge on killing enemies and having at least 25 surge increases HP/MP regen.");
                break;
            case 5:
                this.nodeTexture = getMarkTexture(20);
                this.tooltip = getMarkTT("Second Chance", "You have a 25% chance of getting resurrected.");
                break;
            case 6:
                this.nodeTexture = getMarkTexture(21);
                this.tooltip = getMarkTT("Unity", "All stats except mana and health are raised by 5%.");
                break;
            case 7:
                this.nodeTexture = getMarkTexture(22);
                this.tooltip = getMarkTT("Zol Beast", "When invisible you gain 20% extra dexterity, attack and luck.");
                break;
            case 8:
                this.nodeTexture = getMarkTexture(23);
                this.tooltip = getMarkTT("Resolve", "If you have all legendaries equipped you gain a 10% stat boost to ALL stats.");
                break;
            case 9:
                this.nodeTexture = getMarkTexture(24);
                this.tooltip = getMarkTT("Detonation", "Enemies that you kill have a chance to leave a bomb which shortly after detonates. The explosion deal 20000 damage.");
                break;
            case 10:
                this.nodeTexture = getMarkTexture(25);
                this.tooltip = getMarkTT("Guardian", "If your surge is above 60 and you use an ability you summon a golem that does damage based on defense, vitality. Health is then added to it. The golem does not leave until you are below 60 surge.");
                break;
            case 11:
                this.nodeTexture = getMarkTexture(26);
                this.tooltip = getMarkTT("Reaper", "Killing certain amounts of enemies grants bonus");
                break;
            case 12:
                this.nodeTexture = getMarkTexture(27);
                this.tooltip = getMarkTT("Zen", "Using abilities heal you 50 health and 10 mana.");
                break;
            default:
                this.nodeTexture = getMarkTexture(15);
                this.tooltip = getMarkTT("Error, invalid mark", "Please report this to a dev");
                break;
        }
        mouseChildren = false;
        if(bg)
            this.background = drawBackground();
        else
            this.background = new Sprite();
        this.bitmap = new Bitmap(this.nodeTexture);
        this.bitmap.x = -5;
        this.bitmap.y = -4;
        this.hoverTooltipDelegate.setDisplayObject(this);
        this.hoverTooltipDelegate.tooltip = this.tooltip;
        this.drawAsOpen();
    }

    public function getMarkId():int {
        return mark_id;
    }

    private static function getMarkTexture(id:int):BitmapData {
        return TextureRedrawer.redraw(AssetLibrary.getImageFromSet("marks10x10", id), 138, true, 0);
    }

    private static function getMarkTT(name:String, desc:String):TextToolTip {
        return new TextToolTip(0x363636, 0x000000, name, desc, 200);
    }

    private static function drawBackground():Sprite {
        var bg:Sprite = new Sprite();
        bg.graphics.beginFill(5526612);
        bg.graphics.drawRoundRect(0, 0, 84, 84, 6, 6);
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