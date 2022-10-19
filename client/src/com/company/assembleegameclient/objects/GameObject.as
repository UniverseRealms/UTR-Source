﻿package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.engine3d.Model3D;
import com.company.assembleegameclient.engine3d.Object3D;
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.map.Square;
import com.company.assembleegameclient.map.mapoverlay.CharacterStatusText;
import com.company.assembleegameclient.objects.animation.Animations;
import com.company.assembleegameclient.objects.animation.AnimationsData;
import com.company.assembleegameclient.objects.particles.ExplosionEffect;
import com.company.assembleegameclient.objects.particles.HitEffect;
import com.company.assembleegameclient.objects.particles.ParticleEffect;
import com.company.assembleegameclient.objects.particles.ShockerEffect;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.options.Options;
import com.company.assembleegameclient.util.AnimatedChar;
import com.company.assembleegameclient.util.BloodComposition;
import com.company.assembleegameclient.util.ConditionEffect;
import com.company.assembleegameclient.util.MaskedImage;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.AssetLibrary;
import com.company.util.BitmapUtil;
import com.company.util.CachingColorTransformer;
import com.company.util.ConversionUtil;
import com.company.util.GraphicsUtil;
import com.company.util.MoreColorUtil;

import flash.display.BitmapData;
import flash.display.GradientType;
import flash.display.GraphicsBitmapFill;
import flash.display.GraphicsGradientFill;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.filters.ColorMatrixFilter;
import flash.geom.ColorTransform;
import flash.geom.Matrix;
import flash.geom.Point;
import flash.geom.Vector3D;
import flash.utils.Dictionary;
import flash.utils.getQualifiedClassName;
import flash.utils.getTimer;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.messaging.impl.data.WorldPosData;
import kabam.rotmg.stage3D.GraphicsFillExtra;
import kabam.rotmg.stage3D.Object3D.Object3DStage3D;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.BitmapTextFactory;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.text.view.stringBuilder.StringBuilder;

public class GameObject extends BasicObject {

