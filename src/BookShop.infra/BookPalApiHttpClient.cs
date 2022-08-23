using System.Net.Http.Json;
using BookShop.domain.Checkout.Payment;

namespace BookShop.infra;

public class BookPalApiHttpClient : IProcessPayment {
    private readonly HttpClient _httpClient;

    public BookPalApiHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private record PaymentError(string Message);
    public async Task<PaymentReceipt> Process(Payment payment)
    {
        if (payment is not Payment.CardPayment cardPayment)
        {
            throw new NotImplementedException();
        }
        
        var response = await _httpClient.PostAsync("/api/payment/validation", JsonContent.Create(new
        {
            cardPayment.Payment
        }));

        if (response.IsSuccessStatusCode)
        {
            return PaymentReceipt.Success;
        }
        
        
        var error = await response.Content.ReadFromJsonAsync<PaymentError>();
        return PaymentReceipt.Failure(error?.Message ?? "Payment failed");
    }
}