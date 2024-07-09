using System.ComponentModel.DataAnnotations;

namespace varlikHesaplama.Models
{
    public class ufeEndeks
    {
        [Key]
        public int Id { get; set; }
        public decimal Deger { get; set; }
        public decimal DolarKuru { get; set; }
        public DateTime Tarih { get; set; }
    }
}