package com.opusmagus.trade.dtl;

import java.util.Date;
import java.util.UUID;

public class TradeDTO
{
    public transient UUID TradeGUID;
    public Double TradeAmount;
    public Date TradeDate;

    public String ToString()
    {
        return "TradeId:{TradeId.Value},TradeAmount:{TradeAmount}";
    }
}