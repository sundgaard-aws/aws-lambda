package com.opusmagus.trade.handler;

import java.util.Iterator;
import java.util.logging.Level;
import java.util.logging.Logger;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cglib.core.internal.Function;
import org.springframework.cloud.function.adapter.aws.SpringBootStreamHandler;
import org.springframework.context.annotation.Bean;
import org.springframework.messaging.support.GenericMessage;

/*import com.amazonaws.services.lambda.runtime.Context;
import com.amazonaws.services.lambda.runtime.RequestHandler;
import com.amazonaws.services.lambda.runtime.events.SNSEvent;
import com.amazonaws.services.lambda.runtime.events.SNSEvent.SNSRecord;
import com.google.gson.Gson;*/

@SpringBootApplication
public class FunctionHandler extends SpringBootStreamHandler {
	static final Logger logger = Logger.getLogger(FunctionHandler.class.getName());
	//static final Gson gson = new Gson();

	public static void main(String[] args) throws Exception {
		SpringApplication.run(FunctionHandler.class, args);
	}
	/***
	 * 	{"key":"","keyValue":"","tableName":"","json":""}
	 * {"key":"CategoryName","keyValue":"medical","tableName":"Category","json":"{\"title\":\"Medical\"}"}
	 */
	/*public String handleRequest(String genericItem, Context context) {
		logger.info("Lambda function handler method => handleRequest() called...");
				
		// Must allow the policy in IAM inline policy for this to work
		logger.info("Calling TradeBO");		
		return "Lambda function handler method => handleRequest() done!";
	}*/

	@Bean
	public Function<Object, Object> uppercase() {
		return value -> "OK from me";
	}
}
