using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace testankiety.Models
{
    public class UserAcc
    {

        public int UserAccID { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Survay> Survays { get; set; }
    }
}