using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }
        public bool IsSubcribedToNewsletter { get; set; }
        public byte MembershipTypeId { get; set; }
        public MembershipType MembershipType { get; set; }
    }
}