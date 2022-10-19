﻿package com.company.assembleegameclient.util {
public class Currency {

    public static const INVALID:int = -1;
    public static const GOLD:int = 0;
    public static const FAME:int = 1;
    public static const GUILD_FAME:int = 2;
    public static const FORTUNE:int = 3;
    public static const ONRANE:int = 4;
    public static const KANTOS:int = 5;


    public static function typeToName(_arg1:int):String {
        switch (_arg1) {
            case GOLD:
                return ("Gold");
            case FAME:
                return ("Fame");
            case GUILD_FAME:
                return ("Guild Fame");
            case FORTUNE:
                return ("Fortune Token");
            case ONRANE:
                return ("Onrane");
            case KANTOS:
                return ("Kantos");
        }
        return ("");
    }


}
}
