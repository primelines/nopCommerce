using Nop.Core.Configuration;

namespace Nop.Api.Framework;
/// <summary>
/// Represents Api configuration parameters
/// </summary>
public partial class ApiConfig : IConfig
{
    public int AllowedClockSkewInMinutes { get; set; } = 5;

    public string SecurityKey { get; set; } = "NowIsTheTimeForAllGoodMenToComeToTheAideOfTheirCountry";
}