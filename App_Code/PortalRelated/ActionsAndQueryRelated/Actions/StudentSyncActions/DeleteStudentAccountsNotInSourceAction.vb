Public Class DeleteStudentAccountsNotInSourceAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
       PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
       }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="DeleteStudentAccountsNotInSourceAction"/> that allows the provided executor to create accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub


    ''' <summary>
    ''' Deletes accounts of students whose student cannot be found in the provided (based on username)<br></br>
    ''' Relies on <see cref="PeekStudentAccountsNotInSourceQuery.PeekStudentAccountsNotInStudentSource(IStudentSource)"/> to determine accounts to be deleted
    ''' </summary>
    ''' <param name="studentSource"></param>
    ''' <returns>A list of student accounts that should be deleted but were not, if any. This list does not include student accounts whose students are in the provided student source</returns>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Public Function DeleteStudentAccountsNotInStudentSource(studentSource As IStudentSource) As IReadOnlyList(Of StudentAccount)
        Dim actionsAsUser As PortalQueriesAndActions = New PortalQueriesAndActions(ExecutorUsername)
        Dim peeker As PeekStudentAccountsNotInSourceQuery = New PeekStudentAccountsNotInSourceQuery()
        Dim accountsToDelete As IList(Of StudentAccount) = peeker.PeekStudentAccountsNotInStudentSource(studentSource)
        Dim failedToDeleteAccounts As IList(Of StudentAccount) = New List(Of StudentAccount)

        For Each accountToDelete As StudentAccount In accountsToDelete
            Dim success As Boolean
            Try
                success = actionsAsUser.StudentAccountRelated.DeleteStudentAccount(accountToDelete.Username)
            Catch ex As AccountDoesNotExistException
                'In a case where someone delete the account before this method does
                success = True
            End Try

            If Not success Then
                failedToDeleteAccounts.Add(accountToDelete)
            End If
        Next

        Return failedToDeleteAccounts
    End Function

End Class
