namespace varlikHesaplama.Models
{
    public class hesaplanmisDegerler
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public decimal varlikTutari { get; set; }
        public decimal oncekiAyaGoreVarlikArtis { get; set; }
        public decimal varlikDegisimOrani { get; set; }
        public decimal varlikTarihiDolarKuru { get; set; }
        public decimal dolarizasyonVarlikTutari { get; set; }
        public decimal dolarizasyonOncekiAyaGoreVarlikArtis { get; set; }
        public decimal dolarizasyonVarlikDegisimOrani { get; set; }
        public decimal dolarizasyonEtkisiYüzde { get; set; }
        public decimal ufeEndeks { get; set; }
        public decimal enflasyonVarliktutari { get; set; }
        public decimal enflasyonOncekiAyaGoreVarlikArtis { get; set; }
        public decimal enflasyonVarlikDegisimOrani { get; set; }
        public decimal enflasyonEtkisiYuzde { get; set; }
    }
}
