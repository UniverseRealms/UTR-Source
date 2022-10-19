package com.company.assembleegameclient.objects {
public class EventGod {

    public var eventGod_:Boolean;
    public function EventGod(_arg1:XML) {
        this.eventGod_ = _arg1.hasOwnProperty("EventGod");
    }
}
}
