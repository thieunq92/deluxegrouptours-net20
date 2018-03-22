using System;
using System.Collections;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class Agency
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
        protected string _name;
        protected string _phone;
        protected string _address;
        protected Role _role;
        protected IList _users;
        protected string _fax;
        protected string _email;
        protected string _taxCode;
        protected int _contractStatus;
        protected string _description;
        protected string _contract;
        protected User _sale;
        protected string _accountant;
        protected PaymentPeriod _paymentMethod;
        protected DateTime _lastBooking;
        protected IList _bookings;

        protected DateTime? _createdDate;
        protected User _createdBy;
        protected DateTime? _modifiedDate;
        protected User _modifiedBy;
        protected bool _isDeleted;

        #endregion

        #region Constructors

        public Agency()
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

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public virtual string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        public virtual string Address
        {
            get { return _address;}
            set { _address = value; }
        }

        public virtual string Contract
        {
            get { return _contract; }
            set { _contract = value; }
        }

        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public virtual int ContractStatus
        {
            get { return _contractStatus; }
            set { _contractStatus = value; }
        }

        public virtual string TaxCode
        {
            get { return _taxCode; }
            set { _taxCode = value; }
        }

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public virtual Role Role
        {
            get { return _role; }
            set { _role = value; }
        }

        public virtual IList Users
        {
            get
            {
                if (_users==null)
                {
                    _users = new ArrayList();
                }
                return _users;
            }
            set { _users = value; }
        }

        public virtual IList Bookings
        {
            get
            {
                if (_bookings == null)
                {
                    _bookings = new ArrayList();
                }
                return _bookings;
            }
            set { _bookings = value; }
        }

        public virtual User Sale
        {
            get { return _sale; }
            set { _sale = value; }
        }

        public virtual string Accountant
        {
            get { return _accountant; }
            set { _accountant = value; }
        }

        public virtual PaymentPeriod PaymentPeriod
        {
            get { return _paymentMethod; }
            set { _paymentMethod = value; }
        }

        public virtual DateTime LastBooking
        {
            get { return _lastBooking; }
            set { _lastBooking = value; }
        }

        public virtual DateTime? CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        public virtual DateTime? ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public virtual User CreatedBy
        {
            get { return _createdBy;}
            set { _createdBy = value; }
        }

        public virtual User ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; }
        }

        public virtual bool Deleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        public virtual Organization Organization { get; set; }

        public virtual AgencyLevel AgencyLevel { set; get; }

        #endregion
    }

    public enum PaymentPeriod
    {
        None,
        Monthly,
        MonthlyCK
    }

    public class PaymentPeriodClass :  NHibernate.Type.EnumStringType
    {
        public PaymentPeriodClass()
            : base(typeof(PaymentPeriod), 20)
        {

        }
    }

    #endregion
}
