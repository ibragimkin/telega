using System.Reflection;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
namespace FileProcessing
{
    public class IceHill
    {

        [Name("global_id")]
        [JsonPropertyName("global_id")]
        public string GlobalId { get; set; } = "";

        [Name("ObjectName")]
        [JsonPropertyName("ObjectName")]
        public string ObjectName { get; set; } = "";

        [Name("NameWinter")]
        [JsonPropertyName("NameWinter")]
        public string NameWinter { get; set; } = "";

        [Name("PhotoWinter")]
        [JsonPropertyName("PhotoWinter")]
        public string PhotoWinter { get; set; } = "";

        [Name("AdmArea")]
        [JsonPropertyName("AdmArea")]
        public string AdmArea { get; set; } = "";

        [Name("District")]
        [JsonPropertyName("District")]
        public string District { get; set; } = "";

        [Name("Address")]
        [JsonPropertyName("Address")]
        public string Address { get; set; } = "";

        [Name("Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; } = "";

        [Name("WebSite")]
        [JsonPropertyName("WebSite")]
        public string WebSite { get; set; } = "";

        [Name("HelpPhone")]
        [JsonPropertyName("HelpPhone")]
        public string HelpPhone { get; set; } = "";

        [Name("HelpPhoneExtension")]
        [JsonPropertyName("HelpPhoneExtension")]
        public string HelpPhoneExtension { get; set; } = "";

        [Name("WorkingHoursWinter")]
        [JsonPropertyName("WorkingHoursWinter")]
        public string WorkingHoursWinter { get; set; } = "";

        [Name("ClarificationOfWorkingHoursWinter")]
        [JsonPropertyName("ClarificationOfWorkingHoursWinter")]
        public string ClarificationOfWorkingHoursWinter { get; set; } = "";

        [Name("HasEquipmentRental")]
        [JsonPropertyName("HasEquipmentRental")]
        public string HasEquipmentRental { get; set; } = "";

        [Name("EquipmentRentalComments")]
        [JsonPropertyName("EquipmentRentalComments")]
        public string EquipmentRentalComments { get; set; } = "";

        [Name("HasTechService")]
        [JsonPropertyName("HasTechService")]
        public string HasTechService { get; set; } = "";

        [Name("TechServiceComments")]
        [JsonPropertyName("TechServiceComments")]
        public string TechServiceComments { get; set; } = "";

        [Name("HasDressingRoom")]
        [JsonPropertyName("HasDressingRoom")]
        public string HasDressingRoom { get; set; } = "";

        [Name("HasEatery")]
        [JsonPropertyName("HasEatery")]
        public string HasEatery { get; set; } = "";

        [Name("HasToilet")]
        [JsonPropertyName("HasToilet")]
        public string HasToilet { get; set; } = "";

        [Name("HasWifi")]
        [JsonPropertyName("HasWifi")]
        public string HasWifi { get; set; } = "";

        [Name("HasCashMachine")]
        [JsonPropertyName("HasCashMachine")]
        public string HasCashMachine { get; set; } = "";

        [Name("HasFirstAidPost")]
        [JsonPropertyName("HasFirstAidPost")]
        public string HasFirstAidPost { get; set; } = "";

        [Name("HasMusic")]
        [JsonPropertyName("HasMusic")]
        public string HasMusic { get; set; } = "";

        [Name("UsagePeriodWinter")]
        [JsonPropertyName("UsagePeriodWinter")]
        public string UsagePeriodWinter { get; set; } = "";

        [Name("DimensionsWinter")]
        [JsonPropertyName("DimensionsWinter")]
        public string DimensionsWinter { get; set; } = "";

        [Name("Lighting")]
        [JsonPropertyName("Lighting")]
        public string Lighting { get; set; } = "";

        [Name("SurfaceTypeWinter")]
        [JsonPropertyName("SurfaceTypeWinter")]
        public string SurfaceTypeWinter { get; set; } = "";

        [Name("Seats")]
        [JsonPropertyName("Seats")]
        public string Seats { get; set; } = "";

        [Name("Paid")]
        [JsonPropertyName("Paid")]
        public string Paid { get; set; } = "";

        [Name("PaidComments")]
        [JsonPropertyName("PaidComments")]
        public string PaidComments { get; set; } = "";

        [Name("DisabilityFriendly")]
        [JsonPropertyName("DisabilityFriendly")]
        public string DisabilityFriendly { get; set; } = "";

        [Name("ServicesWinter")]
        [JsonPropertyName("ServicesWinter")]
        public string ServicesWinter { get; set; } = "";

        [Name("geoData")]
        [JsonPropertyName("geoData")]
        public string GeoData { get; set; } = "";

        [Name("geodata_center")]
        [JsonPropertyName("geodata_center")]
        public string GeodataCenter { get; set; } = "";

        [Name("geoarea")]
        [JsonPropertyName("geoarea")]
        public string Geoarea { get; set; } = "";
        
        public static int FindIndex(string name)
        {
            Type type = typeof(IceHill);
            PropertyInfo[] properties = type.GetProperties();
            properties = properties.Where(prop => prop.Name != "Item").ToArray(); // Индексатор считается как "Item", поэтому его исключаем.
            for (int i = 0; i < properties.Length; i++)
            {
                if (name == properties[i].Name) return i;
            }
            return 0;
        }
        public static string[] GetFieldNames()
        {
            Type type = typeof(IceHill);
            PropertyInfo[] properties = type.GetProperties();
            properties = properties.Where(prop => prop.Name != "Item").ToArray(); // Индексатор считается как "Item", поэтому его исключаем.
            string[] fieldNames = new string[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                fieldNames[i] = properties[i].Name;
            }
            return fieldNames;
        }

        public static string[][] GetAllFieldNamesKeyboard()
        {
            Type type = typeof(IceHill);
            PropertyInfo[] properties = type.GetProperties();
            properties = properties.Where(prop => prop.Name != "Item").ToArray(); // Индексатор считается как "Item", поэтому его исключаем.
            string[][] fieldNames = new string[properties.Length/3][];
            int k = 0;
            for (int i = 0; i < properties.Length/3; i++)
            {
                fieldNames[i] = new string[3];
                for (int j = 0; j < 3; j++)
                {
                    fieldNames[i][j] = properties[k].Name;
                    k++;
                }
            }
            return fieldNames;
        }
        public static string GetFieldNamesString()
        {
            string text = "";
            int i = 1;
            foreach (string field in GetFieldNames())
            {
                text += $"\n{i++}. "+ field;
            }
            return text;
        }

        /// <summary>
        /// Индексатор для сортировки и фильтрации.
        /// </summary>
        /// <param name="index">Индекс</param>
        /// <returns>Значение поля под нужным индексом.</returns>
        public string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return GlobalId;
                    case 1: return ObjectName;
                    case 2: return NameWinter;
                    case 3: return PhotoWinter;
                    case 4: return AdmArea;
                    case 5: return District;
                    case 6: return Address;
                    case 7: return Email;
                    case 8: return WebSite;
                    case 9: return HelpPhone;
                    case 10: return HelpPhoneExtension;
                    case 11: return WorkingHoursWinter;
                    case 12: return ClarificationOfWorkingHoursWinter;
                    case 13: return HasEquipmentRental;
                    case 14: return EquipmentRentalComments;
                    case 15: return HasTechService;
                    case 16: return TechServiceComments;
                    case 17: return HasDressingRoom;
                    case 18: return HasEatery;
                    case 19: return HasToilet;
                    case 20: return HasWifi;
                    case 21: return HasCashMachine;
                    case 22: return HasFirstAidPost;
                    case 23: return HasMusic;
                    case 24: return UsagePeriodWinter;
                    case 25: return DimensionsWinter;
                    case 26: return Lighting;
                    case 27: return SurfaceTypeWinter;
                    case 28: return Seats;
                    case 29: return Paid;
                    case 30: return PaidComments;
                    case 31: return DisabilityFriendly;
                    case 32: return ServicesWinter;
                    case 33: return GeoData;
                    case 34: return GeodataCenter;
                    case 35: return Geoarea;
                    default: return "";
                }
            }
        }
    }

}
