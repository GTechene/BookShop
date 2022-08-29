using System.Reflection;

namespace BookShop.web.Data; 

public static class Countries {

    private static List<Country>? _countries;
    private static readonly SemaphoreSlim Lock = new(1);
    
    public static IEnumerable<Country> All()
    {
        if (_countries is not null)
        {
            return _countries;
        }

        Lock.Wait();
        
        if (_countries is not null)
        {
            Lock.Release();
            return _countries;
        }
        
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("BookShop.web.Data.countries.txt");

        if (stream is null)
        {
            throw new InvalidOperationException("Cannot find countries.txt");
        }

        var countries = new List<Country>();

        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();

            if (line is null || string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            var tokens = line.Split('|');

            var country = new Country(tokens[0].Trim(), tokens[1].Trim());
            
            countries.Add(country);
        }

        _countries = countries;

        Lock.Release();

        return countries;
    }
}

public record Country(string Code, string Name);