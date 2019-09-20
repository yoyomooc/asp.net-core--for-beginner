using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.CustomerUtil
{
    public class ValidEmailDomainAttribute:ValidationAttribute
    {

        private readonly string allowedDomain;


        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');

            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }



    }
}
