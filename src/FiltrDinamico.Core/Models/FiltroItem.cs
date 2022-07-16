namespace FiltrDinamico.Core.Models
{
    public class FiltroItem
    {
        public string Property { get; set; }
        public string FilterType { get; set; }
        public object Value { get; set; }
        public FiltroItem Or { get; set; }
        public FiltroItem And { get; set; }
    }
}
