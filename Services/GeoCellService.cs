using DekelApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DekelApp.Services
{
    public class GeoCellService : IGeoCellService
    {
        public List<string> CalculateGeoCells(ObservableCollection<TichumAreaModel> tichumAreas)
        {
            var allGeoCells = new HashSet<string>();

            foreach (var area in tichumAreas)
            {
                if (area.Coordinates.Count == 0)
                    continue;

                var latLonPoints = new List<(double Lat, double Lon)>();

                foreach (var coord in area.Coordinates)
                {
                    if (area.CoordinateSystem == CoordinateSystemType.Geographic)
                    {
                        if (double.TryParse(coord.Latitude, out double lat) &&
                            double.TryParse(coord.Longitude, out double lon))
                        {
                            latLonPoints.Add((lat, lon));
                        }
                    }
                    else // UTM
                    {
                        if (double.TryParse(coord.Easting, out double easting) &&
                            double.TryParse(coord.Northing, out double northing) &&
                            !string.IsNullOrWhiteSpace(coord.Zone))
                        {
                            var (lat, lon) = UtmToLatLon(easting, northing, coord.Zone);
                            latLonPoints.Add((lat, lon));
                        }
                    }
                }

                if (latLonPoints.Count == 0)
                    continue;

                // Add the GeoCell for each actual coordinate point
                foreach (var point in latLonPoints)
                {
                    int latCell = (int)Math.Floor(point.Lat);
                    int lonCell = (int)Math.Floor(point.Lon);
                    allGeoCells.Add(FormatGeoCell(latCell, lonCell));
                }
            }

            return allGeoCells.OrderBy(c => c).ToList();
        }

        private static string FormatGeoCell(int lat, int lon)
        {
            string ns = lat >= 0 ? "N" : "S";
            string ew = lon >= 0 ? "E" : "W";
            return $"{Math.Abs(lat):D2}{ns} {Math.Abs(lon):D3}{ew}";
        }

        private static (double Lat, double Lon) UtmToLatLon(double easting, double northing, string zone)
        {
            // Parse zone number and letter
            int zoneNumber = 0;
            bool isNorthern = true;
            for (int i = 0; i < zone.Length; i++)
            {
                if (char.IsLetter(zone[i]))
                {
                    zoneNumber = int.Parse(zone.Substring(0, i));
                    char zoneLetter = char.ToUpper(zone[i]);
                    isNorthern = zoneLetter >= 'N';
                    break;
                }
            }

            // WGS84 ellipsoid parameters
            const double a = 6378137.0;
            const double f = 1.0 / 298.257223563;
            const double b = a * (1.0 - f);
            const double e2 = (a * a - b * b) / (a * a);
            const double e_prime2 = (a * a - b * b) / (b * b);
            const double k0 = 0.9996;

            double x = easting - 500000.0;
            double y = isNorthern ? northing : northing - 10000000.0;

            double lonOrigin = (zoneNumber - 1) * 6.0 - 180.0 + 3.0;

            double M = y / k0;
            double mu = M / (a * (1.0 - e2 / 4.0 - 3.0 * e2 * e2 / 64.0 - 5.0 * e2 * e2 * e2 / 256.0));

            double e1 = (1.0 - Math.Sqrt(1.0 - e2)) / (1.0 + Math.Sqrt(1.0 - e2));

            double phi1 = mu
                + (3.0 * e1 / 2.0 - 27.0 * e1 * e1 * e1 / 32.0) * Math.Sin(2.0 * mu)
                + (21.0 * e1 * e1 / 16.0 - 55.0 * e1 * e1 * e1 * e1 / 32.0) * Math.Sin(4.0 * mu)
                + (151.0 * e1 * e1 * e1 / 96.0) * Math.Sin(6.0 * mu)
                + (1097.0 * e1 * e1 * e1 * e1 / 512.0) * Math.Sin(8.0 * mu);

            double sinPhi1 = Math.Sin(phi1);
            double cosPhi1 = Math.Cos(phi1);
            double tanPhi1 = Math.Tan(phi1);

            double N1 = a / Math.Sqrt(1.0 - e2 * sinPhi1 * sinPhi1);
            double T1 = tanPhi1 * tanPhi1;
            double C1 = e_prime2 * cosPhi1 * cosPhi1;
            double R1 = a * (1.0 - e2) / Math.Pow(1.0 - e2 * sinPhi1 * sinPhi1, 1.5);
            double D = x / (N1 * k0);

            double lat = phi1
                - (N1 * tanPhi1 / R1) * (
                    D * D / 2.0
                    - (5.0 + 3.0 * T1 + 10.0 * C1 - 4.0 * C1 * C1 - 9.0 * e_prime2) * D * D * D * D / 24.0
                    + (61.0 + 90.0 * T1 + 298.0 * C1 + 45.0 * T1 * T1 - 252.0 * e_prime2 - 3.0 * C1 * C1) * D * D * D * D * D * D / 720.0
                );

            double lon = (
                    D
                    - (1.0 + 2.0 * T1 + C1) * D * D * D / 6.0
                    + (5.0 - 2.0 * C1 + 28.0 * T1 - 3.0 * C1 * C1 + 8.0 * e_prime2 + 24.0 * T1 * T1) * D * D * D * D * D / 120.0
                ) / cosPhi1;

            // Convert from radians to degrees
            double latDeg = lat * 180.0 / Math.PI;
            double lonDeg = lonOrigin + lon * 180.0 / Math.PI;

            return (latDeg, lonDeg);
        }
    }
}
