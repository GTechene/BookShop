using BookShop.domain;
using Diverse;

namespace BookShop.AcceptanceTests;

public static class FuzzerExtensions
{
    public static ISBN.ISBN10 GenerateIsbn10(this IFuzz fuzzer)
    {
        return new ISBN.ISBN10(fuzzer.GenerateInteger(1, 100), fuzzer.GenerateInteger(1, 10000), fuzzer.GenerateInteger(1, 1000), fuzzer.GenerateInteger(1, 10));
    }
}