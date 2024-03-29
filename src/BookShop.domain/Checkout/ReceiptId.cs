﻿namespace BookShop.domain.Checkout;

public record ReceiptId(Guid Id) {
    public static ReceiptId Generate()
    {
        return new(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}