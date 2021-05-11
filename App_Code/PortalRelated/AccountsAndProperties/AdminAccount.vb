''' <summary>
''' Represents an admin account of the portal database. This object does not contain the account's password
''' </summary>
Public Class AdminAccount
    Inherits BaseAccount

    'Public ReadOnly Property Username As String
    'Public ReadOnly Property PrivilageMode As PrivilageMode

    Sub New(ByVal username As String, ByVal privilageMode As PrivilageMode)
        MyBase.New(username, privilageMode)
    End Sub

    ''' <summary>
    ''' Compares this object to the given parameter
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns>True if the given object is an admin account and has the same username. False otherwise</returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        If obj.GetType = Me.GetType Then
            Dim otherAccount As AdminAccount = CType(obj, AdminAccount)

            Return Me.Username.Equals(otherAccount.Username)
        Else
            Return False
        End If
    End Function


    ''' <summary>
    ''' Represents a template for building admin accounts. This may not represent an already existing admin account.
    ''' </summary>
    Public Class Builder
        Public Property Username As String
        Public Property PrivilageMode As PrivilageMode
        'TODO do not allow null values to be passed
        Public Sub New(ByVal username As String, ByVal privilageMode As PrivilageMode)
            Me.Username = username
            Me.PrivilageMode = privilageMode
        End Sub

    End Class

End Class
