package com.opusmagus.trade.handler;

import com.google.inject.Guice;
import com.google.inject.Injector;
import com.opusmagus.trade.bl.TradeBO;
import com.opusmagus.trade.dtl.TradeDTO;

public class Console {
    public static void main(String[] args) {        
        System.out.println("Started...");
        Injector injector = Guice.createInjector(new GuiceDIModule());
        TradeBO tradeService = injector.getInstance(TradeBO.class);
        TradeDTO newTrade = new TradeDTO();
        tradeService.BookTrade(newTrade);
        System.out.println("Ended!");
    }
}
