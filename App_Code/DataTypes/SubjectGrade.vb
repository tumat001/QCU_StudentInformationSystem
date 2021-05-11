Public Class SubjectGrade

    Public ReadOnly Property StudentNumber As String
    Public ReadOnly Property Code As String
    Public ReadOnly Property Description As String
    Public ReadOnly Property Units As Byte
    Public ReadOnly Property Grade As String
    Public ReadOnly Property Remarks As String
    Public ReadOnly Property Professor As String
    Public ReadOnly Property [Class] As String

    Private Sub New(ByVal studentNumber As String, ByVal code As String, ByVal description As String,
                    ByVal units As Byte, ByVal grade As String, ByVal remarks As String,
                    ByVal professor As String, ByVal [class] As String)
        Me.StudentNumber = studentNumber
        Me.Code = code
        Me.Description = description
        Me.Units = units
        Me.Grade = grade
        Me.Remarks = remarks
        Me.Professor = professor
        Me.Class = [class]
    End Sub

    Class Builder

        Public Property StudentNumber As String
        Public Property Code As String
        Public Property Description As String
        Public Property Units As Byte
        Public Property Grade As String
        Public Property Remarks As String
        Public Property Professor As String
        Public Property [Class] As String

        Sub New()

        End Sub

        Function Build() As SubjectGrade
            Return New SubjectGrade(StudentNumber, Code, Description, Units, Grade, Remarks, Professor, [Class])
        End Function

    End Class

End Class
