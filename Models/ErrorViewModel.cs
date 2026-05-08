namespace PharmaSphere.Models;

/// <summary>
/// ViewModel for displaying error information.
/// </summary>
public class ErrorViewModel
{
    /// <summary>Gets or sets the unique request ID associated with the error.</summary>
    public string? RequestId { get; set; }

    /// <summary>Gets a value indicating whether to display the request ID.</summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
