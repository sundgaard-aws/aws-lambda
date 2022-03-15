package com.opusmagus.trade.console;

import com.opusmagus.trade.bl.TradeBO;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class ApplicationContext {

  @Bean
  public TradeBO tradeBO() throws Exception {
    return new TradeBO(null, null);
  }
}
