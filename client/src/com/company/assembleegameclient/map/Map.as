package com.company.assembleegameclient.map {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.map.mapoverlay.MapOverlay;
import com.company.assembleegameclient.map.partyoverlay.PartyOverlay;
import com.company.assembleegameclient.objects.BasicObject;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Party;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.objects.particles.ParticleEffect;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.ConditionEffect;

import flash.display.BitmapData;
import flash.display.DisplayObject;
import flash.display.GraphicsBitmapFill;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.display.Sprite;
import flash.display.StageScaleMode;
import flash.display3D.Context3D;
import flash.filters.BlurFilter;
import flash.filters.ColorMatrixFilter;
import flash.geom.ColorTransform;
import flash.geom.Point;
import flash.geom.Rectangle;
import flash.utils.Dictionary;

import kabam.rotmg.assets.EmbeddedAssets;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.game.model.GameModel;
import kabam.rotmg.stage3D.GraphicsFillExtra;
import kabam.rotmg.stage3D.Object3D.Object3DStage3D;
import kabam.rotmg.stage3D.Render3D;
import kabam.rotmg.stage3D.Renderer;
import kabam.rotmg.stage3D.Stage3DConfig;
import kabam.rotmg.stage3D.graphic3D.Program3DFactory;
import kabam.rotmg.stage3D.graphic3D.TextureFactory;

import org.osflash.signals.Signal;

public class Map extends Sprite {

    public static const CLOTH_BAZAAR:String = "Cloth Bazaar";
    public static const NEXUS:String = "Nexus";
    public static const DAILY_QUEST_ROOM:String = "Daily Quest Room";
    public static const GUILD_HALL:String = "Guild Hall";
    public static const NEXUS_EXPLANATION:String = "Nexus_Explanation";
    public static const VAULT:String = "Vault";
    public static const TAVERN:String = "Tavern";
    private static const VISIBLE_SORT_FIELDS:Array = ["sortVal_", "objectId_"];
    private static const VISIBLE_SORT_PARAMS:Array = [Array.NUMERIC, Array.NUMERIC];
    protected static const BLIND_FILTER:ColorMatrixFilter = new ColorMatrixFilter([0.15, 0.15, 0.15, 0, 0, 0.15, 0.15, 0.15, 0, 0, 0.15, 0.15, 0.15, 0, 0, 0.15, 0.15, 0.15, 1, 0]);

    public static var texture:BitmapData;

    public var ifDrawEffectFlag:Boolean = true;
    private var inUpdate_:Boolean = false;
    private var objsToAdd_:Vector.<BasicObject>;
    private var idsToRemove_:Vector.<int>;
    private var lastSoftwareClear:Boolean = false;
    private var darkness:DisplayObject;
    private var graphicsData_:Vector.<IGraphicsData>;
    private var graphicsDataStageSoftware_:Vector.<IGraphicsData>;
    private var graphicsData3d_:Vector.<Object3DStage3D>;
    public var visible_:Array;
    public var visibleUnder_:Array;
    public var visibleSquares_:Vector.<Square>;
    public var visibleHit_:Array;
    public var topSquares_:Vector.<Square>;

    public var hitTEnemies_:Vector.<GameObject>;
    public var hitTPlayers_:Vector.<GameObject>;



    public var goDict_:Dictionary;
    public var gs_:GameSprite;
    public var name_:String;
    public var player_:Player = null;
    public var width_:int;
    public var height_:int;
    protected var allowPlayerTeleport_:Boolean;
    public var map_:Sprite;
    public var mapOverlay_:MapOverlay = null;
    public var partyOverlay_:PartyOverlay = null;
    public var squareList_:Vector.<Square>;
    public var squares_:Vector.<Square>;
    public var boDict_:Dictionary;
    public var merchLookup_:Object;
    public var party_:Party = null;
    public var quest_:Quest = null;
    public var signalRenderSwitch:Signal;
    protected var wasLastFrameGpu:Boolean = false;



    public function Map(gs:GameSprite) {
        super();
        this.gs_ = gs;
        this.map_ = new Sprite();
        this.goDict_ = new Dictionary();
        this.boDict_ = new Dictionary();
        this.squareList_ = new Vector.<Square>();
        this.squares_ = new Vector.<Square>();
        this.objsToAdd_ = new Vector.<BasicObject>();
        this.idsToRemove_ = new Vector.<int>();
        this.mapOverlay_ = new MapOverlay();
        this.partyOverlay_ = new PartyOverlay(this);
        this.party_ = new Party(this);
        this.quest_ = new Quest(this);
        this.darkness = new EmbeddedAssets.DarknessBackground();
        this.graphicsData_ = new Vector.<IGraphicsData>();
        this.graphicsDataStageSoftware_ = new Vector.<IGraphicsData>();
        this.graphicsData3d_ = new Vector.<Object3DStage3D>();
        this.visible_ = new Array();
        this.visibleUnder_ = new Array();
        this.visibleSquares_ = new Vector.<Square>();
        this.visibleHit_ = new Array();
        this.topSquares_ = new Vector.<Square>();
        this.merchLookup_ = new Object();
        this.signalRenderSwitch = new Signal(Boolean);
        this.hitTEnemies_ = new Vector.<GameObject>();
        this.hitTPlayers_ = new Vector.<GameObject>();
        StaticInjectorContext.getInjector().getInstance(GameModel).gameObjects = goDict_;
        this.wasLastFrameGpu = Parameters.isGpuRender();
    }

    public function dispose():void {
        var square:Square;
        var go:GameObject;
        var bo:BasicObject;
        this.gs_ = null;
        this.map_ = null;
        for each(go in this.goDict_) {
            go.dispose();
        }
        this.goDict_ = null;
        for each( bo in boDict_) {
            bo.dispose();
        }
        this.boDict_ = null;
        for each(square in squareList_) {
            square.dispose();
        }
        this.squareList_.length = 0;
        this.squareList_ = null;
        this.squares_.length = 0;
        this.squares_ = null;
        this.objsToAdd_ = null;
        this.idsToRemove_ = null;
        this.mapOverlay_ = null;
        this.partyOverlay_ = null;
        this.player_ = null;
        this.party_ = null;
        this.quest_ = null;
        this.hitTPlayers_.length = 0;
        this.hitTEnemies_.length = 0;
        this.hitTPlayers_ = null;
        this.hitTEnemies_ = null;
        this.merchLookup_ = null;
        TextureFactory.disposeTextures();
        GraphicsFillExtra.dispose();
        Program3DFactory.getInstance().dispose();
    }

    public function setProps(w:int, h:int, name:String, allowTP:Boolean):void {
        width_ = w;
        height_ = h;
        name_ = name;
        allowPlayerTeleport_ = allowTP;
    }

    public function initialize():void {
        squares_.length = (width_ * height_);
        addChild(map_);
        addChild(mapOverlay_);
        addChild(partyOverlay_);
    }

    public function update(time:int, dt:int) : void {
        var bo:BasicObject = null;
        var go:GameObject = null;
        var objId:int = 0;
        this.inUpdate_ = true;

        this.hitTPlayers_.length = 0;
        this.hitTEnemies_.length = 0;
        for each(go in this.goDict_) {
            if (!go.update(time, dt)) {
                this.idsToRemove_.push(go.objectId_);
            } else {
                if (go.props_.isEnemy_) {
                    if (!go.isUntargetable()) {
                        this.hitTEnemies_.push(go);
                    }
                } else if (go.props_.isPlayer_ || go.props_.isAlly_) {
                    if (!go.isUntargetable()) {
                        this.hitTPlayers_.push(go);
                    }
                }
            }
        }
        for each(bo in this.boDict_) {
            if (!bo.update(time, dt)) {
                this.idsToRemove_.push(bo.objectId_);
            }
        }
        this.inUpdate_ = false;
        for each(bo in this.objsToAdd_) {
            this.internalAddObj(bo);
        }
        this.objsToAdd_.length = 0;
        for each(objId in this.idsToRemove_) {
            this.internalRemoveObj(objId);
        }
        this.idsToRemove_.length = 0;
        this.party_.update(time, dt);
    }

    public function pSTopW(x:Number, y:Number):Point {
        var square:Square = null;
        for each (square in this.visibleSquares_) {
            if (square.faces_.length != 0 && square.faces_[0].face_.contains(x, y)) {
                return new Point(square.center_.x, square.center_.y);
            }
        }
        return null;
    }

    public function setGroundTile(x:int, y:int, tileType:uint) : void {
        var yi:int = 0;
        var ind:int = 0;
        var n:Square = null;
        var square:Square = this.getSquare(x, y);
        square.setTileType(tileType);
        var xend:int = x < this.width_ - 1 ? int(x + 1) : int(x);
        var yend:int = y < this.height_ - 1 ? int(y + 1) : int(y);
        for (var xi:int = x > 0 ? int(x - 1) : int(x); xi <= xend; xi++) {
            for (yi = y > 0 ? int(y - 1) : int(y); yi <= yend; yi++) {
                ind = xi + yi * this.width_;
                n = this.squares_[ind];
                if (n != null && (n.props_.hasEdge_ || n.tileType_ != tileType)) {
                    n.faces_.length = 0;
                }
            }
        }
    }

    public function addObj(bo:BasicObject, x:Number, y:Number):void {
        bo.x_ = x;
        bo.y_ = y;
        var effect:ParticleEffect = bo as ParticleEffect;
        if (effect) {
            effect.reducedDrawEnabled = !(Parameters.data_.particleEffect);
        }
        if (this.inUpdate_) {
            this.objsToAdd_.push(bo);
        } else {
            this.internalAddObj(bo);
        }
    }

    public function internalAddObj(bo:BasicObject):void {
        if (!bo.addTo(this, bo.x_, bo.y_)) return;
        var _local2:Dictionary = (((bo is GameObject)) ? goDict_ : boDict_);
        if (_local2[bo.objectId_] != null) return;
        _local2[bo.objectId_] = bo;
    }

    public function removeObj(id:int):void {
        if (this.inUpdate_) {
            this.idsToRemove_.push(id);
        } else {
            this.internalRemoveObj(id);
        }
    }

    public function internalRemoveObj(objId:int):void {
        var dict:Dictionary = goDict_;
        var bo:BasicObject = dict[objId];
        if (bo == null) {
            dict = boDict_;
            bo = dict[objId];
            if (bo == null) {
                return;
            }
        }
        bo.removeFromMap();
        delete dict[objId];
    }

    public function getSquare(posX:Number, posY:Number):Square {
        if (posX < 0 || posX >= this.width_ || posY < 0 || posY >= this.height_) {
            return null;
        }
        var ind:int = int(posX) + int(posY) * this.width_;
        var square:Square = this.squares_[ind];
        if (square == null) {
            square = new Square(this, int(posX), int(posY));
            this.squares_[ind] = square;
            this.squareList_.push(square);
        }
        return square;
    }

    public function lookupSquare(x:int, y:int):Square {
        if (x < 0 || x >= this.width_ || y < 0 || y >= this.height_) {
            return null;
        }
        return this.squares_[x + y * this.width_];
    }

    public function draw(camera:Camera, time:int):void {
        var isGpuRender:Boolean = Parameters.isGpuRender();
        var square:Square = null;
        var go:GameObject = null;
        var bo:BasicObject = null;
        var render3D:Render3D = null;
        var filter:uint = 0;
        var i:int = 0;

        if (wasLastFrameGpu != isGpuRender) {
            var context:Context3D = WebMain.STAGE.stage3Ds[0].context3D;
            if (wasLastFrameGpu && context != null && context.driverInfo.toLowerCase().indexOf("disposed") == -1) {
                context.clear();
                context.present();
            } else {
                map_.graphics.clear();
            }
            signalRenderSwitch.dispatch(wasLastFrameGpu);
            wasLastFrameGpu = isGpuRender;
        }

        if (isGpuRender) {
            WebMain.STAGE.stage3Ds[0].x = 400 - Stage3DConfig.HALF_WIDTH;
            WebMain.STAGE.stage3Ds[0].y = 300 - Stage3DConfig.HALF_HEIGHT;
        }

        var screenRect:Rectangle = camera.clipRect_;
        if (stage.scaleMode == StageScaleMode.NO_SCALE) {
            x = ((-(screenRect.x) * 800) / (WebMain.sWidth / Parameters.data_["mscale"]));
            y = ((-(screenRect.y) * 600) / (WebMain.sHeight / Parameters.data_["mscale"]));
        } else {
            x = -(screenRect.x);
            y = -(screenRect.y);
        }

        this.visible_.length = 0;
        this.visibleUnder_.length = 0;
        this.visibleSquares_.length = 0;
        this.topSquares_.length = 0;

        this.graphicsData_.length = 0;
        this.graphicsDataStageSoftware_.length = 0;
        this.graphicsData3d_.length = 0;

        for(var xi:int = -15; xi <= 15; xi++)
        {
            for(var yi:int = -15; yi <= 15; yi++) {
                if (xi * xi + yi * yi <= 225) {
                    square = lookupSquare(xi + player_.x_, yi + player_.y_);
                    if (square != null) {
                        square.lastVisible_ = time;
                        square.draw(this.graphicsData_, camera, time);
                        this.visibleSquares_.push(square);
                        if (square.topFace_ != null) {
                            this.topSquares_.push(square);
                        }
                    }
                }
            }
        }

        // visible game objects
        for each(go in this.goDict_) {
            go.drawn_ = false;
            if (!go.dead_) {
                square = go.square_;
                if (!(square == null || square.lastVisible_ != time)) {
                    go.drawn_ = true;
                    go.computeSortVal(camera);
                    if (go.props_.drawUnder_) {
                        if (go.props_.drawOnGround_) {
                            go.draw(this.graphicsData_, camera, time);
                        } else {
                            this.visibleUnder_.push(go);
                        }
                    } else {
                        this.visible_.push(go);
                    }
                }
            }
        }

        // visible basic objects (projectiles, particles and such)
        for each(bo in this.boDict_) {
            bo.drawn_ = false;
            square = bo.square_;
            if (!(square == null || square.lastVisible_ != time)) {
                bo.drawn_ = true;
                bo.computeSortVal(camera);
                this.visible_.push(bo);
            }
        }

        // draw visible under
        if (this.visibleUnder_.length > 0) {
            this.visibleUnder_.sortOn(VISIBLE_SORT_FIELDS, VISIBLE_SORT_PARAMS);
            for each(bo in this.visibleUnder_) {
                bo.draw(this.graphicsData_, camera, time);
            }
        }

        // draw shadows
        this.visible_.sortOn(VISIBLE_SORT_FIELDS, VISIBLE_SORT_PARAMS);
        if (Parameters.data_.drawShadows) {
            for each(bo in this.visible_) {
                if (bo.hasShadow_) {
                    bo.drawShadow(this.graphicsData_, camera, time);
                }
            }
        }

        // draw visible
        for each(bo in this.visible_) {
            bo.draw(this.graphicsData_, camera, time);
            if (isGpuRender) {
                bo.draw3d(this.graphicsData3d_);
            }
        }

        // draw top squares
        if (this.topSquares_.length > 0) {
            for each(square in this.topSquares_) {
                square.drawTop(this.graphicsData_, camera, time);
            }
        }

        if (isGpuRender && Renderer.inGame) {
            filter = this.getFilterIndex();
            render3D = StaticInjectorContext.getInjector().getInstance(Render3D);
            render3D.dispatch(this.graphicsData_, this.graphicsData3d_, width_, height_, camera, filter);
            i = 0;
            while (i < this.graphicsData_.length) {
                if (((this.graphicsData_[i] is GraphicsBitmapFill) && (GraphicsFillExtra.isSoftwareDraw(GraphicsBitmapFill(this.graphicsData_[i]))))) {
                    this.graphicsDataStageSoftware_.push(this.graphicsData_[i]);
                    this.graphicsDataStageSoftware_.push(this.graphicsData_[(i + 1)]);
                    this.graphicsDataStageSoftware_.push(this.graphicsData_[(i + 2)]);
                } else {
                    if (((this.graphicsData_[i] is GraphicsSolidFill) && (GraphicsFillExtra.isSoftwareDrawSolid(GraphicsSolidFill(this.graphicsData_[i]))))) {
                        this.graphicsDataStageSoftware_.push(this.graphicsData_[i]);
                        this.graphicsDataStageSoftware_.push(this.graphicsData_[(i + 1)]);
                        this.graphicsDataStageSoftware_.push(this.graphicsData_[(i + 2)]);
                    }
                }
                i++;
            }
            if (this.graphicsDataStageSoftware_.length > 0) {
                map_.graphics.clear();
                map_.graphics.drawGraphicsData(this.graphicsDataStageSoftware_);
                if (this.lastSoftwareClear) {
                    this.lastSoftwareClear = false;
                }
            } else {
                if (!this.lastSoftwareClear) {
                    map_.graphics.clear();
                    this.lastSoftwareClear = true;
                }
            }
            if ((time % 149) == 0) {
                GraphicsFillExtra.manageSize();
            }
        } else {
            map_.graphics.clear();
            map_.graphics.drawGraphicsData(this.graphicsData_);
        }

        map_.filters.length = 0;
        if (player_ != null && (player_.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.MAP_FILTER_BITMASK) != 0) {
            filters = [];
            if (player_.isBlind()) {
                filters.push(BLIND_FILTER);
            }
            map_.filters = filters;
        } else if (map_.filters.length > 0) {
            map_.filters = [];
        }

        mapOverlay_.draw(camera, time);
        partyOverlay_.draw(camera, time);

        if (player_ && player_.isDarkness()) {
            this.darkness.x = -300;
            this.darkness.y = ((Parameters.data_.centerOnPlayer) ? Number(-525) : Number(-515));
            this.darkness.alpha = 0.95;
            addChild(this.darkness);
        } else if (contains(this.darkness)) {
            removeChild(this.darkness);
        }
    }

    private function getFilterIndex():uint {
        var filterIndex:uint = 0;
        if (player_ != null && (player_.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.MAP_FILTER_BITMASK) != 0) {
            if (player_.isPaused()) {
                filterIndex = Renderer.STAGE3D_FILTER_PAUSE;
            } else if (player_.isBlind()) {
                filterIndex = Renderer.STAGE3D_FILTER_BLIND;
            }
        }
        return filterIndex;
    }

    public function allowPlayerTeleport():Boolean {
        if(this.player_.isHidden()) return true;
        return this.allowPlayerTeleport_;
    }
}
}
