Imports System.Data.SqlClient

Public Class ChangeDisabledStateOfStudentAccountAction
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
    ''' Changes the current disabled state of the provided username with the provided value
    ''' </summary>
    ''' <param name="username">The account with the username to change the password of</param>
    ''' <param name="newDisabledState">The new disabled statue to use</param>
    ''' <returns>True if disabled state of account was changed (even if the previous value is the same). False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function ChangedDisabledStateOfStudentAccount(username As String, newDisabledState As Boolean) As Boolean
        If Not PortalQueriesAndActions.StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If


        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = String.Format("UPDATE [{0}] SET [{1}] = @NewValue WHERE [{2}] = @Username",
                                    STUDENT_TABLE_NAME, STUDENT_TABLE_DISABLED_COLUMN_NAME, STUDENT_TABLE_USERNAME_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.AddWithValue("NewValue", newDisabledState)
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
