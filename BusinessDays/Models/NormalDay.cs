namespace BusinessDays.Models
{

    public class NormalDay
    {
        public DateTime Date { get; set; }
        public State[]? States { get; set; }
        public double DaysToAdd { get; set; }
        public string Country { get; set; } = "AU";

    }
}
