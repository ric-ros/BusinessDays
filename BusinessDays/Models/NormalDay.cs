namespace BusinessDays.Models
{

    public class NormalDay
    {
        public DateTime Date { get; set; }
        public State[] States { get; set; } = new[] { State.all };
        public double DaysToAdd { get; set; }
        public string[] Countries { get; set; } = new[] { "Australia" };

    }
}
