Public Class PrivilageLevelNotMetException
    Inherits System.ApplicationException

    Public ReadOnly UnacceptedPrivilageLevel As PrivilageMode

    Sub New(unacceptedPrivilageLevel As PrivilageMode)
        Me.UnacceptedPrivilageLevel = unacceptedPrivilageLevel
    End Sub

    Public Overrides Function ToString() As String
        Return "Unaccepted privilage level: " + UnacceptedPrivilageLevel.ToString
    End Function

End Class
