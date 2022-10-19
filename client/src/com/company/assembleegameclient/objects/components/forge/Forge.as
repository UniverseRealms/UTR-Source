package com.company.assembleegameclient.objects.components.forge {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.IInteractiveObject;
import com.company.assembleegameclient.ui.panels.Panel;

public class Forge extends GameObject implements IInteractiveObject {

    public function Forge(xml:XML) {
        super(xml);
        isInteractive_ = true;
    }

    public function getPanel(gs:GameSprite):Panel {
        return new ForgePanel(gs);
    }
}
}
