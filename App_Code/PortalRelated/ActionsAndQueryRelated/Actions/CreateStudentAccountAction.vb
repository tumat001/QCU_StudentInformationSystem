Imports System.Data.SqlClient

Public Class CreateStudentAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.NORMAL_ADMIN, PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="CreateAdminAccountAction"/> that allows the provided executor to create accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Attemps to create a student account with the provided username and password.
    ''' </summary>
    ''' <param name="buildInfo"></param>
    ''' <param name="password"></param>
    ''' <returns>True if the account was successfuly created, False otherwise</returns>
    ''' <exception cref="AccountAlreadyExistsException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function CreateStudentAccount(buildInfo As StudentAccount.Builder, password As String) As Boolean
        'Exception check
        If PortalQueriesAndActions.IfAccountExists(buildInfo.Username) Then
            Throw New AccountAlreadyExistsException(buildInfo.Username)
        End If

        'Input Constraints Check
        NewUsernameConstraint.Evaluate(buildInfo.Username)
        NewPasswordConstraint.Evaluate(password)
        NewEmailAddressConstraint.Evaluate(buildInfo.EmailAddress)

        'Hash the Password
        password = PortalQueriesAndActions.Hasher.HashWithGeneratedSalt(password)

        'Open Connection and Do SQL Stuffs
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("INSERT INTO [StudentAccountTable] ([{0}], [{1}], [{2}]) VALUES (@UsernameValue, @PasswordValue, @EmailAddressValue)",
                                    STUDENT_TABLE_USERNAME_COLUMN_NAME, STUDENT_TABLE_PASSWORD_COLUMN_NAME, STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
                command.Parameters.Add(New SqlParameter("UsernameValue", buildInfo.Username))
                command.Parameters.Add(New SqlParameter("PasswordValue", password))
                command.Parameters.Add(New SqlParameter("EmailAddressValue", buildInfo.EmailAddress))
                Dim success As Boolean = command.ExecuteNonQuery() = 1

                sqlTrans.Commit()
                Return success
            Catch e As Exception

                sqlTrans.Rollback()
                Return False
            End Try

        End Using
    End Function

End Class
