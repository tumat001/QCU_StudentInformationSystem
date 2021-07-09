Imports System.Data.SqlClient

Class ChangePrivilageModeOfAdminAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="ChangePrivilageModeOfAdminAccountAction"/> that allows the provided executor to change the privilage mode of other admin accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Changes the privilage mode of an admin account to the provided privilage mode
    ''' </summary>
    ''' <param name="username">The account to change the privilage mode of</param>
    ''' <param name="newPrivilageMode">The new privilage mode. Cannot be <see cref="PrivilageMode.DEFAULT_ADMIN"/></param>
    ''' <returns>True if privilage mode of provided account was changed. False otherwise (
    ''' or if provided privilage mode was <see cref="PrivilageMode.DEFAULT_ADMIN"/>)</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function ChangePrivilageModeOfAdminAccount(username As String, newPrivilageMode As PrivilageMode) As Boolean
        If Not PortalQueriesAndActions.AdminQueriesAndActions.IfAdminAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If

        If newPrivilageMode.Equals(PrivilageMode.DEFAULT_ADMIN) Then
            Return False
        End If

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("UPDATE [{0}] SET [{1}] = @NewPrivilageValue WHERE [{2}] = @Username AND [{3}] <> @DefaultAdmin",
                                        ADMIN_TABLE_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME, ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.AddWithValue("NewPrivilageValue", newPrivilageMode.PrivilageModeAsString)
                command.Parameters.AddWithValue("Username", username)
                command.Parameters.AddWithValue("DefaultAdmin", PrivilageMode.DEFAULT_ADMIN.PrivilageModeAsString)

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
