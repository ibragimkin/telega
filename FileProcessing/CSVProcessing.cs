using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;


namespace FileProcessing
{

    public class CSVProcessing
    {
        /// <summary>
        /// Читает данные с потока MemoryStream вида CSV и конвертирует их в список объектов.
        /// </summary>
        /// <param name="stream">Поток MemoryStream с данными.</param>
        /// <returns>Коллекция объектов IceHill.</returns>
        public async Task<List<IceHill>> Read(Stream stream)
        {

            using (StreamReader sr = new StreamReader(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);
                using (var csv = new CsvReader(sr, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";", // Установка разделителя полей
                    HasHeaderRecord = true,
                }))
                {

                    csv.Context.RegisterClassMap<IceHillCsvMapper>();

                    try
                    {
                        var result = await csv.GetRecordsAsync<IceHill>().ToListAsync();
                        return result;
                    }
                    catch { }
                }
            }
            return null;
        }

        /// <summary>
        /// Записывает данные списка объектов в виде CSV в MemoryStream.
        /// </summary>
        /// <param name="iceHills">Список объектов.</param>
        /// <returns>MemoryStream с данными.</returns>
        public MemoryStream Write(List<IceHill> iceHills)
        {
            var memoryStream = new MemoryStream();

            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            }))
            {
                csvWriter.Context.RegisterClassMap<IceHillCsvMapper>(); // Помогает распределять значения по нужным полям.
                csvWriter.WriteRecords(iceHills);
                writer.Flush();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
