using SuperPanel.Abstractions.Actions;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SuperPanel.Engines.Actions
{
    public class JsonSerialization : IJsonSerialization
    {
        private JsonSerializerOptions _deserializeOptions;
        public JsonSerialization()
        {
            _deserializeOptions = DeserializeOptions();
        }

        public async Task<T> Deserialize<T>(Stream stream)
        {
            T obj = await JsonSerializer.DeserializeAsync<T>(stream, _deserializeOptions);

            return obj;
        }
        public T Deserialize<T>(string str)
        {
            T obj = JsonSerializer.Deserialize<T>(str, _deserializeOptions);

            return obj;
        }
        private JsonSerializerOptions DeserializeOptions()
        {
            var opts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            opts.Converters.Add(new JsonStringEnumConverter());

            return opts;
        }
    }
}
