using System.Text.Json;

namespace FileProcessing
{
    public class JsonProcessing
    {
        /// <summary>
        /// Читает данные с потока MemoryStream вида Json и конвертирует их в список объектов.
        /// </summary>
        /// <param name="stream">Поток MemoryStream с данными.</param>
        /// <returns>Коллекция объектов IceHill.</returns>
        public async Task<List<IceHill>> Read(MemoryStream stream)
        {
            stream.Position = 0;
            try
            {
                List<IceHill> iceHills = await JsonSerializer.DeserializeAsync<List<IceHill>>(stream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // Игнорировать размер при сопоставлении Json полей к полям объекта.
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }) ?? new List<IceHill>(); // Если что-то пойдет не так, вернет пустой список.
                return iceHills;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Записывает данные списка объектов в виде Json в MemoryStream.
        /// </summary>
        /// <param name="iceHills">Список объектов.</param>
        /// <returns>MemoryStream с данными.</returns>
        public async Task<MemoryStream> Write(List<IceHill> iceHills)
        {
            var memoryStream = new MemoryStream();
            await JsonSerializer.SerializeAsync(memoryStream, iceHills,
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
