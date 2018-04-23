Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.ExpressApp.Security.Strategy
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base

Namespace EmployeeAsUserExample.Module.BusinessObjects
	<ImageName("BO_Role")>
	Public Class EmployeeRole
		Inherits SecuritySystemRoleBase

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		<Association("Employees-EmployeeRoles")>
		Public ReadOnly Property Employees() As XPCollection(Of Employee)
			Get
				Return GetCollection(Of Employee)("Employees")
			End Get
		End Property
	End Class
End Namespace
