using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CareerTech.Models
{
    public class PartnerManagementViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyID { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Url_Img { get; set; }
        public string Status { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
    public class orderDetailViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Required]
        public string CompanyID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string SubscriptionName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public double TotalPrice { get; set; }

    }
    public class PaymentViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string UserID { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }
        [Required]
        public string SubscriptionName { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }

    }
}