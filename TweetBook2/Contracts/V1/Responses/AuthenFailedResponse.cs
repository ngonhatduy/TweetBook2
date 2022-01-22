using System.Collections.Generic;

namespace TweetBook2.Contracts.V1.Responses
{
    public class AuthenFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
