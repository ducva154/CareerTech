namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Experience")]
    public partial class Experience
    {
        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string PortfolioID { get; set; }

        [Required]
        [StringLength(255)]
        public string Time { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string Detail { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
