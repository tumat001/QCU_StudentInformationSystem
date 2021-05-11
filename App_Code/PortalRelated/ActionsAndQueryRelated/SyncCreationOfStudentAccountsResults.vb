Public Class SyncCreationOfStudentAccountsResults

    Public ReadOnly Property FailedToCreateAccountsFor As IReadOnlyList(Of Student) = New List(Of Student)
    ''' <summary>
    ''' A collection of accounts with usernames as values, and passwords for keys.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CreatedAccounts As IReadOnlyDictionary(Of String, String) = New Dictionary(Of String, String)

    Sub New(ByVal failedToCreateAccountsFor As IList(Of Student),
                    ByVal createdAccounts As IDictionary(Of String, String))
        Me.FailedToCreateAccountsFor = failedToCreateAccountsFor
        Me.CreatedAccounts = createdAccounts
    End Sub

End Class
