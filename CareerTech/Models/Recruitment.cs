namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recruitment")]
    public partial class Recruitment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recruitment()
        {
            Users = new HashSet<ApplicationUser>();
        }

        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyProfileID { get; set; }

        [Required]
        [StringLength(255)]
        public string JobID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string Desc { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string Requirement { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string Benefit { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string Salary { get; set; }

        [Required]
        [StringLength(255)]
        public string WorkTime { get; set; }

        public int? Total { get; set; }

        [Required]
        [StringLength(255)]
        public string Position { get; set; }

        public bool? Gender { get; set; }

        public virtual CompanyProfile CompanyProfile { get; set; }

        public virtual Job Job { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
