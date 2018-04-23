using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;

namespace EmployeeAsUserExample.Module.BusinessObjects {
    [ImageName("BO_Role")]
    public class EmployeeRole : SecuritySystemRoleBase {
        public EmployeeRole(Session session)
            : base(session) {
        }
        [Association("Employees-EmployeeRoles")]
        public XPCollection<Employee> Employees {
            get {
                return GetCollection<Employee>("Employees");
            }
        }
    }
}
