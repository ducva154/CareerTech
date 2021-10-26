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
        public string RecId { get; set; }

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
        public string CandidateID { get; set; }


        public string FullName { get; set; }


        public string Email { get; set; }


        public string Phone { get; set; }


        public string PortfolioID { get; set; }


        public string RecruitmentID { get; set; }

        public string UserID { get; set; }


        public DateTime DateApply { get; set; }

        public string Status { get; set; }


    }

    //public class  CandidateFilter{
    //    public CandidateFilterViewModel Cadidate { get; set; }
    //    public FilterViewModel Filter { get; set; }
    //}

    public class CandidateFilterViewModel
    {
        public string FullName { get; set; }

        public string Career { get; set; }

        public string Address { get; set; }

        public int? Age { get; set; }

        public bool? Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string PortfolioID { get; set; }
    }


    public class FilterViewModel
    {
        public string Address { get; set; }

        public string Career { get; set; }

        public bool? Gender { get; set; }
    }

    public class SearchRecruitmentViewModel
    {
        [Required]
        [StringLength(255)]
        public string RecruitmentID { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyProfileID { get; set; }

        [Required]
        [StringLength(255)]
        public string JobID { get; set; }

        public string Url_Avatar { get; set; }

        [Required()]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string Salary { get; set; }

        [Required]
        [StringLength(255)]
        public string JobName { get; set; }
    }

    public class DashboardRecruitmentViewModel
    {
        [Required]
        [StringLength(255)]
        public string RecruitmentID { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyProfileID { get; set; }

        [Required()]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(255)]
        public string Status { get; set; }
    }


}