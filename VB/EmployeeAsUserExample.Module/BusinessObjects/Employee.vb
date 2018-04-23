Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Security
Imports DevExpress.Persistent.Validation
Imports DevExpress.Persistent.Base.Security
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Utils
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering

Namespace EmployeeAsUserExample.Module.BusinessObjects
    <DefaultClassOptions> _
    Public Class Employee
        Inherits Person
        Implements ISecurityUser, IAuthenticationStandardUser, IAuthenticationActiveDirectoryUser, ISecurityUserWithRoles, IPermissionPolicyUser, ICanInitialize

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
        <RuleRequiredField("EmployeeUserNameRequired", DefaultContexts.Save),
    RuleUniqueValue("EmployeeUserNameIsUnique", DefaultContexts.Save,
    "The login with the entered user name was already registered within the system.")>
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
        Public Property ChangePasswordOnFirstLogon() As Boolean _
    Implements IAuthenticationStandardUser.ChangePasswordOnFirstLogon
            Get
                Return _changePasswordOnFirstLogon
            End Get
            Set(ByVal value As Boolean)
                SetPropertyValue("ChangePasswordOnFirstLogon", _changePasswordOnFirstLogon, value)
            End Set
        End Property
        Private _storedPassword As String
        <Browsable(False), Size(SizeAttribute.Unlimited), Persistent(), SecurityBrowsable()>
        Protected Property StoredPassword() As String
            Get
                Return _storedPassword
            End Get
            Set(ByVal value As String)
                _storedPassword = value
            End Set
        End Property
        Public Function ComparePassword(ByVal password As String) As Boolean _
    Implements IAuthenticationStandardUser.ComparePassword
            Return PasswordCryptographer.VerifyHashedPasswordDelegate(Me.StoredPassword, password)
        End Function
        Public Sub SetPassword(ByVal password As String) _
    Implements IAuthenticationStandardUser.SetPassword
            Me.StoredPassword = PasswordCryptographer.HashPasswordDelegate(password)
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

        <Association("Employees-EmployeeRoles"), RuleRequiredField("EmployeeRoleIsRequired", DefaultContexts.Save, TargetCriteria := "IsActive", CustomMessageTemplate := "An active employee must have at least one role assigned")> _
        Public ReadOnly Property EmployeeRoles() As XPCollection(Of EmployeeRole)
            Get
                Return GetCollection(Of EmployeeRole)("EmployeeRoles")
            End Get
        End Property

        #Region "IPermissionPolicyUser Members"
        Private ReadOnly Property IPermissionPolicyUser_Roles() As IEnumerable(Of IPermissionPolicyRole) Implements IPermissionPolicyUser.Roles
            Get
                Return EmployeeRoles.OfType(Of IPermissionPolicyRole)()
            End Get
        End Property
        #End Region
'        
'        #region IOperationPermissionProvider Members
'        IEnumerable<IOperationPermission> IOperationPermissionProvider.GetPermissions() {
'            return new IOperationPermission[0];
'        }
'        IEnumerable<IOperationPermissionProvider> IOperationPermissionProvider.GetChildren() {
'            return new EnumerableConverter<IOperationPermissionProvider, EmployeeRole>(EmployeeRoles);
'        }
'        #endregion
'        
        #Region "ICanInitialize Members"
        Private Sub ICanInitialize_Initialize(ByVal objectSpace As IObjectSpace, ByVal security As SecurityStrategyComplex) Implements ICanInitialize.Initialize
            Dim newUserRole As EmployeeRole = CType(objectSpace.FindObject(Of EmployeeRole)(New BinaryOperator("Name", security.NewUserRoleName)), EmployeeRole)
            If newUserRole Is Nothing Then
                newUserRole = objectSpace.CreateObject(Of EmployeeRole)()
                newUserRole.Name = security.NewUserRoleName
                newUserRole.IsAdministrative = True
                newUserRole.Employees.Add(Me)
            End If
        End Sub
        #End Region

    End Class
End Namespace
