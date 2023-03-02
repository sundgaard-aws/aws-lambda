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
    LambdaLogger logger = context.getLogger();
    String response = "200 OK";
    logger.log("ENVIRONMENT VARIABLES: " + gson.toJson(System.getenv()));
    logger.log("CONTEXT: " + gson.toJson(context));
    logger.log("EVENT: " + gson.toJson(event));
    logger.log("EVENT TYPE: " + event.getClass());
    TradeDTO newTrade = new TradeDTO();
    try {
      tradeService.BookTrade(newTrade);
    } catch (Exception e) {
      e.printStackTrace();
      response = "501 ERROR";
    }
    return response;
  }
}