    protected static const PAUSED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);
    protected static const CURSED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.redFilterMatrix);
    protected static const SHOCKED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);
    protected static const IDENTITY_MATRIX:Matrix = new Matrix();
    private static const ZERO_LIMIT:Number = 1E-5;
    private static const NEGATIVE_ZERO_LIMIT:Number = -(ZERO_LIMIT);
    public static const ATTACK_PERIOD:int = 300;

    public var nameBitmapData_:BitmapData = null;
    private var nameFill_:GraphicsBitmapFill = null;
    private var namePath_:GraphicsPath = null;
    public var shockEffect:ShockerEffect;
    private var isShocked:Boolean;
    private var isShockedTransformSet:Boolean = false;
    private var isCharging:Boolean;
    private var isChargingTransformSet:Boolean = false;
    public var props_:ObjectProperties;
    public var name_:String;
    public var radius_:Number = 0.5;
    public var radiusenemy_:Number = 0.5;
    public var radiusplayer_:Number = 0.45;
    public var facing_:Number = 0;
    public var flying_:Boolean = false;
    public var attackAngle_:Number = 0;
    public var attackStart_:int = 0;
    public var animatedChar_:AnimatedChar = null;
    public var texture_:BitmapData = null;
    public var mask_:BitmapData = null;
    public var randomTextureData_:Vector.<TextureData> = null;
    public var obj3D_:Object3D = null;
    public var object3d_:Object3DStage3D = null;
    public var effect_:ParticleEffect = null;
    public var animations_:Animations = null;
    public var pvp_:Boolean = false;
    public var dead_:Boolean = false;
    protected var portrait_:BitmapData = null;
    protected var texturingCache_:Dictionary = null;
    public var maxHP_:int = 200;
    public var hp_:int = 200;
    public var size_:int = 100;
    public var level_:int = -1;
    public var defense_:int = 0;
    public var slotTypes_:Vector.<int> = null;
    public var equipment_:Vector.<int> = null;
    public var lockedSlot:Vector.<int> = null;
    public var condition_:Vector.<uint>;
    protected var tex1Id_:int = 0;
    protected var tex2Id_:int = 0;
    public var isInteractive_:Boolean = false;
    public var objectType_:int;
    private var nextBulletId_:uint = 1;
    private var sizeMult_:Number = 1;
    public var sinkLevel_:int = 0;
    public var hallucinatingTexture_:BitmapData = null;
    public var flash_:FlashDescription = null;
    public var connectType_:int = -1;
    private var isStunImmune_:Boolean = false;
    private var isWeakImmune_:Boolean = false;
    private var isBleedingImmune_:Boolean = false;
    private var isParalyzeImmune_:Boolean = false;
    private var isDazedImmune_:Boolean = false;
    private var ishpScaleSet:Boolean = false;
    protected var lastTickUpdateTime_:int = 0;
    protected var myLastTickId_:int = -1;
    protected var posAtTick_:Point;
    protected var tickPosition_:Point;
    protected var moveVec_:Vector3D;
    protected var bitmapFill_:GraphicsBitmapFill;
    protected var path_:GraphicsPath;
    protected var vS_:Vector.<Number>;
    protected var uvt_:Vector.<Number>;
    protected var fillMatrix_:Matrix;
    private var backFill_:GraphicsSolidFill = null;
    private var backPath_:GraphicsPath = null;
    private var barFill_:GraphicsSolidFill = null;
    private var barFillPath_:GraphicsPath = null;
    private var protBarFill_:GraphicsSolidFill = null;
    private var protBarFillPath_:GraphicsPath = null;
    private var icons_:Vector.<BitmapData> = null;
    private var iconFills_:Vector.<GraphicsBitmapFill> = null;
    private var iconPaths_:Vector.<GraphicsPath> = null;
    protected var shadowGradientFill_:GraphicsGradientFill = null;
    protected var shadowPath_:GraphicsPath = null;
    protected var glowColor_:int = 0;

    public function GameObject(_arg1:XML) {
        var _local4:int;
        this.props_ = ObjectLibrary.defaultProps_;
        this.condition_ = new <uint>[0, 0];
        this.posAtTick_ = new Point();
        this.tickPosition_ = new Point();
        this.moveVec_ = new Vector3D();
        this.bitmapFill_ = new GraphicsBitmapFill(null, null, false, false);
        this.path_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, null);
        this.vS_ = new Vector.<Number>();
        this.uvt_ = new Vector.<Number>();
        this.fillMatrix_ = new Matrix();
        super();
        if (_arg1 == null) {
            return;
        }
        this.objectType_ = int(_arg1.@type);
        this.props_ = ObjectLibrary.propsLibrary_[this.objectType_];
        hasShadow_ = (this.props_.shadowSize_ > 0);
        var _local2:TextureData = ObjectLibrary.typeToTextureData_[this.objectType_];
        this.texture_ = _local2.texture_;
        this.mask_ = _local2.mask_;
        this.animatedChar_ = _local2.animatedChar_;
        this.randomTextureData_ = _local2.randomTextureData_;
        if (_local2.effectProps_ != null) {
            this.effect_ = ParticleEffect.fromProps(_local2.effectProps_, this);
        }
        if (this.texture_ != null) {
            this.sizeMult_ = (this.texture_.height / 8);
        }
        if (_arg1.hasOwnProperty("Model")) {
            this.obj3D_ = Model3D.getObject3D(String(_arg1.Model));
            this.object3d_ = Model3D.getStage3dObject3D(String(_arg1.Model));
            if (this.texture_ != null) {
                this.object3d_.setBitMapData(this.texture_);
            }
        }
        var _local3:AnimationsData = ObjectLibrary.typeToAnimationsData_[this.objectType_];
        if (_local3 != null) {
            this.animations_ = new Animations(_local3);
        }
        z_ = this.props_.z_;
        this.flying_ = this.props_.flying_;
        if (_arg1.hasOwnProperty("MaxHitPoints")) {
            this.hp_ = (this.maxHP_ = int(_arg1.MaxHitPoints));
        }
        if (_arg1.hasOwnProperty("Defense")) {
            this.defense_ = int(_arg1.Defense);
        }
        if (_arg1.hasOwnProperty("SlotTypes")) {
            this.slotTypes_ = ConversionUtil.toIntVector(_arg1.SlotTypes);
            this.equipment_ = new Vector.<int>(this.slotTypes_.length);
            _local4 = 0;
            while (_local4 < this.equipment_.length) {
                this.equipment_[_local4] = -1;
                _local4++;
            }
            this.lockedSlot = new Vector.<int>(this.slotTypes_.length);
        }
        if (_arg1.hasOwnProperty("Tex1")) {
            this.tex1Id_ = int(_arg1.Tex1);
        }
        if (_arg1.hasOwnProperty("Tex2")) {
            this.tex2Id_ = int(_arg1.Tex2);
        }
        if (_arg1.hasOwnProperty("StunImmune")) {
            this.isStunImmune_ = true;
        }
        if (_arg1.hasOwnProperty("ParalyzeImmune")) {
            this.isParalyzeImmune_ = true;
        }
        if (_arg1.hasOwnProperty("DazedImmune")) {
            this.isDazedImmune_ = true;
        }
        this.props_.loadSounds();
    }

    public static function damageWithDefense(_arg1:int, _arg2:int, _arg3:Boolean, _arg4:Vector.<uint>):int { //player concerned
        var _local5:int = _arg2;

        if(_arg3){
            _local5 = _local5 / 4;
        }
        if (!((_arg4[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_BIT) == 0)) {
            _local5 = _local5 / 4;
        }
        else {
            if ((_arg4[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORED_BIT) != 0) {
                _local5 = (_local5 * 2);
            }
        }
        var _local6:int = _arg1 * 3 / 20 ;
        var _local7:int = Math.max(_local6, (_arg1 - _local5));
        if ((_arg4[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVULNERABLE_BIT) != 0) {
            _local7 = 0;
        }
        if ((_arg4[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_BIT) != 0) {
            _local7 = (_local7 * 0.9);
        }
        if ((_arg4[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_BIT) != 0) {
            _local7 = (_local7 * 1.25);
        }
        return (_local7);
    }

    public static function damageWithDefenseEnemy(_arg1:int, _arg2:int, _arg3:Boolean, _arg4:Vector.<uint>):int { //player concerned
        var _local5:int = _arg2;

        if(_arg3){
            _local5 = _local5 / 4;
        }
        if (!((_arg4[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_BIT) == 0)) {
            _local5 = _local5 / 4;
        }
        else {
            if ((_arg4[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORED_BIT) != 0) {
                _local5 = (_local5 * 2);
            }
        }
        var _local6:int = _arg1 * 3 / 20 ;
        var _local7:int = Math.max(_local6, (_arg1 - _local5));
        if ((_arg4[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVULNERABLE_BIT) != 0) {
            _local7 = 0;
        }
        if ((_arg4[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_BIT) != 0) {
            _local7 = (_local7 * 0.9);
        }
        if ((_arg4[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_BIT) != 0) {
            _local7 = (_local7 * 1.25);
        }
        return (_local7);
    }


    public function setObjectId(objId:int):void {
        var texData:TextureData;
        objectId_ = objId;

        if (this.randomTextureData_ != null) {
            texData = this.randomTextureData_[objectId_ % this.randomTextureData_.length];
            this.texture_ = texData.texture_;
            this.mask_ = texData.mask_;
            this.animatedChar_ = texData.animatedChar_;
            if (this.object3d_ != null) {
                this.object3d_.setBitMapData(this.texture_);
            }
        }
    }

    public function setAltTexture(altTexId:int):void {
        var altTexData:TextureData;
        var curTexData:TextureData = ObjectLibrary.typeToTextureData_[this.objectType_];

        if (altTexId == 0) {
            altTexData = curTexData;
        } else {
            altTexData = curTexData.getAltTextureData(altTexId);
            if (altTexData == null) {
                return;
            }
        }

        this.texture_ = altTexData.texture_;
        this.mask_ = altTexData.mask_;
        this.animatedChar_ = altTexData.animatedChar_;

        if (this.effect_ != null) {
            map_.removeObj(this.effect_.objectId_);
            this.effect_ = null;
        }

        if (altTexData.effectProps_ != null && !Parameters.data_.noParticlesMaster) {
            this.effect_ = ParticleEffect.fromProps(altTexData.effectProps_, this);
            if (map_ != null) {
                map_.addObj(this.effect_, x_, y_);
            }
        }
    }

    public function canBeHitBy(_arg1:GameObject) : Boolean {
        return (this.pvp_ && _arg1.pvp_);
    }

    public function setTex1(_arg1:int):void {
        if (_arg1 == this.tex1Id_) {
            return;
        }
        this.tex1Id_ = _arg1;
        this.clearCache();
    }

    public function setTex2(_arg1:int):void {
        if (_arg1 == this.tex2Id_) {
            return;
        }
        this.tex2Id_ = _arg1;
        this.clearCache();
    }

    public function setSize(size:int):void {
        this.size_ = size;
        if (this is Player)
            this.clearCache();
    }

    public function setGlow(glow:int):void {
        if (this.glowColor_ == glow) {
            return;
        }
        this.glowColor_ = glow;
        this.clearCache();
    }

    public function clearCache():void {
        this.texturingCache_ = new Dictionary();
        this.portrait_ = null;
    }

    public function playSound(_arg1:int):void {
        SoundEffectLibrary.play(this.props_.sounds_[_arg1]);
    }

    override public function dispose():void {
        var _local1:Object;
        var _local2:BitmapData;
        var _local3:Dictionary;
        var _local4:Object;
        var _local5:BitmapData;
        super.dispose();
        this.texture_ = null;
        if (this.portrait_ != null) {
            this.portrait_.dispose();
            this.portrait_ = null;
        }
        if (this.texturingCache_ != null) {
            for each (_local1 in this.texturingCache_) {
                _local2 = (_local1 as BitmapData);
                if (_local2 != null) {
                    _local2.dispose();
                }
                else {
                    _local3 = (_local1 as Dictionary);
                    for each (_local4 in _local3) {
                        _local5 = (_local4 as BitmapData);
                        if (_local5 != null) {
                            _local5.dispose();
                        }
                    }
                }
            }
            this.texturingCache_ = null;
        }
        if (this.obj3D_ != null) {
            this.obj3D_.dispose();
            this.obj3D_ = null;
        }
        if (this.object3d_ != null) {
            this.object3d_.dispose();
            this.object3d_ = null;
        }
        this.slotTypes_ = null;
        this.equipment_ = null;
        this.lockedSlot = null;
        if (this.nameBitmapData_ != null) {
            this.nameBitmapData_.dispose();
            this.nameBitmapData_ = null;
        }
        this.nameFill_ = null;
        this.namePath_ = null;
        this.bitmapFill_ = null;
        this.path_.commands = null;
        this.path_.data = null;
        this.vS_ = null;
        this.uvt_ = null;
        this.fillMatrix_ = null;
        this.icons_ = null;
        this.iconFills_ = null;
        this.iconPaths_ = null;
        this.shadowGradientFill_ = null;
        if (this.shadowPath_ != null) {
            this.shadowPath_.commands = null;
            this.shadowPath_.data = null;
            this.shadowPath_ = null;
        }
    }

    public function isQuiet():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.QUIET_BIT) == 0)));
    }

    public function isWeak():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.WEAK_BIT) == 0)));
    }

    public function isSlowed():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.SLOWED_BIT) == 0)));
    }

    public function isSick():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.SICK_BIT) == 0)));
    }

    public function isDazed():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DAZED_BIT) == 0)));
    }

    public function isStunned():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.STUNNED_BIT) == 0)));
    }

    public function isBlind():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.BLIND_BIT) == 0)));
    }

    public function isDrunk():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DRUNK_BIT) == 0)));
    }

    public function isConfused():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.CONFUSED_BIT) == 0)));
    }

    public function isStunImmune():Boolean {
        return (((!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.STUN_IMMUNE_BIT) == 0))) || (this.isStunImmune_)));
    }

    public function isWeakImmune():Boolean {
        return (((!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.WEAK_IMMUNE_BIT) == 0))) || (this.isWeakImmune_)));
    }

    public function isBleedingImmune():Boolean {
        return (((!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.BLEEDING_IMMUNE_BIT) == 0))) || (this.isBleedingImmune_)));
    }

    public function isInvisible():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVISIBLE_BIT) == 0)));
    }

    public function isParalyzed():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.PARALYZED_BIT) == 0)));
    }

    public function isSpeedy():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.SPEEDY_BIT) == 0)));
    }

    public function isNinjaSpeedy():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.NINJA_SPEEDY_BIT) == 0)));
    }

    public function isHallucinating():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.HALLUCINATING_BIT) == 0)));
    }

    public function isHealing():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.HEALING_BIT) == 0)));
    }

    public function isEmpowered():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.EMPOWERED_BIT) == 0)));
    }

    public function isDamaging():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DAMAGING_BIT) == 0)));
    }

    public function isBerserk():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.BERSERK_BIT) == 0)));
    }

    public function isPaused():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.PAUSED_BIT) == 0)));
    }

    public function isStasis():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.STASIS_BIT) == 0)));
    }

    public function isInvincible():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVINCIBLE_BIT) == 0)));
    }

    public function isInvulnerable():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVULNERABLE_BIT) == 0)));
    }

    public function isArmored():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORED_BIT) == 0)));
    }

    public function isArmorBroken():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_BIT) == 0)));
    }

    public function isArmorBrokenImmune():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_IMMUNE_BIT) == 0)));
    }

    public function isSlowedImmune():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SLOWED_IMMUNE_BIT) == 0)));
    }

    public function isUnstable():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.UNSTABLE_BIT) == 0)));
    }

    public function isSwiftness():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SWIFTNESS_BIT) == 0)));
    }

    public function isDarkness():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DARKNESS_BIT) == 0)));
    }

    public function isParalyzeImmune():Boolean {
        return (((this.isParalyzeImmune_) || (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PARALYZED_IMMUNE_BIT) == 0)))));
    }

    public function isDazedImmune():Boolean {
        return (((this.isDazedImmune_) || (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.DAZED_IMMUNE_BIT) == 0)))));
    }

    public function isPetrified():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_BIT) == 0)));
    }

    public function isPetrifiedImmune():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_IMMUNE_BIT) == 0)));
    }

    public function isCursed():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_BIT) == 0)));
    }

    public function isCursedImmune():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_IMMUNE_BIT) == 0)));
    }

    public function isHidden() : Boolean
    {
        return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.HIDDEN_BIT) != 0;
    }

    public function isSamuraiBerserk():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SAMURAIBERSERK_BIT) == 0)));
    }

    public function isRelentless():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.RELENTLESS_BIT) == 0)));
    }

    public function isVengeance():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.VENGEANCE_BIT) == 0)));
    }

    public function isAlliance():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.ALLIANCE_BIT) == 0)));
    }

    public function isGrasp():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.GRASPOFZOL_BIT) == 0)));
    }

    public function isBravery():Boolean {
        return (!(((this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.BRAVERY_BIT) == 0)));
    }

    public function isUntargetable() :Boolean {
        return isInvincible() || isPaused() || isStasis() || dead_;
    }

    public function isSafe(_arg1:int = 20):Boolean {
        var _local2:GameObject;
        var _local3:int;
        var _local4:int;
        for each (_local2 in map_.goDict_) {
            if ((((_local2 is Character)) && (_local2.props_.isEnemy_))) {
                _local3 = (((x_ > _local2.x_)) ? (x_ - _local2.x_) : (_local2.x_ - x_));
                _local4 = (((y_ > _local2.y_)) ? (y_ - _local2.y_) : (_local2.y_ - y_));
                if ((((_local3 < _arg1)) && ((_local4 < _arg1)))) {
                    return (false);
                }
            }
        }
        return (true);
    }

    public function getName():String {
        return ((((((this.name_ == null)) || ((this.name_ == "")))) ? ObjectLibrary.typeToDisplayId_[this.objectType_] : this.name_));
    }

    public function getColor():uint {
        return (BitmapUtil.mostCommonColor(this.texture_));
    }

    public function getBulletId():uint {
        var _local1:uint = this.nextBulletId_;
        this.nextBulletId_ = ((this.nextBulletId_ + 1) % 128);
        return (_local1);
    }

    public function distTo(_arg1:WorldPosData):Number {
        var _local2:Number = (_arg1.x_ - x_);
        var _local3:Number = (_arg1.y_ - y_);
        return (Math.sqrt(((_local2 * _local2) + (_local3 * _local3))));
    }

    public function toggleShockEffect(_arg1:Boolean):void {
        if (_arg1) {
            this.isShocked = true;
        }
        else {
            this.isShocked = false;
            this.isShockedTransformSet = false;
        }
    }

    public function toggleChargingEffect(_arg1:Boolean):void {
        if (_arg1) {
            this.isCharging = true;
        }
        else {
            this.isCharging = false;
            this.isChargingTransformSet = false;
        }
    }

    override public function addTo(_arg1:Map, _arg2:Number, _arg3:Number):Boolean {
        map_ = _arg1;
        this.posAtTick_.x = (this.tickPosition_.x = _arg2);
        this.posAtTick_.y = (this.tickPosition_.y = _arg3);
        if (!this.moveTo(_arg2, _arg3)) {
            map_ = null;
            return (false);
        }
        if (this.effect_ != null) {
            map_.addObj(this.effect_, _arg2, _arg3);
        }
        return (true);
    }

    override public function removeFromMap():void {
        if (((this.props_.static_) && (!((square_ == null))))) {
            if (square_.obj_ == this) {
                square_.obj_ = null;
            }
            square_ = null;
        }
        if (this.effect_ != null) {
            map_.removeObj(this.effect_.objectId_);
        }
        super.removeFromMap();
        this.dispose();
    }

    public function moveTo(_arg1:Number, _arg2:Number):Boolean {
        var _local3:Square = map_.getSquare(_arg1, _arg2);
        if (_local3 == null) {
            return (false);
        }
        x_ = _arg1;
        y_ = _arg2;
        if (this.props_.static_) {
            if (square_ != null) {
                square_.obj_ = null;
            }
            _local3.obj_ = this;
        }
        square_ = _local3;
        if (this.obj3D_ != null) {
            this.obj3D_.setPosition(x_, y_, 0, this.props_.rotation_);
        }
        if (this.object3d_ != null) {
            this.object3d_.setPosition(x_, y_, 0, this.props_.rotation_);
        }
        return (true);
    }

    override public function update(_arg1:int, _arg2:int):Boolean {
        var _local4:int;
        var _local5:Number;
        var _local6:Number;
        var _local3:Boolean;
        if (!(((this.moveVec_.x == 0)) && ((this.moveVec_.y == 0)))) {
            if (this.myLastTickId_ < map_.gs_.gsc_.lastTickId_) {
                this.moveVec_.x = 0;
                this.moveVec_.y = 0;
                this.moveTo(this.tickPosition_.x, this.tickPosition_.y);
            }
            else {
                _local4 = (_arg1 - this.lastTickUpdateTime_);
                _local5 = (this.posAtTick_.x + (_local4 * this.moveVec_.x));
                _local6 = (this.posAtTick_.y + (_local4 * this.moveVec_.y));
                this.moveTo(_local5, _local6);
                _local3 = true;
            }
        }
        if (this.props_.whileMoving_ != null) {
            if (!_local3) {
                z_ = this.props_.z_;
                this.flying_ = this.props_.flying_;
            }
            else {
                z_ = this.props_.whileMoving_.z_;
                this.flying_ = this.props_.whileMoving_.flying_;
            }
        }
        return (true);
    }

    public function onGoto(_arg1:Number, _arg2:Number, _arg3:int):void {
        this.moveTo(_arg1, _arg2);
        this.lastTickUpdateTime_ = _arg3;
        this.tickPosition_.x = _arg1;
        this.tickPosition_.y = _arg2;
        this.posAtTick_.x = _arg1;
        this.posAtTick_.y = _arg2;
        this.moveVec_.x = 0;
        this.moveVec_.y = 0;
    }

    public function onTickPos(_arg1:Number, _arg2:Number, _arg3:int, _arg4:int):void {
        if (this.myLastTickId_ < map_.gs_.gsc_.lastTickId_) {
            this.moveTo(this.tickPosition_.x, this.tickPosition_.y);
        }
        this.lastTickUpdateTime_ = map_.gs_.lastUpdate_;
        this.tickPosition_.x = _arg1;
        this.tickPosition_.y = _arg2;
        this.posAtTick_.x = x_;
        this.posAtTick_.y = y_;
        this.moveVec_.x = ((this.tickPosition_.x - this.posAtTick_.x) / _arg3);
        this.moveVec_.y = ((this.tickPosition_.y - this.posAtTick_.y) / _arg3);
        this.myLastTickId_ = _arg4;
    }

    public function criticalDamage(_arg_1:int, _arg_2:int, _arg_3:Vector.<uint>, _arg_4:Boolean, _arg_5:Projectile, multi:Number):void {
        var _local_7:int;
        var _local_8:uint;
        var _local_9:ConditionEffect;
        var _local_10:CharacterStatusText;
        var _local_13:String;
        var _local_14:Vector.<uint>;
        var _local_15:Boolean;
        var _local_6:Boolean;
        if (_arg_4) {
            this.dead_ = true;
        }
        else {
            if (_arg_3 != null) {
                _local_7 = 0;
                for each (_local_8 in _arg_3) {
                    _local_9 = null;
                    switch (_local_8) {
                        case ConditionEffect.NOTHING:
                            break;
                        case ConditionEffect.CONFUSED:
                            _local_9 = ConditionEffect.effects_[_local_8];
                            break;
                        case ConditionEffect.GROUND_DAMAGE:
                            _local_6 = true;
                            break;
                    }
                    if (_local_9 != null) {
                        if (_local_8 < ConditionEffect.NEW_CON_THREASHOLD) {
                            if ((this.condition_[ConditionEffect.CE_FIRST_BATCH] | _local_9.bit_) == this.condition_[ConditionEffect.CE_FIRST_BATCH]) continue;
                            this.condition_[ConditionEffect.CE_FIRST_BATCH] = (this.condition_[ConditionEffect.CE_FIRST_BATCH] | _local_9.bit_);
                        }
                        else {
                            if ((this.condition_[ConditionEffect.CE_SECOND_BATCH] | _local_9.bit_) == this.condition_[ConditionEffect.CE_SECOND_BATCH]) continue;
                            this.condition_[ConditionEffect.CE_SECOND_BATCH] = (this.condition_[ConditionEffect.CE_SECOND_BATCH] | _local_9.bit_);
                        }
                        _local_13 = _local_9.localizationKey_;
                        this.showConditionEffect(_local_7, _local_13);
                        _local_7 = (_local_7 + 500);
                    }
                }
            }
        }
        if (!((this.props_.isEnemy_) && (Parameters.data_.disableEnemyParticles))) {
            _local_14 = BloodComposition.getBloodComposition(this.objectType_, this.texture_, this.props_.bloodProb_, this.props_.bloodColor_);
            if (this.dead_) {
                map_.addObj(new ExplosionEffect(_local_14, this.size_, 30), x_, y_);
            }
            else {
                if (_arg_5 != null) {
                    map_.addObj(new HitEffect(_local_14, this.size_, 10, _arg_5.angle_, _arg_5.projProps_.speed_), x_, y_);
                }
                else {
                    map_.addObj(new ExplosionEffect(_local_14, this.size_, 10), x_, y_);
                }
            }
        }
        if (!_arg_4
                && ((Parameters.data_.noEnemyDamage && (_arg_5 == null || (this.props_.isEnemy_ && _arg_5.ownerId_ != map_.player_.objectId_)))
                        || (Parameters.data_.noAllyDamage && this.props_.isPlayer_ && this != map_.player_))) {
            return;
        }
        if (_arg_2 > 0) {
            _local_15 = ((((this.isArmorBroken()) || (((!((_arg_5 == null))) && (_arg_5.projProps_.armorPiercing_))))) || (_local_6));
            this.showDamageText2(_arg_2, _local_15, multi);
        }
    }

    public function showDamageText2(_arg_1:int, _arg_2:Boolean, multi:Number):void {
        var _local_3:String = ("-" + _arg_1 + " (" + multi + "x)");
        var _local_4:CharacterStatusText = new CharacterStatusText(this, ((_arg_2) ? 0xFF4500 : 0xFFFF00), 1000);
        _local_4.setStringBuilder(new StaticStringBuilder(_local_3));
        map_.mapOverlay_.addStatusText(_local_4);
    }

    public function damage(_arg1:int, dmgAmount:int, _arg3:Vector.<uint>, dead:Boolean, projectile:Projectile):void {
        var offsetTime:int;
        var condEffect:uint;
        var ce:ConditionEffect;
        var _local10:CharacterStatusText;
        var locKey:String;
        var colors:Vector.<uint>;
        var pierced:Boolean;
        var groundDamage:Boolean;
        if (dead && !this.props_.isPlayer_) {
            this.dead_ = true;
        }
        else {
            if (_arg3 != null) {
                offsetTime = 0;
                for each (condEffect in _arg3) {
                    ce = null;
                    switch (condEffect) {
                        case ConditionEffect.NOTHING:
                            break;
                        case ConditionEffect.CONFUSED:
                            ce = ConditionEffect.effects_[condEffect];
                            break;
                        case ConditionEffect.GROUND_DAMAGE:
                            groundDamage = true;
                            break;
                    }
                    if (ce != null) {
                        if (condEffect < ConditionEffect.NEW_CON_THREASHOLD) {
                            if ((this.condition_[ConditionEffect.CE_FIRST_BATCH] | ce.bit_) == this.condition_[ConditionEffect.CE_FIRST_BATCH]) continue;
                            this.condition_[ConditionEffect.CE_FIRST_BATCH] = (this.condition_[ConditionEffect.CE_FIRST_BATCH] | ce.bit_);
                        }
                        else {
                            if ((this.condition_[ConditionEffect.CE_SECOND_BATCH] | ce.bit_) == this.condition_[ConditionEffect.CE_SECOND_BATCH]) continue;
                            this.condition_[ConditionEffect.CE_SECOND_BATCH] = (this.condition_[ConditionEffect.CE_SECOND_BATCH] | ce.bit_);
                        }
                        locKey = ce.localizationKey_;
                        this.showConditionEffect(offsetTime, locKey);
                        offsetTime = (offsetTime + 500);
                    }
                }
            }
        }
        if (!dead
                && ((Parameters.data_.noEnemyDamage && this.props_.isEnemy_ && (projectile == null || projectile.ownerId_ != map_.player_.objectId_))
                        || (Parameters.data_.noAllyDamage && this.props_.isPlayer_ && this.objectId_ != map_.player_.objectId_))) {
            return;
        }
        if (!((this.props_.isEnemy_ || this.props_.isAlly_) && (Parameters.data_.disableEnemyParticles))) {
            colors = BloodComposition.getBloodComposition(this.objectType_, this.texture_, this.props_.bloodProb_, this.props_.bloodColor_);
            if (this.dead_) {
                map_.addObj(new ExplosionEffect(colors, this.size_, 30), x_, y_);
            }
            else {
                if (projectile != null) {
                    map_.addObj(new HitEffect(colors, this.size_, 10, projectile.angle_, projectile.projProps_.speed_), x_, y_);
                }
                else {
                    map_.addObj(new ExplosionEffect(colors, this.size_, 10), x_, y_);
                }
            }
        }
        if (dmgAmount > 0) {
            pierced = this.isArmorBroken() || projectile != null && projectile.projProps_.armorPiercing_ || groundDamage;
            if (this is Player && (this as Player).protectionPoints_ > 0) {
                this.showDamageText3(dmgAmount, pierced);
            } else {
                this.showDamageText(dmgAmount, pierced);
            }
        }
    }

    public function showConditionEffect(_arg1:int, _arg2:String):void {
        var _local3:CharacterStatusText = new CharacterStatusText(this, 0xFF0000, 3000, _arg1);
        _local3.setStringBuilder(new LineBuilder().setParams(_arg2));
        map_.mapOverlay_.addStatusText(_local3);
    }

    public function showDamageText(_arg1:int, _arg2:Boolean):void {
        var _local3:String = ("-" + _arg1);
        var _local4:CharacterStatusText = new CharacterStatusText(this, ((_arg2) ? 0x9000FF : 0xFF0000), 1000);
        _local4.setStringBuilder(new StaticStringBuilder(_local3));
        map_.mapOverlay_.addStatusText(_local4);
    }

    public function showDamageText3(_arg1:int, _arg2:Boolean):void {
        var _local3:String = ("-" + _arg1);
        var _local4:CharacterStatusText = new CharacterStatusText(this, ((_arg2) ? 0xFFFFFF : 0xFFFFFF), 1000);
        _local4.setStringBuilder(new StaticStringBuilder(_local3));
        map_.mapOverlay_.addStatusText(_local4);
    }

    protected function makeNameBitmapData():BitmapData {
        var _local1:StringBuilder = new StaticStringBuilder(this.name_);
        var _local2:BitmapTextFactory = StaticInjectorContext.getInjector().getInstance(BitmapTextFactory);
        return (_local2.make(_local1, 16, 0xFFFFFF, true, IDENTITY_MATRIX, true));
    }

    public function drawName(_arg1:Vector.<IGraphicsData>, _arg2:Camera):void {
        if (this.nameBitmapData_ == null) {
            this.nameBitmapData_ = this.makeNameBitmapData();
            this.nameFill_ = new GraphicsBitmapFill(null, new Matrix(), false, false);
            this.namePath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
        }
        var _local3:int = ((this.nameBitmapData_.width / 2) + 1);
        var _local4:int = 30;
        var _local5:Vector.<Number> = this.namePath_.data;
        _local5.length = 0;
        _local5.push((posS_[0] - _local3), posS_[1], (posS_[0] + _local3), posS_[1], (posS_[0] + _local3), (posS_[1] + _local4), (posS_[0] - _local3), (posS_[1] + _local4));
        this.nameFill_.bitmapData = this.nameBitmapData_;
        var _local6:Matrix = this.nameFill_.matrix;
        _local6.identity();
        _local6.translate(_local5[0], _local5[1]);
        _arg1.push(this.nameFill_);
        _arg1.push(this.namePath_);
        _arg1.push(GraphicsUtil.END_FILL);
    }

    protected function getHallucinatingTexture():BitmapData {
        if (this.hallucinatingTexture_ == null) {
            this.hallucinatingTexture_ = AssetLibrary.getImageFromSet("lofiChar8x8", int((Math.random() * 239)));
        }
        return (this.hallucinatingTexture_);
    }

    protected function getTexture(_arg_1:Camera, _arg_2:int) : BitmapData
    {
        var _local_4:Number = NaN;
        var _local_5:int = 0;
        var _local_6:MaskedImage = null;
        var _local_7:int = 0;
        var _local_8:BitmapData = null;
        var _local_9:int = 0;
        var _local_10:BitmapData = null;
        var _local_11:BitmapData = null;
        var _local_12:BitmapData = this.texture_;
        var _local_13:int = this.size_;
        if(this is Container && (Parameters.data_.bigBag))
        {
            if(this.objectType_ == 0x050C)
            {
                this.texture_ = AssetLibrary.getImageFromSet("lofiObj4", 0xa5);
            }

            if(this.objectType_ == 0x7245)
            {
                this.texture_ = AssetLibrary.getImageFromSet("lofiObj4", 0xa6);
            }

        }
        if(this.animatedChar_ != null)
        {
            _local_4 = 0;
            _local_5 = AnimatedChar.STAND;
            if(_arg_2 < this.attackStart_ + ATTACK_PERIOD)
            {
                if(!this.props_.dontFaceAttacks_)
                {
                    this.facing_ = this.attackAngle_;
                }
                _local_4 = (_arg_2 - this.attackStart_) % ATTACK_PERIOD / ATTACK_PERIOD;
                _local_5 = AnimatedChar.ATTACK;
            }
            else if(this.moveVec_.x != 0 || this.moveVec_.y != 0)
            {
                _local_7 = int(0.5 / this.moveVec_.length);
                _local_7 = _local_7 + (400 - _local_7 % 400);
                if(this.moveVec_.x > ZERO_LIMIT || this.moveVec_.x < NEGATIVE_ZERO_LIMIT || this.moveVec_.y > ZERO_LIMIT || this.moveVec_.y < NEGATIVE_ZERO_LIMIT)
                {
                    this.facing_ = Math.atan2(this.moveVec_.y,this.moveVec_.x);
                    _local_5 = AnimatedChar.WALK;
                }
                else
                {
                    _local_5 = AnimatedChar.STAND;
                }
                _local_4 = _arg_2 % _local_7 / _local_7;
            }
            _local_6 = this.animatedChar_.imageFromFacing(this.facing_,_arg_1,_local_5,_local_4);
            _local_12 = _local_6.image_;
            _local_11 = _local_6.mask_;
        }
        else if(this.animations_ != null)
        {
            _local_8 = this.animations_.getTexture(_arg_2);
            if(_local_8 != null)
            {
                _local_12 = _local_8;
            }
        }
        if(this.props_.drawOnGround_ || this.obj3D_ != null)
        {
            return _local_12;
        }
        if(this.obj3D_ != null){
            if(Parameters.isGpuRender()){
                _local_12 = TextureRedrawer.redraw(_local_12, this.size_, true, 0);
                return _local_12
            }
            return _local_12
        }
        if(_arg_1.isHallucinating_)
        {
            _local_9 = _local_12 == null?int(8):int(_local_12.width);
            _local_12 = this.getHallucinatingTexture();
            _local_11 = null;
            _local_13 = int(this.size_ * Math.min(1.5,_local_9 / _local_12.width));
        }
        if(this.isStasis() || this.isPetrified())
        {
            _local_12 = CachingColorTransformer.filterBitmapData(_local_12,PAUSED_FILTER);
        }
        if(this.tex1Id_ == 0 && this.tex2Id_ == 0)
        {
            if(this.isCursed() && Parameters.data_.curseIndication)
            {
                _local_12 = TextureRedrawer.redraw(_local_12,_local_13,false,16711680);
            }
            else
            {
                _local_12 = TextureRedrawer.redraw(_local_12,_local_13,false,0);
            }
        }
        else
        {
            _local_10 = null;
            if(this.texturingCache_ == null)
            {
                this.texturingCache_ = new Dictionary();
            }
            else
            {
                _local_10 = this.texturingCache_[_local_12];
            }
            if(_local_10 == null)
            {
                _local_10 = TextureRedrawer.resize(_local_12,_local_11,_local_13,false,this.tex1Id_,this.tex2Id_);
                _local_10 = GlowRedrawer.outlineGlow(_local_10,0);
                this.texturingCache_[_local_12] = _local_10;
            }
            _local_12 = _local_10;
        }
        return _local_12;
    }

    public function useAltTexture(_arg1:String, _arg2:int):void {
        this.texture_ = AssetLibrary.getImageFromSet(_arg1, _arg2);
        this.sizeMult_ = (this.texture_.height / 8);
    }

    public function getPortrait():BitmapData {
        var _local1:BitmapData;
        var _local2:int;
        if (this.portrait_ == null) {
            _local1 = (((this.props_.portrait_) != null) ? this.props_.portrait_.getTexture() : this.texture_);
            _local2 = ((4 / _local1.width) * 100);
            this.portrait_ = TextureRedrawer.resize(_local1, this.mask_, _local2, true, this.tex1Id_, this.tex2Id_);
            this.portrait_ = GlowRedrawer.outlineGlow(this.portrait_, 0);
        }
        return (this.portrait_);
    }

    public function setAttack(_arg1:int, _arg2:Number):void {
        this.attackAngle_ = _arg2;
        this.attackStart_ = getTimer();
    }

    override public function draw3d(_arg1:Vector.<Object3DStage3D>):void {
        if (this.object3d_ != null) {
            _arg1.push(this.object3d_);
        }
    }

    protected function drawHpBar(gfx:Vector.<IGraphicsData>, offsetY:int):void {
        var scale:Number;
        if (this.barFillPath_ == null) {
            this.backFill_ = new GraphicsSolidFill();
            this.backPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
            this.barFill_ = new GraphicsSolidFill(0x10FF00);
            this.barFillPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
            this.protBarFill_ = new GraphicsSolidFill(0xFFFFFF);
            this.protBarFillPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
        }
        this.backFill_.color = 1118481;
        this.backPath_.data.length = 0;
        (this.backPath_.data as Vector.<Number>).push(posS_[0] - 20 - 1.2,
                posS_[1] + offsetY - 1.2,
                posS_[0] + 20 + 1.2,
                posS_[1] + offsetY - 1.2,
                posS_[0] + 20 + 1.2,
                posS_[1] + offsetY + 5 + 1.2,
                posS_[0] - 20 - 1.2,
                posS_[1] + offsetY + 5 + 1.2);
        gfx.push(this.backFill_);
        gfx.push(this.backPath_);
        gfx.push(GraphicsUtil.END_FILL);
        if (this.hp_ > 0) {
            scale = this.hp_ / this.maxHP_ * 40;
            this.barFillPath_.data.length = 0;
            (this.barFillPath_.data as Vector.<Number>).push(posS_[0] - 20,
                    posS_[1] + offsetY,
                    posS_[0] - 20 + scale,
                    posS_[1] + offsetY,
                    posS_[0] - 20 + scale,
                    posS_[1] + offsetY + 5,
                    posS_[0] - 20,
                    posS_[1] + offsetY + 5);
            gfx.push(this.barFill_);
            gfx.push(this.barFillPath_);
            gfx.push(GraphicsUtil.END_FILL);
        }
        if (this.props_.isPlayer_) {
            var player:Player = this as Player;
            if (player && player.protectionPoints_ > 0) {
                scale = player.protectionPoints_ / player.protectionPointsMax_ * 40;
                this.protBarFillPath_.data.length = 0;
                (this.protBarFillPath_.data as Vector.<Number>).push(posS_[0] - 20,
                        posS_[1] + offsetY,
                        posS_[0] - 20 + scale,
                        posS_[1] + offsetY,
                        posS_[0] - 20 + scale,
                        posS_[1] + offsetY + 5,
                        posS_[0] - 20,
                        posS_[1] + offsetY + 5);
                gfx.push(this.protBarFill_);
                gfx.push(this.protBarFillPath_);
                gfx.push(GraphicsUtil.END_FILL);
            }
        }
        GraphicsFillExtra.setSoftwareDrawSolid(this.protBarFill_, true);
        GraphicsFillExtra.setSoftwareDrawSolid(this.barFill_, true);
        GraphicsFillExtra.setSoftwareDrawSolid(this.backFill_, true);
    }

    override public function draw(_arg1:Vector.<IGraphicsData>, _arg2:Camera, _arg3:int):void {
        var _local8:BitmapData;
        var _local9:uint;
        var _local10:uint;
        var _local4:BitmapData = this.getTexture(_arg2, _arg3);
        if (this.props_.drawOnGround_) {
            if (square_.faces_.length == 0) {
                return;
            }
            this.path_.data = square_.faces_[0].face_.vout_;
            this.bitmapFill_.bitmapData = _local4;
            square_.baseTexMatrix_.calculateTextureMatrix(this.path_.data);
            this.bitmapFill_.matrix = square_.baseTexMatrix_.tToS_;
            _arg1.push(this.bitmapFill_);
            _arg1.push(this.path_);
            _arg1.push(GraphicsUtil.END_FILL);
            return;
        }
        if (this.obj3D_ != null && !Parameters.isGpuRender()) {
            this.obj3D_.draw(_arg1, _arg2, this.props_.color_, _local4);
            return;
        }
        /*if (((!((this.obj3D_ == null))) && (Parameters.isGpuRender()))) {
            _arg1.push(null);
            return;
        }*/

        var _local5:int = _local4.width;
        var _local6:int = _local4.height;
        var _local7:int = (square_.sink_ + this.sinkLevel_);
        if ((((_local7 > 0)) && (((this.flying_) || (((!((square_.obj_ == null))) && (square_.obj_.props_.protectFromSink_))))))) {
            _local7 = 0;
        }
        if (Parameters.isGpuRender()) {
            if (_local7 != 0) {
                GraphicsFillExtra.setSinkLevel(this.bitmapFill_, Math.max((((_local7 / _local6) * 1.65) - 0.02), 0));
                _local7 = (-(_local7) + 0.02);
            }
            else {
                if ((((_local7 == 0)) && (!((GraphicsFillExtra.getSinkLevel(this.bitmapFill_) == 0))))) {
                    GraphicsFillExtra.clearSink(this.bitmapFill_);
                }
            }
        }
        this.vS_.length = 0;
        this.vS_.push((posS_[3] - (_local5 / 2)), ((posS_[4] - _local6) + _local7), (posS_[3] + (_local5 / 2)), ((posS_[4] - _local6) + _local7), (posS_[3] + (_local5 / 2)), posS_[4], (posS_[3] - (_local5 / 2)), posS_[4]);
        this.path_.data = this.vS_;
        if (this.flash_ != null) {
            if (!this.flash_.doneAt(_arg3)) {
                if (Parameters.isGpuRender()) {
                    this.flash_.applyGPUTextureColorTransform(_local4, _arg3);
                }
                else {
                    _local4 = this.flash_.apply(_local4, _arg3);
                }
            }
            else {
                this.flash_ = null;
            }
        }
        if (((this.isShocked) && (!(this.isShockedTransformSet)))) {
            if (Parameters.isGpuRender()) {
                GraphicsFillExtra.setColorTransform(_local4, new ColorTransform(-1, -1, -1, 1, 0xFF, 0xFF, 0xFF, 0));
            }
            else {
                _local8 = _local4.clone();
                _local8.colorTransform(_local8.rect, new ColorTransform(-1, -1, -1, 1, 0xFF, 0xFF, 0xFF, 0));
                _local8 = CachingColorTransformer.filterBitmapData(_local8, SHOCKED_FILTER);
                _local4 = _local8;
            }
            this.isShockedTransformSet = true;
        }
        if (((this.isCharging) && (!(this.isChargingTransformSet)))) {
            if (Parameters.isGpuRender()) {
                GraphicsFillExtra.setColorTransform(_local4, new ColorTransform(1, 1, 1, 1, 0xFF, 0xFF, 0xFF, 0));
            }
            else {
                _local8 = _local4.clone();
                _local8.colorTransform(_local8.rect, new ColorTransform(1, 1, 1, 1, 0xFF, 0xFF, 0xFF, 0));
                _local4 = _local8;
            }
            this.isChargingTransformSet = true;
        }
        this.bitmapFill_.bitmapData = _local4;
        this.fillMatrix_.identity();
        this.fillMatrix_.translate(this.vS_[0], this.vS_[1]);
        this.bitmapFill_.matrix = this.fillMatrix_;
        _arg1.push(this.bitmapFill_);
        _arg1.push(this.path_);
        _arg1.push(GraphicsUtil.END_FILL);
        if (((((((!(this.isPaused())) && (((this.condition_[ConditionEffect.CE_FIRST_BATCH]) || (this.condition_[ConditionEffect.CE_SECOND_BATCH]))))))))) {
            this.drawConditionIcons(_arg1, _arg2, _arg3);
        }
        if (((((this.props_.showName_) && (!((this.name_ == null))))) && (!((this.name_.length == 0))))) {
            this.drawName(_arg1, _arg2);
        }
        if (((((((((this.props_) && (((this.props_.isEnemy_) || (this.props_.isPlayer_) || (this.props_.isAlly_))))) && (!(this.isInvisible())))) && (!(this.isInvulnerable())))) && (!(this.props_.noMiniMap_)))) {
            _local9 = ((_local4.getPixel32((_local4.width / 4), (_local4.height / 4)) | _local4.getPixel32((_local4.width / 2), (_local4.height / 2))) | _local4.getPixel32(((_local4.width * 3) / 4), ((_local4.height * 3) / 4)));
            _local10 = (_local9 >> 24);
            if (_local10 != 0) {
                hasShadow_ = true;
                if (Parameters.data_.HPBar) {
                    this.drawHpBar(_arg1,
                            int((this.props_.isPlayer_ && this != map_.player_ ? 12 : 0) + 6));
                }
            }
            else {
                hasShadow_ = false;
            }
        }
    }

    public function drawConditionIcons(_arg1:Vector.<IGraphicsData>, _arg2:Camera, _arg3:int):void {
        var _local9:BitmapData;
        var _local10:GraphicsBitmapFill;
        var _local11:GraphicsPath;
        var _local12:Number;
        var _local13:Number;
        var _local14:Matrix;
        if (this.icons_ == null) {
            this.icons_ = new Vector.<BitmapData>();
            this.iconFills_ = new Vector.<GraphicsBitmapFill>();
            this.iconPaths_ = new Vector.<GraphicsPath>();
        }
        this.icons_.length = 0;
        var _local4:int = (_arg3 / 500);
        ConditionEffect.getConditionEffectIcons(this.condition_[ConditionEffect.CE_FIRST_BATCH], this.icons_, _local4);
        ConditionEffect.getConditionEffectIcons2(this.condition_[ConditionEffect.CE_SECOND_BATCH], this.icons_, _local4);
        var _local5:Number = posS_[3];
        var _local6:Number = this.vS_[1];
        var _local7:int = this.icons_.length;
        var _local8:int;
        while (_local8 < _local7) {
            _local9 = this.icons_[_local8];
            if (_local8 >= this.iconFills_.length) {
                this.iconFills_.push(new GraphicsBitmapFill(null, new Matrix(), false, false));
                this.iconPaths_.push(new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>()));
            }
            _local10 = this.iconFills_[_local8];
            _local11 = this.iconPaths_[_local8];
            _local10.bitmapData = _local9;
            _local12 = ((_local5 - ((_local9.width * _local7) / 2)) + (_local8 * _local9.width));
            _local13 = (_local6 - (_local9.height / 2));
            _local11.data.length = 0;
            _local11.data.push(_local12, _local13, (_local12 + _local9.width), _local13, (_local12 + _local9.width), (_local13 + _local9.height), _local12, (_local13 + _local9.height));
            _local14 = _local10.matrix;
            _local14.identity();
            _local14.translate(_local12, _local13);
            _arg1.push(_local10);
            _arg1.push(_local11);
            _arg1.push(GraphicsUtil.END_FILL);
            _local8++;
        }
    }

    override public function drawShadow(_arg1:Vector.<IGraphicsData>, _arg2:Camera, _arg3:int):void {
        if (this.shadowGradientFill_ == null) {
            this.shadowGradientFill_ = new GraphicsGradientFill(GradientType.RADIAL, [this.props_.shadowColor_, this.props_.shadowColor_], [0.5, 0], null, new Matrix());
            this.shadowPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
        }
        var _local4:Number = (((this.size_ / 100) * (this.props_.shadowSize_ / 100)) / 3/** this.sizeMult_*/);
        var _local5:Number = (30 * _local4);
        var _local6:Number = (15 * _local4);
        this.shadowGradientFill_.matrix.createGradientBox((_local5 * 2), (_local6 * 2), 0, (posS_[0] - _local5), (posS_[1] - _local6));
        _arg1.push(this.shadowGradientFill_);
        this.shadowPath_.data.length = 0;
        this.shadowPath_.data.push((posS_[0] - _local5), (posS_[1] - _local6), (posS_[0] + _local5), (posS_[1] - _local6), (posS_[0] + _local5), (posS_[1] + _local6), (posS_[0] - _local5), (posS_[1] + _local6));
        _arg1.push(this.shadowPath_);
        _arg1.push(GraphicsUtil.END_FILL);
    }

    public function clearTextureCache():void {
        this.texturingCache_ = new Dictionary();
    }

    public function toString():String {
        return ((((((((((("[" + getQualifiedClassName(this)) + " id: ") + objectId_) + " type: ") + ObjectLibrary.typeToDisplayId_[this.objectType_]) + " pos: ") + x_) + ", ") + y_) + "]"));
    }


}
}
