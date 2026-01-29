using System;
using System.ComponentModel.DataAnnotations;

namespace college_project.Models
{
    public class StudentRegistration
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Desired Major")]
        public string Major { get; set; }

        [Display(Name = "Qualification Certificate")]
        public string QualificationImagePath { get; set; }

        [Display(Name = "Identity Document")]
        public string IdentityImagePath { get; set; }

        [Display(Name = "Personal Photo")]
        public string PersonalPhotoPath { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public bool IsTransparencyCharterAgreed { get; set; }
    }
}