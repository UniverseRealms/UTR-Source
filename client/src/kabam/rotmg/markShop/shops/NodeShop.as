package kabam.rotmg.markShop.shops {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.sound.SoundEffectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.util.Currency;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.core.StaticInjectorContext;

import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;

import kabam.rotmg.markShop.NodeDisplay;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.util.components.LegacyBuyButton;

public class NodeShop extends Sprite {

    private const background:ShopBackground = new ShopBackground();
    private var shop:Sprite;
    private var player:Player;
    private var selNode:NodeDisplay;
    private var newNode:NodeDisplay;
    private var node1:NodeDisplay;
    private var node2:NodeDisplay;
    private var node3:NodeDisplay;
    private var node4:NodeDisplay;

    public var closeButton:DeprecatedTextButton;
    public var buyButton:LegacyBuyButton;
    public var selectedSlot:int;
    public var selectedNode:int;

    public function NodeShop(p:Player): void {
        this.player = p;
        this.shop = new Sprite();
        this.selectedSlot = 1;
    }

    public function makeShopUI():void {
        shop.addChild(this.background);
        this.setupNodes();
        addChild(this.shop);
        this.centerModal();
    }

    private function setupNodes():void {
        var id:int = 0;
        for(var y:int = 0; y < 6; y++)
            for(var x:int = 0; x < 2; x++){
                var NodeIcon:NodeDisplay = new NodeDisplay(id);
                id++;
                NodeIcon.x = 35 + 44 * x;
                NodeIcon.y = 89 + 44 * y;
                NodeIcon.addEventListener(MouseEvent.CLICK, this.selectNode);
                shop.addChild(NodeIcon);
            }

        selNode = new NodeDisplay(this.player.node1_);
        selNode.x = 159;
        selNode.y = 89 + 44;
        shop.addChild(selNode);

        newNode = new NodeDisplay(0);
        newNode.x = 159;
        newNode.y = 221 + 44;
        shop.addChild(newNode);

        addNode(this.node1 ,this.player.node1_, 239, 75 + 44, 1);
        addNode(this.node2 ,this.player.node2_, 239, 135 + 44, 2);
        addNode(this.node3 ,this.player.node3_, 239, 195 + 44, 3);
        addNode(this.node4 ,this.player.node4_, 239, 255 + 44, 4);

        this.buyButton = new LegacyBuyButton("", 12, 15, Currency.ONRANE);
        this.buyButton.x = 157;
        this.buyButton.y = 271 + 44;
        this.buyButton.setEnabled(true);
        this.validateButton();
        shop.addChild(this.buyButton);

        this.closeButton = new DeprecatedTextButton(14, "Close");
        this.closeButton.x = 154;
        this.closeButton.y = 271 + 84 + 28;
        this.closeButton.addEventListener(MouseEvent.CLICK, this.onClose);
        shop.addChild(this.closeButton);
    }

    private function validateButton():void {
        if(selNode.getNodeId() != 0) {
            this.buyButton.setPrice(7, Currency.ONRANE);
            this.buyButton.x = 160;
        }
        else {
            this.buyButton.setPrice(15, Currency.ONRANE);
            this.buyButton.x = 157;
        }

        if(selNode.getNodeId() == newNode.getNodeId())
            this.buyButton.setEnabled(false);
        else
            this.buyButton.setEnabled(true);
    }

    private function addNode(nd:NodeDisplay ,id:int, x:int, y:int, slot:int, f:Boolean=true):void {
        nd = new NodeDisplay(id, true, slot);
        nd.x = x;
        nd.y = y;
        nd.addEventListener(MouseEvent.CLICK, this.selectEquipped);
        shop.addChild(nd);
        if (f) {
            var slot_text:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(18).setColor(0xFFFFFF).setBold(true).setVerticalAlign(TextFieldDisplayConcrete.MIDDLE);
            slot_text.setStringBuilder(new LineBuilder().setParams("Slot " + slot.toString()));
            slot_text.filters = [new DropShadowFilter(0, 0, 0)];
            slot_text.x = nd.x + 45;
            slot_text.y = nd.y + 20;
            shop.addChild(slot_text);
        }
    }

    private function selectNode(e:MouseEvent):void {
        SoundEffectLibrary.play("button_click");
        shop.removeChild(newNode);
        selectedNode = e.target.getNodeId();
        newNode = new NodeDisplay(e.target.getNodeId());
        newNode.x = 159;
        newNode.y = 221 + 44;
        this.validateButton();
        shop.addChild(newNode);
    }

    private function selectEquipped(e:MouseEvent): void {
        SoundEffectLibrary.play("button_click");
        shop.removeChild(selNode);
        selectedSlot = e.target.getNodeSlot();
        selNode = new NodeDisplay(e.target.getNodeId());
        selNode.x = 159;
        selNode.y = 89 + 44;
        this.validateButton();
        shop.addChild(selNode);
    }

    private function centerModal():void {
        this.scaleX = 800 / stage.stageWidth;
        this.scaleY = 600 / stage.stageHeight;
        this.x = (((WebMain.sWidth - 200) / 2) - (this.width / 2));
        this.y = ((WebMain.sHeight / 2) - (this.height / 2));
    }

    private function onClose(e:MouseEvent):void {
        if(Boolean(parent) && Boolean(parent.contains(this)))
        {
            SoundEffectLibrary.play("button_click");
            parent.removeChild(this);
        }
    }

    public function updateShop(a:int, b:int, c:int, d:int):void {
        addNode(this.node1 , a, 239, 75 + 44, 1, false);
        addNode(this.node2 , b, 239, 135 + 44, 2, false);
        addNode(this.node3 , c, 239, 195 + 44, 3, false);
        addNode(this.node4 , d, 239, 255 + 44, 4, false);

        shop.removeChild(selNode);
        switch (this.selectedSlot){
            case 1:
                selNode = new NodeDisplay(a, true, 1);
                break;
            case 2:
                selNode = new NodeDisplay(b, true, 2);
                break;
            case 3:
                selNode = new NodeDisplay(c, true, 3);
                break;
            case 4:
                selNode = new NodeDisplay(d, true, 4);
                break;
        }
        selNode.x = 159;
        selNode.y = 89 + 44;
        shop.addChild(selNode);

        this.validateButton();
    }
}
}
