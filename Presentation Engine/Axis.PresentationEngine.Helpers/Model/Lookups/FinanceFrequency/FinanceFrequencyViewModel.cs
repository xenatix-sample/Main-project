namespace Axis.PresentationEngine.Helpers.Model
{
    /// <summary>
    /// Model to hold Finance Frequency View data
    /// </summary>
    public class FinanceFrequencyViewModel : BaseViewModel
    {
        public int FinanceFrequencyID { get; set; }
        public string FinanceFrequency { get; set; }
        public string FinanceFrequencyType { get; set; }
        public decimal FrequencyFactor { get; set; }
        public string MeasureBy { get; set; }
    }
}
