﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BudgetCalculatorFrontEnd.Models
{
    public class UsersModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}