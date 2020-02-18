using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Dtos.Validations;
using Vidly.Models;

namespace Vidly.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field Name is required")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Min18YearsIfAMember]
        public DateTime? BirthDate { get; set; }

        public bool IsSubcribedToNewsletter { get; set; }

        public byte MembershipTypeId { get; set; }
    }
}