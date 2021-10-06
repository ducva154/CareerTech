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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
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

        [Column(TypeName = "date")]
        public DateTime? Period { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
