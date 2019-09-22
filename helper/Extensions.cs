using System;
using Microsoft.AspNetCore.Http;

namespace FirstApp.API.helper
{
    public static class Extensions
    {
        public static void addApplicationError(this HttpResponse response,string msg){
            
            response.Headers.Add("Application-Error", msg);
            response.Headers.Add("Access-Control-Expose-Headers","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");

        }

        public static int CalculateAge(this DateTime dateTime){
            var age = DateTime.Today.Year - dateTime.Year;
            if(dateTime.AddYears(age) > DateTime.Today)
              age--; 
            
            return age;

        }
    }
}