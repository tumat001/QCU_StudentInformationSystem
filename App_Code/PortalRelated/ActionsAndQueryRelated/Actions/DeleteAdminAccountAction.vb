Imports System.Data.SqlClient

Class DeleteAdminAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="DeleteAdminAccountAction"/> that allows the provided executor to delete accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Deletes specified admin account. The deletion fails if the specified admin account is the default admin.
    ''' </summary>
    ''' <returns>True if the account was deleted, False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function DeleteAdminAccount(username As String) As Boolean
        If Not PortalQueriesAndActions.AdminQueriesAndActions.IfAdminAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                command.CommandText = String.Format("DELETE FROM [{0}] WHERE [{1}] <> @DefaultAdmin AND [{2}] = @AdminUsername",
                                  ADMIN_TABLE_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME, ADMIN_TABLE_USERNAME_COLUMN_NAME)
                command.Parameters.Add(New SqlParameter("DefaultAdmin", PrivilageMode.DEFAULT_ADMIN.ToString))
                command.Parameters.Add(New SqlParameter("AdminUsername", username))

                Dim affectedRows As Integer = command.ExecuteNonQuery()
                sqlTrans.Commit()
                Return affectedRows = 1

            Catch e As Exception

                sqlTrans.Rollback()
                Return False
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Deletes all admin accounts, except for the default admin
    ''' </summary>
    ''' <returns>Number of deleted accounts</returns>
    Public Function DeleteAllAdminAccounts() As Integer
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                command.CommandText = String.Format("DELETE FROM [{0}] WHERE [{1}] <> @DefaultAdmin",
                                  ADMIN_TABLE_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME)
                command.Parameters.Add(New SqlParameter("DefaultAdmin", PrivilageMode.DEFAULT_ADMIN.ToString))

                Dim affectedRows As Integer = command.ExecuteNonQuery()
                sqlTrans.Commit()
                Return affectedRows
            Catch e As Exception
                sqlTrans.Rollback()
                Return 0
            End Try
        End Using
    End Function

End Class
