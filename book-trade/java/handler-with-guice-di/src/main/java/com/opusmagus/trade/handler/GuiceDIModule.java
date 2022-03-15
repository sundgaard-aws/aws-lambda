package com.opusmagus.trade.handler;

import com.google.inject.AbstractModule;
import com.opusmagus.trade.bl.ITradeBO;
import com.opusmagus.trade.bl.TradeBO;
import com.opusmagus.trade.dal.ITradeDAC;
import com.opusmagus.trade.dal.dynamodb.TradeDAC;

public class GuiceDIModule  extends AbstractModule {
    @Override
    protected void configure() {
        bind(ITradeBO.class).to(TradeBO.class);
        bind(ITradeDAC.class).to(TradeDAC.class);
    }
}
