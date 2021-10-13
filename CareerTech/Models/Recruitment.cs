namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  
    [Table("Recruitment")]
    public partial class Recruitment
    {
        public Recruitment()
        {
            Candidates = new HashSet<Candidate>();
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

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string Salary { get; set; }

        [Required]
        [StringLength(255)]
        public string Workingform { get; set; }

        [Required]
        [StringLength(255)]
        public string Amount { get; set; }

        [Required]
        [StringLength(255)]
        public string Position { get; set; }

        [Required]
        [StringLength(255)]
        public string Experience { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public bool? Gender { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string DetailDesc { get; set; }

        public virtual CompanyProfile CompanyProfile { get; set; }

        public virtual Job Job { get; set; }

      
        public virtual ICollection<Candidate> Candidates { get; set; }

    }
}
