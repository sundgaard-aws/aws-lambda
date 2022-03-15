package com.opusmagus.trade.dal.dynamodb;

import java.util.List;
import java.util.logging.Logger;

import com.amazonaws.services.dynamodbv2.AmazonDynamoDB;
import com.amazonaws.services.dynamodbv2.AmazonDynamoDBClientBuilder;
import com.amazonaws.services.dynamodbv2.document.DynamoDB;
import com.amazonaws.services.dynamodbv2.document.Item;
import com.amazonaws.services.dynamodbv2.document.Table;
import com.google.gson.Gson;
import com.google.inject.Inject;
import com.opusmagus.trade.dal.ITradeDAC;
import com.opusmagus.trade.dtl.TradeDTO;


public class TradeDAC implements ITradeDAC
{        
    private static final String TRADE_TABLE_NAME = "trade";
    private static final String TRADE_TABLE_HASH_KEY = "trade_guid";
    private Logger logger;

    @Inject
    public TradeDAC(Object connStr, Logger logger) {
        this.logger = logger;
    }

    public TradeDTO RestoreTrade(long tradeId) {
        // TODO Auto-generated method stub
        return null;
    }

    public List<TradeDTO> RestorePage(int startPage, int pageSize) {
        // TODO Auto-generated method stub
        return null;
    }


    public void StoreTrade(TradeDTO trade) {
        final AmazonDynamoDB ddb = AmazonDynamoDBClientBuilder.defaultClient();
        final Gson gson = new Gson();
		logger.info("AmazonDynamoDB client created....");
			
		try {
			DynamoDB dynamoDB = new DynamoDB(ddb);
			logger.info("dynamoDB created....");
			Table table = dynamoDB.getTable(TRADE_TABLE_NAME);
			logger.info("got table....");

			Item dynamoItem = new Item();
			dynamoItem.withPrimaryKey(TRADE_TABLE_HASH_KEY, trade.TradeGUID.toString());
			dynamoItem.withJSON(TRADE_TABLE_NAME, gson.toJson(trade));
			
			logger.info("created item....");
			table.putItem(dynamoItem);
			logger.info("putting item....");
						
		}
		catch(Throwable e) {
			logger.severe(e.getMessage());
		}
		finally {
			//context.getClientContext().
		}    
    }
}

