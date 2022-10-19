package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.objects.animation.AnimationsData;
import com.company.assembleegameclient.objects.components.forge.Forge;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
import com.company.util.AssetLibrary;
import com.company.util.ConversionUtil;
import com.company.util.PointUtil;

import flash.display.BitmapData;
import flash.geom.Matrix;
import flash.utils.Dictionary;
import flash.utils.getDefinitionByName;

import kabam.rotmg.assets.EmbeddedData;
import kabam.rotmg.constants.GeneralConstants;
import kabam.rotmg.constants.ItemConstants;
import kabam.rotmg.messaging.impl.data.StatData;
import kabam.rotmg.skilltree.ui.SkillTreeButton;

public class ObjectLibrary {

    public static const IMAGE_SET_NAME:String = "lofiObj3";
    public static const IMAGE_ID:int = 0xFF;
    public static const propsLibrary_:Dictionary = new Dictionary();
    public static const xmlLibrary_:Dictionary = new Dictionary();
    public static const idToType_:Dictionary = new Dictionary();
    public static const typeToDisplayId_:Dictionary = new Dictionary();
    public static const typeToTextureData_:Dictionary = new Dictionary();
    public static const typeToTopTextureData_:Dictionary = new Dictionary();
    public static const setLibrary_:Dictionary = new Dictionary();
    public static const typeToAnimationsData_:Dictionary = new Dictionary();
    public static const typeToIdItems_:Dictionary = new Dictionary();
    public static const idToTypeItems_:Dictionary = new Dictionary();
    public static const petXMLDataLibrary_:Dictionary = new Dictionary();
    public static const preloadedCustom_:Dictionary = new Dictionary();
    public static const skinSetXMLDataLibrary_:Dictionary = new Dictionary();
    public static const dungeonsXMLLibrary_:Dictionary = new Dictionary(true);
    public static const ENEMY_FILTER_LIST:Vector.<String> = new <String>["None", "Hp", "Defense"];
    public static const TILE_FILTER_LIST:Vector.<String> = new <String>["ALL", "Walkable", "Unwalkable", "Slow", "Speed=1"];
    public static const defaultProps_:ObjectProperties = new ObjectProperties(null);

    public static const SkillLibrary_:Vector.<XML> = new Vector.<XML>();

    public static const TYPE_MAP:Object = {
        "Ally": Ally,
        "CaveWall": CaveWall,
        "Character": Character,
        "CharacterChanger": CharacterChanger,
        "ClosedGiftChest": ClosedGiftChest,
        "ClosedVaultChest": ClosedVaultChest,
        "ConnectedWall": ConnectedWall,
        "Container": Container,
        "DoubleWall": DoubleWall,
        "GameObject": GameObject,
        "GuildBoard": GuildBoard,
        "GuildChronicle": GuildChronicle,
        "GuildHallPortal": GuildHallPortal,
        "GuildMerchant": GuildMerchant,
        "GuildRegister": GuildRegister,
        "Merchant": Merchant,
        "MoneyChanger": MoneyChanger,
        "NameChanger": NameChanger,
        "ReskinVendor": ReskinVendor,
        "OneWayContainer": OneWayContainer,
        "Player": Player,
        "Portal": Portal,
        "Projectile": Projectile,
        "DailyLoginRewards": DailyLoginRewards,
        "Sign": Sign,
        "SpiderWeb": SpiderWeb,
        "Stalagmite": Stalagmite,
        "Wall": Wall,
        "SorForger": SorForger,
        "MarketNPC":MarketNPC,
        "MarketObject":MarketObject,
        "PotionStorage": PotionStorage
    };

    public static var textureDataFactory:TextureDataFactory = new TextureDataFactory();
    public static var playerChars_:Vector.<XML> = new Vector.<XML>();
    public static var hexTransforms_:Vector.<XML> = new Vector.<XML>();
    public static var playerClassAbbr_:Dictionary = new Dictionary();
    private static var currentDungeon:String = "";


