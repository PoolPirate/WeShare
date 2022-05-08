using System.Text.Json.Serialization;

namespace WeShare.WebAPI.Forms;

public class PostSubmitForm
{
    [JsonPropertyName("headers")]
    public Dictionary<string, string[]> Headers { get; set; }

    [JsonPropertyName("payload")]
    public byte[] Payload { get; set; }

    public PostSubmitForm()
    {
    }
}
