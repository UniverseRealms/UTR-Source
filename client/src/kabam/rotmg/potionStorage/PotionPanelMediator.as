package kabam.rotmg.potionStorage
{
import kabam.rotmg.potionStorage.UI.PotionStorage;

import com.company.assembleegameclient.game.AGameSprite;
import flash.events.MouseEvent;
import kabam.rotmg.dialogs.control.OpenDialogNoModalSignal;
import robotlegs.bender.bundles.mvcs.Mediator;

public class PotionPanelMediator extends Mediator
{


   [Inject]
   public var view:PotionPanel;

   [Inject]
   public var openDialog:OpenDialogNoModalSignal;

   [Inject]
   public var gameSprite:AGameSprite;

   public function PotionPanelMediator()
   {
      super();
   }

   override public function initialize() : void
   {
      this.view.button.addEventListener(MouseEvent.CLICK,this.onButtonLeftClick);
   }

   private function onButtonLeftClick(_arg1:MouseEvent) : void
   {
      this.openDialog.dispatch(new PotionStorage(this.gameSprite));
   }

   override public function destroy() : void
   {
      super.destroy();
   }
}
}
