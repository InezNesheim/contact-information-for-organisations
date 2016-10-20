namespace AltinnDesktopTool.Configuration
{
    //TODO: define UI settings

    /// <summary>
    /// Configuration for User Interface
    /// </summary>
    public interface IUiEnvironmentConfig
    {
        /// <summary>
        /// Name of environment
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Name of theme selected for this enviorenment
        /// </summary>
        string ThemeName { get; set; }
    }
}
