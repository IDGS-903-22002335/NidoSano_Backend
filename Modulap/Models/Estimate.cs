using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Modulap.Models
{
    public class Estimate
    {

        [Key]
        public Guid IdEstimate { get; set; }

        public Guid? ChickenCoopId { get; set; }

        public string ClientId { get; set; }

        public DateTime CreationDate { get; set; }
        public string ChickenCoopLocation { get; set; } // donde se encuentra el gallinero

        public int QuantityChickens { get; set; }   // cantidad para los gallineros

        // Funcionalidades
        public string EnvironmentalMonitoring { get; set; } // Monitoreo de humedad y temperaturda
        public string Airqualitymonitoring { get; set; } // Monitoreo de calidad del Aire

        public string Naturallightingmonitoring { get; set; } // Monitoreo de luz natural

        public string Automaticfeeddispenser { get; set; } // dispensador automatico de alimentacion

        public string Waterlevelgauge { get; set; } // Medidor de nivel del agua

        public string NightMotionSensor { get; set; } //sensor de movimiento nocnoturno
        public string connectiontype { get; set; } // tipo de conexion
        public string PhysicalInstallation { get; set; } // instalacion Fisica

        public Estado Status { get; set; }

        public decimal PriceTotal { get; set; }

        // reference to chickenCoop
        public ChickenCoop ChickenCoop { get; set; }

        //reference to AppUser
        public AppUser Client { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

    }

    public enum Estado
    {
        pendiente,
        Comprar
        }

}
