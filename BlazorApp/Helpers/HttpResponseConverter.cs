using Newtonsoft.Json;

namespace BlazorApp.Helpers
{
    public static class HttpResponseConverter
    {
        public static async Task<T?> StreamWithNewtonsoftJson<T>(HttpResponseMessage response)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();

            try
            {
                return serializer.Deserialize<T>(jsonReader);
            }
            catch (JsonReaderException)
            {
                Console.WriteLine("Invalid JSON.");
            }

            return default(T);
        }
    }
}
