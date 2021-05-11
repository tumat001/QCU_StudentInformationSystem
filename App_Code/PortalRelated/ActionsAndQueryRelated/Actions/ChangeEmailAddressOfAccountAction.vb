Imports System.Data.SqlClient

Class ChangeEmailAddressOfAccountAction
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
    ''' Changes the email address of the student account into the provided email address
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="newEmail"></param>
    ''' <returns>True if the email address of the account was changed, false otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function ChangeEmailAddressOfStudentAccount(username As String, newEmail As String) As Boolean
        If Not PortalQueriesAndActions.StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If
        NewEmailAddressConstraint.Evaluate(newEmail)

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("UPDATE [{0}] SET [{1}] = @NewEmailValue WHERE [{2}] = @Username",
                                    STUDENT_TABLE_NAME, STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME, STUDENT_TABLE_USERNAME_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.AddWithValue("NewEmailValue", newEmail)
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
