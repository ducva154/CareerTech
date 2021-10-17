namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CompanyProfile")]
    public partial class CompanyProfile
    {
        public CompanyProfile()
        {
            Recruitments = new HashSet<Recruitment>();
        }

        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string Desc { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Url_Avatar { get; set; }

        [StringLength(255)]
        public string Url_Background { get; set; }

        public string Status { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Recruitment> Recruitments { get; set; }
    }
}
