namespace LMS.Data_.Helper
{
    public class PaymobSetting
    {
        public string apiKey { get; set; }
        public string publicKey { get; set; }
        public string SecretKey { get; set; }
        public string HMAC { get; set; }
        public string MerchantId { get; set; }
        public string CardIntegrationId { get; set; }
        public string MobileIntegrationId { get; set; }
        public List<string> AllowedIPs { get; set; }


    }
}
