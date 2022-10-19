package kabam.rotmg.memMarket.utils {
import kabam.rotmg.memMarket.content.MemMarketItem;

public class SortUtils
{
    /* Sorting options */
    public static const LOWEST_TO_HIGHEST:String = "Lowest -> Highest";
    public static const HIGHEST_TO_LOWEST:String = "Highest -> Lowest";
    //public static const FAME_TO_GOLD:String = "Fame -> Gold";
    //public static const GOLD_TO_FAME:String = "Gold -> Fame";
    public static const JUST_ADDED:String = "Just added";
    public static const ENDING_SOON:String = "Ending soon";
    public static const SORT_CHOICES:Vector.<String> = new <String>
    [
        LOWEST_TO_HIGHEST,
        HIGHEST_TO_LOWEST,
        //FAME_TO_GOLD,
        //GOLD_TO_FAME,
        JUST_ADDED,
        ENDING_SOON
    ];

    public static function lowestToHighest(itemA:MemMarketItem, itemB:MemMarketItem) : int
    {
        if (itemA.data_.price_ < itemB.data_.price_)
        {
            return -1;
        }
        else if (itemA.data_.price_ > itemB.data_.price_)
        {
            return 1;
        }
        else return 0;
    }

    public static function highestToLowest(itemA:MemMarketItem, itemB:MemMarketItem) : int
    {
        if (itemA.data_.price_ < itemB.data_.price_)
        {
            return 1;
        }
        else if (itemA.data_.price_ > itemB.data_.price_)
        {
            return -1;
        }
        else return 0;
    }

    /*public static function fameToGold(itemA:MemMarketItem, itemB:MemMarketItem) : int
    {
        if (itemA.data_.currency_ < itemB.data_.currency_)
        {
            return 1;
        }
        else if (itemA.data_.currency_ > itemB.data_.currency_)
        {
            return -1;
        }
        else return 0;
    }

    public static function goldToFame(itemA:MemMarketItem, itemB:MemMarketItem) : int
    {
        if (itemA.data_.currency_ < itemB.data_.currency_)
        {
            return -1;
        }
        else if (itemA.data_.currency_ > itemB.data_.currency_)
        {
            return 1;
        }
        else return 0;
    } */

    public static function justAdded(itemA:MemMarketItem, itemB:MemMarketItem) : int
    {
        if (itemA.data_.startTime_ < itemB.data_.startTime_)
        {
            return 1;
        }
        else if (itemA.data_.startTime_ > itemB.data_.startTime_)
        {
            return -1;
        }
        else return 0;
    }

    public static function endingSoon(itemA:MemMarketItem, itemB:MemMarketItem) : int
    {
        if (itemA.data_.timeLeft_ < itemB.data_.timeLeft_)
        {
            return -1;
        }
        else if (itemA.data_.timeLeft_ > itemB.data_.timeLeft_)
        {
            return 1;
        }
        else return 0;
    }
}
}
