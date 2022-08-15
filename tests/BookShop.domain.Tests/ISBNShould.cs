using NFluent;
using Xunit;

namespace BookShop.domain.Tests;

public class ISBNShould {
    [Fact]
    public void Parse_a_valid_ISBN13()
    {
        var isbn = ISBN.Parse("978-817525766-5");

        Check.That(isbn).IsInstanceOf<ISBN.ISBN13>()
            .And.HasFieldsWithSameValues(new
            {
                Gs1Prefix = 978,
                RegistrationGroup = 81,
                Registrant = 7525,
                Publication = 766,
                CheckDigit = 5
            });
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN13_with_Gs1_Prefix_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("97A-817525766-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN13_with_an_invalid_Gs1_Prefix()
    {
        Check.ThatCode(() => ISBN.Parse("999-817525766-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN13_with_registration_group_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("978-!17525766-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN13_with_registrant_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("978-8175s5766-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN13_with_publication_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("978-8175257*6-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN13_with_invalid_check_digit()
    {
        Check.ThatCode(() => ISBN.Parse("978-817525766-x"))
            .Throws<ISBN.InvalidIsbnException>();
    }


    [Fact]
    public void Parse_a_valid_ISBN10()
    {
        var isbn = ISBN.Parse("817525766-5");

        Check.That(isbn).IsInstanceOf<ISBN.ISBN10>()
            .And.HasFieldsWithSameValues(new
            {
                RegistrationGroup = 81,
                Registrant = 7525,
                Publication = 766,
                CheckDigit = 5
            });
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN10_with_registration_group_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("?17525766-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN12_with_registrant_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("8175s5766-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN10_with_publication_containing_non_digit_char()
    {
        Check.ThatCode(() => ISBN.Parse("8175257*6-5"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_parsing_an_ISBN10_with_invalid_check_digit()
    {
        Check.ThatCode(() => ISBN.Parse("817525766-x"))
            .Throws<ISBN.InvalidIsbnException>();
    }


    [Fact]
    public void Fail_when_string_is_too_short()
    {
        Check.ThatCode(() => ISBN.Parse("8175257"))
            .Throws<ISBN.InvalidIsbnException>();
    }

    [Fact]
    public void Fail_when_string_is_too_long()
    {
        Check.ThatCode(() => ISBN.Parse("8175257231654987"))
            .Throws<ISBN.InvalidIsbnException>();
    }
}