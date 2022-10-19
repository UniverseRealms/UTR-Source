package com.company.assembleegameclient.appengine {
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.AnimatedChar;
import com.company.assembleegameclient.util.AnimatedChars;
import com.company.assembleegameclient.util.MaskedImage;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.CachingColorTransformer;
import flash.display.BitmapData;
import flash.geom.ColorTransform;

public class SavedCharacter {

    private static const notAvailableCT:ColorTransform = new ColorTransform(0, 0, 0, 0.5, 0, 0, 0, 0);
    private static const dimCT:ColorTransform = new ColorTransform(0.75, 0.75, 0.75, 1, 0, 0, 0, 0);

    public var charXML_:XML;
    public var name_:String = null;

    public function SavedCharacter(_arg1:XML, _arg2:String)
    {
        super();
        this.charXML_ = _arg1;
        this.name_ = _arg2;
    }

    public static function getImage(_arg1:SavedCharacter, _arg2:XML, _arg3:int, _arg4:int, _arg5:Number, available:Boolean, active:Boolean):BitmapData {
        var _local8:AnimatedChar = AnimatedChars.getAnimatedChar(String(_arg2.AnimatedTexture.File), int(_arg2.AnimatedTexture.Index));
        var _local9:MaskedImage = _local8.imageFromDir(_arg3, _arg4, _arg5);
        var _local10:int = (((_arg1) != null) ? _arg1.tex1() : null);
        var _local11:int = (((_arg1) != null) ? _arg1.tex2() : null);
        var _local12:BitmapData = TextureRedrawer.resize(_local9.image_, _local9.mask_, 100, false, _local10, _local11);
        _local12 = GlowRedrawer.outlineGlow(_local12, 0);
        if (!available) {
            _local12 = CachingColorTransformer.transformBitmapData(_local12, notAvailableCT);
        }
        else {
            if (!active) {
                _local12 = CachingColorTransformer.transformBitmapData(_local12, dimCT);
            }
        }
        return (_local12);
    }

    public static function compare(_arg1:SavedCharacter, _arg2:SavedCharacter):Number {
        var _local3:Number = ((Parameters.data_.charIdUseMap.hasOwnProperty(_arg1.charId())) ? Parameters.data_.charIdUseMap[_arg1.charId()] : 0);
        var _local4:Number = ((Parameters.data_.charIdUseMap.hasOwnProperty(_arg2.charId())) ? Parameters.data_.charIdUseMap[_arg2.charId()] : 0);
        if (_local3 != _local4) {
            return ((_local4 - _local3));
        }
        return ((_arg2.xp() - _arg1.xp()));
    }


    public function charId():int {
        return (int(this.charXML_.@id));
    }

    public function name():String {
        return (this.name_);
    }

    public function objectType():int {
        return (int(this.charXML_.ObjectType));
    }

    public function skinType():int {
        return (int(this.charXML_.Texture));
    }

    public function level():int {
        return (int(this.charXML_.Level));
    }

    public function tex1():int {
        return (int(this.charXML_.Tex1));
    }

    public function tex2():int {
        return (int(this.charXML_.Tex2));
    }

    public function xp():int {
        return (int(this.charXML_.Exp));
    }

    public function fame():int {
        return (int(this.charXML_.CurrentFame));
    }

    public function displayId():String {
        return (ObjectLibrary.typeToDisplayId_[this.objectType()]);
    }

    public function hp() : int
    {
        return int(this.charXML_.MaxHitPoints);
    }

    public function mp() : int
    {
        return int(this.charXML_.MaxMagicPoints);
    }

    public function att() : int
    {
        return int(this.charXML_.Attack);
    }

    public function def() : int
    {
        return int(this.charXML_.Defense);
    }

    public function spd() : int
    {
        return int(this.charXML_.Speed);
    }

    public function dex() : int
    {
        return int(this.charXML_.Dexterity);
    }

    public function vit() : int
    {
        return int(this.charXML_.HpRegen);
    }

    public function wis() : int
    {
        return int(this.charXML_.MpRegen);
    }

    public function lck() : int
    {
        return int(this.charXML_.Luck);
    }

    public function res() : int
    {
        return int(this.charXML_.Restoration);
    }


}
}
