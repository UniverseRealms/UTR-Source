package kabam.rotmg.messaging.impl.outgoing {

    import com.company.assembleegameclient.objects.Player;

    import flash.utils.IDataOutput;

    public class PlayerCheat extends OutgoingMessage {

        public function PlayerCheat(_arg1:uint, _arg2:Function) {
            super(_arg1, _arg2);
        }

        override public function writeToOutput(_arg1:IDataOutput):void {
            _arg1.writeInt(Player._atk);
            _arg1.writeInt(Player._def);
            _arg1.writeInt(Player._spd);
            _arg1.writeInt(Player._vit);
            _arg1.writeInt(Player._wis);
            _arg1.writeInt(Player._dex);
            _arg1.writeInt(Player._mindmg);
            _arg1.writeInt(Player._maxdmg);
            _arg1.writeInt(Player._firerate)
        }

        override public function toString():String {
            return (formatToString(
                    "PLAYERCHEAT", "Player._atk", "Player._def", "Player._spd", "Player._vit", "Player._wis", "Player._dex", "Player._mindmg", "Player._maxdmg", "Player._firerate"
            ));
        }

    }
}//package _0A_g
