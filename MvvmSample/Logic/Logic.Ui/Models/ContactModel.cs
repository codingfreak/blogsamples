namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.Models
{
    using System;
    using System.Linq;

    using BaseTypes;

    public class ContactModel : BaseModel
    {
        #region properties

        public DateTimeOffset ContactDate { get; set; }

        public string Notes { get; set; }

        public string Title { get; set; }

        #endregion
    }
}