namespace ApiVersioningSample.Models.v1_1
{
    using System;
    using System.Linq;

    public class ArticleModel
    {
        #region properties

        public long Id { get; set; }

        public string Label { get; set; }

        public string Number { get; set; }

        public decimal Price { get; set; }

        #endregion
    }
}