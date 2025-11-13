using Nop.Core.Configuration;

namespace Nop.Core.Domain.ArtificialIntelligence;

/// <summary>
/// Artificial intelligence settings
/// </summary>
public partial class ArtificialIntelligenceSettings : ISettings
{
    /// <summary>
    /// Gets or sets a value indicating whether we should use AI
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the AI provider type
    /// </summary>
    public ArtificialIntelligenceProviderType ProviderType { get; set; }

    /// <summary>
    /// Gets or sets the Gemini API key
    /// </summary>
    public string GeminiApiKey { get; set; }

    /// <summary>
    /// Gets or sets the ChatGPT API key
    /// </summary>
    public string ChatGptApiKey { get; set; }

    /// <summary>
    /// Gets or sets the DeepSeek API key
    /// </summary>
    public string DeepSeekApiKey { get; set; }

    /// <summary>
    /// Gets or sets a period (in seconds) before the request times out
    /// </summary>
    public int? RequestTimeout { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether generation of product descriptions with AI is allowed
    /// </summary>
    public bool AllowProductDescriptionGeneration { get; set; }

    /// <summary>
    /// Gets or sets a query format string to generate product description with AI
    /// </summary>
    public string ProductDescriptionQuery { get; set; }
}