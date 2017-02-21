using System.Collections.Generic;

/// <summary>
/// WebOperationCriteria contains data that specifies how a WWW operation will be performed.
/// 
/// Note: this may need to be expanded if we ever anticipate supporting binary fields.
/// </summary>
public sealed class WebOperationCriteria
{
    /// <summary>
    /// The URL of the WWW operation.
    /// </summary>
    public string URL
    {
        get;
        private set;
    }

    /// <summary>
    /// The data fields of the operation.
    /// </summary>
    public Dictionary<string, string> Fields
    {
        get;
        private set;
    }

    /// <summary>
    /// Constructs a new WebOperationCriteria.
    /// </summary>
    /// <param name="URL">The URL of the operation.</param>
    /// <param name="fields">A Dictionary containing the data fields of the operation.</param>
    public WebOperationCriteria(string URL, Dictionary<string, string> fields)
    {
        Asserter.NotNullOrEmpty(URL, "WebOperationCriteria.ctor:URL is null or empty");
        Asserter.NotNull(fields, "WebOperationCriteria.ctor:fields is null or empty");

        this.URL = URL;
        Fields = fields;
    }
}
