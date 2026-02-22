using DekelApp.Models;
using System.Linq;

namespace DekelApp.Services
{
    public class ValidationService : IValidationService
    {
        public bool ValidateAppData(AppData appData, out string errorMessage)
        {
            errorMessage = string.Empty;

            // 0 & 1 & 2. Tichum validation: At least one area must be defined. Each area must have coordinates/file AND at least one layer
            if (!appData.TichumAreas.Any())
            {
                errorMessage = "At least one Tichum area must be defined.";
                return false;
            }

            foreach (var area in appData.TichumAreas)
            {
                bool isFileUploaded = area.IsFileUploaded;
                int validCount;
                
                if (area.CoordinateSystem == CoordinateSystemType.UTM)
                {
                    validCount = area.Coordinates.Count(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid);
                }
                else
                {
                    validCount = area.Coordinates.Count(c => c.IsLatitudeValid && c.IsLongitudeValid);
                }
                
                if (!isFileUploaded && validCount < 3)
                {
                    errorMessage = $"Tichum area '{area.Name}' must have either an uploaded file or at least 3 valid coordinates.";
                    return false;
                }


                foreach (var coord in area.Coordinates)
                {
                    bool isValid;
                    if (area.CoordinateSystem == CoordinateSystemType.UTM)
                    {
                        isValid = coord.IsEastingValid && coord.IsNorthingValid && coord.IsZoneValid;
                    }
                    else
                    {
                        isValid = coord.IsLatitudeValid && coord.IsLongitudeValid;
                    }

                    if (!isValid)
                    {
                        errorMessage = $"All entered coordinates in Tichum area '{area.Name}' must be valid.";
                        return false;
                    }
                }

                bool hasDuplicates = false;
                if (area.CoordinateSystem == CoordinateSystemType.UTM)
                {
                    var validList = area.Coordinates.Where(c => c.IsEastingValid && c.IsNorthingValid && c.IsZoneValid).ToList();
                    var distinctCount = validList.Select(c => $"{c.Easting?.Trim()}_{c.Northing?.Trim()}_{c.Zone?.Trim()}").Distinct().Count();
                    hasDuplicates = distinctCount < validList.Count;
                }
                else
                {
                    var validList = area.Coordinates.Where(c => c.IsLatitudeValid && c.IsLongitudeValid).ToList();
                    var distinctCount = validList.Select(c => $"{c.Latitude?.Trim()}_{c.Longitude?.Trim()}").Distinct().Count();
                    hasDuplicates = distinctCount < validList.Count;
                }

                if (hasDuplicates)
                {
                    errorMessage = $"Tichum area '{area.Name}' must not contain identical coordinates.";
                    return false;
                }
            }

            // 3. All coordinates in Yeadim are valid
            foreach (var target in appData.YeadimTargets)
            {
                if (string.IsNullOrWhiteSpace(target.Name))
                {
                    errorMessage = "All target names in Yeadim must be filled.";
                    return false;
                }

                bool coordsValid;
                if (appData.YeadimCoordinateSystem == CoordinateSystemType.UTM)
                {
                    coordsValid = !string.IsNullOrWhiteSpace(target.Easting) && 
                                  !string.IsNullOrWhiteSpace(target.Northing) && 
                                  !string.IsNullOrWhiteSpace(target.Zone) &&
                                  target.IsEastingValid && target.IsNorthingValid && target.IsZoneValid;
                }
                else
                {
                    coordsValid = !string.IsNullOrWhiteSpace(target.Latitude) && 
                                  !string.IsNullOrWhiteSpace(target.Longitude) &&
                                  target.IsLatitudeValid && target.IsLongitudeValid;
                }

                if (!coordsValid)
                {
                    errorMessage = "All coordinate fields in Yeadim targets must be filled and valid.";
                    return false;
                }
            }

            bool hasYeadimDuplicates = false;
            if (appData.YeadimCoordinateSystem == CoordinateSystemType.UTM)
            {
                var validYeadim = appData.YeadimTargets.Where(t => t.IsEastingValid && t.IsNorthingValid && t.IsZoneValid).ToList();
                var distinctCount = validYeadim.Select(t => $"{t.Easting?.Trim()}_{t.Northing?.Trim()}_{t.Zone?.Trim()}").Distinct().Count();
                hasYeadimDuplicates = distinctCount < validYeadim.Count;
            }
            else
            {
                var validYeadim = appData.YeadimTargets.Where(t => t.IsLatitudeValid && t.IsLongitudeValid).ToList();
                var distinctCount = validYeadim.Select(t => $"{t.Latitude?.Trim()}_{t.Longitude?.Trim()}").Distinct().Count();
                hasYeadimDuplicates = distinctCount < validYeadim.Count;
            }

            if (hasYeadimDuplicates)
            {
                errorMessage = "Yeadim targets must not contain identical coordinates.";
                return false;
            }

            // 4. At least one format is selected
            var f = appData.Formats;
            if (!f.CDB && !f.MFT && !f.TXP)
            {
                errorMessage = "At least one format must be selected.";
                return false;
            }

            // 5. All fields in General Info filled except General Notes
            var gi = appData.GeneralInfo;
            if (string.IsNullOrWhiteSpace(gi.UnitName) || string.IsNullOrWhiteSpace(gi.OperationName) || gi.Date == null)
            {
                errorMessage = "Unit Name, Operation Name, and Date are required in General Info.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(gi.ClassificationLevel))
            {
                errorMessage = "יש לבחור רמת סיווג במידע הכללי.";
                return false;
            }

            return true;
        }
    }
}
