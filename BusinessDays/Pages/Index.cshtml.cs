
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
        private readonly ILogger<IndexModel> _logger;
        [BindProperty]
        public List<PublicHoliday> PublicHolidays { get; set; }
        [BindProperty]
        public double DaysToAdd { get; set; }

        [BindProperty]
        public string Result { get; set; }
        [BindProperty]
        public NormalDay ChosenDay { get; set; } = new NormalDay
        {
            Date = DateTime.Now,
            States = new[] { State.all }
        };

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            RetrieveData();
        }

        private void RetrieveData(double daysRange = 0)
        {

            string fileContents = System.IO.File.ReadAllText("data.json");

            var opts = new JsonSerializerOptions
            {
                Converters = { new StateEnumConverter() }
            };

            PublicHolidays = JsonSerializer.Deserialize<List<PublicHoliday>>(fileContents, opts);


            var whereFunc = new Func<PublicHoliday, bool>(
                (publicHoliday) =>
                    {
                        var first = daysRange > 0
                        ? publicHoliday.Date >= ChosenDay.Date && publicHoliday.Date <= ChosenDay.Date.AddDays(daysRange)
                        : ChosenDay.Date >= publicHoliday.Date && ChosenDay.Date.AddDays(daysRange) <= publicHoliday.Date;

                        var second = ChosenDay.States.Any(x => publicHoliday.States.Any(k => k == x));


                        return first && second;
                    }
                );

            PublicHolidays = PublicHolidays?.Where(whereFunc).ToList();
        }

        public IActionResult OnPost()
        {
            RetrieveData(DaysToAdd);

            var result =
                PublicHolidays.FirstOrDefault(x =>
                    x.Date.ToShortDateString() == ChosenDay.Date.ToShortDateString() &&
                    (x.States.Any(x => ChosenDay.States.Any(k => k == x)) || x.States.Contains(State.all))
                );

            return Page();
        }

        public IActionResult OnPostCheckYear()
        {
            RetrieveData(DaysToAdd);


            return Page();
        }
    }
}