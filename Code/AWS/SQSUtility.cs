using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.SQS;
using Amazon.Runtime;
using Amazon.SQS.Model;

namespace MyIDGenerator.Code
{
    class SQSUtility
    {
        private static AmazonSQS amazonSQS;

        /**
         * Default Constructor
         */
        public SQSUtility()
        {
            //Get the Credentials
            AWSCredentials objCredentials = Credentials.getCredentials();

            //Create AmazonSQS client			
            amazonSQS = new AmazonSQSClient(objCredentials);
        }


        /**
         * Post the response message to Amazon SQS
         * @param strMessage
         */
        public void PostResponse(String strRequest)
        {

            try
            {
                //Get the Queue URL
                String strQueueURL = Credentials.getResponseQueueURL();

                //Create the message request
                SendMessageRequest objMessageRequest = new SendMessageRequest();
                objMessageRequest.QueueUrl = strQueueURL;
                objMessageRequest.MessageBody = strRequest;

                //Send the message
                amazonSQS.SendMessage(objMessageRequest);
            }
            catch (AmazonServiceException e)
            {
                Console.Out.WriteLine(e.Message);
            }
            catch (AmazonClientException e)
            {
                Console.Out.WriteLine(e.Message);
            }

        }

        /**
         * Retrieves the request message from Amazon SQS
         */
        public Message GetRequest()
        {
		    //Declarations
		    List<Message> lstMessages;
            Message objMessage = new Message();
		    DeleteMessageRequest objDeleteMessageRequest;
		
		    try {
			    //Get the Queue URL
			    String strQueueURL = Credentials.getRequestQueueURL();
			
			    //Get the receive request 
			    ReceiveMessageRequest objReceiveMessageRequest = new ReceiveMessageRequest();
                objReceiveMessageRequest.QueueUrl = strQueueURL;
			
			    //Receive the message 
			    ReceiveMessageResponse objReceiveMessageResponse = amazonSQS.ReceiveMessage(objReceiveMessageRequest);
			
                ReceiveMessageResult objReceiveMessageResult = objReceiveMessageResponse.ReceiveMessageResult;

			    //Get the messages
			    lstMessages = objReceiveMessageResult.Message;
								
			
			    foreach(Message message in lstMessages) {
				    //Get the response message
                    objMessage = message;
				               
				    //Delete the message request
				    objDeleteMessageRequest = new DeleteMessageRequest();
                    objDeleteMessageRequest.QueueUrl = strQueueURL;
                    objDeleteMessageRequest.ReceiptHandle = message.ReceiptHandle;
                    amazonSQS.DeleteMessage(objDeleteMessageRequest);
			    }
		    } catch (AmazonServiceException e) {
			    Console.Out.WriteLine(e.Message);
		    } catch (AmazonClientException e) {
			    Console.Out.WriteLine(e.Message);
		    }

            return objMessage;
	    }
    }
}
