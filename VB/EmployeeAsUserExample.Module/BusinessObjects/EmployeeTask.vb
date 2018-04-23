Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.SystemModule

Namespace EmployeeAsUserExample.Module.BusinessObjects
    <DefaultClassOptions, ImageName("BO_Task"), ListViewFilter("All Tasks", ""), ListViewFilter("My Tasks", "[Owner.Oid] = CurrentUserId()")> _
    Public Class EmployeeTask
        Inherits Task

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Private owner_Renamed As Employee
        <Association("Employee-Task")> _
        Public Property Owner() As Employee
            Get
                Return owner_Renamed
            End Get
            Set(ByVal value As Employee)
                SetPropertyValue("Owner", owner_Renamed, value)
            End Set
        End Property
    End Class
End Namespace
