using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace college_project.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please select a major.")]
        [Display(Name = "Desired Major")]
        public string Major { get; set; }

        [Required(ErrorMessage = "Please upload your qualification certificate.")]
        [Display(Name = "Qualification Certificate")]
        public IFormFile QualificationImage { get; set; }

        [Required(ErrorMessage = "Please upload your identity document.")]
        [Display(Name = "Identity Document")]
        public IFormFile IdentityImage { get; set; }

        [Required(ErrorMessage = "Please upload a personal photo.")]
        [Display(Name = "Personal Photo")]
        public IFormFile PersonalPhoto { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the Transparency Charter to proceed.")]
        public bool AgreeToCharter { get; set; }
    }
}