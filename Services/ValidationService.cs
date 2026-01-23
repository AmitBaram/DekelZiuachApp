using DekelApp.Models;
using System.Linq;

namespace DekelApp.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidateAppData(AppData appData, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 0. At least one layer must be selected
            var l = appData.Layers;
            if (!l.Orthophoto && !l.DTM && !l.Buildings && !l.Fences && !l.Roads && !l.Vegetation && !l.PowerLines)
            {
                errorMessage = "At least one layer must be selected in the Layers page.";
                return false;
            }

            // 1. Tichum validation: File uploaded OR at least 3 valid coordinates
            int validTichumCount = appData.TichumCoordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);
            if (!appData.IsTichumFileUploaded && validTichumCount < 3)
            {
                errorMessage = "Tichum must have either an uploaded file or at least 3 valid coordinates.";
                return false;
            }

            // 2. Mikud validation: Empty OR File uploaded OR at least 3 valid coordinates
            bool isMikudEmpty = appData.MikudCoordinates.Count == 0;
            int validMikudCount = appData.MikudCoordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);
            if (!isMikudEmpty && !appData.IsMikudFileUploaded && validMikudCount < 3)
            {
                errorMessage = "Mikud must be either empty, have an uploaded file, or at least 3 valid coordinates.";
                return false;
            }

            // 3. All filled UTM coordinates in Tichum and Mikud must be valid (if they exist)
            foreach (var coord in appData.TichumCoordinates)
            {
                if (!coord.IsEastingValid || !coord.IsNorthingValid || !coord.IsZoneValid)
                {
                    errorMessage = "All entered coordinates in Tichum must be valid.";
                    return false;
                }
            }

            foreach (var coord in appData.MikudCoordinates)
            {
                if (!coord.IsEastingValid || !coord.IsNorthingValid || !coord.IsZoneValid)
                {
                    errorMessage = "All entered coordinates in Mikud must be valid.";
                    return false;
                }
            }

            // 4. All UTM coordinates in Yeadim are valid
            foreach (var target in appData.YeadimTargets)
            {
                if (string.IsNullOrWhiteSpace(target.Name) || string.IsNullOrWhiteSpace(target.Easting) || string.IsNullOrWhiteSpace(target.Northing) || string.IsNullOrWhiteSpace(target.Zone))
                {
                    errorMessage = "All fields in Yeadim targets must be filled.";
                    return false;
                }
            }

            // 3. At least one format is selected
            var f = appData.Formats;
            if (!f.CDB && !f.MFT && !f.VBS3 && !f.VBS4)
            {
                errorMessage = "At least one format must be selected.";
                return false;
            }

            // 4. All fields in General Info filled except General Notes
            var gi = appData.GeneralInfo;
            if (string.IsNullOrWhiteSpace(gi.UnitName) || string.IsNullOrWhiteSpace(gi.OperationName) || gi.Date == null)
            {
                errorMessage = "Unit Name, Operation Name, and Date are required in General Info.";
                return false;
            }

            return true;
        }
    }
}
