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

// Handler value: example.Handler
public class FunctionHandler implements RequestHandler<Map<String,String>, String>{
  Gson gson = new GsonBuilder().setPrettyPrinting().create();
  @Override
  public String handleRequest(Map<String,String> event, Context context)
  {
    LambdaLogger logger = context.getLogger();
    String response = "200 OK";
    // log execution details
    logger.log("ENVIRONMENT VARIABLES: " + gson.toJson(System.getenv()));
    logger.log("CONTEXT: " + gson.toJson(context));
    
    // process event
    Injector injector = Guice.createInjector(new GuiceDIModule());
    //Finally, at runtime we retrieve a GuiceUserService instance with a non-null accountService dependency:
    TradeBO tradeService = injector.getInstance(TradeBO.class);
    TradeDTO newTrade = new TradeDTO();
    tradeService.BookTrade(newTrade);
    //assertNotNull(guiceUserService.getAccountService());

    logger.log("EVENT: " + gson.toJson(event));
    logger.log("EVENT TYPE: " + event.getClass());
    return response;
  }
}