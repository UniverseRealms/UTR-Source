package kabam.rotmg.pets.util {
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.LineBreakDesign;

import flash.display.Bitmap;
import flash.display.Shape;
import flash.filters.DropShadowFilter;
import flash.text.TextFormatAlign;

import kabam.rotmg.pets.view.components.DialogCloseButton;
import kabam.rotmg.pets.view.components.PopupWindowBackground;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;

public class PetsViewAssetFactory
{
    public static function returnPetSlotTitle():TextFieldDisplayConcrete {
        var _local1:TextFieldDisplayConcrete;
        _local1 = new TextFieldDisplayConcrete();
        _local1.setSize(PetsConstants.MEDIUM_TEXT_SIZE).setColor(0xB3B3B3).setBold(true).setHorizontalAlign(TextFormatAlign.CENTER).setWordWrap(true).setTextWidth(100);
        _local1.filters = [new DropShadowFilter(0, 0, 0)];
        _local1.y = PetsConstants.PET_SLOT_TITLE_Y;
        return (_local1);
    }

    public static function returnMediumCenteredTextfield(_arg1:uint, _arg2:uint):TextFieldDisplayConcrete {
        var _local3:TextFieldDisplayConcrete = new TextFieldDisplayConcrete();
        _local3.setSize(PetsConstants.MEDIUM_TEXT_SIZE).setColor(_arg1).setBold(true).setHorizontalAlign(TextFormatAlign.CENTER).setWordWrap(true).setTextWidth(_arg2);
        return (_local3);
    }

    public static function returnPetSlotShape(_arg1:uint, _arg2:uint, _arg3:int, _arg4:Boolean, _arg5:Boolean, _arg6:int = 2):Shape {
        var _local7:Shape = new Shape();
        ((_arg4) && (_local7.graphics.beginFill(0x464646, 1)));
        ((_arg5) && (_local7.graphics.lineStyle(_arg6, _arg2)));
        _local7.graphics.drawRoundRect(0, _arg3, _arg1, _arg1, 16, 16);
        _local7.x = ((100 - _arg1) * 0.5);
        return (_local7);
    }

    public static function returnCloseButton(_arg1:int):DialogCloseButton {
        var _local2:DialogCloseButton;
        _local2 = new DialogCloseButton();
        _local2.y = 4;
        _local2.x = ((_arg1 - _local2.width) - 5);
        return (_local2);
    }

    public static function returnTooltipLineBreak():LineBreakDesign {
        var _local1:LineBreakDesign;
        _local1 = new LineBreakDesign(173, 0);
        _local1.x = 5;
        _local1.y = 64;
        return (_local1);
    }

    public static function returnBitmap(_arg1:uint, _arg2:uint = 80):Bitmap {
        return (new Bitmap(ObjectLibrary.getRedrawnTextureFromType(_arg1, _arg2, true)));
    }

    public static function returnCaretakerBitmap(_arg1:uint):Bitmap {
        return (new Bitmap(ObjectLibrary.getRedrawnTextureFromType(_arg1, 80, true, true, 10)));
    }

    public static function returnTopAlignedTextfield(_arg1:int, _arg2:int, _arg3:Boolean, _arg4:Boolean = false):TextFieldDisplayConcrete {
        var _local5:TextFieldDisplayConcrete = new TextFieldDisplayConcrete();
        _local5.setSize(_arg2).setColor(_arg1).setBold(_arg3);
        _local5.filters = ((_arg4) ? [new DropShadowFilter(0, 0, 0)] : []);
        return (_local5);
    }

    public static function returnTextfield(_arg1:int, _arg2:int, _arg3:Boolean, _arg4:Boolean = false):TextFieldDisplayConcrete {
        var _local5:TextFieldDisplayConcrete = new TextFieldDisplayConcrete();
        _local5.setSize(_arg2).setColor(_arg1).setBold(_arg3);
        _local5.setVerticalAlign(TextFieldDisplayConcrete.BOTTOM);
        _local5.filters = ((_arg4) ? [new DropShadowFilter(0, 0, 0)] : []);
        return (_local5);
    }

    public static function returnEggHatchWindowBackground(_arg1:uint, _arg2:uint):PopupWindowBackground {
        var _local3:PopupWindowBackground = new PopupWindowBackground();
        _local3.draw(_arg1, _arg2);
        _local3.divide(PopupWindowBackground.HORIZONTAL_DIVISION, 30);
        _local3.divide(PopupWindowBackground.HORIZONTAL_DIVISION, 206);
        return (_local3);
    }


}
}
