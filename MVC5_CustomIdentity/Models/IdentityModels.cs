using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using MVC5_CustomIdentity.Controllers;

namespace MVC5_CustomIdentity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class EmailUniqueAttribute : ValidationAttribute
    {
      
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                //create instance of object of type ApplicationDbContext to access database
                ApplicationDbContext db = new ApplicationDbContext();
               // UserManager<ApplicationUser> um = new UserManager<ApplicationUser>()

           
                
                string email = value.ToString();
            



                 
                var query = db.Users
                  .Where(s => s.Email.ToLower()
                       .Equals(email.ToLower()))
                  .Select(
                       s => new { Id = s.Id, Email = s.Email }
                  );
             
               

                if (query.Count() > 0)
                {
                    //return error
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            

 

                //if program reach this point, we know that all requirements were met,
                //therefore returning Success
                return ValidationResult.Success;
            }


            return base.IsValid(value, validationContext);
        }

    }


    public class EmailRegexAttribute : ValidationAttribute
    {

       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                //create List of Regex
                List<Regex> regexList = new List<Regex>{
                          //and add object contain each expression from the resource
                          new Regex(EmailRegexList.NotWhiteSpace, RegexOptions.IgnoreCase),
                          new Regex(EmailRegexList.EmailDomainCheck, RegexOptions.IgnoreCase),
                          new Regex(EmailRegexList.EmailRegexSimple, RegexOptions.IgnoreCase),
                          new Regex(EmailRegexList.EmailRegexRFC5322, RegexOptions.IgnoreCase)
                };

                //iterate through each expression from the list
                foreach (Regex r in regexList)
                {
                    //if supplied value does not match currently checked regex
                    if (!r.IsMatch(value.ToString().ToLower()))
                    {
                        //create errorMessage
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        //and immediately exit loop returning Validation result
                        //because 1 requirement is not met, which is enough for error
                        return new ValidationResult(errorMessage);
                    }
                }
                //if program reach this point, we know that all requirements were met,
                //therefore returning Success
                return ValidationResult.Success;
            }

            return base.IsValid(value, validationContext);
        }

    }



    public class ApplicationUser : IdentityUser
    {

        [Required]
        [MaxLength(200)]
        [Display(Name = "E-mail")]
        [EmailRegex]
        [EmailUnique]
        public string Email { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}