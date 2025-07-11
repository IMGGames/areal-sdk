using System;
using System.Globalization;
using System.Linq;

namespace Areal.SDK.IAP {
    public class Product {
        public string Id;
        public readonly decimal LocalPrice;
        public readonly string IsoCurrencyCode;
        public readonly string LocalPriceString;

        public Product(string id, decimal localPrice, string isoCurrencyCode) {
            Id = id;
            LocalPrice = localPrice;
            IsoCurrencyCode = isoCurrencyCode;

            var culture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).FirstOrDefault(c => {
                try {
                    return new RegionInfo(c.Name).ISOCurrencySymbol.Equals(isoCurrencyCode, StringComparison.OrdinalIgnoreCase);
                }
                catch {
                    return false;
                }
            });

            LocalPriceString = culture == null ? $"{localPrice} {isoCurrencyCode}" : string.Format(culture, "{0:C}", localPrice);
        }
    }
}