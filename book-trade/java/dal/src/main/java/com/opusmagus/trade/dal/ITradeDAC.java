package com.opusmagus.trade.dal;

import java.util.List;

import com.opusmagus.trade.dtl.TradeDTO;

public interface ITradeDAC
{
    TradeDTO RestoreTrade(long tradeId);
    void StoreTrade(TradeDTO trade);
    List<TradeDTO> RestorePage(int startPage, int pageSize);
}

