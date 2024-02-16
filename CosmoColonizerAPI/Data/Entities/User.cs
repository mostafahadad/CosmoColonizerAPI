﻿namespace CosmoColonizerAPI.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? PlanetId { get; set; }
        public Planet Planet { get; set; }
    }
}
