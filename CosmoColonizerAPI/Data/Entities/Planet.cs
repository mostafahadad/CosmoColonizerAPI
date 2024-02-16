namespace CosmoColonizerAPI.Data.Entities
{
    public class Planet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public double? Temperature { get; set; }
        public double? OxygenVolume { get; set; }
        public double? WaterVolume { get; set; }
        public double? Gravity { get; set; }
        public double? AtmosphericPressure { get; set; }
    }
}
