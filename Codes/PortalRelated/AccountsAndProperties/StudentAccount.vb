''' <summary>
''' Represents an existing student account of the portal database. This does not contain the account's password
''' </summary>
Public Class StudentAccount
    Inherits BaseAccount

    Public ReadOnly Property EmailAddress As String

    Sub New(ByVal username As String, ByVal emailAddress As String)
        MyBase.New(username, PrivilageMode.STUDENT)
        Me.EmailAddress = emailAddress
    End Sub

    ''' <summary>
    ''' Compares this object to the given parameter
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns>True if the given object is a student account and has the same username. False otherwise</returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        If obj.GetType = Me.GetType Then
            Dim otherAccount As StudentAccount = CType(obj, StudentAccount)

            Return Me.Username.Equals(otherAccount.Username)
        Else
            Return False
        End If
    End Function


    ''' <summary>
    ''' Represents a template for building student accounts. This may not represent an already existing student account.
    ''' </summary>
    Public Class Builder
        'TODO do not allow null values to be passed
        Public Property Username As String
        Public Property EmailAddress As String = ""

        Public Sub New(username As String)
            Me.Username = username
        End Sub

    End Class

End Class