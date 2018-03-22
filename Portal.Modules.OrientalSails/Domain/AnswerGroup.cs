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
    public class AnswerGroup
    {
        #region Member Variables

        protected int _id;
        protected QuestionGroup _group;
        protected AnswerSheet _answerSheet;
        protected string _comment;

        #endregion

        #region Constructors

        public AnswerGroup()
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

        public virtual QuestionGroup Group
        {
            get { return _group; }
            set { _group = value; }
        }

        public virtual AnswerSheet AnswerSheet
        {
            get { return _answerSheet; }
            set { _answerSheet = value; }
        }

        public virtual string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        #endregion
    }

    #endregion
}
