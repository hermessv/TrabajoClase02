using System;

namespace efscaffold.Repositories.Dtos
{
    public class IngresoResultDto
    {
        public int AutomovilId { get; set; }
        public string Tipo { get; set; } = null!;
        public int ParqueoId { get; set; }
        public string Provincia { get; set; } = null!;
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public decimal? MontoTotal { get; set; }
    }
}