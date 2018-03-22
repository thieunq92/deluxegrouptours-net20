using System;
using System.Collections.Generic;
using System.Web;

namespace Portal.Modules.OrientalSails.Domain
{
    public class AgencyLevel
    {
        public virtual int Id { set; get; }
        public virtual string Name { set; get;}
        public virtual IList<Agency> AgencyList { set; get; }
        public virtual IList<AgencyCommission> AgencyCommissionList { set; get; }
    }
}