    public static function parseDungeonXML(_arg1:String, _arg2:XML):void {
        var _local3:int = (_arg1.indexOf("_") + 1);
        var _local4:int = _arg1.indexOf("CXML");
        currentDungeon = _arg1.substr(_local3, (_local4 - _local3));
        dungeonsXMLLibrary_[currentDungeon] = new Dictionary(true);
        parseFromXML(_arg2, parseDungeonCallback);
    }

    private static function parseDungeonCallback(_arg1:int, _arg2:XML) : void {
        if (((!((currentDungeon == ""))) && (!((dungeonsXMLLibrary_[currentDungeon] == null))))) {
            dungeonsXMLLibrary_[currentDungeon][_arg1] = _arg2;
            propsLibrary_[_arg1].belonedDungeon = currentDungeon;
        }
    }

    public static function parseFromXML(xml:XML, _arg2:Function = null, preload:Boolean = false):void {
        var objextXML:XML;
        var id:String;
        var displayId:String;
        var objectType:int;
        var found:Boolean;
        var i:int;
        for each (objextXML in xml.Object) {
            id = String(objextXML.@id);
            displayId = id;
            if (objextXML.hasOwnProperty("DisplayId")) {
                displayId = objextXML.DisplayId;
            }
            if (objextXML.hasOwnProperty("Group")) {
                if (objextXML.Group == "Hexable") {
                    hexTransforms_.push(objextXML);
                }
            }
            objectType = int(objextXML.@type);
            if (objextXML.hasOwnProperty("PetBehavior") || (objextXML.hasOwnProperty("PetAbility"))) {
                petXMLDataLibrary_[objectType] = objextXML;
            }
            else {
                propsLibrary_[objectType] = new ObjectProperties(objextXML);
                xmlLibrary_[objectType] = objextXML;
                idToType_[id] = objectType;
                typeToDisplayId_[objectType] = displayId;

                if (String(objextXML.Class) == "Equipment")
                {
                    typeToIdItems_[objectType] = id.toLowerCase();
                    idToTypeItems_[id.toLowerCase()] = objectType
                }
                if (preload)
                {
                    preloadedCustom_[objectType] = id.toLowerCase();
                }
                if (_arg2 != null) {
                    (_arg2(objectType, objextXML));
                }
                if (String(objextXML.Class) == "Player") {
                    playerClassAbbr_[objectType] = String(objextXML.@id).substr(0, 2);
                    found = false;
                    i = 0;
                    while (i < playerChars_.length) {
                        if (int(playerChars_[i].@type) == objectType) {
                            playerChars_[i] = objextXML;
                            found = true;
                        }
                        i++;
                    }
                    if (!found) {
                        playerChars_.push(objextXML);
                    }
                }
                typeToTextureData_[objectType] = textureDataFactory.create(objextXML);
                if (objextXML.hasOwnProperty("Top")) {
                    typeToTopTextureData_[objectType] = textureDataFactory.create(XML(objextXML.Top));
                }
                if (objextXML.hasOwnProperty("Animation")) {
                    typeToAnimationsData_[objectType] = new AnimationsData(objextXML);
                }
            }
        }
    }


    public static function parseSkillTree(xml:XML): void{
        var objextXML:XML;
        var id:int;
        for each (objextXML in xml.Object) {
            id = int(objextXML.@id);
            SkillLibrary_[id] = objextXML;
        }

    }

    public static function getIdFromType(_arg1:int):String {
        var _local2:XML = xmlLibrary_[_arg1];
        if (_local2 == null) {
            return (null);
        }
        return (String(_local2.@id));
    }

    public static function getPropsFromId(_arg1:String):ObjectProperties {
        var _local2:int = idToType_[_arg1];
        return (propsLibrary_[_local2]);
    }

    public static function getXMLfromId(_arg1:String):XML {
        var _local2:int = idToType_[_arg1];
        return (xmlLibrary_[_local2]);
    }

    public static function getObjectFromType(objectType:int):GameObject {
        var objectXML:XML;
        var typeReference:String;
        try {
            objectXML = xmlLibrary_[objectType];
            typeReference = objectXML.Class;
        }
        catch (e:Error) {
            throw (new Error(("Type: 0x" + objectType.toString(16))));
        }
        var typeClass:Class = ((TYPE_MAP[typeReference]) || (makeClass(typeReference)));
        return (new (typeClass)(objectXML));
    }

