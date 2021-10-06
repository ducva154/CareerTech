namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Introduction")]
    public partial class Introduction
    {
        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string Detail { get; set; }

        [StringLength(255)]
        public string Url_Image { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}