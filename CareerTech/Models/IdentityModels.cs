using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CareerTech.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Abouts = new HashSet<About>();
            CompanyProfiles = new HashSet<CompanyProfile>();
            Introductions = new HashSet<Introduction>();
            Orders = new HashSet<Order>();
            Portfolios = new HashSet<Portfolio>();
            Solutions = new HashSet<Solution>();
            Times = new HashSet<Time>();
            Candidates = new HashSet<Candidate>();
        }

        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string FullName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "varchar")]
        public string Url_Image { get; set; }

        public virtual ICollection<About> Abouts { get; set; }

        public virtual ICollection<CompanyProfile> CompanyProfiles { get; set; }

        public virtual ICollection<Introduction> Introductions { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Portfolio> Portfolios { get; set; }

        public virtual ICollection<Solution> Solutions { get; set; }

        public virtual ICollection<Time> Times { get; set; }

        public virtual ICollection<Candidate> Candidates { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Experience> Experiences { get; set; }
        public virtual DbSet<Introduction> Introductions { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<Recruitment> Recruitments { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Time> Times { get; set; }

        public virtual DbSet<Candidate> Candidates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");

            modelBuilder.Entity<About>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .Property(e => e.Url_Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .Property(e => e.Url_Background)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyProfile>()
                .HasMany(e => e.Recruitments)
                .WithRequired(e => e.CompanyProfile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Education>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Education>()
                .Property(e => e.PortfolioID)
                .IsUnicode(false);

            modelBuilder.Entity<Education>()
                .Property(e => e.Time)
                .IsUnicode(false);

            modelBuilder.Entity<Experience>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Experience>()
                .Property(e => e.PortfolioID)
                .IsUnicode(false);

            modelBuilder.Entity<Experience>()
                .Property(e => e.Time)
                .IsUnicode(false);

            modelBuilder.Entity<Introduction>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Introduction>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Introduction>()
                .Property(e => e.Url_Image)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Recruitments)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.SubscriptionID)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.OrderID)
                .IsUnicode(false);

            modelBuilder.Entity<Portfolio>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Portfolio>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Portfolio>()
                .Property(e => e.Url_Domain)
                .IsUnicode(false);

            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.Educations)
                .WithRequired(e => e.Portfolio)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.Experiences)
                .WithRequired(e => e.Portfolio)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Portfolio)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.Profiles)
                .WithRequired(e => e.Portfolio)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.Skills)
                .WithRequired(e => e.Portfolio)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Product>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.PortfolioID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Url_Image)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Domain)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.PortfolioID)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Url_avatar)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Instagram_url)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Facebook_url)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Twitter_url)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.Youtube_url)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
                .Property(e => e.CompanyProfileID)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
                .Property(e => e.JobID)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
             .HasMany(e => e.Candidates)
             .WithRequired(e => e.Recruitment)
             .WillCascadeOnDelete(true);


            //modelBuilder.Entity<Recruitment>()
            //    .HasMany(e => e.Users)
            //    .WithMany(e => e.Recruitments)
            //    .Map(m => m.ToTable("Candidate").MapLeftKey("RecruitmentID").MapRightKey("UserID"));

            modelBuilder.Entity<Candidate>()
               .Property(c => c.ID)
               .IsUnicode(false);

            modelBuilder.Entity<Candidate>()
                .Property(c => c.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Candidate>()
                .Property(c => c.RecruitmentID)
                .IsUnicode(false);

            modelBuilder.Entity<Skill>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Skill>()
                .Property(e => e.PortfolioID)
                .IsUnicode(false);

            modelBuilder.Entity<Solution>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Solution>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Solution>()
                .Property(e => e.Url_image)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Subscription>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Subscription)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Time>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Time>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Abouts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CompanyProfiles)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Introductions)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Portfolios)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Solutions)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Times)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
               .HasMany(e => e.Candidates)
               .WithRequired(e => e.User)
               .WillCascadeOnDelete(false);

        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}