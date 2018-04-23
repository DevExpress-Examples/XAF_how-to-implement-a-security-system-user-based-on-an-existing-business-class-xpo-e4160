Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base

Imports DevExpress.Persistent.BaseImpl.PermissionPolicy

Namespace EmployeeAsUserExample.Module.BusinessObjects
    <ImageName("BO_Role")> _
    Public Class EmployeeRole
        Inherits PermissionPolicyRoleBase
        Implements IPermissionPolicyRoleWithUsers

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        <Association("Employees-EmployeeRoles")> _
        Public ReadOnly Property Employees() As XPCollection(Of Employee)
            Get
                Return GetCollection(Of Employee)("Employees")
            End Get
        End Property
        Private ReadOnly Property IPermissionPolicyRoleWithUsers_Users() As IEnumerable(Of IPermissionPolicyUser) Implements IPermissionPolicyRoleWithUsers.Users
            Get
                Return Employees.OfType(Of IPermissionPolicyUser)()
            End Get
        End Property
    End Class
End Namespace
