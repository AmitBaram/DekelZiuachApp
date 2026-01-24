using DekelApp.Models;
using System.Linq;

namespace DekelApp.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidateAppData(AppData appData, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 0. At least one layer must be selected in both Tichum and Mikud
            var tl = appData.Tichum.Layers;
            if (!tl.Orthophoto && !tl.DTM && !tl.Buildings && !tl.Fences && !tl.Roads && !tl.Vegetation && !tl.PowerLines)
            {
                errorMessage = "At least one layer must be selected in the Tichum Layers page.";
                return false;
            }

            // 1. Tichum validation: File uploaded OR at least 3 valid coordinates
            int validTichumCount = appData.Tichum.Coordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);
            if (!appData.Tichum.IsFileUploaded && validTichumCount < 3)
            {
                errorMessage = "Tichum must have either an uploaded file or at least 3 valid coordinates.";
                return false;
            }

            // 2. Mikud validation: Each area must have coordinates/file AND at least one layer
            if (!appData.MikudAreas.Any())
            {
                errorMessage = "At least one Mikud area must be defined.";
                return false;
            }

            foreach (var area in appData.MikudAreas)
            {
                bool isFileUploaded = area.IsFileUploaded;
                int validCount = area.Coordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);
                
                if (!isFileUploaded && validCount < 3)
                {
                    errorMessage = $"Mikud area '{area.Name}' must have either an uploaded file or at least 3 valid coordinates.";
                    return false;
                }

                var ml = area.Layers;
                if (!ml.Orthophoto && !ml.DTM && !ml.Buildings && !ml.Fences && !ml.Roads && !ml.Vegetation && !ml.PowerLines)
                {
                    errorMessage = $"At least one layer must be selected for Mikud area '{area.Name}'.";
                    return false;
                }

                foreach (var coord in area.Coordinates)
                {
                    if (!coord.IsEastingValid || !coord.IsNorthingValid || !coord.IsZoneValid)
                    {
                        errorMessage = $"All entered coordinates in Mikud area '{area.Name}' must be valid.";
                        return false;
                    }
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
