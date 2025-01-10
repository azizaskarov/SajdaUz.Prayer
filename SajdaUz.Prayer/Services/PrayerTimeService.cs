namespace SajdaUz.Prayer.Services;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class PrayerTimeService
{
    private readonly HttpClient _httpClient;

    public PrayerTimeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PrayerTimes> GetPrayerTimesByCityAsync(string city, string country)
    {
        var response = await _httpClient.GetStringAsync($"https://api.aladhan.com/v1/timingsByCity?city={city}&country={country}&method=2");
        var prayerTimes = JsonSerializer.Deserialize<JsonElement>(response);

        return MapPrayerTimes(prayerTimes);
    }

    public async Task<PrayerTimes> GetPrayerTimesByCoordinatesAsync(double latitude, double longitude)
    {
        var response = await _httpClient.GetStringAsync($"https://api.aladhan.com/v1/timings?latitude={latitude}&longitude={longitude}&method=2");
        var prayerTimes = JsonSerializer.Deserialize<JsonElement>(response);

        return MapPrayerTimes(prayerTimes);
    }

    public async Task<HijriDate> GetHijriDateByCoordinatesAsync(double latitude, double longitude)
    {
        var response = await _httpClient.GetStringAsync($"https://api.aladhan.com/v1/timings?latitude={latitude}&longitude={longitude}&method=2");
        var data = JsonSerializer.Deserialize<JsonElement>(response);

        var hijriDate = data.GetProperty("data").GetProperty("date").GetProperty("hijri");

        return new HijriDate
        {
            Day = hijriDate.GetProperty("day").GetString()!,
            Month = hijriDate.GetProperty("month").GetProperty("en").GetString()!,
            Year = hijriDate.GetProperty("year").GetString()!
        };
    }

    private PrayerTimes MapPrayerTimes(JsonElement prayerTimes)
    {
        return new PrayerTimes
        {
            Fajr = prayerTimes.GetProperty("data").GetProperty("timings").GetProperty("Fajr").GetString()!,
            Dhuhr = prayerTimes.GetProperty("data").GetProperty("timings").GetProperty("Dhuhr").GetString()!,
            Asr = prayerTimes.GetProperty("data").GetProperty("timings").GetProperty("Asr").GetString()!,
            Maghrib = prayerTimes.GetProperty("data").GetProperty("timings").GetProperty("Maghrib").GetString()!,
            Isha = prayerTimes.GetProperty("data").GetProperty("timings").GetProperty("Isha").GetString()!,
            Sunrise = prayerTimes.GetProperty("data").GetProperty("timings").GetProperty("Sunrise").GetString()!
        };
    }
}

public class PrayerTimes
{
    public string Fajr { get; set; }
    public string Dhuhr { get; set; }
    public string Asr { get; set; }
    public string Maghrib { get; set; }
    public string Isha { get; set; }
    public string Sunrise { get; set; }
}

public class HijriDate
{
    public string Day { get; set; }
    public string Month { get; set; }
    public string Year { get; set; }
}
