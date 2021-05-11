Imports System.Data.SqlClient

Public Class GetPrivilageModeOfAccountQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Gets the <see cref="PrivilageMode"/> of the student account.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns></returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function GetPrivilageModeOfStudentAccount(username As String) As PrivilageMode
        Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
        Dim ifStudentAccountExists As Boolean = accountChecker.IfStudentAccountExists(username)

        If Not ifStudentAccountExists Then
            Throw New AccountDoesNotExistException(username)
        End If

        Return PrivilageMode.STUDENT
    End Function

    ''' <summary>
    ''' Gets the <see cref="PrivilageMode"/> of an admin account.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>The privilage mode of the admin account</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function GetPrivilageModeOfAdminAccount(username As String) As PrivilageMode
        Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
        Dim ifAdminAccountExists As Boolean = accountChecker.IfAdminAccountExists(username)

        If Not ifAdminAccountExists Then
            Throw New AccountDoesNotExistException(username)
        End If

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)

            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            command.CommandText = String.Format("SELECT [{0}] FROM [{1}] WHERE [{2}] = @UsernameValue",
                                  ADMIN_TABLE_PRIVILAGE_COLUMN_NAME,
                                  ADMIN_TABLE_NAME, ADMIN_TABLE_USERNAME_COLUMN_NAME)
            command.Parameters.Add(New SqlParameter("UsernameValue", username))

            Dim reader As SqlDataReader = command.ExecuteReader()
            If reader.Read Then
                Dim privilageMode As PrivilageMode = privilageMode.MapStringToAdminPrivilageMode(reader.GetSqlString(0).ToString(), PrivilageMode.NORMAL_ADMIN)
                Return privilageMode
            Else
                Return Nothing
            End If
        End Using
    End Function

End Class
