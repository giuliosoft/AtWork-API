using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AtWork_API.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("name=atwork_dev")
        {
        }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<MobilePreference> MobilePreferences { get; set; }
        public virtual DbSet<CombinedNewsItem> CombinedNewsItems { get; set; }
        public virtual DbSet<VolunteerNewsItem> VolunteerNewsItems { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<NewsItem>().ToTable("tbl_News");
        //    modelBuilder.Entity<Volunteer>().ToTable("tbl_Volunteers");
        //    modelBuilder.Entity<VolunteerNewsItem>().ToTable("vw_News_Volunteer");
        //    modelBuilder.Entity<CombinedNewsItem>().ToTable("vw_NewsCombined");
        //    modelBuilder.Entity<MobilePreference>().ToTable("tbl_MobilePreferences");
        //}

    }
}