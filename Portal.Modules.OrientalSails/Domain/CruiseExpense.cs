using System;
using System.Collections;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class CruiseExpense : IComparable
    {
        #region Static Columns Name

        public static string BIRTHDAY = "Birthday";
        public static string BOOKING = "Booking";
        public static string COUNTRY = "Country";
        public static string FULLNAME = "Fullname";
        public static string PASSPORT = "Passport";
        public static string BOOKINGROOM = "BookingRoom";

        #endregion

        #region Member Variables

        protected int _id;
        protected int _customerFrom;
        protected int _custeromTo;
        protected double _price;
        protected int _currency;
        protected CruiseExpenseTable _table;

        #endregion

        #region Constructors

        public CruiseExpense()
        {
            _id = -1;
        }

        #endregion

        #region Public Properties

        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual int CustomerFrom
        {
            get { return _customerFrom; }
            set { _customerFrom = value; }
        }

        public virtual int CustomerTo
        {
            get { return _custeromTo; }
            set { _custeromTo = value; }
        }

        public virtual double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public virtual int Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public virtual CruiseExpenseTable Table
        {
            get { return _table;}
            set { _table = value; }
        }

        #endregion

        public int CompareTo(object obj)
        {
            return CustomerFrom - ((CruiseExpense)obj).CustomerFrom;
        }
    }

    #endregion
}
