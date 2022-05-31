package com.opusmagus.trade.handler;

import java.util.Date;
import java.util.UUID;

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
        newTrade.TradeGUID = UUID.randomUUID();
        newTrade.TradeAmount = 100000.00;
        newTrade.TradeDate = new Date();
        try {
            tradeService.BookTrade(newTrade);
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println("Ended!");
    }
}
