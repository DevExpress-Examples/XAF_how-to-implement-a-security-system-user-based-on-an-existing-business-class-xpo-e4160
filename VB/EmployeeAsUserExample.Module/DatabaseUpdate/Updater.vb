Imports System

Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Security
Imports EmployeeAsUserExample.Module.BusinessObjects

Namespace EmployeeAsUserExample.Module.DatabaseUpdate
    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub
        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            Dim adminEmployeeRole As EmployeeRole = ObjectSpace.FindObject(Of EmployeeRole)(New BinaryOperator("Name", SecurityStrategy.AdministratorRoleName))
            If adminEmployeeRole Is Nothing Then
                adminEmployeeRole = ObjectSpace.CreateObject(Of EmployeeRole)()
                adminEmployeeRole.Name = SecurityStrategy.AdministratorRoleName
                adminEmployeeRole.IsAdministrative = True
            End If
            Dim adminEmployee As Employee = ObjectSpace.FindObject(Of Employee)(New BinaryOperator("UserName", "Administrator"))
            If adminEmployee Is Nothing Then
                adminEmployee = ObjectSpace.CreateObject(Of Employee)()
                adminEmployee.UserName = "Administrator"
                adminEmployee.FirstName = "Andrew"
                adminEmployee.LastName = "Fuller"
                adminEmployee.SetPassword("")
                adminEmployee.EmployeeRoles.Add(adminEmployeeRole)
            End If
            Dim task1 As EmployeeTask = ObjectSpace.FindObject(Of EmployeeTask)(New BinaryOperator("Subject", "Sample task that belongs to Andrew"))
            If task1 Is Nothing Then
                task1 = ObjectSpace.CreateObject(Of EmployeeTask)()
                task1.Subject = "Sample task that belongs to Andrew"
                task1.Owner = adminEmployee
            End If
            Dim task2 As EmployeeTask = ObjectSpace.FindObject(Of EmployeeTask)(New BinaryOperator("Subject", "Sample task that belongs to nobody"))
            If task2 Is Nothing Then
                task2 = ObjectSpace.CreateObject(Of EmployeeTask)()
                task2.Subject = "Sample task that belongs to nobody"
            End If
            ObjectSpace.CommitChanges()
        End Sub
    End Class
End Namespace
