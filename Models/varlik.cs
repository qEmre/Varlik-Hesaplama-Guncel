using System.ComponentModel.DataAnnotations;

namespace varlikHesaplama.Models
{
    public class varlik
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tutari { get; set; }
        public DateTime Tarihi { get; set; }
    }
}
