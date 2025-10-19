using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efscaffold.Models.Prq
{
    [Table("PRQ_IngresoAutomoviles")]
    public partial class PrqIngresoAutomovil
    {
        [Key]
        [Column("consecutive")]
        public int Consecutive { get; set; }

        [Required]
        [Column("prq_parqueo_id")]
        public int PrqParqueoId { get; set; }

        [Required]
        [Column("prq_automovil_id")]
        public int PrqAutomovilId { get; set; }

        [Required]
        [Column("entry_datetime")]
        public DateTime EntryDateTime { get; set; }

        [Column("exit_datetime")]
        public DateTime? ExitDateTime { get; set; }

        // Navigation properties
        [ForeignKey("PrqParqueoId")]
        public virtual PrqParqueo? Parqueo { get; set; }

        [ForeignKey("PrqAutomovilId")]
        public virtual PrqAutomovil? Automovil { get; set; }
    }
}