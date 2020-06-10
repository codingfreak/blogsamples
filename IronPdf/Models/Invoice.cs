namespace codingfreaks.blogsamples.IronPdf.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Invoice
    {
        #region properties

        public DateTimeOffset InvoiceDate { get; set; }

        public string Number { get; set; }

        public IEnumerable<InvoicePosition> Positions { get; set; }

        public double PriceIncludingTaxes => PriceWithoutTaxes + Taxes;

        public string PriceIncludingTaxesFormatted => PriceIncludingTaxes.ToString("C");

        public double PriceWithoutTaxes => Positions?.Sum(p => p.Price) ?? 0;

        public string PriceWithoutTaxesFormatted => PriceWithoutTaxes.ToString("C");

        public double Taxes => PriceWithoutTaxes / 1.19;

        public string TaxesFormatted => Taxes.ToString("C");

        #endregion
    }
}