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
'using DevExpress.ExpressApp.Security.Strategy;

Namespace EmployeeAsUserExample.Module.BusinessObjects
	<DefaultClassOptions>
	Public Class Employee
		Inherits Person
		Implements ISecurityUser, IAuthenticationStandardUser, IAuthenticationActiveDirectoryUser, ISecurityUserWithRoles, IOperationPermissionProvider, ICanInitialize

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		<Association("Employee-Task")>
		Public ReadOnly Property OwnTasks() As XPCollection(Of EmployeeTask)
			Get
				Return GetCollection(Of EmployeeTask)("OwnTasks")
			End Get
		End Property

		#Region "ISecurityUser Members"
'INSTANT VB NOTE: The variable userName was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private _userName As String = String.Empty
		Private ReadOnly Property ISecurityUser_UserName() As String Implements ISecurityUser.UserName
			Get
				Return UserName
			End Get
		End Property
		Private ReadOnly Property IAuthenticationStandardUser_UserName() As String Implements IAuthenticationStandardUser.UserName
			Get
				Return UserName
			End Get
		End Property
		<RuleRequiredField("EmployeeUserNameRequired", DefaultContexts.Save), RuleUniqueValue("EmployeeUserNameIsUnique", DefaultContexts.Save, "The login with the entered user name was already registered within the system.")>
		Public Property UserName() As String Implements IAuthenticationActiveDirectoryUser.UserName
			Get
				Return _userName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("UserName", _userName, value)
			End Set
		End Property
'INSTANT VB NOTE: The variable isActive was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private _isActive As Boolean = True
		Private ReadOnly Property ISecurityUser_IsActive() As Boolean Implements ISecurityUser.IsActive
			Get
				Return IsActive
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
'INSTANT VB NOTE: The variable changePasswordOnFirstLogon was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private _changePasswordOnFirstLogon As Boolean
		Public Property ChangePasswordOnFirstLogon() As Boolean Implements IAuthenticationStandardUser.ChangePasswordOnFirstLogon
			Get
				Return _changePasswordOnFirstLogon
			End Get
			Set(ByVal value As Boolean)
				SetPropertyValue("ChangePasswordOnFirstLogon", _changePasswordOnFirstLogon, value)
			End Set
		End Property
'INSTANT VB NOTE: The variable storedPassword was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private _storedPassword As String
		<Browsable(False), Size(SizeAttribute.Unlimited), Persistent, SecurityBrowsable>
		Protected Property StoredPassword() As String
			Get
				Return _storedPassword
			End Get
			Set(ByVal value As String)
				_storedPassword = value
			End Set
		End Property
		Public Function ComparePassword(ByVal password As String) As Boolean Implements IAuthenticationStandardUser.ComparePassword
            Return (New PasswordCryptographer()).AreEqual(Me._storedPassword, password)
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

		<Association("Employees-EmployeeRoles"), RuleRequiredField("EmployeeRoleIsRequired", DefaultContexts.Save, TargetCriteria := "IsActive", CustomMessageTemplate := "An active employee must have at least one role assigned")>
		Public ReadOnly Property EmployeeRoles() As XPCollection(Of EmployeeRole)
			Get
				Return GetCollection(Of EmployeeRole)("EmployeeRoles")
			End Get
		End Property

		#Region "IOperationPermissionProvider Members"
		Private Function IOperationPermissionProvider_GetPermissions() As IEnumerable(Of IOperationPermission) Implements IOperationPermissionProvider.GetPermissions
			Return New IOperationPermission(){}
		End Function
		Private Function IOperationPermissionProvider_GetChildren() As IEnumerable(Of IOperationPermissionProvider) Implements IOperationPermissionProvider.GetChildren
			Return New EnumerableConverter(Of IOperationPermissionProvider, EmployeeRole)(EmployeeRoles)
		End Function
		#End Region

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
