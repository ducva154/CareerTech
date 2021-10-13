using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CareerTech.Models
{
    public class Candidate
    {
        [Key]
        [StringLength(255)]
        public string ID { get; set; }


        [Column(TypeName = "date")]
        public DateTime DateApply { get; set; }

        [Required]
        [StringLength(255)]
        public string UserID { get; set; }

        [Required]
        [StringLength(255)]
        public string RecruitmentID { get; set; }

    
        [StringLength(255)]
        public string Status { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Recruitment Recruitment { get; set; }


    }
}