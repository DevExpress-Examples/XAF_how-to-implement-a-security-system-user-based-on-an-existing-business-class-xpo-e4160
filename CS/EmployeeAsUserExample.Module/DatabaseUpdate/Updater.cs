using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using EmployeeAsUserExample.Module.BusinessObjects;

namespace EmployeeAsUserExample.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            EmployeeRole adminEmployeeRole = ObjectSpace.FindObject<EmployeeRole>(
                new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (adminEmployeeRole == null) {
                adminEmployeeRole = ObjectSpace.CreateObject<EmployeeRole>();
                adminEmployeeRole.Name = SecurityStrategy.AdministratorRoleName;
                adminEmployeeRole.IsAdministrative = true;
            }
            Employee adminEmployee = ObjectSpace.FindObject<Employee>(
                new BinaryOperator("UserName", "Administrator"));
            if (adminEmployee == null) {
                adminEmployee = ObjectSpace.CreateObject<Employee>();
                adminEmployee.UserName = "Administrator";
                adminEmployee.FirstName = "Andrew";
                adminEmployee.LastName = "Fuller";
                adminEmployee.SetPassword("");
                adminEmployee.EmployeeRoles.Add(adminEmployeeRole);
            }
            EmployeeTask task1 = ObjectSpace.FindObject<EmployeeTask>(
                new BinaryOperator("Subject", "Sample task that belongs to Andrew"));
            if (task1 == null) {
                task1 = ObjectSpace.CreateObject<EmployeeTask>();
                task1.Subject = "Sample task that belongs to Andrew";
                task1.Owner = adminEmployee;
            }
            EmployeeTask task2 = ObjectSpace.FindObject<EmployeeTask>(
                new BinaryOperator("Subject", "Sample task that belongs to nobody"));
            if (task2 == null) {
                task2 = ObjectSpace.CreateObject<EmployeeTask>();
                task2.Subject = "Sample task that belongs to nobody"; 
            }    
            ObjectSpace.CommitChanges();
        }
    }
}
