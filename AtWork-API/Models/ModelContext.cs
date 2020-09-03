namespace AtWork_API.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=atwork_dev")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<CombinedNewsItem> CombinedNewsItems { get; set; }
        public virtual DbSet<MobilePreference> MobilePreferences { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }
        public virtual DbSet<Post_Activity_Selected_Emp> Post_Activity_Selected_Emp { get; set; }
        public virtual DbSet<Post_News_Selected_Emp> Post_News_Selected_Emp { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_Activities> tbl_Activities { get; set; }
        public virtual DbSet<tbl_Activities_Keywords_Selected> tbl_Activities_Keywords_Selected { get; set; }
        public virtual DbSet<tbl_Activity_Dates> tbl_Activity_Dates { get; set; }
        public virtual DbSet<tbl_Activity_Employees_Custom> tbl_Activity_Employees_Custom { get; set; }
        public virtual DbSet<tbl_Activity_Feedback> tbl_Activity_Feedback { get; set; }
        public virtual DbSet<tbl_Activity_GetTogether_Emoticons> tbl_Activity_GetTogether_Emoticons { get; set; }
        public virtual DbSet<tbl_Activity_LanguagesSelected> tbl_Activity_LanguagesSelected { get; set; }
        public virtual DbSet<tbl_Activity_Pictures> tbl_Activity_Pictures { get; set; }
        public virtual DbSet<tbl_ActivityLanguages> tbl_ActivityLanguages { get; set; }
        public virtual DbSet<tbl_audit> tbl_audit { get; set; }
        public virtual DbSet<tbl_audit_company> tbl_audit_company { get; set; }
        public virtual DbSet<tbl_Cat_Corporate> tbl_Cat_Corporate { get; set; }
        public virtual DbSet<tbl_Cat_CorporateEvents> tbl_Cat_CorporateEvents { get; set; }
        public virtual DbSet<tbl_Cat_Culture> tbl_Cat_Culture { get; set; }
        public virtual DbSet<tbl_Cat_Education> tbl_Cat_Education { get; set; }
        public virtual DbSet<tbl_Cat_Sports> tbl_Cat_Sports { get; set; }
        public virtual DbSet<tbl_Categories> tbl_Categories { get; set; }
        public virtual DbSet<tbl_Classes> tbl_Classes { get; set; }
        public virtual DbSet<tbl_Companies> tbl_Companies { get; set; }
        public virtual DbSet<tbl_EducationLevels> tbl_EducationLevels { get; set; }
        public virtual DbSet<tbl_Feedback_Feeling> tbl_Feedback_Feeling { get; set; }
        public virtual DbSet<tbl_Feedback_Improve> tbl_Feedback_Improve { get; set; }
        public virtual DbSet<tbl_JobSync> tbl_JobSync { get; set; }
        public virtual DbSet<tbl_Keywords> tbl_Keywords { get; set; }
        public virtual DbSet<tbl_Keywords_Custom> tbl_Keywords_Custom { get; set; }
        public virtual DbSet<tbl_News> tbl_News { get; set; }
        public virtual DbSet<tbl_News_Comments> tbl_News_Comments { get; set; }
        public virtual DbSet<tbl_News_Comments_Likes> tbl_News_Comments_Likes { get; set; }
        public virtual DbSet<tbl_News_Documents> tbl_News_Documents { get; set; }
        public virtual DbSet<tbl_News_Employees_Custom> tbl_News_Employees_Custom { get; set; }
        public virtual DbSet<tbl_News_Reviews> tbl_News_Reviews { get; set; }
        public virtual DbSet<tbl_Petitions> tbl_Petitions { get; set; }
        public virtual DbSet<tbl_SubCategories> tbl_SubCategories { get; set; }
        public virtual DbSet<tbl_Volunteer_Interests> tbl_Volunteer_Interests { get; set; }
        public virtual DbSet<tbl_Volunteers> tbl_Volunteers { get; set; }
        public virtual DbSet<tbl_Vortex_Activity_Employee> tbl_Vortex_Activity_Employee { get; set; }
        public virtual DbSet<tbl_Vortex_Activity_Employee_Hours> tbl_Vortex_Activity_Employee_Hours { get; set; }
        public virtual DbSet<tbl_Vortex_Companies_Classes> tbl_Vortex_Companies_Classes { get; set; }
        public virtual DbSet<tbl_Vortex_Companies_Classes_Values> tbl_Vortex_Companies_Classes_Values { get; set; }
        public virtual DbSet<tbl_Vortex_Employee_Classes> tbl_Vortex_Employee_Classes { get; set; }
        public virtual DbSet<tblUserNet> tblUserNets { get; set; }
        public virtual DbSet<VolunteerNewsItem> VolunteerNewsItems { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<tbl_blog> tbl_blog { get; set; }
        public virtual DbSet<tbl_blog_authors> tbl_blog_authors { get; set; }
        public virtual DbSet<tbl_CMSContent> tbl_CMSContent { get; set; }
        public virtual DbSet<tbl_CMSContentMenu> tbl_CMSContentMenu { get; set; }
        public virtual DbSet<tbl_CMSContentMenuTemplate> tbl_CMSContentMenuTemplate { get; set; }
        public virtual DbSet<tbl_CMSContentMenuWeb> tbl_CMSContentMenuWeb { get; set; }
        public virtual DbSet<tbl_CMSContentTemplate> tbl_CMSContentTemplate { get; set; }
        public virtual DbSet<tbl_MobileInterest> tbl_MobileInterest { get; set; }
        public virtual DbSet<tbl_MobilePreferences> tbl_MobilePreferences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CombinedNewsItem>()
                .Property(e => e.newsContent)
                .IsUnicode(false);

            modelBuilder.Entity<NewsItem>()
                .Property(e => e.newsContent)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Activities>()
                .Property(e => e.proDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Activities>()
                .Property(e => e.proSpecialRequirements)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Activities>()
                .Property(e => e.proAddActivity_AdditionalInfo)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Activity_Feedback>()
                .Property(e => e.ActivityFeedback_Like)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Activity_Feedback>()
                .Property(e => e.ActivityFeedbackComments)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Activity_Feedback>()
                .Property(e => e.ActivityFeedbackAdditional)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Companies>()
                .Property(e => e.coDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_News>()
                .Property(e => e.newsContent)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_News_Comments>()
                .Property(e => e.comContent)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_News_Comments>()
                .HasMany(e => e.tbl_News_Comments_Likes)
                .WithRequired(e => e.tbl_News_Comments)
                .HasForeignKey(e => e.newsCommentId);

            modelBuilder.Entity<tbl_Petitions>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Petitions>()
                .Property(e => e.motivation)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Volunteers>()
                .Property(e => e.volAbout)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_Vortex_Activity_Employee_Hours>()
                .Property(e => e.volHours)
                .HasPrecision(10, 2);

            modelBuilder.Entity<VolunteerNewsItem>()
                .Property(e => e.newsContent)
                .IsUnicode(false);

            modelBuilder.Entity<Volunteer>()
                .Property(e => e.volAbout)
                .IsUnicode(false);

            modelBuilder.Entity<tbl_MobilePreferences>()
                .Property(e => e.Interests)
                .IsUnicode(false);

        }
    }
}
