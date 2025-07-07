using System.ComponentModel.DataAnnotations;

namespace college_project.Models
{
    public class Certificate
    {
        public int Id { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public string CertificateImagePath { get; set; }
    }
}
