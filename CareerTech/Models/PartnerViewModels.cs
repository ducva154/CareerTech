﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerTech.Models
{
    public class ProfileViewModels
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được để trống")]
        public string Phone { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Không được để trống")]
        public string Email { get; set; }
    }




}