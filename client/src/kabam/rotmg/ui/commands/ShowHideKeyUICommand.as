package kabam.rotmg.ui.commands {
import flash.display.DisplayObjectContainer;

import kabam.rotmg.ui.model.HUDModel;

import kabam.rotmg.ui.view.KeysView;

public class ShowHideKeyUICommand {

    [Inject]
    public var contextView:DisplayObjectContainer;

    [Inject]
    public var isKeyUIToBeShown:Boolean = true;

    private static var show:Boolean = true;
    private var view:KeysView;

    public function execute():void {
        if (show) {
            view = new KeysView();
            view.x = 4;
            view.y = 4;
            this.contextView.addChild(view);
            show = false;
        }
        else {
            this.contextView.removeChild(view);
            view = null;
            show = true;
            }
        }
    }
}
