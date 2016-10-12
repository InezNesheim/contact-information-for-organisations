using System;

namespace RestClient.Resources
{
    [Serializable]
    public class RestClientException : Exception
    {
        public RestClientException() { }
        public RestClientException(string message) : base(message) { }
        public RestClientException(string message, Exception inner) : base(message, inner) { }
        protected RestClientException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

}
