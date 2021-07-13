Imports System.Data.SqlClient

Class ChangePasswordOfOtherAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.NORMAL_ADMIN, PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="ChangePasswordOfOtherAccountAction"/> that allows the provided executor to change the password of other accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Change the current password of the provided username with the provided password
    ''' </summary>
    ''' <param name="username">The account with the username to change the password of</param>
    ''' <param name="newPassword">The new password to use</param>
    ''' <returns>True if password of account was changed (even if the new password is equal to the old one). False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function ChangePasswordOfStudentAccount(username As String, newPassword As String) As Boolean
        If Not PortalQueriesAndActions.StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If
        NewPasswordConstraint.Evaluate(newPassword)

        'Hash the password
        newPassword = PortalQueriesAndActions.Hasher.HashWithGeneratedSalt(newPassword)

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
                command.Parameters.AddWithValue("Username", username)

                Dim affectedRows = command.ExecuteNonQuery()
                sqlTrans.Commit()

                Return affectedRows = 1
            Catch e As Exception

                sqlTrans.Rollback()
                Return False
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Changes the current password of the provided username with the provided password
    ''' </summary>
    ''' <param name="username">The account with the username to change the password of</param>
    ''' <param name="newPassword">The new password to use</param>
    ''' <returns>True if password of account was changed (even if the new password is equal to the old one). False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function ChangePasswordOfAdminAccount(username As String, newPassword As String) As Boolean
        NewPasswordConstraint.Evaluate(newPassword)
        If Not PortalQueriesAndActions.AdminQueriesAndActions.IfAdminAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If

        'Hash the password
        newPassword = PortalQueriesAndActions.Hasher.HashWithGeneratedSalt(newPassword)

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
                command.Parameters.AddWithValue("Username", username)

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
