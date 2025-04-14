namespace chatbot.Configuration
{
    public class AzureOpenAISettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentId { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = "2024-06-01";
}

}