// Disables warnings on Parse methods hiding the one from the ISBN abstract record. It is on purpose, to have a helper on the main record that will call the derived records' Parse while keeping the visibility of the derived records' Parse to public, making them usable by calling code. TL;DR : this is to avoid using different naming and introducing confusion.
// ReSharper disable MemberHidesStaticFromOuterClass
#pragma warning disable CS0108, CS0114

namespace BookShop.domain;

public abstract record ISBN {

    // TODO : implement checksum algo
    // TODO : instead of throwing exceptions here maybe we could use a Result
    private static string Clean(string isbn)
    {
        return isbn.Replace("-", "");
    }

    /// <summary>
    ///     Convert a string to an isbn
    /// </summary>
    /// <param name="isbn"></param>
    /// <returns>An instance of ISBN.ISBN10 or ISBN.ISBN13</returns>
    /// <exception cref="InvalidIsbn">Parse isbn is invalid</exception>
    public static ISBN Parse(string isbn)
    {
        var cleanedIsbn = Clean(isbn);

        return cleanedIsbn.Length switch
        {
            10 => ISBN10.Parse(cleanedIsbn),
            13 => ISBN13.Parse(cleanedIsbn),
            _ => throw new InvalidIsbn("String is too short, only ISBN10 and ISBN13 are supported")
        };
    }

    public record ISBN10(int RegistrationGroup, int Registrant, int Publication, int CheckDigit) : ISBN {
        internal static ISBN10 Parse(string isbn)
        {
            var cleanedIsbn = Clean(isbn);

            if (cleanedIsbn.Length != 10)
            {
                throw new InvalidIsbn($"Expected ISBN with 10 digits. Actual : {isbn.Length}");
            }

            var registrationGroup = ParseRegistrationGroup(cleanedIsbn);
            var registrant = ParseRegistrant(cleanedIsbn);

            var publication = ParsePublication(cleanedIsbn);

            var checkDigit = ParseCheckDigit(cleanedIsbn);

            return new ISBN10(registrationGroup, registrant, publication, checkDigit);
        }

        private static int ParseCheckDigit(string cleanedIsbn)
        {
            var checkDigitStr = cleanedIsbn.Substring(9, 1);

            if (!int.TryParse(checkDigitStr, out var checkDigit))
            {
                throw new InvalidIsbn($"Invalid CheckDigit : {checkDigitStr}. Expected : 1 digit.");
            }

            return checkDigit;
        }

        private static int ParsePublication(string cleanedIsbn)
        {
            var publicationStr = cleanedIsbn.Substring(6, 3);

            if (!int.TryParse(publicationStr, out var publication))
            {
                throw new InvalidIsbn($"Invalid Publication : {publicationStr}. Expected : 3 digits.");
            }

            return publication;
        }

        private static int ParseRegistrant(string cleanedIsbn)
        {
            var registrantStr = cleanedIsbn.Substring(2, 4);

            if (!int.TryParse(registrantStr, out var registrant))
            {
                throw new InvalidIsbn($"Invalid Registrant : {registrantStr}. Expected : 4 digits.");
            }

            return registrant;
        }

        private static int ParseRegistrationGroup(string cleanedIsbn)
        {
            var registrationGroupStr = cleanedIsbn[..2];

            if (!int.TryParse(registrationGroupStr, out var registrationGroup))
            {
                throw new InvalidIsbn($"Invalid Registration group : {registrationGroupStr}. Expected : 2 digits.");
            }

            return registrationGroup;
        }

        public override string ToString()
        {
            var registrationGroup = $"{RegistrationGroup}".PadLeft(2, '0');
            var registrantStr = $"{Registrant}".PadLeft(4, '0');
            var publicationStr = $"{Publication}".PadLeft(3, '0');

            return $"{registrationGroup}{registrantStr}{publicationStr}-{CheckDigit}";
        }
    }

    public record ISBN13(int Gs1Prefix, int RegistrationGroup, int Registrant, int Publication, int CheckDigit) : ISBN {
        internal static ISBN13 Parse(string isbn)
        {
            var cleanedIsbn = Clean(isbn);

            if (cleanedIsbn.Length != 13)
            {
                throw new InvalidIsbn($"Expected ISBN with 13 digits. Actual : {isbn.Length}");
            }

            var gs1Prefix = ParseGs1Prefix(cleanedIsbn);
            var registrationGroup = ParseRegistrationGroup(cleanedIsbn);
            var registrant = ParseRegistrant(cleanedIsbn);

            var publication = ParsePublication(cleanedIsbn);

            var checkDigit = ParseCheckDigit(cleanedIsbn);

            return new ISBN13(gs1Prefix, registrationGroup, registrant, publication, checkDigit);
        }

        private static int ParseCheckDigit(string cleanedIsbn)
        {
            var checkDigitStr = cleanedIsbn.Substring(12, 1);

            if (!int.TryParse(checkDigitStr, out var checkDigit))
            {
                throw new InvalidIsbn($"Invalid CheckDigit : {checkDigitStr}. Expected : 1 digit.");
            }

            return checkDigit;
        }

        private static int ParsePublication(string cleanedIsbn)
        {
            var publicationStr = cleanedIsbn.Substring(9, 3);

            if (!int.TryParse(publicationStr, out var publication))
            {
                throw new InvalidIsbn($"Invalid Publication : {publicationStr}. Expected : 3 digits.");
            }

            return publication;
        }

        private static int ParseRegistrant(string cleanedIsbn)
        {
            var registrantStr = cleanedIsbn.Substring(5, 4);

            if (!int.TryParse(registrantStr, out var registrant))
            {
                throw new InvalidIsbn($"Invalid Registrant : {registrantStr}. Expected : 4 digits.");
            }

            return registrant;
        }

        private static int ParseRegistrationGroup(string cleanedIsbn)
        {
            var registrationGroupStr = cleanedIsbn.Substring(3, 2);

            if (!int.TryParse(registrationGroupStr, out var registrationGroup))
            {
                throw new InvalidIsbn($"Invalid Registration group : {registrationGroupStr}. Expected : 2 digits.");
            }

            return registrationGroup;
        }

        private static int ParseGs1Prefix(string cleanedIsbn)
        {
            var gs1PrefixStr = cleanedIsbn[..3];

            if (!int.TryParse(gs1PrefixStr, out var gs1Prefix))
            {
                throw new InvalidIsbn($"Invalid Gs1 Prefix : {gs1PrefixStr}. Expected : 3 digits.");
            }

            if (gs1Prefix is not 978 or 979)
            {
                throw new InvalidIsbn($"Invalid Gs1 Prefix : {gs1PrefixStr}. Only 978 and 979 are supported.");
            }

            return gs1Prefix;
        }

        public override string ToString()
        {
            var registrationGroup = $"{RegistrationGroup}".PadLeft(2, '0');
            var registrantStr = $"{Registrant}".PadLeft(4, '0');
            var publicationStr = $"{Publication}".PadLeft(3, '0');
            return $"{Gs1Prefix}-{registrationGroup}{registrantStr}{publicationStr}-{CheckDigit}";
        }
    }

    public class InvalidIsbn : Exception {
        
         
        public InvalidIsbn(string message) : base(message) {}
    }
}