    private static function makeClass(_arg1:String):Class {
        var _local2:String = ("com.company.assembleegameclient.objects." + _arg1);
        return ((getDefinitionByName(_local2) as Class));
    }

    public static function getTextureFromType(_arg1:int):BitmapData {
        var _local2:TextureData = typeToTextureData_[_arg1];
        if (_local2 == null) {
            return (null);
        }
        return (_local2.getTexture());
    }

    public static function getBitmapData(_arg1:int):BitmapData {
        var _local2:TextureData = typeToTextureData_[_arg1];
        var _local3:BitmapData = ((_local2) ? _local2.getTexture() : null);
        if (_local3) {
            return (_local3);
        }
        return (AssetLibrary.getImageFromSet(IMAGE_SET_NAME, IMAGE_ID));
    }
    public static function getSetXMLFromType(_arg_1:int) : XML
    {
        var _local_2:XML = null;
        var _local_3:int = 0;
        if(setLibrary_[_arg_1] != undefined)
        {
            return setLibrary_[_arg_1];
        }
        for each(_local_2 in EmbeddedData.skinsEquipmentSetsXML.EquipmentSet)
        {
            _local_3 = int(_local_2.@type);
            setLibrary_[_local_3] = _local_2;
        }
        return setLibrary_[_arg_1];
    }
    public static function getRedrawnTextureFromType(objType:int, size:int, includeBottom:Boolean, useCaching:Boolean = true, scaleValue:Number = 5):BitmapData {
        var texture:BitmapData = getBitmapData(objType);
        if (Parameters.itemTypes16.indexOf(objType) != -1) {
            size = (size * 0.5);
        }
        var textureData:TextureData = typeToTextureData_[objType];
        var mask:BitmapData = ((textureData) ? textureData.mask_ : null);
        var objectXML:XML = xmlLibrary_[objType] || new XML();

        if (mask == null && objectXML.hasOwnProperty("RT")) {
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0x351c75, useCaching, scaleValue, 1.5));
        }

        if (mask == null && objectXML.hasOwnProperty("Heroic")) {
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0x10d000, useCaching, scaleValue, 1.5));
        }

        if (mask == null && objectXML.hasOwnProperty("Ancestral")) {
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0x5080f0, useCaching, scaleValue, 1.5));
        }

        if (mask == null && objectXML.hasOwnProperty("Godly")) {
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0xE01DBF, useCaching, scaleValue, 1.5));
        }
        if (mask == null && objectXML.hasOwnProperty("GodSlayer")) {
            //                             Image, imagesize, padding,glowcolor,smult,  Wholesize
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0xd80d38, useCaching, scaleValue, 1.4));
        }
        if (mask == null && objectXML.hasOwnProperty("Eternal")) {
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0x6a38ea, useCaching, scaleValue, 1.5));
        }
        if (mask == null && objectXML.hasOwnProperty("Divine")) {
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0xf04040, useCaching, scaleValue, 1.5));
        }else if(mask == null && objectXML.hasOwnProperty("Outfit")){
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0xFFFFFF, useCaching, scaleValue));
        }else if(mask == null ){
            return (TextureRedrawer.redraw(texture, size, includeBottom, 0, useCaching, scaleValue));
        }
        var tex1:int = ((objectXML.hasOwnProperty("Tex1")) ? int(objectXML.Tex1) : 0);
        var tex2:int = ((objectXML.hasOwnProperty("Tex2")) ? int(objectXML.Tex2) : 0);
        texture = TextureRedrawer.resize(texture, mask, size, includeBottom, tex1, tex2, scaleValue);
        texture = GlowRedrawer.outlineGlow(texture, 0);
        return (texture);
    }
    public static function getItemIcon(_arg_1:int) : BitmapData
    {
        var _local_6:int = 0;
        var _local_9:int = 0;
        var _local_7:* = null;
        var _local_3:* = null;
        var _local_2:* = null;
        var _local_8:* = null;
        var _local_10:* = null;
        var _local_4:* = null;
        var _local_5:Matrix = new Matrix();
        if(_arg_1 == -1)
        {
            _local_7 = scaleBitmapData(AssetLibrary.getImageFromSet("lofiInterface",7),2);
            _local_5.translate(4,4);
            _local_3 = new BitmapData(22,22,true,0);
            _local_3.draw(_local_7,_local_5);
            return _local_3;
        }
        _local_2 = xmlLibrary_[_arg_1];
        _local_8 = typeToTextureData_[_arg_1];
        _local_10 = Boolean(_local_8)?_local_8.mask_:null;
        _local_6 = "Tex1" in _local_2?int(_local_2.Tex1):int(0);
        _local_9 = "Tex2" in _local_2?int(_local_2.Tex2):int(0);
        _local_4 = getTextureFromType(_arg_1);
        if((_local_6 != 0 || _local_9 != 0) && (_arg_1 != 317 && _arg_1 != 318))
        {
            _local_4 = TextureRedrawer.retextureNoSizeChange(_local_4,_local_10,_local_6,_local_9);
            _local_5.scale(0.2,0.2);
        }
        _local_7 = scaleBitmapData(_local_4,2);
        _local_5.translate(4,4);
        _local_3 = new BitmapData(22,22,true,0);
        _local_3.draw(_local_7,_local_5);
        _local_3 = GlowRedrawer.outlineGlow(_local_3,0);
        _local_3.applyFilter(_local_3,_local_3.rect,PointUtil.ORIGIN);
        return _local_3;
    }

    public static function scaleBitmapData(_arg_1:BitmapData, _arg_2:Number) : BitmapData
    {
        _arg_2 = Math.abs(_arg_2);
        var _local_4:int = int(_arg_1.width * _arg_2) || int(1);
        var _local_6:int = int(_arg_1.height * _arg_2) || int(1);
        var _local_3:BitmapData = new BitmapData(_local_4,_local_6,true,0);
        var _local_5:Matrix = new Matrix();
        _local_5.scale(_arg_2,_arg_2);
        _local_3.draw(_arg_1,_local_5);
        return _local_3;
    }
    public static function getSizeFromType(_arg1:int):int {
        var _local2:XML = xmlLibrary_[_arg1];
        if (!_local2.hasOwnProperty("Size")) {
            return (100);
        }
        return (int(_local2.Size));
    }

    public static function getSlotTypeFromType(_arg1:int):int {
        var _local2:XML = xmlLibrary_[_arg1];
        if (!_local2.hasOwnProperty("SlotType")) {
            return (-1);
        }
        return (int(_local2.SlotType));
    }

    public static function isEquippableByPlayer(_arg1:int, _arg2:Player):Boolean {
        if (_arg1 == ItemConstants.NO_ITEM) {
            return (false);
        }
        var _local3:XML = xmlLibrary_[_arg1];
        var _local4:int = int(_local3.SlotType.toString());
        var _local5:uint;
        while (_local5 < GeneralConstants.NUM_EQUIPMENT_SLOTS) {
            if (_arg2.slotTypes_[_local5] == _local4) {
                return (true);
            }
            _local5++;
        }
        return (false);
    }

    public static function getMatchingSlotIndex(_arg1:int, _arg2:Player):int {
        var _local3:XML;
        var _local4:int;
        var _local5:uint;
        if (_arg1 != ItemConstants.NO_ITEM) {
            _local3 = xmlLibrary_[_arg1];
            _local4 = int(_local3.SlotType);
            _local5 = 0;
            while (_local5 < GeneralConstants.NUM_EQUIPMENT_SLOTS) {
                if (_arg2.slotTypes_[_local5] == _local4) {
                    return (_local5);
                }
                _local5++;
            }
        }
        return (-1);
    }

    public static function isUsableByPlayer(_arg1:int, _arg2:Player):Boolean {
        if ((((_arg2 == null)) || ((_arg2.slotTypes_ == null)))) {
            return (true);
        }
        var _local3:XML = xmlLibrary_[_arg1];
        if ((((_local3 == null)) || (!(_local3.hasOwnProperty("SlotType"))))) {
            return (false);
        }
        var _local4:int = _local3.SlotType;
        if ((((_local4 == ItemConstants.POTION_TYPE)) || ((_local4 == ItemConstants.EGG_TYPE))  || _local4 == ItemConstants.NEW_EGG_LIMITED_EDITION_TYPE)) {
            return (true);
        }
        var _local5:int;
        while (_local5 < _arg2.slotTypes_.length) {
            if (_arg2.slotTypes_[_local5] == _local4) {
                return (true);
            }
            _local5++;
        }
        return (false);
    }

    public static function isSoulbound(_arg1:int):Boolean {
        var _local2:XML = xmlLibrary_[_arg1];
        return (!(_local2 == null) && _local2.hasOwnProperty("Soulbound"));
    }

    public static function isTiered(_arg1:int):Boolean {
        var _local2:XML = xmlLibrary_[_arg1];
        return (!(_local2 == null) && _local2.hasOwnProperty("Tier"));
    }

    public static function usableBy(_arg1:int):Vector.<String> {
        var _local5:XML;
        var _local6:Vector.<int>;
        var _local7:int;
        var _local2:XML = xmlLibrary_[_arg1];
        if ((((_local2 == null)) || (!(_local2.hasOwnProperty("SlotType"))))) {
            return (null);
        }
        var _local3:int = _local2.SlotType;
        if ((((((_local3 == ItemConstants.POTION_TYPE)) || ((_local3 == ItemConstants.RING_TYPE)))) || ((_local3 == ItemConstants.EGG_TYPE)))) {
            return (null);
        }
        var _local4:Vector.<String> = new Vector.<String>();
        for each (_local5 in playerChars_) {
            _local6 = ConversionUtil.toIntVector(_local5.SlotTypes);
            _local7 = 0;
            while (_local7 < _local6.length) {
                if (_local6[_local7] == _local3) {
                    _local4.push(typeToDisplayId_[int(_local5.@type)]);
                    break;
                }
                _local7++;
            }
        }
        return (_local4);
    }

    public static function playerMeetsRequirements(_arg1:int, _arg2:Player):Boolean {
        var _local4:XML;
        if (_arg2 == null) {
            return (true);
        }
        var _local3:XML = xmlLibrary_[_arg1];
        for each (_local4 in _local3.EquipRequirement) {
            if (!playerMeetsRequirement(_local4, _arg2)) {
                return (false);
            }
        }
        return (true);
    }

    public static function playerMeetsRequirement(_arg1:XML, _arg2:Player):Boolean {
        var _local3:int;
        if (_arg1.toString() == "Stat") {
            _local3 = int(_arg1.@value);
            switch (int(_arg1.@stat)) {
                case StatData.MAX_HP_STAT:
                    return ((_arg2.maxHP_ >= _local3));
                case StatData.MAX_MP_STAT:
                    return ((_arg2.maxMP_ >= _local3));
                case StatData.LEVEL_STAT:
                    return ((_arg2.level_ >= _local3));
                case StatData.ATTACK_STAT:
                    return ((_arg2.attack_ >= _local3));
                case StatData.DEFENSE_STAT:
                    return ((_arg2.defense_ >= _local3));
                case StatData.SPEED_STAT:
                    return ((_arg2.speed_ >= _local3));
                case StatData.VITALITY_STAT:
                    return ((_arg2.vitality_ >= _local3));
                case StatData.WISDOM_STAT:
                    return ((_arg2.wisdom_ >= _local3));
                case StatData.DEXTERITY_STAT:
                    return ((_arg2.dexterity_ >= _local3));
                case StatData.LUCK_STAT:
                    return ((_arg2.luck_ >= _local3));
                case StatData.RESTORATION_STAT:
                    return ((_arg2.restoration_ >= _local3));
            }
        }
        return (false);
    }

    public static function getPetDataXMLByType(_arg1:int):XML {
        return (petXMLDataLibrary_[_arg1]);
    }


}
}
