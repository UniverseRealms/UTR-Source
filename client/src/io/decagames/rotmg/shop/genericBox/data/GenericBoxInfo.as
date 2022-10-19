package io.decagames.rotmg.shop.genericBox.data
{
import com.company.assembleegameclient.util.TimeUtil;
import io.decagames.rotmg.utils.date.TimeLeft;
import org.osflash.signals.Signal;

public class GenericBoxInfo
{


   protected var _id:String;

   protected var _title:String;

   protected var _description:String;

   protected var _weight:String;

   protected var _contents:String;

   protected var _priceAmount:int;

   protected var _priceCurrency:int;

   protected var _saleAmount:int;

   protected var _saleCurrency:int;

   protected var _quantity:int;

   protected var _startTime:Date;

   protected var _endTime:Date;

   protected var _unitsLeft:int = -1;

   protected var _totalUnits:int = -1;

   protected var _maxPurchase:int = -1;

   protected var _purchaseLeft:int = -1;

   protected var _slot:int = 0;

   protected var _tags:String = "";

   public const updateSignal:Signal = new Signal();

   public function GenericBoxInfo()
   {
      super();
   }

   public function get id() : String
   {
      return this._id;
   }

   public function set id(param1:String) : void
   {
      this._id = param1;
   }

   public function get title() : String
   {
      return this._title;
   }

   public function set title(param1:String) : void
   {
      this._title = param1;
   }

   public function get description() : String
   {
      return this._description;
   }

   public function set description(param1:String) : void
   {
      this._description = param1;
   }

   public function get weight() : String
   {
      return this._weight;
   }

   public function set weight(param1:String) : void
   {
      this._weight = param1;
   }

   public function get contents() : String
   {
      return this._contents;
   }

   public function set contents(param1:String) : void
   {
      this._contents = param1;
   }

   public function get priceAmount() : int
   {
      return this._priceAmount;
   }

   public function set priceAmount(param1:int) : void
   {
      this._priceAmount = param1;
   }

   public function get priceCurrency() : int
   {
      return this._priceCurrency;
   }

   public function set priceCurrency(param1:int) : void
   {
      this._priceCurrency = param1;
   }

   public function get saleAmount() : int
   {
      return this._saleAmount;
   }

   public function set saleAmount(param1:int) : void
   {
      this._saleAmount = param1;
   }

   public function get saleCurrency() : int
   {
      return this._saleCurrency;
   }

   public function set saleCurrency(param1:int) : void
   {
      this._saleCurrency = param1;
   }

   public function get quantity() : int
   {
      return this._quantity;
   }

   public function set quantity(param1:int) : void
   {
      this._quantity = param1;
   }

   public function get startTime() : Date
   {
      return this._startTime;
   }

   public function set startTime(param1:Date) : void
   {
      this._startTime = param1;
   }

   public function get endTime() : Date
   {
      return this._endTime;
   }

   public function set endTime(param1:Date) : void
   {
      this._endTime = param1;
   }

   public function get unitsLeft() : int
   {
      return this._unitsLeft;
   }

   public function set unitsLeft(param1:int) : void
   {
      this._unitsLeft = param1;
      this.updateSignal.dispatch();
   }

   public function get totalUnits() : int
   {
      return this._totalUnits;
   }

   public function set totalUnits(param1:int) : void
   {
      this._totalUnits = param1;
   }

   public function get slot() : int
   {
      return this._slot;
   }

   public function set slot(param1:int) : void
   {
      this._slot = param1;
   }

   public function get tags() : String
   {
      return this._tags;
   }

   public function set tags(param1:String) : void
   {
      this._tags = param1;
   }

   public function getSecondsToEnd() : Number
   {
      if(!this._endTime)
      {
         return 2147483647;
      }
      var _loc1_:Date = new Date();
      return (this._endTime.time - _loc1_.time) / 1000;
   }

   public function getSecondsToStart() : Number
   {
      var _loc1_:Date = new Date();
      return (this._startTime.time - _loc1_.time) / 1000;
   }

   public function isOnSale() : Boolean
   {
      return this._saleAmount > -1;
   }

   public function isNew() : Boolean
   {
      var _loc1_:Date = new Date();
      if(this._startTime.time > _loc1_.time)
      {
         return false;
      }
      return Math.ceil(TimeUtil.secondsToDays((_loc1_.time - this._startTime.time) / 1000)) <= 1;
   }

   public function getStartTimeString() : String
   {
      var _loc2_:String = "Available in: ";
      var _loc1_:Number = this.getSecondsToStart();
      if(_loc1_ <= 0)
      {
         return "";
      }
      if(_loc1_ > 86400)
      {
         _loc2_ = _loc2_ + TimeLeft.parse(_loc1_,"%dd %hh");
      }
      else if(_loc1_ > 3600)
      {
         _loc2_ = _loc2_ + TimeLeft.parse(_loc1_,"%hh %mm");
      }
      else
      {
         _loc2_ = _loc2_ + TimeLeft.parse(_loc1_,"%mm %ss");
      }
      return _loc2_;
   }

   public function getEndTimeString() : String
   {
      if(!this._endTime)
      {
         return "";
      }
      var _loc2_:String = "Ends in: ";
      var _loc1_:Number = this.getSecondsToEnd();
      if(_loc1_ <= 0)
      {
         return "";
      }
      if(_loc1_ > 86400)
      {
         _loc2_ = _loc2_ + TimeLeft.parse(_loc1_,"%dd %hh");
      }
      else if(_loc1_ > 3600)
      {
         _loc2_ = _loc2_ + TimeLeft.parse(_loc1_,"%hh %mm");
      }
      else
      {
         _loc2_ = _loc2_ + TimeLeft.parse(_loc1_,"%mm %ss");
      }
      return _loc2_;
   }

   public function get maxPurchase() : int
   {
      return this._maxPurchase;
   }

   public function set maxPurchase(param1:int) : void
   {
      this._maxPurchase = param1;
   }

   public function get purchaseLeft() : int
   {
      return this._purchaseLeft;
   }

   public function set purchaseLeft(param1:int) : void
   {
      this._purchaseLeft = param1;
      this.updateSignal.dispatch();
   }
}
}
