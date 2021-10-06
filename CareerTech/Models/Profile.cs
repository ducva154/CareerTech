namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Profile")]
    public partial class Profile
    {
        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string PortfolioID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Position { get; set; }

        [Column(TypeName = "Longtext")]
        public string Desc { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        public int? Age { get; set; }

        public bool? Gender { get; set; }

        [Required]
        [StringLength(255)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Url_avatar { get; set; }

        [StringLength(255)]
        public string Instagram_url { get; set; }

        [StringLength(255)]
        public string Facebook_url { get; set; }

        [StringLength(255)]
        public string Twitter_url { get; set; }

        [StringLength(255)]
        public string Youtube_url { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
