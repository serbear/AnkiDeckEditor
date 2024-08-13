namespace AnkiDeckEditor.Libs.HanumanInstitute;

/// <summary>
/// When implemented alongside IViewLocator, allows to force single page navigation even on desktop.  
/// </summary>
public interface IViewLocatorNavigation
{
    /// <summary>
    /// Gets or sets whether to force single-page navigation. Setting this to true can allow running in single-page
    /// mode on desktop.
    /// </summary>
    bool ForceSinglePageNavigation { get; set; }

    /// <summary>
    /// Gets whether the application runs in single-page navigation mode.
    /// </summary>
    bool UseSinglePageNavigation { get; }
}