namespace CareerTech.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Skill
    {
        [StringLength(255)]
        public string ID { get; set; }

        [Required]
        [StringLength(255)]
        public string PortfolioID { get; set; }

        [Required]
        [StringLength(255)]
        public string SkillName { get; set; }

        public int SkillLevel { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
