using System;
using System.Collections;
using CMS.Core.Domain;
using Portal.Modules.OrientalSails.Web.Util;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class BookingHistory
    {
        #region Static Columns Name

        #endregion

        #region Constructors

        public BookingHistory()
        {
        }

        #endregion

        #region Public Properties

        public virtual int Id { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual User User { get; set; }

        public virtual SailsTrip Trip { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual StatusType Status { get; set; }

        public virtual Agency Agency { get; set; }

        public virtual double Total { get; set; }

        public virtual string TotalCurrency { get; set; }

        public virtual string SpecialRequest { get; set; }
        #endregion
    }

    #endregion
}
