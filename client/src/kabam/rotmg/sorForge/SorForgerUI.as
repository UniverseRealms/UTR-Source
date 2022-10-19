/**
 * Created by Omniraptor on 6/25/17.
 */
package kabam.rotmg.sorForge {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.FrameChef;
import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;


import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.display.DisplayObject;

import flash.events.Event;
import flash.events.MouseEvent;

import kabam.rotmg.questrewards.components.ModalItemSlot;
import kabam.rotmg.pets.view.components.DialogCloseButton;

import org.osflash.signals.Signal;

public class SorForgerUI extends FrameChef {

    public const cancel:Signal = new Signal();

    public var gameSprite:GameSprite;
    public var slot1:ModalItemSlot;
    public var slot4:ModalItemSlot;
    public var forgeButton:DeprecatedTextButton;
    private var essenceImage:Essence_ImageEmbed;
    private var sacredForge:SacredForge_ImageEmbed;
    public var XButton:DialogCloseButton;
    public function SorForgerUI(_arg_1:GameSprite) {
        this.gameSprite = _arg_1;
        super("Forge your Sacred Item.", "", 200);
        this.addForgeUI();
        this.forgeButton = new DeprecatedTextButton(16, "Forge");
        this.forgeButton.y = 335;
        this.forgeButton.x = 45;
        addChild(this.forgeButton);
        XButton.addEventListener(MouseEvent.CLICK, this.onClose);
    }

    public function getItemSlot1():ModalItemSlot {
        return (this.slot1);
    }
    public function getItemSlot4():ModalItemSlot {
        return (this.slot4);
    }

    private function addForgeUI() : void {
        this.sacredForge = new SacredForge_ImageEmbed();
        this.sacredForge.y = -54;
        this.sacredForge.x = -70;
        this.sacredForge.scaleY = 2.38;
        this.sacredForge.scaleX = 2.43;
        addChild(this.sacredForge);
        this.XButton = new DialogCloseButton();
        this.XButton.x = ((this.w_ - this.XButton.width) - 7);
        this.XButton.y += 17;
        addChild(this.XButton);
        this.slot1 = new ModalItemSlot(true, false);
        this.slot1.y = 254;
        this.slot1.x = 24;
        addChild(this.slot1);
        this.slot4 = new ModalItemSlot(true, false);
        this.slot4.y = 90;
        this.slot4.x = this.slot1.x;
        this.slot4.playOutLineAnimation(-1);
        addChild(this.slot4);
        this.essenceImage = new Essence_ImageEmbed();
        this.essenceImage.y = this.slot1.y - 11;
        this.essenceImage.x = this.slot1.x - 70;
        addChild(this.essenceImage)
    }



    private function onClose(e:Event) : void {
    }


}
}//package com.company.assembleegameclient.account.ui

