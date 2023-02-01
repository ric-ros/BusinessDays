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
        wa,
        all //just as backup - shouldn't be used anytime
    }

    public class PublicHoliday
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("states")]
        public State[] States { get; set; } = new[] { State.all };
        [JsonPropertyName("countries")]
        public string[] Countries { get; set; } = new[] { "Australia" };

        public PublicHoliday() { }
        public PublicHoliday(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
        public PublicHoliday(string name, DateTime date, State[] states, string[] countries)
        {
            Name = name;
            Date = date;
            States = states;
            Countries = countries;
        }

    }

    class StateEnumConverter : JsonConverter<State>
    {
        public override State Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string state = reader.GetString();
            return (State)Enum.Parse(typeof(State), state, true);
        }

        public override void Write(Utf8JsonWriter writer, State value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
