using System;
using System.Collections.Generic;
using System.Text;

namespace Side_Hustle_Manager.Services
{
    public static class LocationService
    {
        public static async Task<Location?> GetCurrentLocation()
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                return location;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
