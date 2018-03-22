using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Domain;

namespace Portal.Modules.OrientalSails.Domain
{
    #region Customer

    /// <summary>
    /// Customer object for NHibernate mapped table 'os_Customer'.
    /// </summary>
    public class AnswerOption
    {
        #region Member Variables

        protected int _id;
        protected Question _question;
        protected AnswerSheet _answerSheet;
        protected int _option;

        #endregion

        #region Constructors

        public AnswerOption()
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

        public virtual Question Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public virtual AnswerSheet AnswerSheet
        {
            get { return _answerSheet; }
            set { _answerSheet = value; }
        }

        public virtual int Option
        {
            get { return _option; }
            set { _option = value; }
        }

        #endregion
    }

    #endregion
}
