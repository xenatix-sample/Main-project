namespace Axis.Model.Common
{
    /// <summary>
    /// Model to hold Finance Frequency data
    /// </summary>
    public class FinanceFrequencyModel : BaseEntity
    {
        public int FinanceFrequencyID { get; set; }
        public string FinanceFrequency { get; set; }
        public string FinanceFrequencyType { get; set; }
        public int FrequencyFactor { get; set; }
        public string MeasureBy { get; set; }      
    }
}
