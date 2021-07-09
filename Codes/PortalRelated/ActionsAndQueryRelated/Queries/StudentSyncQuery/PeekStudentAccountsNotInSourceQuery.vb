Public Class PeekStudentAccountsNotInSourceQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Collects a list of accounts whose username (student number for this implementation) cannot be found in the student source
    ''' </summary>
    ''' <param name="studentSource"></param>
    ''' <returns>A list of student accounts not in the student source</returns>
    Public Function PeekStudentAccountsNotInStudentSource(studentSource As IStudentSource) As IReadOnlyList(Of StudentAccount)
        Dim studentsNotInSource As IList(Of StudentAccount) = New List(Of StudentAccount)
        Dim accountsGetter As GetAccountQuery = New GetAccountQuery()

        For Each candidateAccount As StudentAccount In accountsGetter.GetAllStudentAccounts
            If studentSource.GetStudent(candidateAccount.Username) Is Nothing Then
                studentsNotInSource.Add(candidateAccount)
            End If
        Next

        Return studentsNotInSource
    End Function

End Class
