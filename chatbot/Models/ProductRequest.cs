namespace chatbot.Models;
public class ProductRequest
{
    public string Reference { get; set; } = string.Empty;
    public Dictionary<string, string> Characteristics { get; set; } = new();
}

public class ProductResult
{
    public string Reference { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}


