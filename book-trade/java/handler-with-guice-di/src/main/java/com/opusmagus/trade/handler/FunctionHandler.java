package com.opusmagus.trade.handler;

import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import com.amazonaws.services.lambda.runtime.LambdaLogger;

import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.inject.Guice;
import com.google.inject.Injector;
import com.opusmagus.trade.bl.TradeBO;
import com.opusmagus.trade.dtl.TradeDTO;
import java.util.Map;

public class FunctionHandler implements RequestHandler<Map<String,String>, String>{
  private static Gson gson;
  private static TradeBO tradeService;
  static {
    Injector injector = Guice.createInjector(new GuiceDIModule());
    tradeService = injector.getInstance(TradeBO.class);
    gson = new GsonBuilder().setPrettyPrinting().create();
  }
  @Override
  public String handleRequest(Map<String,String> event, Context context)
  {
    String response = "200 OK";
    TradeDTO newTrade = gson.fromJson(event.get("trade"), TradeDTO.class);
    try {
      tradeService.BookTrade(newTrade);
    } catch (Exception e) {
      response = "501 ERROR";
    }
    return response;
  }
}