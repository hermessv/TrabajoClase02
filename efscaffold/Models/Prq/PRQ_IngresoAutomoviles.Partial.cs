using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace efscaffold.Models.Prq
{
    public partial class PrqIngresoAutomovil
    {
        /// <summary>
        /// Difference in minutes between ExitDateTime and EntryDateTime, or null if ExitDateTime is null.
        /// </summary>
        [NotMapped]
        public int? DurationMinutes
        {
            get
            {
                if (!ExitDateTime.HasValue) return null;
                return (int)Math.Round((ExitDateTime.Value - EntryDateTime).TotalMinutes);
            }
        }

        /// <summary>
        /// Duration in hours (minutes / 60.0), rounded to 2 decimals, or null if no exit time.
        /// </summary>
        [NotMapped]
        public double? DurationHours
        {
            get
            {
                if (!DurationMinutes.HasValue) return null;
                double hours = DurationMinutes.Value / 60.0;
                return Math.Round(hours, 2);
            }
        }

        /// <summary>
        /// Total amount = DurationHours * PRQ_Parqueo.PricePerHour, or null if no exit time or no related PRQ_Parqueo.
        /// </summary>
        [NotMapped]
        public decimal? TotalAmount
        {
            get
            {
                if (!DurationHours.HasValue) return null;
                if (Parqueo == null) return null;
                // PricePerHour assumed non-null in PrqParqueo
                decimal hours = (decimal)DurationHours.Value;
                return Math.Round(hours * Parqueo.PricePerHour, 2);
            }
        }
    }
}