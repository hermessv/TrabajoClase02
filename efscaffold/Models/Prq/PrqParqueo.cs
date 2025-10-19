using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efscaffold.Models.Prq
{
    [Table("PRQ_Parqueo")]
    public class PrqParqueo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("province_name")]
        [MaxLength(100)]
        public string ProvinceName { get; set; } = null!;

        [Required]
        [Column("name")]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        [Column("price_per_hour", TypeName = "decimal(10,2)")]
        public decimal PricePerHour { get; set; }

        // Navigation
        public virtual ICollection<PrqIngresoAutomovil>? Ingresos { get; set; }
    }
}