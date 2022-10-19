package kabam.rotmg.game.view.components {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.sound.SoundEffectLibrary;

import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.text.TextFormatAlign;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import kabam.rotmg.markShop.MarkDisplay;
import kabam.rotmg.markShop.NodeDisplay;
import kabam.rotmg.markShop.shops.MarkShop;
import kabam.rotmg.markShop.shops.NodeShop;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.ui.model.TabStripModel;
import kabam.rotmg.ui.signals.UpdateMarksSignal;

public class MarksTabContent extends Sprite {

    private var background:Sprite;
    private var text:TextFieldDisplayConcrete;
    public var nodeDisplay1:NodeDisplay;
    public var nodeDisplay2:NodeDisplay;
    public var nodeDisplay3:NodeDisplay;
    public var nodeDisplay4:NodeDisplay;
    public var markDisplay:MarkDisplay;
    private var player:Player;
    public var markShop:MarkShop;
    public var nodeShop:NodeShop;

    public function MarksTabContent(_arg1:Player) {
        this.player = _arg1;
        this.background = new Sprite();
        this.background.name = TabStripModel.MARKS;
        this.nodeShop = new NodeShop(this.player);
        this.markShop = new MarkShop(this.player);
        addChild(this.background);
        this.showNodeDisplay(this.player.node1_, this.player.node2_, this.player.node3_, this.player.node4_, this.player.mark_);
        this.text = new TextFieldDisplayConcrete().setSize(14).setBold(true).setColor(0xFFFFFF);
        this.text.setStringBuilder(new LineBuilder().setParams("Click a Node/Mark \nto open the shop"));
        this.text.x = 30;
        this.text.y = 95;
        addChild(this.text);
    }

    private function showNodeDisplay(one:int, two:int, three:int, four:int, mark:int):void {
        this.nodeDisplay1 = new NodeDisplay(one);
        this.nodeDisplay1.x = 7;
        this.nodeDisplay1.y = 8;
        this.nodeDisplay1.addEventListener(MouseEvent.CLICK, this.launchNodeShop);
        addChild(this.nodeDisplay1);

        this.nodeDisplay2 = new NodeDisplay(two);
        this.nodeDisplay2.x = 51;
        this.nodeDisplay2.y = 8;
        this.nodeDisplay2.addEventListener(MouseEvent.CLICK, this.launchNodeShop);
        addChild(this.nodeDisplay2);

        this.nodeDisplay3 = new NodeDisplay(three);
        this.nodeDisplay3.x = 7;
        this.nodeDisplay3.y = 52;
        this.nodeDisplay3.addEventListener(MouseEvent.CLICK, this.launchNodeShop);
        addChild(this.nodeDisplay3);

        this.nodeDisplay4 = new NodeDisplay(four);
        this.nodeDisplay4.x = 51;
        this.nodeDisplay4.y = 52;
        this.nodeDisplay4.addEventListener(MouseEvent.CLICK, this.launchNodeShop);
        addChild(this.nodeDisplay4);

        this.markDisplay = new MarkDisplay(mark);
        this.markDisplay.x = 95;
        this.markDisplay.y = 8;
        this.markDisplay.addEventListener(MouseEvent.CLICK, this.launchMarkShop);
        addChild(this.markDisplay);
    }

    private function launchNodeShop(e:MouseEvent):void {
        var openDialog:OpenDialogSignal = StaticInjectorContext.getInjector().getInstance(OpenDialogSignal);
        openDialog.dispatch(this.nodeShop);
        SoundEffectLibrary.play("button_click");
    }

    private function launchMarkShop(e:MouseEvent):void {
        var openDialog:OpenDialogSignal = StaticInjectorContext.getInjector().getInstance(OpenDialogSignal);
        openDialog.dispatch(this.markShop);
        SoundEffectLibrary.play("button_click");
    }

    private function removeDisplay():void {
        removeChild(this.nodeDisplay1);
        removeChild(this.nodeDisplay2);
        removeChild(this.nodeDisplay3);
        removeChild(this.nodeDisplay4);
        removeChild(this.markDisplay);
    }

    public function updateMarksUI(a:int, b:int, c:int, d:int, m:int):void {
        this.removeDisplay();
        this.showNodeDisplay(a, b, c, d, m);
    }
}
}
