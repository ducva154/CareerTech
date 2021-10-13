namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Subscription")]
    public partial class Subscription
    {
        public Subscription()
        {
            Orders = new HashSet<Order>();
        }

        [StringLength(255)]
        public string ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public float? Price { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public int? Period { get; set; }

        [Column(TypeName = "Longtext")]
        [Required]
        public string DetailDesc { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
