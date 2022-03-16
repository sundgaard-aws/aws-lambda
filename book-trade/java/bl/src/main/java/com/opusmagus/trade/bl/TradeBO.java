package com.opusmagus.trade.bl;

import java.util.List;
import java.util.logging.Logger;

import com.google.inject.Inject;
import com.opusmagus.trade.dal.ITradeDAC;
import com.opusmagus.trade.dtl.TradeDTO;

public class TradeBO implements ITradeBO
{
    private ITradeDAC tradeDAC;
    private Logger logger;
    
    @Inject
    public TradeBO(ITradeDAC tradeDAC, Logger logger) throws Exception {
        if(tradeDAC==null) throw new Exception("tradeDAC was null");
        if(logger==null) throw new Exception("logger was null");
        this.tradeDAC=tradeDAC;
        this.logger=logger;
    }
    
    public void BookTrade(TradeDTO trade) throws Exception {
        logger.info("BookTrade started...");
        tradeDAC.StoreTrade(trade);
        logger.info("BookTrade done.");
    }

    public List<TradeDTO> GetTradeList(int startPage, int pageSize)
    {
        return tradeDAC.RestorePage(startPage, pageSize);
    }

    public void PriceTrade(TradeDTO trade)
    {
        trade.TradeAmount = Math.random();
    }
}
