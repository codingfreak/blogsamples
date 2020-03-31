namespace ApiVersioningSample.Models.v2_0
{
    using System;
    using System.Linq;

    public class ArticleModel
    {
        #region properties

        public long Id { get; set; }

        public string Label { get; set; }

        public string Number { get; set; }

        public decimal TotalPrice { get; set; }

        #endregion
    }
}