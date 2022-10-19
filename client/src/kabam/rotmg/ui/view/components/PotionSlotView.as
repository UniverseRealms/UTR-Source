package kabam.rotmg.ui.view.components
{
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;
import com.company.util.GraphicsUtil;
import com.company.util.MoreColorUtil;
import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.DisplayObject;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.events.TimerEvent;
import flash.filters.ColorMatrixFilter;
import flash.filters.DropShadowFilter;
import flash.geom.Point;
import flash.utils.Timer;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import org.osflash.signals.Signal;
import org.osflash.signals.natives.NativeSignal;
 
public class PotionSlotView extends Sprite
{
 
    public static var BUTTON_WIDTH:int = 84;
 
    private static var BUTTON_HEIGHT:int = 24;
 
    private static var SMALL_SIZE:int = 4;
 
    private static var CENTER_ICON_X:int = 6;
 
    private static var LEFT_ICON_X:int = -6;
 
    public static const READABILITY_SHADOW_1:DropShadowFilter = new DropShadowFilter(0,0,0,1,4,4,2);
 
    public static const READABILITY_SHADOW_2:DropShadowFilter = new DropShadowFilter(0,0,0,1,4,4,3);
 
    private static const DOUBLE_CLICK_PAUSE:uint = 250;
 
    private static const DRAG_DIST:int = 3;
 
 
    public var position:int;
 
    public var objectType:int;
 
    public var click:NativeSignal;
 
    public var buyUse:Signal;
 
    public var drop:Signal;
 
    private var lightGrayFill:GraphicsSolidFill;
 
    private var midGrayFill:GraphicsSolidFill;
 
    private var darkGrayFill:GraphicsSolidFill;
 
    private var outerPath:GraphicsPath;
 
    private var innerPath:GraphicsPath;
 
    private var useGraphicsData:Vector.<IGraphicsData>;
 
    private var buyOuterGraphicsData:Vector.<IGraphicsData>;
 
    private var buyInnerGraphicsData:Vector.<IGraphicsData>;
 
    private var text:TextFieldDisplayConcrete;
 
    private var subText:TextFieldDisplayConcrete;
 
    private var costIcon:Bitmap;
 
    private var potionIconDraggableSprite:Sprite;
 
    private var potionIcon:Bitmap;
 
    private var bg:Sprite;
 
    private var grayscaleMatrix:ColorMatrixFilter;
 
    private var doubleClickTimer:Timer;
 
    private var dragStart:Point;
 
    private var pendingSecondClick:Boolean;
 
    private var isDragging:Boolean;
 
    public function PotionSlotView(param1:Array, param2:int)
    {
        var _loc3_:BitmapData = null;
        this.lightGrayFill = new GraphicsSolidFill(5526612,1);
        this.midGrayFill = new GraphicsSolidFill(4078909,1);
        this.darkGrayFill = new GraphicsSolidFill(2368034,1);
        this.outerPath = new GraphicsPath(new Vector.<int>(),new Vector.<Number>());
        this.innerPath = new GraphicsPath(new Vector.<int>(),new Vector.<Number>());
        this.useGraphicsData = new <IGraphicsData>[this.lightGrayFill,this.outerPath,GraphicsUtil.END_FILL];
        this.buyOuterGraphicsData = new <IGraphicsData>[this.midGrayFill,this.outerPath,GraphicsUtil.END_FILL];
        this.buyInnerGraphicsData = new <IGraphicsData>[this.darkGrayFill,this.innerPath,GraphicsUtil.END_FILL];
        super();
        mouseChildren = false;
        this.position = param2;
        this.grayscaleMatrix = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);
        _loc3_ = AssetLibrary.getImageFromSet("lofiObj3",225);
        _loc3_ = TextureRedrawer.redraw(_loc3_,30,false,0);
        this.text = new TextFieldDisplayConcrete().setSize(18).setColor(16777215).setTextWidth(BUTTON_WIDTH).setTextHeight(BUTTON_HEIGHT);
        this.text.filters = [READABILITY_SHADOW_1];
        this.subText = new TextFieldDisplayConcrete().setSize(12).setColor(11974326).setTextWidth(BUTTON_WIDTH).setTextHeight(BUTTON_HEIGHT);
        this.subText.filters = [READABILITY_SHADOW_1];
        this.subText.y = 8;
        this.costIcon = new Bitmap(_loc3_);
        this.costIcon.x = 52;
        this.costIcon.y = -6;
        this.costIcon.visible = false;
        this.bg = new Sprite();
        GraphicsUtil.clearPath(this.outerPath);
        GraphicsUtil.drawCutEdgeRect(0,0,BUTTON_WIDTH,BUTTON_HEIGHT,4,param1,this.outerPath);
        GraphicsUtil.drawCutEdgeRect(2,2,BUTTON_WIDTH - SMALL_SIZE,BUTTON_HEIGHT - SMALL_SIZE,4,param1,this.innerPath);
        this.bg.graphics.drawGraphicsData(this.buyOuterGraphicsData);
        this.bg.graphics.drawGraphicsData(this.buyInnerGraphicsData);
        addChild(this.bg);
        addChild(this.costIcon);
        addChild(this.text);
        addChild(this.subText);
        this.potionIconDraggableSprite = new Sprite();
        this.doubleClickTimer = new Timer(DOUBLE_CLICK_PAUSE,1);
        this.doubleClickTimer.addEventListener(TimerEvent.TIMER_COMPLETE,this.onDoubleClickTimerComplete);
        addEventListener(MouseEvent.MOUSE_DOWN,this.onMouseDown);
        addEventListener(MouseEvent.MOUSE_UP,this.onMouseUp);
        addEventListener(MouseEvent.MOUSE_OUT,this.onMouseOut);
        addEventListener(Event.REMOVED_FROM_STAGE,this.onRemovedFromStage);
        this.click = new NativeSignal(this,MouseEvent.CLICK,MouseEvent);
        this.buyUse = new Signal();
        this.drop = new Signal(DisplayObject);
    }
 
    public function setData(param1:int, param2:int, param3:Boolean, param4:int = -1) : void
    {
        var _loc5_:int = 0;
        var _loc6_:BitmapData = null;
        var _loc7_:Bitmap = null;
        if(param4 != -1)
        {
            this.objectType = param4;
            if(this.potionIcon != null)
            {
                removeChild(this.potionIcon);
            }
            _loc6_ = ObjectLibrary.getRedrawnTextureFromType(param4,55,false);
            this.potionIcon = new Bitmap(_loc6_);
            this.potionIcon.y = -11;
            addChild(this.potionIcon);
            _loc6_ = ObjectLibrary.getRedrawnTextureFromType(param4,80,true);
            _loc7_ = new Bitmap(_loc6_);
            _loc7_.x = _loc7_.x - 30;
            _loc7_.y = _loc7_.y - 30;
            this.potionIconDraggableSprite.addChild(_loc7_);
        }
        var _loc8_:* = param1 > 0;
        if(_loc8_)
        {
            this.setTextString(String(param1));
            this.subText.setStringBuilder(new StaticStringBuilder("/6"));
            this.subText.x = 67;
            _loc5_ = CENTER_ICON_X;
            this.bg.graphics.clear();
            this.bg.graphics.drawGraphicsData(this.useGraphicsData);
            this.text.x = 55;
            this.text.y = 2;
            this.text.setBold(true);
            this.text.setSize(18);
        }
        else
        {
            this.setTextString(String(param2));
            this.subText.setStringBuilder(new StaticStringBuilder(""));
            _loc5_ = LEFT_ICON_X;
            this.bg.graphics.clear();
            this.bg.graphics.drawGraphicsData(this.buyOuterGraphicsData);
            this.bg.graphics.drawGraphicsData(this.buyInnerGraphicsData);
            this.text.x = this.costIcon.x - this.text.width + 10;
            this.text.y = 4;
            this.text.setBold(false);
            this.text.setSize(14);
        }
        if(this.potionIcon)
        {
            this.potionIcon.x = _loc5_;
        }
        if(!_loc8_)
        {
            if(Parameters.data_.contextualPotionBuy)
            {
                this.text.setBold(false);
                this.text.setColor(16777215);
                this.costIcon.filters = [];
                this.costIcon.visible = true;
            }
            else
            {
                this.text.setColor(11184810);
                this.costIcon.filters = [this.grayscaleMatrix];
                this.costIcon.visible = true;
            }
        }
        else
        {
            if(param1 <= 1)
            {
                this.text.setColor(16589603);
            }
            else if(param1 <= 3)
            {
                this.text.setColor(16611363);
            }
            else if(param1 >= 6)
            {
                this.text.setColor(3007543);
            }
            this.costIcon.filters = [];
            this.costIcon.visible = false;
        }
    }
 
    public function setTextString(param1:String) : void
    {
        this.text.setStringBuilder(new StaticStringBuilder(param1));
    }
 
    private function onMouseOut(param1:MouseEvent) : void
    {
        this.setPendingDoubleClick(false);
    }
 
    private function onMouseUp(param1:MouseEvent) : void
    {
        if(this.isDragging)
        {
            return;
        }
        if(param1.shiftKey)
        {
            this.setPendingDoubleClick(false);
            this.buyUse.dispatch();
        }
        else if(!this.pendingSecondClick)
        {
            this.setPendingDoubleClick(true);
        }
        else
        {
            this.setPendingDoubleClick(false);
            this.buyUse.dispatch();
        }
    }
 
    private function onMouseDown(param1:MouseEvent) : void
    {
        if(!this.costIcon.visible)
        {
            this.beginDragCheck(param1);
        }
    }
 
    private function setPendingDoubleClick(param1:Boolean) : void
    {
        this.pendingSecondClick = param1;
        if(this.pendingSecondClick)
        {
            this.doubleClickTimer.reset();
            this.doubleClickTimer.start();
        }
        else
        {
            this.doubleClickTimer.stop();
        }
    }
 
    private function beginDragCheck(param1:MouseEvent) : void
    {
        this.dragStart = new Point(param1.stageX,param1.stageY);
        addEventListener(MouseEvent.MOUSE_MOVE,this.onMouseMoveCheckDrag);
        addEventListener(MouseEvent.MOUSE_OUT,this.cancelDragCheck);
        addEventListener(MouseEvent.MOUSE_UP,this.cancelDragCheck);
    }
 
    private function cancelDragCheck(param1:MouseEvent) : void
    {
        removeEventListener(MouseEvent.MOUSE_MOVE,this.onMouseMoveCheckDrag);
        removeEventListener(MouseEvent.MOUSE_OUT,this.cancelDragCheck);
        removeEventListener(MouseEvent.MOUSE_UP,this.cancelDragCheck);
    }
 
    private function onMouseMoveCheckDrag(param1:MouseEvent) : void
    {
        var _loc2_:Number = param1.stageX - this.dragStart.x;
        var _loc3_:Number = param1.stageY - this.dragStart.y;
        var _loc4_:Number = Math.sqrt(_loc2_ * _loc2_ + _loc3_ * _loc3_);
        if(_loc4_ > DRAG_DIST)
        {
            this.cancelDragCheck(null);
            this.setPendingDoubleClick(false);
            this.beginDrag();
        }
    }
 
    private function onDoubleClickTimerComplete(param1:TimerEvent) : void
    {
        this.setPendingDoubleClick(false);
    }
 
    private function beginDrag() : void
    {
        this.isDragging = true;
        this.potionIconDraggableSprite.startDrag(true);
        stage.addChild(this.potionIconDraggableSprite);
        this.potionIconDraggableSprite.addEventListener(MouseEvent.MOUSE_UP,this.endDrag);
    }
 
    private function endDrag(param1:MouseEvent) : void
    {
        this.isDragging = false;
        this.potionIconDraggableSprite.stopDrag();
        this.potionIconDraggableSprite.x = this.dragStart.x;
        this.potionIconDraggableSprite.y = this.dragStart.y;
        stage.removeChild(this.potionIconDraggableSprite);
        this.potionIconDraggableSprite.removeEventListener(MouseEvent.MOUSE_UP,this.endDrag);
        this.drop.dispatch(this.potionIconDraggableSprite.dropTarget);
    }
 
    private function onRemovedFromStage(param1:Event) : void
    {
        this.setPendingDoubleClick(false);
        this.cancelDragCheck(null);
        if(this.isDragging)
        {
            this.potionIconDraggableSprite.stopDrag();
        }
    }
}
}