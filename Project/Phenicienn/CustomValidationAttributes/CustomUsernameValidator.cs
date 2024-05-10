using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phenicienn.CustomValidationAttributes
{
    public class CustomUsernameValidator : ValidationAttribute
    {


        public CustomUsernameValidator()
        {   
        }

        public override bool IsValid(object value)
        {
            string strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                return Regex.IsMatch(strValue, "^[a-zA-Z_]");
            }
            return true;
        }
    }
}
