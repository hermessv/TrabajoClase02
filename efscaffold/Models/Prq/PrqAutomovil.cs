using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efscaffold.Models.Prq
{
    [Table("PRQ_Automoviles")]
    public class PrqAutomovil
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("color")]
        [MaxLength(50)]
        public string Color { get; set; } = null!;

        [Column("year")]
        public short Year { get; set; }

        [Required]
        [Column("manufacturer")]
        [MaxLength(100)]
        public string Manufacturer { get; set; } = null!;

        [Required]
        [Column("type")]
        [MaxLength(20)]
        public string Type { get; set; } = null!;

        // Navigation
        public virtual ICollection<PrqIngresoAutomovil>? Ingresos { get; set; }
    }
}