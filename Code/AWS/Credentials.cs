using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.Runtime;
using System.Security;

namespace MyIDGenerator.Code
{
    class Credentials : AWSCredentials
    {
        private  String accessKeyId = "AKIAIMWYE73IGN6UVH2Q"; 
	    private String secretAccessKey = "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";
	    private static String requestQueueURL = "https://sqs.us-east-1.amazonaws.com/558145441370/Request";
	    private static String responseQueueURL = "https://sqs.us-east-1.amazonaws.com/558145441370/Response";
	
	    /**
	     * Make the constructor to be private
	     */
	    private Credentials(){
		    
	    }
	
	    /**
	     * Returns the credential instance
	     * @return
	     */
	    public static Credentials getCredentials(){
		    return new Credentials();
	    }
	
	    /**
	     * Returns the URL at which the request queue is running
	     * @return
	     */
	    public static String getRequestQueueURL(){
		    return requestQueueURL;
	    }
	
	    /**
	     * Returns the URL at which the request queue is running
	     * @return
	     */
	    public static String getResponseQueueURL(){
		    return responseQueueURL;
	    }

	    public String getAWSAccessKeyId() {
		    return accessKeyId;
	    }

	    public String getAWSSecretKey() {
		    return secretAccessKey;
	    }

        public override ImmutableCredentials GetCredentials()
        {
            SecureString secret = new SecureString();
            
            Char[] arrSecret = secretAccessKey.ToCharArray();
            
            foreach (var key in arrSecret)
	        {
                secret.AppendChar(key);
	        }            

            //Create the credentials
            ImmutableCredentials objCredentials = new ImmutableCredentials(accessKeyId, secret, null);

            return objCredentials;
        }
    }
}
