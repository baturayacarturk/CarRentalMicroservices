﻿namespace CarRental.Services.Catalog.Settings
{
    public interface IDatabaseSettings
    {
        public string CarCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
