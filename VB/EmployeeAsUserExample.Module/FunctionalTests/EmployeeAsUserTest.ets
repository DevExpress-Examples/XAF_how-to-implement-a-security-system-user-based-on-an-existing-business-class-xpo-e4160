#Application EmployeeAsUserExample.Win
#Application EmployeeAsUserExample.Web

*FillForm
 User Name = Administrator
 Password = ''
 
*Action Log On

*Action Navigation(My Details)

*CheckFieldValues
 User Name = Administrator
 Full Name = Andrew Fuller
 
*Action Navigation(Employee Role)

*CheckTable
 Columns = Name, Is Administrative
 Row = Administrator, True
 
*Action Navigation(Employee Task)

*Action Filter(All Tasks)

*CheckTable
 Columns = Subject
 Row = Sample task that belongs to nobody
 Row = Sample task that belongs to Andrew
 RowCount = 2
 
*Action Filter(My Tasks)

*CheckTable
 Columns = Subject
 Row = Sample task that belongs to Andrew
 RowCount = 1
