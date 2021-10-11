namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Portfolio")]
    public partial class Portfolio
    {
        public Portfolio()
        {
            Educations = new HashSet<Education>();
            Experiences = new HashSet<Experience>();
            Products = new HashSet<Product>();
            Profiles = new HashSet<Profile>();
            Skills = new HashSet<Skill>();
        }

        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool PublicStatus { get; set; }

        [Required]
        [StringLength(255)]
        public string Url_Domain { get; set; }

        public bool MainStatus { get; set; }

        public virtual ICollection<Education> Educations { get; set; }

        public virtual ICollection<Experience> Experiences { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
