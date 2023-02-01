
using BusinessDays.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessDays.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<PublicHoliday> PublicHolidays { get; set; }
        [BindProperty]

        public string Result { get; set; }
        [BindProperty]
        public NormalDay ChosenDay { get; set; } = new NormalDay
        {
            Date = DateTime.Now,
            States = new[] { State.all }
        };

        public IndexModel()
        {
        }

        public void OnGet()
        {
            RetrieveData();
        }

        private void RetrieveData()
        {
            string fileContents = System.IO.File.ReadAllText("data.json");

            var opts = new JsonSerializerOptions
            {
                Converters = { new StateEnumConverter() }
            };

            PublicHolidays = JsonSerializer.Deserialize<List<PublicHoliday>>(fileContents, opts) ?? new();
        }

        public IActionResult OnPostCheckYear()
        {
            // Get from DB
            RetrieveData();

            /**
             * 
             * The Request model would be like the prop ChosenDay. 
             * 
             * Something like: 
             * {
             *      Date            as DateTime,
             *      States          as States[] | string[],
             *      DaysToAdd       as double
             * }
             * 
             */

            var conditions = new Func<PublicHoliday, bool>(
                (publicHoliday) =>
                {
                    var checkRangeDay = ChosenDay.DaysToAdd > 0
                    ? publicHoliday.Date >= ChosenDay.Date && publicHoliday.Date <= ChosenDay.Date.AddDays(ChosenDay.DaysToAdd)
                    : ChosenDay.Date >= publicHoliday.Date && ChosenDay.Date.AddDays(ChosenDay.DaysToAdd) <= publicHoliday.Date;

                    var checkStates = ChosenDay.States.Any(x => publicHoliday.States.Any(k => k == x));


                    return checkRangeDay && checkStates;
                }
                );

            PublicHolidays = PublicHolidays?.Where(conditions).ToList() ?? new();


            return Page();
        }
    }
}