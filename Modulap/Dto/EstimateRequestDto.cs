namespace Modulap.Dto
{
    public class EstimateRequestDto
    {
       
        public string ClientId { get; set; }
        public string ChickenCoopLocation { get; set; } = string.Empty;
        public int QuantityChickens { get; set; } = 0;

        public string EnvironmentalMonitoring { get; set; } = string.Empty;
        public string Airqualitymonitoring { get; set; } = string.Empty;
        public string Naturallightingmonitoring { get; set; } = string.Empty;
        public string Automaticfeeddispenser { get; set; } = string.Empty;
        public string Waterlevelgauge { get; set; } = string.Empty;
        public string NightMotionSensor { get; set; } = string.Empty;
        public string connectiontype { get; set; } = string.Empty;
        public string PhysicalInstallation { get; set; } = string.Empty;
    }
}
