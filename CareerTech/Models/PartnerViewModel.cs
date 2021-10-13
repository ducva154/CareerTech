using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerTech.Models
{
    public class CompanyProfileViewModel
    {
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }


        [Required]
        [AllowHtml]
        [Display(Name = "Company Introduction")]
        public string Desc { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public string Url_Avatar { get; set; }

        public string Url_Background { get; set; }
    }


    public class RecruitmentViewModel
    {
        [Required(ErrorMessage = "Please choose Job Category ")]
        [Display(Name = "Job Category")]
        public string JobID { get; set; }

        [Required(ErrorMessage = "Title is not empty")]
        [StringLength(255)]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Address is not empty")]
        [StringLength(255)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Salary is not empty")]
        [StringLength(255)]
        public string Salary { get; set; }

        [Required(ErrorMessage = "Working form is not empty")]
        [StringLength(255)]
        [Display(Name = "Working Form")]
        public string Workingform { get; set; }

        [Required(ErrorMessage = "Amount form is not empty")]
        [StringLength(255)]
        public string Amount { get; set; }

        [Required(ErrorMessage = "Position form is not empty")]
        [StringLength(255)]
        public string Position { get; set; }

        [Required(ErrorMessage = "Experience form is not empty")]
        [StringLength(255)]
        public string Experience { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public bool? Gender { get; set; }

        [Required(ErrorMessage = " Detail Description is not empty")]
        [AllowHtml]
        [Display(Name = "Job description")]
        public string DetailDesc { get; set; }
    }

    public class CandidateViewModel
    {
        
    }

}