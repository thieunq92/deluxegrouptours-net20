using System;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class AgencyContact
    {        
        #region Member Variables

        protected int _id;
        protected string _name;
        protected string _phone;
        protected string _email;
        protected bool _enabled;
        protected Agency _agency;
        protected string _position;

        #endregion

        #region Constructors

        public AgencyContact()
        {
            _id = -1;
            _enabled = true;
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

        public virtual string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public virtual bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public virtual Agency Agency
        {
            get { return _agency; }
            set { _agency = value; }
        }

        public virtual int AgencyId
        {
            get
            {
                if (_agency!=null)
                {
                    return _agency.Id;
                }
                return -1;
            }
        }

        public virtual string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual DateTime? Birthday { set; get; }
        public virtual bool IsBooker { get; set; }
        public virtual String Note { set; get; }
        #endregion
    }

    #endregion
}
