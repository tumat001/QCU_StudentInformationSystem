Imports System.Data.SqlClient

Class ChangePasswordOfOwnAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = PrivilageMode.ALL_MODES

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="ChangePasswordOfOwnAccountAction"/> that allows the provided executor to change the password of own account.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Changes the current password of the executing account with the provided password
    ''' </summary>
    ''' <param name="newPassword">The new password to use</param>
    ''' <returns>True if password of account was changed (even if the new password is equal to the old one). False otherwise</returns>
    Public Function ChangePasswordOfOwnAccount(newPassword As String) As Boolean
        Dim username As String = ExecutorUsername
        If Not PortalQueriesAndActions.IfAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If

        NewPasswordConstraint.Evaluate(newPassword)

        'Hash the password
        newPassword = PortalQueriesAndActions.Hasher.HashWithGeneratedSalt(newPassword)

        If PortalQueriesAndActions.StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Return ChangePasswordOfOwnAsStudent(newPassword)
        Else
            Return ChangePasswordOfOwnAsAdmin(newPassword)
        End If

    End Function

    Private Function ChangePasswordOfOwnAsStudent(newPassword As String) As Boolean
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("UPDATE [{0}] SET [{1}] = @NewPasswordValue WHERE [{2}] = @Username",
                                    STUDENT_TABLE_NAME, STUDENT_TABLE_PASSWORD_COLUMN_NAME, STUDENT_TABLE_USERNAME_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.AddWithValue("NewPasswordValue", newPassword)
                command.Parameters.AddWithValue("Username", ExecutorUsername)

                Dim affectedRows = command.ExecuteNonQuery()
                sqlTrans.Commit()

                Return affectedRows = 1
            Catch e As Exception

                sqlTrans.Rollback()
                Return False
            End Try
        End Using
    End Function

    Private Function ChangePasswordOfOwnAsAdmin(newPassword As String) As Boolean
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("UPDATE [{0}] SET [{1}] = @NewPasswordValue WHERE [{2}] = @Username",
                                    ADMIN_TABLE_NAME, ADMIN_TABLE_PASSWORD_COLUMN_NAME, ADMIN_TABLE_USERNAME_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.AddWithValue("NewPasswordValue", newPassword)
                command.Parameters.AddWithValue("Username", ExecutorUsername)

                Dim affectedRows = command.ExecuteNonQuery()
                sqlTrans.Commit()
                Return affectedRows = 1

            Catch e As Exception
                sqlTrans.Rollback()
                Return False
            End Try
        End Using
    End Function

End Class
