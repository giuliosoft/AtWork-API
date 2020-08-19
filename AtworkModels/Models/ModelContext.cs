namespace AtworkModels.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public partial class ModelContext : DbContext
    {
        public ModelContext() : base("name=atwork_dev")
        {
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivitySelectedKeyword> ActivitySelectedKeywords { get; set; }
        public virtual DbSet<ActivityDate> ActivityDates { get; set; }
        public virtual DbSet<ActivityEmployeeCustom> ActivityEmployeesCustom { get; set; }
        public virtual DbSet<ActivityFeedback> ActivityFeedbacks { get; set; }
        public virtual DbSet<ActivityGetTogetherEmoticon> ActivityGetTogetherEmoticons { get; set; }
        public virtual DbSet<ActivitySelectedLanguage> ActivitySelectedLanguages { get; set; }
        public virtual DbSet<ActivityPicture> ActivityPictures { get; set; }
        public virtual DbSet<ActivityLanguage> ActivityLanguages { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<AuditCompany> AuditCompanies { get; set; }
        public virtual DbSet<CorporateCategory> CorporateCategories { get; set; }
        public virtual DbSet<CorporateEventCategory> CorporateEventCategories { get; set; }
        public virtual DbSet<CultureCategory> CultureCategories { get; set; }
        public virtual DbSet<EducationCategory> EducationCategories { get; set; }
        public virtual DbSet<SportCategory> SportCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<EducationLevel> EducationLevels { get; set; }
        public virtual DbSet<FeedbackFeeling> FeedbackFeelings { get; set; }
        public virtual DbSet<FeedbackImprove> FeedbackImproves { get; set; }
        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<CustomKeyword> CustomKeywords { get; set; }
        public virtual DbSet<NewsItem> NewsItems { get; set; }
        public virtual DbSet<NewsComment> NewsComments { get; set; }
        public virtual DbSet<NewsCommentLike> NewsCommentLikes { get; set; }
        public virtual DbSet<NewsDocument> NewsDocuments { get; set; }
        public virtual DbSet<NewsEmployeeCustom> NewsEmployeeCustoms { get; set; }
        public virtual DbSet<NewsReview> NewsReviews { get; set; }
        public virtual DbSet<Petition> Petitions { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<VolunteerInterest> VolunteerInterests { get; set; }
        public virtual DbSet<VortexActivityEmployee> VortexActivityEmployees { get; set; }
        public virtual DbSet<VortexActivityEmployeeHour> VortexActivityEmployeeHours { get; set; }
        public virtual DbSet<VortexCompanyClass> VortexCompanyClasses { get; set; }
        public virtual DbSet<VortexCompanyClassValue> VortexCompanyClassValues { get; set; }
        public virtual DbSet<VortexEmployeeClass> VortexEmployeeClasses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogAuthor> BlogAuthors { get; set; }
        public virtual DbSet<CMSContent> CMSContents { get; set; }
        public virtual DbSet<CMSContentMenu> CMSContentMenus { get; set; }
        public virtual DbSet<CMSContentMenuTemplate> CMSContentMenuTemplates { get; set; }
        public virtual DbSet<CMSWebContentMenu> CMSWebContentMenus { get; set; }
        public virtual DbSet<CMSContentTemplate> CMSContentTemplates { get; set; }
        public virtual DbSet<ActivityDateFullType> ActivityDateFullTypes { get; set; }
        public virtual DbSet<ActivityEmployeeFinalCount> ActivityEmployeeCountFinalCounts { get; set; }
        public virtual DbSet<BlogContent> BlogContents { get; set; }
        public virtual DbSet<CompanyNewsItem> CompanyNewsItems { get; set; }
        public virtual DbSet<VolunteerNewsItem> VolunteerNewsItems { get; set; }
        public virtual DbSet<CombinedNewsItem> CombinedNewsItems { get; set; }
        public virtual DbSet<DocumentOriginNewsItem> DocumentOriginNewsItems { get; set; }
        public virtual DbSet<VortexActivityEmployeeDetail> VortexActivityEmployeeDetails { get; set; }
        public virtual DbSet<VortexAvailableActivity> VortexAvailableActivities { get; set; }
        public virtual DbSet<MobilePreference> MobilePreferences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>().ToTable("tbl_Activities");
            modelBuilder.Entity<ActivitySelectedKeyword>().ToTable("tbl_Activities_Keywords_Selected");
            modelBuilder.Entity<ActivityDate>().ToTable("tbl_Activity_Dates");
            modelBuilder.Entity<ActivityEmployeeCustom>().ToTable("tbl_Activity_Employees_Custom");
            modelBuilder.Entity<ActivityFeedback>().ToTable(" tbl_Activity_Feedback");
            modelBuilder.Entity<ActivityGetTogetherEmoticon>().ToTable("tbl_Activity_GetTogether_Emoticons");
            modelBuilder.Entity<ActivitySelectedLanguage>().ToTable("tbl_Activity_LanguagesSelected");
            modelBuilder.Entity<ActivityPicture>().ToTable("tbl_Activity_Pictures");
            modelBuilder.Entity<ActivityLanguage>().ToTable("tbl_ActivityLanguages");
            modelBuilder.Entity<Audit>().ToTable("tbl_audit");
            modelBuilder.Entity<AuditCompany>().ToTable("tbl_audit_company");
            modelBuilder.Entity<CorporateCategory>().ToTable("tbl_Cat_Corporate");
            modelBuilder.Entity<CorporateEventCategory>().ToTable("tbl_Cat_CorporateEvents");
            modelBuilder.Entity<CultureCategory>().ToTable("tbl_Cat_Culture");
            modelBuilder.Entity<EducationCategory>().ToTable("tbl_Cat_Education");
            modelBuilder.Entity<SportCategory>().ToTable("tbl_Cat_Sports");
            modelBuilder.Entity<Category>().ToTable("tbl_Categories");
            modelBuilder.Entity<Class>().ToTable("tbl_Classes");
            modelBuilder.Entity<Company>().ToTable("tbl_Companies");
            modelBuilder.Entity<EducationLevel>().ToTable("tbl_EducationLevels");
            modelBuilder.Entity<FeedbackFeeling>().ToTable("tbl_Feedback_Feeling");
            modelBuilder.Entity<FeedbackImprove>().ToTable("tbl_Feedback_Improve");
            modelBuilder.Entity<Keyword>().ToTable("tbl_Keywords");
            modelBuilder.Entity<CustomKeyword>().ToTable("tbl_Keywords_Custom");
            modelBuilder.Entity<NewsItem>().ToTable("tbl_News");
            modelBuilder.Entity<NewsComment>().ToTable("tbl_News_Comments");
            modelBuilder.Entity<NewsCommentLike>().ToTable("tbl_News_Comments_Likes");
            modelBuilder.Entity<NewsDocument>().ToTable("tbl_News_Documents");
            modelBuilder.Entity<NewsEmployeeCustom>().ToTable("tbl_News_Employees_Custom");
            modelBuilder.Entity<NewsReview>().ToTable("tbl_News_Reviews");
            modelBuilder.Entity<Petition>().ToTable("tbl_Petitions");
            modelBuilder.Entity<SubCategory>().ToTable("tbl_SubCategories");
            modelBuilder.Entity<Volunteer>().ToTable("tbl_Volunteers");
            modelBuilder.Entity<VolunteerInterest>().ToTable("tbl_Volunteer_Interests");
            modelBuilder.Entity<VortexActivityEmployee>().ToTable("tbl_Vortex_Activity_Employee");
            modelBuilder.Entity<VortexActivityEmployeeHour>().ToTable("tbl_Vortex_Activity_Employee_Hours");
            modelBuilder.Entity<VortexCompanyClass>().ToTable("tbl_Vortex_Companies_Classes");
            modelBuilder.Entity<VortexCompanyClassValue>().ToTable("tbl_Vortex_Companies_Classes_Values");
            modelBuilder.Entity<VortexEmployeeClass>().ToTable("tbl_Vortex_Employee_Classes");
            modelBuilder.Entity<User>().ToTable("tblUserNets");
            modelBuilder.Entity<Blog>().ToTable("tbl_blog");
            modelBuilder.Entity<BlogAuthor>().ToTable("tbl_blog_authors");
            modelBuilder.Entity<CMSContent>().ToTable("tbl_CMSContent");
            modelBuilder.Entity<CMSContentMenu>().ToTable("tbl_CMSContentMenu");
            modelBuilder.Entity<CMSContentMenuTemplate>().ToTable("tbl_CMSContentMenuTemplate");
            modelBuilder.Entity<CMSWebContentMenu>().ToTable("tbl_CMSContentMenuWeb");
            modelBuilder.Entity<CMSContentTemplate>().ToTable("tbl_CMSContentTemplate");
            modelBuilder.Entity<ActivityDateFullType>().ToTable("vw_Activity_Date_Type_Full");
            modelBuilder.Entity<ActivityEmployeeFinalCount>().ToTable("vw_Activity_Employee_Count_Final");
            modelBuilder.Entity<BlogContent>().ToTable("vw_BlogContent");
            modelBuilder.Entity<CompanyNewsItem>().ToTable("vw_News_Company");
            modelBuilder.Entity<VolunteerNewsItem>().ToTable("vw_News_Volunteer");
            modelBuilder.Entity<CombinedNewsItem>().ToTable("vw_NewsCombined");
            modelBuilder.Entity<DocumentOriginNewsItem>().ToTable("vw_NewsDocumentOrigin");
            modelBuilder.Entity<VortexActivityEmployeeDetail>().ToTable("vw_Vortex_Activities_Employees_Details");
            modelBuilder.Entity<VortexAvailableActivity>().ToTable("vw_Vortex_Activity_Available");
            modelBuilder.Entity<MobilePreference>().ToTable("tbl_MobilePreferences");
        }
    }
}
