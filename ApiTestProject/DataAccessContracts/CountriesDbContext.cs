﻿namespace DataAccessContracts
{
    using System;
    using System.Data.Entity;

    public class CountriesDbContext : DbContext
    {
        public CountriesDbContext() : base("CountriesDbContext")
        {
        }

        public virtual DbSet<CountryDto> Countries { get; set; }
        public virtual DbSet<CountrySubdivisionDto> CountrySubdivisions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<CountryDto>()
                        .ToTable("Countries", "dbo");

            modelBuilder.Entity<CountrySubdivisionDto>()
                        .ToTable("CountrySubdivisions", "dbo");
        }
    }
}
