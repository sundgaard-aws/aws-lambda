package com.opusmagus.trade.console;


import java.util.List;
import java.util.logging.Logger;

import com.opusmagus.trade.bl.TradeBO;
import com.opusmagus.trade.dtl.TradeDTO;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Console implements CommandLineRunner
{
    private static final Logger logger = Logger.getLogger(Console.class.getName());
    
    @Autowired private TradeBO tradeBO;
    public static void main(String[] args)
    {
        logger.info("Console started...");
        SpringApplication.run(Console.class, args);
        logger.info("Console ended.");             
    }
    @Override
    public void run(String... args) throws Exception {
        
        // Build configuration
        /*var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
        .AddJsonFile("appsettings.json", false)
        .Build();*/

            //setup our DI
        /*var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<ITradeDAC, MSSQL_DAL.TradeDAC>()
            .AddSingleton<TradeBO, TradeBO>()
            .AddSingleton<IConfigurationRoot>(configuration)
            .BuildServiceProvider();*/

        //configure console logging
        //serviceProvider.GetService<ILoggerFactory>();
        //var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
        logger.info("Starting application");		    

        //do the actual work here
        //TradeBO tradeBO = serviceProvider.GetService<TradeBO>();
        TradeDTO trade = new TradeDTO();
        tradeBO.PriceTrade(trade);
        tradeBO.BookTrade(trade);

        // list page of trades
        List<TradeDTO> tradeList = tradeBO.GetTradeList(0, 20);
        //foreach(var tradeX in tradeList) Console.WriteLine(trade);   
        
    }
}
