using System.ComponentModel.DataAnnotations;

namespace varlikHesaplama.Models
{
    public class kurBilgisi
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
    }
}
