using Newtonsoft.Json;

namespace Core.Entities
{
    public class FirebaseMessage
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string token { get; set; }
        public Notification notification { get; set; }
        //public Android android { get; set; }
        //public Apns apns { get; set; }
        //public Webpush webpush { get; set; }
    }

    public class Notification
    {
        public string title { get; set; }
        public string body { get; set; }
        //public string click_action { get; set; }
    }

    public class Android
    {
        public string ttl { get; set; }
        public Notification notification { get; set; }
    }

    public class Apns
    {
        public Headers headers { get; set; }
        public Payload payload { get; set; }
    }

    public class Aps
    {
        public string category { get; set; }
    }

    public class Headers
    {
        [JsonProperty("apns-priority")]
        public string apnspriority { get; set; }
        public string TTL { get; set; }
    }

    public class Payload
    {
        public Aps aps { get; set; }
    }

    public class Webpush
    {
        public Headers headers { get; set; }
    }
}
