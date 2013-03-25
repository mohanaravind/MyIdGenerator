using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDB;
using Amazon.Runtime;
using Amazon.DynamoDB.Model;

namespace MyIDGenerator.Code.AWS
{
    class DynamoDBUtility
    {
        private static DynamoDBUtility dynamoDBUtility;

        private AmazonDynamoDBClient dynamoDB;

        public static DynamoDBUtility getDynamoDBUtility()
        {
            if (dynamoDBUtility == null)
                dynamoDBUtility = new DynamoDBUtility();

            return dynamoDBUtility;
        }

        /**
         * Default constructor
         */
        private DynamoDBUtility()
        {

            //Credentials
            AWSCredentials credentials;
            try
            {
                //credentials = new PropertiesCredentials(DynamoDBUtility.class.getResourceAsStream("AwsCredentials.properties"));
                credentials = Credentials.getCredentials();
                dynamoDB = new AmazonDynamoDBClient(credentials);

            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }

        }

        /**
         * Returns the list of data
         * @return
         */
        public List<String> GetData(String strTableName)
        {
		    //Declarations
		    List<String> lstMessages = new List<String>();
		
		    //Create the scan filter
            ScanRequest scanRequest = new ScanRequest();
            scanRequest.TableName =  strTableName;

            ScanResponse scanResponse = dynamoDB.Scan(scanRequest);
            ScanResult scanResult = scanResponse.ScanResult;
        
            foreach (Dictionary<String, AttributeValue> item in scanResult.Items ) {
			    lstMessages.Add(item["Message"].S);
		    }

		
		    return lstMessages;
	    }

        /**
         * Inserts a record for the item 
         * @param strTableName
         * @param strID
         * @param strMessage
         */
        public void InsertData(String strTableName, String strID, String strMessage)
        {
            //Create the item which has to be inserted
            Dictionary<String, AttributeValue> item = newItem(strID, strMessage);

            //Create the insertion request
            PutItemRequest putItemRequest = new PutItemRequest();
            putItemRequest.TableName = strTableName;
            putItemRequest.Item = item;

            //Insert the item onto DB
            dynamoDB.PutItem(putItemRequest);
        }

        /**
         * Deletes the request data
         */
        public void DeleteData(String strTableName, String strRequestID)
        {
            //Declarations
            AttributeValue id = new AttributeValue();
            id.S = strRequestID;

            Key key = new Key();
            key.HashKeyElement = id;

            DeleteItemRequest deleteItemRequest = new DeleteItemRequest();
            deleteItemRequest.TableName = strTableName;
            deleteItemRequest.Key = key;

            dynamoDB.DeleteItem(deleteItemRequest);
        }

        /**
         * Adding a new item
         * @param requestID
         * @param strMessage
         * @return
         */
        private static Dictionary<String, AttributeValue> newItem(String strID, String strMessage)
        {
            Dictionary<String, AttributeValue> item = new Dictionary<String, AttributeValue>();

            AttributeValue id = new AttributeValue();
            id.S = strID;

            AttributeValue message = new AttributeValue();
            message.S = strMessage;

            item.Add("ID", id);
            item.Add("Message", message);

            return item;
        }
    }
}
