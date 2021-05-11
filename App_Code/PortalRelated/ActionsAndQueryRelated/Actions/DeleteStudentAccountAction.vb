Imports System.Data.SqlClient

Public Class DeleteStudentAccountAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.NORMAL_ADMIN, PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="DeleteStudentAccountAction"/> that allows the provided executor to delete accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub

    ''' <summary>
    ''' Deletes the student account associated with the given username
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>True when the student account is deleted, false otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function DeleteStudentAccount(username As String) As Boolean
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
                Dim sqlCom As String = String.Format("DELETE FROM [StudentAccountTable] WHERE [{0}] = @UsernameValue",
                                    STUDENT_TABLE_USERNAME_COLUMN_NAME)
                command.CommandText = sqlCom
                command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
                command.Parameters.Add(New SqlParameter("UsernameValue", username))
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
    ''' Deletes all student accounts to connected database responsible for carrying student accounts
    ''' </summary>
    ''' <returns>The number of accounts deleted</returns>
    Public Function DeleteAllStudentAccounts() As Integer
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Try
                Dim sqlCom As String = "DELETE FROM [StudentAccountTable]"
                command.CommandText = sqlCom
                command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
                Dim affectedRows As Integer = command.ExecuteNonQuery()
                sqlTrans.Commit()

                Return affectedRows
            Catch e As Exception
                sqlTrans.Rollback()
                Return False
            End Try

        End Using
    End Function

End Class
