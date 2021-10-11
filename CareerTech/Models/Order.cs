namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public Order()
        {
            Payments = new HashSet<Payment>();
        }

        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string SubscriptionID { get; set; }

        [Required]
        [StringLength(255)]
        public string UserID { get; set; }

        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; }

        public double TotalPrice { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }

        public virtual Subscription Subscription { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
