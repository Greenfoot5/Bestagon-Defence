namespace BestagonDefence.UI.Shop;

/// <summary>
/// Represents which method to use when deciding how many shop options should be hidden
/// </summary>
public enum HiddenMode
{
    /// <summary>
    /// No hidden options
    /// </summary>
    Disabled,
    /// <summary>
    /// A set number of hidden options
    /// </summary>
    Count,
    /// <summary>
    /// A chance to hide an option (applied for each shop option)
    /// </summary>
    Chance
}