using System.Configuration;

namespace ChristmasPartyMonitor.Utilities
{
    public static class Config
    {
        public static string FaceServiceClientKey => ConfigurationManager.AppSettings["FaceServiceClientKey"];
        public static string EmotionServiceClientKey => ConfigurationManager.AppSettings["EmotionServiceClientKey"];
        public static float IdentifyConfidenceThreshold => float.Parse(ConfigurationManager.AppSettings["IdentifyConfidenceThreshold"]);
        public static string SlackMessageBaseUrl => ConfigurationManager.AppSettings["SlackMessageBaseUrl"];
        public static string SlackMessageEndpointUrl => ConfigurationManager.AppSettings["SlackMessageEndpointUrl"];
        public static string SlackMessageUsername => ConfigurationManager.AppSettings["SlackMessageUsername"];
        public static string SlackMessageChannel => ConfigurationManager.AppSettings["SlackMessageChannel"];
        public static string SlackMessageIconUrl => ConfigurationManager.AppSettings["SlackMessageIconUrl"];
    }
}