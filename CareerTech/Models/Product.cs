namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string PortfolioID { get; set; }

        [Required]
        [StringLength(255)]
        public string Url_Image { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Skill { get; set; }

        [StringLength(255)]
        public string Domain { get; set; }

        public int? TeamSize { get; set; }

        [StringLength(255)]
        public string ProjectTech { get; set; }

        [StringLength(255)]
        public string WorkProces { get; set; }

        [StringLength(255)]
        public string Company { get; set; }

        [StringLength(255)]
        public string ProjectRole { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
