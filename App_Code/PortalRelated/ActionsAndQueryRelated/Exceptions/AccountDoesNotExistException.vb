Public Class AccountDoesNotExistException
    Inherits System.ApplicationException

    Public ReadOnly InvalidUsername As String

    Sub New(invalidUsername As String)
        Me.InvalidUsername = invalidUsername
    End Sub

    Public Overrides Function ToString() As String
        Return "Invalid username: " + InvalidUsername
    End Function

End Class
