using PayPal.Api;
using System.Collections.Generic;

namespace CareerTech.Utils
{
    public class PayPalService
    {
        public readonly static string CliendId;
        public readonly static string ClientSecret;
        static PayPalService()
        {
            GetConfig();
            CliendId = "AZTT6YiG3mdaDmMlCYMIr7U9AEaAHyg8bEvg0HftwJJrk5SpKbfuLf4LTYriyqjVxykm55pbSf_IZqpY";
            ClientSecret = "EPUbIsBfjTRbd0ZW18bvFElMV5WQJYtYF_nN9bFAOQF3gco5aed1vGbUJoheUSsl3E9XbieKkz8MUxYX";
        }
        public static Dictionary<string, string> GetConfig()
        {
            return ConfigManager.Instance.GetProperties();
        }

        //create access token
        public static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(CliendId, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            var apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }

    }
}