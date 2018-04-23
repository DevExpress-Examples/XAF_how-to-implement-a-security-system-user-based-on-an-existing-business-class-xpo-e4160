using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;

using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace EmployeeAsUserExample.Module.BusinessObjects {
    [ImageName("BO_Role")]
    public class EmployeeRole : PermissionPolicyRoleBase, IPermissionPolicyRoleWithUsers {
        public EmployeeRole(Session session)
            : base(session) {
        }
        [Association("Employees-EmployeeRoles")]
        public XPCollection<Employee> Employees {
            get {
                return GetCollection<Employee>("Employees");
            }
        }
        IEnumerable<IPermissionPolicyUser> IPermissionPolicyRoleWithUsers.Users {
            get { return Employees.OfType<IPermissionPolicyUser>(); }
        } 
    }
}
