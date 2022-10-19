package com.company.assembleegameclient.util.redrawers {
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.PointUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.BlendMode;
import flash.display.GradientType;
import flash.display.Shape;
import flash.filters.BitmapFilterQuality;
import flash.filters.GlowFilter;
import flash.geom.Matrix;
import flash.utils.Dictionary;

public class GlowRedrawer {

    private static const GRADIENT_MAX_SUB:uint = 0x282828;
    private static const GLOW_FILTER:GlowFilter = new GlowFilter(0, 0.5, 12, 12, 2, BitmapFilterQuality.LOW, false, false);
    private static const GODSLAYERGLOW_FILTER:GlowFilter = new GlowFilter(0, 0.8, 2, 2, 255, 1, false, false);
    private static const SACREDGLOW_FILTER:GlowFilter = new GlowFilter(0, 0.8, 2, 2, 255, 1, false, false);
    private static const GLOW_FILTER_ALT:GlowFilter = new GlowFilter(0, 0.8, 2, 2, 255, 1, false, false);

    private static var tempMatrix_:Matrix = new Matrix();
    private static var gradient_:Shape = getGradient();
    private static var glowHashes:Dictionary = new Dictionary();


    public static function outlineGlow(texture:BitmapData, glowColor:uint, outlineSize:Number = 1.4, caching:Boolean = false):BitmapData {
        var hash:String = getHash(glowColor, outlineSize);
        if (caching && isCached(texture, hash)) {
            return (glowHashes[texture][hash]);
        }
        var newTexture:BitmapData = texture.clone();
        tempMatrix_.identity();
        tempMatrix_.scale((texture.width / 0x0100), (texture.height / 0x0100));
        newTexture.draw(gradient_, tempMatrix_, null, BlendMode.SUBTRACT);
        var origBitmap:Bitmap = new Bitmap(texture);
        newTexture.draw(origBitmap, null, null, BlendMode.ALPHA);
        TextureRedrawer.OUTLINE_FILTER.blurX = outlineSize;
        TextureRedrawer.OUTLINE_FILTER.blurY = outlineSize;
        var _local8:uint;
        TextureRedrawer.OUTLINE_FILTER.color = _local8;
        newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, TextureRedrawer.OUTLINE_FILTER);
        /*if (glowColor != 0xFFFFFFFF) {
            if (Parameters.isGpuRender() && !(glowColor == 0)) {

                GLOW_FILTER_ALT.color = glowColor;
                GLOW_FILTER.color = glowColor;
                newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, GLOW_FILTER_ALT);
                newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, GLOW_FILTER);
            }
            else {
                GLOW_FILTER.color = glowColor;
                newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, GLOW_FILTER);
            }
        }
        if (glowColor == 0xd80d38)
        {
            GODSLAYERGLOW_FILTER.color = glowColor;
            newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, GLOW_FILTER);
        }
        if (glowColor == 0xd3a625)
        {
            SACREDGLOW_FILTER.color = 0x6afffb;
            newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, GLOW_FILTER);
        }*/
        if(glowColor != 0 && glowColor != 0xFFFFFFFF) { //Not white or Black

            GLOW_FILTER_ALT.color = glowColor;
            newTexture.applyFilter(newTexture, newTexture.rect, PointUtil.ORIGIN, GLOW_FILTER_ALT);
        }
        GLOW_FILTER.color = glowColor;
        newTexture.applyFilter(newTexture,newTexture.rect,PointUtil.ORIGIN,GLOW_FILTER);


        if (caching) {
            cache(texture, glowColor, outlineSize, newTexture);
        }
        return (newTexture);
    }

    private static function cache(_arg1:BitmapData, _arg2:uint, _arg3:Number, _arg4:BitmapData):void {
        var _local6:Object;
        var _local5:String = getHash(_arg2, _arg3);
        if ((_arg1 in glowHashes)) {
            glowHashes[_arg1][_local5] = _arg4;
        }
        else {
            _local6 = {};
            _local6[_local5] = _arg4;
            glowHashes[_arg1] = _local6;
        }
    }

    private static function isCached(_arg1:BitmapData, _arg2:String):Boolean {
        var _local3:Object;
        if ((_arg1 in glowHashes)) {
            _local3 = glowHashes[_arg1];
            if ((_arg2 in _local3)) {
                return (true);
            }
        }
        return (false);
    }

    private static function getHash(_arg1:uint, _arg2:Number):String {
        return ((int((_arg2 * 10)).toString() + _arg1));
    }

    private static function getGradient():Shape {
        var _local1:Shape = new Shape();
        var _local2:Matrix = new Matrix();
        _local2.createGradientBox(0x0100, 0x0100, (Math.PI / 2), 0, 0);
        _local1.graphics.beginGradientFill(GradientType.LINEAR, [0, GRADIENT_MAX_SUB], [1, 1], [127, 0xFF], _local2);
        _local1.graphics.drawRect(0, 0, 0x0100, 0x0100);
        _local1.graphics.endFill();
        return (_local1);
    }


}
}
