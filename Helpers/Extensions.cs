using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectingApp.API.Helpers
{
    // as this is extension class so we do not want to create its object
    public static class Extensions
    {
        // the message passed will be set into headers
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            // here we set the message in headers
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Header");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        // for add the calculate age method in datetime class
        public static int CalculateAge(this DateTime theDateTime)
        {
            // thedatetime is the birthdate of user
            // age has the age in numbers
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
