Public Class StringConstraintsViolatedException
    Inherits ApplicationException

    Public ReadOnly Property errorMessage As String

    Public Sub New(errorMessage As String)
        Me.errorMessage = errorMessage
    End Sub

    Public Overrides Function ToString() As String
        Return errorMessage
    End Function

End Class
