Public Class AccountAlreadyExistsException
    Inherits System.ApplicationException

    Public ReadOnly TakenUsername As String

    Sub New(takenUsername As String)
        Me.TakenUsername = takenUsername
    End Sub

    Public Overrides Function ToString() As String
        Return "Taken username: " + TakenUsername
    End Function

End Class
