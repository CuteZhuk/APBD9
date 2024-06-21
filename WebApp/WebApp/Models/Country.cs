namespace WebApp.Models;

using System.Collections.Generic;

public class Country
{
    public Country()
    {
        CountryTrips = new HashSet<CountryTrip>();
    }

    public int IdCountry { get; set; }
    public string Name { get; set; }

    public virtual ICollection<CountryTrip> CountryTrips { get; set; }
}