using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessing
{
    /// <summary>
    /// Класс-маппер для сопоставления столбцов CSV с полями класса IceHill.
    /// </summary>
    internal class IceHillCsvMapper : ClassMap<IceHill>
    {
        internal IceHillCsvMapper()
        {
            Map(m => m.GlobalId).Name("global_id");
            Map(m => m.ObjectName).Name("ObjectName");
            Map(m => m.NameWinter).Name("NameWinter");
            Map(m => m.PhotoWinter).Name("PhotoWinter");
            Map(m => m.AdmArea).Name("AdmArea");
            Map(m => m.District).Name("District");
            Map(m => m.Address).Name("Address");
            Map(m => m.Email).Name("Email");
            Map(m => m.WebSite).Name("WebSite");
            Map(m => m.HelpPhone).Name("HelpPhone");
            Map(m => m.HelpPhoneExtension).Name("HelpPhoneExtension");
            Map(m => m.WorkingHoursWinter).Name("WorkingHoursWinter");
            Map(m => m.ClarificationOfWorkingHoursWinter).Name("ClarificationOfWorkingHoursWinter");
            Map(m => m.HasEquipmentRental).Name("HasEquipmentRental");
            Map(m => m.EquipmentRentalComments).Name("EquipmentRentalComments");
            Map(m => m.HasTechService).Name("HasTechService");
            Map(m => m.TechServiceComments).Name("TechServiceComments");
            Map(m => m.HasDressingRoom).Name("HasDressingRoom");
            Map(m => m.HasEatery).Name("HasEatery");
            Map(m => m.HasToilet).Name("HasToilet");
            Map(m => m.HasWifi).Name("HasWifi");
            Map(m => m.HasCashMachine).Name("HasCashMachine");
            Map(m => m.HasFirstAidPost).Name("HasFirstAidPost");
            Map(m => m.HasMusic).Name("HasMusic");
            Map(m => m.UsagePeriodWinter).Name("UsagePeriodWinter");
            Map(m => m.DimensionsWinter).Name("DimensionsWinter");
            Map(m => m.Lighting).Name("Lighting");
            Map(m => m.SurfaceTypeWinter).Name("SurfaceTypeWinter");
            Map(m => m.Seats).Name("Seats");
            Map(m => m.Paid).Name("Paid");
            Map(m => m.PaidComments).Name("PaidComments");
            Map(m => m.DisabilityFriendly).Name("DisabilityFriendly");
            Map(m => m.ServicesWinter).Name("ServicesWinter");
            Map(m => m.GeoData).Name("geoData");
            Map(m => m.GeodataCenter).Name("geodata_center");
            Map(m => m.Geoarea).Name("geoarea");
        }
    }
}
