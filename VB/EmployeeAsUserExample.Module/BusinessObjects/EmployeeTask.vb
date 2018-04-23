Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.SystemModule

Namespace EmployeeAsUserExample.Module.BusinessObjects
	<DefaultClassOptions, ImageName("BO_Task"), ListViewFilter("All Tasks", ""), ListViewFilter("My Tasks", "[Owner.Oid] = CurrentUserId()")>
	Public Class EmployeeTask
		Inherits Task

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
'INSTANT VB NOTE: The variable owner was renamed since Visual Basic does not allow variables and other class members to have the same name:
		Private _owner As Employee
		<Association("Employee-Task")>
		Public Property Owner() As Employee
			Get
				Return _owner
			End Get
			Set(ByVal value As Employee)
				SetPropertyValue("Owner", _owner, value)
			End Set
		End Property
	End Class
End Namespace
