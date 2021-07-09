Public MustInherit Class BaseAccount

    Public ReadOnly Property Username As String
    Public ReadOnly Property PrivilageMode As PrivilageMode

    Protected Sub New(username As String, privilageMode As PrivilageMode)
        Me.Username = username
        Me.PrivilageMode = privilageMode
    End Sub

    ''' <summary>
    ''' Compares this object to the given parameter
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns>True if the given object is a base account and has the same username. False otherwise</returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        If obj.GetType = Me.GetType Then
            Dim otherAccount As BaseAccount = CType(obj, BaseAccount)

            Return Me.Username.Equals(otherAccount.Username)
        Else
            Return False
        End If
    End Function

End Class
