namespace BusinessDays.Models
{

    public class NormalDay
    {
        public DateTime Date { get; set; }
        public int DayRange { get; set; }
        public State[] States { get; set; } = new[] { State.all };
        public string[] Countries { get; set; } = new[] { "Australia" };

    }
}
