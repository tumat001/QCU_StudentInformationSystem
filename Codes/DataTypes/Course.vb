Public Class Course

    Public ReadOnly Property NameShort As String

    Public ReadOnly Property NameFull As String

    Public ReadOnly Property CourseId As String

    Sub New(shortName As String, fullName As String, courseId As String)
        NameShort = shortName
        NameFull = fullName
        Me.CourseId = courseId
    End Sub


    Public Overrides Function ToString() As String
        Return NameShort
    End Function

End Class
