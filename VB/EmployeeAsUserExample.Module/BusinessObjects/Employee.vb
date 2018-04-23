Imports System.Text
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Utils
'using DevExpress.ExpressApp.Security.Strategy;

Namespace EmployeeAsUserExample.Module.BusinessObjects
    <DefaultClassOptions()> _
    Public Class Employee
        Inherits Person
        Implements ISecurityUser, IAuthenticationStandardUser, IAuthenticationActiveDirectoryUser, ISecurityUserWithRoles, IOperationPermissionProvider

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub
        <Association("Employee-Task")> _
        Public ReadOnly Property OwnTasks() As XPCollection(Of EmployeeTask)
            Get
                Return GetCollection(Of EmployeeTask)("OwnTasks")
            End Get
        End Property

#Region "ISecurityUser Members"
        Private _userName As String = String.Empty
        Private ReadOnly Property UserName_() As String Implements ISecurityUser.UserName, IAuthenticationStandardUser.UserName
            Get
                Return _userName
            End Get
        End Property
        <RuleRequiredField("EmployeeUserNameRequired", DefaultContexts.Save), RuleUniqueValue("EmployeeUserNameIsUnique", DefaultContexts.Save, "The login with the entered user name was already registered within the system.")> _
        Public Property UserName() As String Implements IAuthenticationActiveDirectoryUser.UserName
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                SetPropertyValue("UserName", _userName, value)
            End Set
        End Property
        Private _isActive As Boolean = True
        Private ReadOnly Property IsActive_() As Boolean Implements ISecurityUser.IsActive
            Get
                Return _isActive
            End Get
        End Property
        Public Property IsActive() As Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("IsActive", _isActive, value)
            End Set
        End Property
#End Region

#Region "IAuthenticationStandardUser Members"
        Private _changePasswordOnFirstLogon As Boolean
        Public Property ChangePasswordOnFirstLogon() As Boolean Implements IAuthenticationStandardUser.ChangePasswordOnFirstLogon
            Get
                Return _changePasswordOnFirstLogon
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("ChangePasswordOnFirstLogon", _changePasswordOnFirstLogon, value)
            End Set
        End Property
        Private _storedPassword As String
        <Browsable(False), Size(SizeAttribute.Unlimited), Persistent(), SecurityBrowsable()> _
        Protected Property StoredPassword() As String
            Get
                Return _storedPassword
            End Get
            Set(ByVal value As String)
                _storedPassword = value
            End Set
        End Property
        Public Function ComparePassword(ByVal password As String) As Boolean Implements IAuthenticationStandardUser.ComparePassword
            Return SecurityUserBase.ComparePassword(Me._storedPassword, password)
        End Function
        Public Sub SetPassword(ByVal password As String) Implements IAuthenticationStandardUser.SetPassword
            Me._storedPassword = (New PasswordCryptographer()).GenerateSaltedPassword(password)
            OnChanged("StoredPassword")
        End Sub
#End Region


#Region "ISecurityUserWithRoles Members"
        Private ReadOnly Property ISecurityUserWithRoles_Roles() As IList(Of ISecurityRole) Implements ISecurityUserWithRoles.Roles
            Get
                Dim result As IList(Of ISecurityRole) = New List(Of ISecurityRole)()
                For Each role As EmployeeRole In EmployeeRoles
                    result.Add(role)
                Next role
                Return result
            End Get
        End Property
#End Region

        <Association("Employees-EmployeeRoles"), RuleRequiredField("EmployeeRoleIsRequired", DefaultContexts.Save, TargetCriteria:="IsActive", CustomMessageTemplate:="An active employee must have at least one role assigned")> _
        Public ReadOnly Property EmployeeRoles() As XPCollection(Of EmployeeRole)
            Get
                Return GetCollection(Of EmployeeRole)("EmployeeRoles")
            End Get
        End Property

#Region "IOperationPermissionProvider Members"
        Private Function IOperationPermissionProvider_GetPermissions() As IEnumerable(Of IOperationPermission) Implements IOperationPermissionProvider.GetPermissions
            Return New IOperationPermission() {}
        End Function
        Private Function IOperationPermissionProvider_GetChildren() As IEnumerable(Of IOperationPermissionProvider) Implements IOperationPermissionProvider.GetChildren
            Return New EnumerableConverter(Of IOperationPermissionProvider, EmployeeRole)(EmployeeRoles)
        End Function
#End Region
    End Class
End Namespace
