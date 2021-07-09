''' <summary>
''' A class that captures infomation about a student
''' </summary>
Public Class Student

    Public ReadOnly Property FirstName As String = ""
    Public ReadOnly Property MiddleName As String = ""
    Public ReadOnly Property LastName As String = ""
    Public ReadOnly Property AuxName As String = ""
    Public ReadOnly Property StudentNumber As String = ""
    Public ReadOnly Property Course As String = ""
    Public ReadOnly Property Campus As String = ""
    Public ReadOnly Property EmailAddress As String = Nothing

    Sub New(ByVal studentNumber As String, ByVal firstName As String, ByVal middleName As String, ByVal lastName As String, ByVal auxName As String,
            ByVal course As String, ByVal campus As String, ByVal emailAddress As String)
        Me.FirstName = firstName
        Me.MiddleName = middleName
        Me.LastName = lastName
        Me.AuxName = auxName
        Me.StudentNumber = studentNumber
        Me.Course = course
        Me.Campus = campus
        Me.EmailAddress = emailAddress
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        If obj.GetType = Me.GetType Then
            Dim otherStudent As Student = CType(obj, Student)
            Return otherStudent.FirstName.Equals(Me.FirstName) And
                   otherStudent.MiddleName.Equals(Me.MiddleName) And
                   otherStudent.LastName.Equals(Me.LastName) And
                   otherStudent.AuxName.Equals(Me.AuxName) And
                   otherStudent.StudentNumber.Equals(Me.StudentNumber) And
                   otherStudent.Course.Equals(Me.Course) And
                   otherStudent.Campus.Equals(Me.Campus)
        End If
        Return False
    End Function

    Public Overrides Function ToString() As String
        Return FirstName + " " + LastName
    End Function

End Class
