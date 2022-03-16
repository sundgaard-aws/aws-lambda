package com.opusmagus.trade.bl;

import java.util.List;

import com.opusmagus.trade.dtl.TradeDTO;

public interface ITradeBO
{
    public void BookTrade(TradeDTO trade) throws Exception;
    public List<TradeDTO> GetTradeList(int startPage, int pageSize);
    public void PriceTrade(TradeDTO trade);
}
