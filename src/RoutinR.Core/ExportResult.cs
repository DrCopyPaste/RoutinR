namespace RoutinR.Core
{
    /// <summary>
    /// represents one try to export an item
    /// </summary>
    public class ExportResult
    {
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public readonly string ErrorMessage;

        /// <summary>
        /// Initializes a new ExportResult instance
        /// </summary>
        /// <param name="errorMessage">
        /// Contents of the error message (optional)
        /// </param>
        public ExportResult(string errorMessage = "")
        {
            ErrorMessage = errorMessage;
        }
    }
}
