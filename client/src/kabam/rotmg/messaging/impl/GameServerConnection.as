package kabam.rotmg.messaging.impl {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.objects.Projectile;

import flash.utils.ByteArray;

import kabam.lib.net.api.MessageProvider;

import kabam.lib.net.impl.SocketServer;
import kabam.rotmg.memMarket.signals.MemMarketAddSignal;
import kabam.rotmg.memMarket.signals.MemMarketBuySignal;
import kabam.rotmg.memMarket.signals.MemMarketMyOffersSignal;
import kabam.rotmg.memMarket.signals.MemMarketRemoveSignal;
import kabam.rotmg.memMarket.signals.MemMarketSearchSignal;
import kabam.rotmg.messaging.impl.data.MarketOffer;
import kabam.rotmg.messaging.impl.data.PlayerShopItem;
import kabam.rotmg.messaging.impl.incoming.market.MarketAddResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketBuyResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketMyOffersResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketRemoveResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketSearchResult;
import kabam.rotmg.messaging.impl.outgoing.PlayerCheat;
import kabam.rotmg.messaging.impl.outgoing.market.MarketAdd;
import kabam.rotmg.messaging.impl.outgoing.market.MarketBuy;
import kabam.rotmg.messaging.impl.outgoing.market.MarketMyOffers;
import kabam.rotmg.messaging.impl.outgoing.market.MarketRemove;
import kabam.rotmg.messaging.impl.outgoing.market.MarketSearch;
import kabam.rotmg.servers.api.Server;

import org.osflash.signals.Signal;

public class GameServerConnection {

    public static const FAILURE:int = 0;
    public static const CREATE_SUCCESS:int = 1;
    public static const CREATE:int = 2;
    public static const PLAYERSHOOT:int = 3;
    public static const MOVE:int = 4;
    public static const PLAYERTEXT:int = 5;
    public static const TEXT:int = 6;
    public static const SERVERPLAYERSHOOT:int = 7;
    public static const DAMAGE:int = 8;
    public static const UPDATE:int = 9;
    public static const UPDATEACK:int = 10;
    public static const NOTIFICATION:int = 11;
    public static const NEWTICK:int = 12;
    public static const INVSWAP:int = 13;
    public static const USEITEM:int = 14;
    public static const SHOWEFFECT:int = 15;
    public static const HELLO:int = 16;
    public static const GOTO:int = 17;
    public static const INVDROP:int = 18;
    public static const INVRESULT:int = 19;
    public static const RECONNECT:int = 20;
    public static const PING:int = 21;
    public static const PONG:int = 22;
    public static const MAPINFO:int = 23;
    public static const LOAD:int = 24;
    public static const PIC:int = 25;
    public static const SETCONDITION:int = 26;
    public static const TELEPORT:int = 27;
    public static const USEPORTAL:int = 28;
    public static const DEATH:int = 29;
    public static const BUY:int = 30;
    public static const BUYRESULT:int = 31;
    public static const AOE:int = 32;
    public static const GROUNDDAMAGE:int = 33;
    public static const PLAYERHIT:int = 34;
    public static const ENEMYHIT:int = 35;
    public static const AOEACK:int = 36;
    public static const SHOOTACK:int = 37;
    public static const OTHERHIT:int = 38;
    public static const SQUAREHIT:int = 39;
    public static const GOTOACK:int = 40;
    public static const EDITACCOUNTLIST:int = 41;
    public static const ACCOUNTLIST:int = 42;
    public static const QUESTOBJID:int = 43;
    public static const CHOOSENAME:int = 44;
    public static const NAMERESULT:int = 45;
    public static const CREATEGUILD:int = 46;
    public static const GUILDRESULT:int = 47;
    public static const GUILDREMOVE:int = 48;
    public static const GUILDINVITE:int = 49;
    public static const ALLYSHOOT:int = 50;
    public static const ENEMYSHOOT:int = 51;
    public static const REQUESTTRADE:int = 52;
    public static const TRADEREQUESTED:int = 53;
    public static const TRADESTART:int = 54;
    public static const CHANGETRADE:int = 55;
    public static const TRADECHANGED:int = 56;
    public static const ACCEPTTRADE:int = 57;
    public static const CANCELTRADE:int = 58;
    public static const TRADEDONE:int = 59;
    public static const TRADEACCEPTED:int = 60;
    public static const CLIENTSTAT:int = 61;
    public static const CHECKCREDITS:int = 62;
    public static const ESCAPE:int = 63;
    public static const FILE:int = 64;
    public static const INVITEDTOGUILD:int = 65;
    public static const JOINGUILD:int = 66;
    public static const CHANGEGUILDRANK:int = 67;
    public static const PLAYSOUND:int = 68;
    public static const GLOBAL_NOTIFICATION:int = 69;
    public static const RESKIN:int = 70;
    public static const ENTER_ARENA:int = 71;
    public static const ACCEPT_ARENA_DEATH:int = 72;
    public static const VERIFY_EMAIL:int = 73;
    public static const RESKIN_UNLOCK:int = 74;
    public static const PASSWORD_PROMPT:int = 75;
    public static const SERVER_FULL:int = 76;
    public static const QUEUE_PING:int = 77;
    public static const QUEUE_PONG:int = 78;
    public static const MARKET_COMMAND:int = 79;
    public static const QUEST_ROOM_MSG:int = 80;
    public static const KEY_INFO_REQUEST:int = 81;
    public static const KEY_INFO_RESPONSE:int = 82;
    public static const MARKET_RESULT:int = 83;
    public static const SET_FOCUS:int = 84;
    public static const SWITCH_MUSIC:int = 85;
    public static const CLAIM_LOGIN_REWARD_MSG:int = 86;
    public static const LOGIN_REWARD_MSG:int = 87;
    public static const LAUNCH_RAID:int = 88;
    public static const SORFORGE:int = 89;
    public static const SORFORGEREQUEST:int = 90;
    public static const FORGEITEM:int = 91;
    public static const UNBOXREQUEST:int = 92;
    public static const UNBOXRESULT:int = 93;
    public static const ALERTNOTICE:int = 94;
    public static const MARKREQUEST:int = 95;
    public static const QOLACTION:int = 96;
    public static const GAMBLESTART:int = 97;
    public static const REQUESTGAMBLE:int = 98;
    public static const PLAYERCHEAT:int = 99;
    public static const CORRECTSERVER:int = 100;
    public static const REQUESTTOKEN:int = 101;
    public static const RECEIVETOKEN:int = 102;
    public static const INVITEDTOPARTY:int = 103;
    public static const ALLYHIT:int = 104;
    public static const UPDATEMARKS:int = 105;
    public static const MARKET_SEARCH:int = 106;
    public static const MARKET_SEARCH_RESULT:int = 107;
    public static const MARKET_BUY:int = 108;
    public static const MARKET_BUY_RESULT:int = 109;
    public static const MARKET_ADD:int = 110;
    public static const MARKET_ADD_RESULT:int = 111;
    public static const MARKET_REMOVE:int = 112;
    public static const MARKET_REMOVE_RESULT:int = 113;
    public static const MARKET_MY_OFFERS:int = 114;
    public static const MARKET_MY_OFFERS_RESULT:int = 115;
    public static const LOOT_NOTIFY:int = 116;

    public static const USE_POTION:int = 169;
    public static const BUY_SKILL:int = 170;
    public static const UPDATE_TREE:int = 171;
    public static const SKILL_TREE:int = 172;

    public static var instance:GameServerConnection;

    public var changeMapSignal:Signal;
    public var gs_:GameSprite;
    public var server_:Server;
    public var gameId_:int;
    public var createCharacter_:Boolean;
    public var charId_:int;
    public var keyTime_:int;
    public var key_:ByteArray;
    public var mapJSON_:String;
    public var isFromArena_:Boolean = false;
    public var lastTickId_:int = -1;
    public var jitterWatcher_:JitterWatcher;
    public var serverConnection:SocketServer;
    public var outstandingBuy_:Boolean;
    private var messages:MessageProvider;


    public function chooseName(_arg1:String):void {
    }

    public function createGuild(_arg1:String):void {
    }

    public function connect():void {
    }

    public function disconnect():void {
    }

    public function checkCredits():void {
    }

    public function escape():void {
    }

    public function useItem(_arg1:int, _arg2:int, _arg3:int, _arg4:int, _arg5:Number, _arg6:Number, _arg7:int):void {
    }

    public function useItem_new(_arg1:GameObject, _arg2:int):Boolean {
        return (false);
    }

    public function enableJitterWatcher():void {
    }

    public function disableJitterWatcher():void {
    }
    /* Market */
    public function onMarketSearchResult(searchResult:MarketSearchResult) : void
    {
        MemMarketSearchSignal.instance.dispatch(searchResult);
    }

    /* Market */
    public function onMarketBuyResult(buyResult:MarketBuyResult) : void
    {
        MemMarketBuySignal.instance.dispatch(buyResult);
    }

    /* Market */
    public function onMarketAddResult(addResult:MarketAddResult) : void
    {
        MemMarketAddSignal.instance.dispatch(addResult);
    }

    /* Market */
    public function onMarketRemoveResult(removeResult:MarketRemoveResult) : void
    {
        MemMarketRemoveSignal.instance.dispatch(removeResult);
    }

    /* Market */
    public function onMarketMyOffersResult(myOffersResult:MarketMyOffersResult) : void
    {
        MemMarketMyOffersSignal.instance.dispatch(myOffersResult);
    }

    public function editAccountList(_arg1:int, _arg2:Boolean, _arg3:int):void {
    }

    public function guildRemove(_arg1:String):void {
    }

    public function guildInvite(_arg1:String):void {
    }

    public function requestTrade(_arg1:String):void {
    }

    public function requestGamble(_arg1:String, _arg2:int):void {
    }

    public function changeTrade(_arg1:Vector.<Boolean>):void {
    }

    public function acceptTrade(_arg1:Vector.<Boolean>, _arg2:Vector.<Boolean>):void {
    }

    public function cancelTrade():void {
    }

    public function joinGuild(_arg1:String):void {
    }

    public function changeGuildRank(_arg1:String, _arg2:int):void {
    }

    public function isConnected():Boolean {
        return (false);
    }

    public function teleport(_arg1:int):void {
    }

    public function usePortal(_arg1:int):void {
    }

    public function getNextDamage(_arg1:uint, _arg2:uint):uint {
        return (0);
    }

    public function groundDamage(_arg1:int, _arg2:Number, _arg3:Number):void {
    }

    public function playerShoot(_arg1:int, _arg2:Projectile):void {
    }

    public function playerHit(_arg1:int, _arg2:int):void {
    }

    public function enemyHit(_arg1:int, _arg2:int, _arg3:int, _arg4:Boolean):void {
    }

    public function allyHit(time:int, bulletId:int, targetId:int, objectId:int):void {
    }

    public function otherHit(_arg1:int, _arg2:int, _arg3:int, _arg4:int):void {
    }

    public function squareHit(_arg1:int, _arg2:int, _arg3:int):void {
    }

    public function shootAck(_arg1:int):void {
    }

    public function playerText(_arg1:String):void {
    }

    public function invSwap(_arg1:Player, _arg2:GameObject, _arg3:int, _arg4:int, _arg5:GameObject, _arg6:int, _arg7:int):Boolean {
        return (false);
    }

    public function invSwapPotion(_arg1:Player, _arg2:GameObject, _arg3:int, _arg4:int, _arg5:GameObject, _arg6:int, _arg7:int):Boolean {
        return (false);
    }

    public function invDrop(_arg1:GameObject, _arg2:int, _arg3:int):void {
    }

    public function setCondition(_arg1:uint, _arg2:Number):void {
    }

    public function buy(_arg1:int, _arg2:int):void {
    }

    public function questFetch():void {
    }

    public function questRedeem(_arg1:int, _arg2:int, _arg3:int):void {
    }

    public function keyInfoRequest(_arg1:int):void {
    }

    public function gotoQuestRoom():void {
    }

    public function requestMarketOffers() : void
    {
    }

    public function removeMarketOffer(param1:PlayerShopItem) : void
    {
    }

    public function addOffer(param1:Vector.<MarketOffer>) : void
    {
    }

    public function PlayerCheatCheck() : void
    {
    }

    public function correctServerCheck() :void
    {
    }

    public function marketSearch(itemType:int) : void { }
    public function marketRemove(id:int) : void { }
    public function marketMyOffers() : void { }
    public function marketBuy(id:int) : void { }
    public function marketAdd(items:Vector.<int>, price:int, currency:int, hours:int) : void { }
}
}