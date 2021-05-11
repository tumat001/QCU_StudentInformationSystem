Imports System.Data.SqlClient

Public Class CreateAccountsForAccountlessStudentsAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="CreateAccountsForAccountlessStudentsAction"/> that allows the provided executor to create accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Creates accounts for students in provided student source who do not yet have an account (based on username)<br></br>
    ''' Relies on <see cref="PeekAccountlessStudentsQuery.PeekAccountlessStudents(IStudentSource)"/> to determine accounts to be created
    ''' </summary>
    ''' <param name="studentSource"></param>
    ''' <returns>A <see cref="SyncCreationOfStudentAccountsResults"/> that contains a list of students whose accounts were not created, 
    ''' and a dictionary containing the generated accounts with their generated passwords
    ''' </returns>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Public Function CreateAccountsForAccountlessStudents(studentSource As IStudentSource) As SyncCreationOfStudentAccountsResults
        Dim actionAsExecutor As PortalQueriesAndActions = New PortalQueriesAndActions(ExecutorUsername)
        Dim peeker As PeekAccountlessStudentsQuery = New PeekAccountlessStudentsQuery()
        Dim accountlessStudents As IList(Of Student) = peeker.PeekAccountlessStudents(studentSource)
        Dim failedCreatedAccounts As IList(Of Student) = New List(Of Student)
        Dim createdAccounts As IDictionary(Of String, String) = New Dictionary(Of String, String)

        For Each accountlessStudent As Student In accountlessStudents
            Dim studentNumber As String = accountlessStudent.StudentNumber
            Dim emailAddress As String = accountlessStudent.EmailAddress
            Dim studentBuilder As StudentAccount.Builder = New StudentAccount.Builder(studentNumber)
            studentBuilder.EmailAddress = emailAddress

            Dim generatedPassword As String
            Try
                generatedPassword = actionAsExecutor.StudentAccountRelated.AttemptCreateStudentAccount(studentBuilder)
            Catch ex As AccountAlreadyExistsException
                generatedPassword = Nothing
            End Try

            If generatedPassword Is Nothing Then
                failedCreatedAccounts.Add(accountlessStudent)
            Else
                createdAccounts.Add(accountlessStudent.StudentNumber, generatedPassword)
            End If
        Next

        Return New SyncCreationOfStudentAccountsResults(failedCreatedAccounts, createdAccounts)
    End Function

End Class
