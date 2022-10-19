package kabam.rotmg.potionStorage {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.panels.Panel;

import kabam.rotmg.pets.util.PetsViewAssetFactory;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class PotionPanel extends Panel {

   private const titleText:TextFieldDisplayConcrete = PetsViewAssetFactory.returnTextfield(0xFFFFFF, 16, true);

   private var title:String = "Potion Storage";
   private var buttonText:String = "Open";
   private var objectType:int;
   internal var button:DeprecatedTextButton;

   public function PotionPanel(_arg_1:GameSprite, _arg_2:int) {
      super(_arg_1);
      this.objectType = _arg_2;
      this.titleText.setStringBuilder(new LineBuilder().setParams(this.title));
      addChild(this.titleText);
      this.button = new DeprecatedTextButton(16, this.buttonText);
      this.button.textChanged.addOnce(this.alignButton);
      addChild(this.button);
   }

   private function alignButton():void {
      this.button.x = ((WIDTH / 2) - (this.button.width / 2));
      this.button.y = ((HEIGHT - this.button.height) - 4);
      this.titleText.x = WIDTH / 2 - this.titleText.width / 2;
      this.titleText.y = HEIGHT - this.titleText.height - 4 - 30;
   }


}
}