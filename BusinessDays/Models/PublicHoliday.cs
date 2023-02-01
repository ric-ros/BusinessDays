using System.Formats.Asn1;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusinessDays.Models
{
    public enum State
    {
        act,
        nsw,
        nt,
        qld,
        sa,
        tas,
        vic,
        wa
    }

    public class PublicHoliday
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("states")]
        public State[]? States { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; } = "AU";
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("month")]
        public int Month { get; set; }
        [JsonPropertyName("day")]
        public int Day { get; set; }
        [JsonPropertyName("date")]
        public DateTimeOffset Date => new(new DateTime(Year, Month, Day));

        public PublicHoliday() { }
        public PublicHoliday(string name, DateTime date)
        {
            Name = name;
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }
        public PublicHoliday(string name, DateTime date, string country, State[]? states)
        {
            Name = name;
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
            States = states;
            Country = country;
        }

    }

    class StateEnumConverter : JsonConverter<State>
    {
        public override State Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (State)Enum.Parse(typeof(State), reader.GetString(), true);
        }

        public override void Write(Utf8JsonWriter writer, State value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLowerInvariant());
        }
    }
}
