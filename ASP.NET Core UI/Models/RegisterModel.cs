﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Services.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_UI.Models
{
    public class RegisterModel:IValidatableObject
    {
        [EmailAddress(ErrorMessage ="Adresa nu este valida")]
        [MaxLength(100,ErrorMessage ="Adresa are cel mult 100 de caractere")]
        [Required(ErrorMessage ="Campul este obligatoriu")]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "Prenumele are cel mult 100 de caractere")]
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Surname { get; set; }

        [MaxLength(100,ErrorMessage ="Numele are cel mult 100 de caractere")]
        [Required(ErrorMessage ="Campul este obligatoriu")]
        public string Name { get; set; }

        [MaxLength(100,ErrorMessage ="Parola are cel mult 100 de caractere")]
        [Required(ErrorMessage = "Campul este obligatoriu")]
        public string Password { get; set; }
        public int LocalityId { get; set; }
        public List<SelectListItem> Localities { get; set; }

        public int CountyId { get; set; }
        public List<SelectListItem> Counties { get; set; }

        public IEnumerable<ValidationResult>Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();
            var service = validationContext.GetService(typeof(UserAccountService)) as UserAccountService;
            var emailExists = service.EmailExists(Email);
            if (emailExists)
                result.Add(new ValidationResult("Email-ul exisa deja", new List<string> { nameof(Email) }));
            return result;

        }

    }
}
