Imports System.Data.SqlClient

Public Class GetAccountQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Gets student account information with the provided username. This information does not contain the account's password
    ''' </summary>
    ''' <param name="username">The account username whose information should be gotten from</param>
    ''' <returns>A student account that contains information. Returns Nothing when no account with such username exists</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function GetStudentAccount(username As String) As StudentAccount
        Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
        Dim ifStudentAccountExists As Boolean = accountChecker.IfStudentAccountExists(username)

        If Not ifStudentAccountExists Then
            Throw New AccountDoesNotExistException(username)
        End If

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlText As String = String.Format("SELECT [{0}] FROM [StudentAccountTable] WHERE [{1}] = @UsernameValue",
                                    STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME, STUDENT_TABLE_USERNAME_COLUMN_NAME)

            command.CommandText = sqlText
            command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
            command.Parameters.Add(New SqlParameter("UsernameValue", username))
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read Then
                Dim studentUsername As String = username
                Dim emailAddress As String = reader.GetSqlString(0).ToString()
                reader.Close()

                Return New StudentAccount(studentUsername, emailAddress)
            Else
                reader.Close()
                Return Nothing
            End If

        End Using
    End Function

    ''' <summary>
    ''' Gets all student accounts, along with their information. This does not include the account's passwords
    ''' </summary>
    ''' <returns>A list of student accounts containing their information</returns>
    Public Function GetAllStudentAccounts() As IReadOnlyList(Of StudentAccount)
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            Dim sqlCom As String = String.Format("Select [{0}], [{1}] FROM [StudentAccountTable]",
                                       STUDENT_TABLE_USERNAME_COLUMN_NAME, STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME)
            command.CommandText = sqlCom
            command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
            Dim reader As SqlDataReader = command.ExecuteReader()

            Dim listOfStudents As IList(Of StudentAccount) = New List(Of StudentAccount)
            While reader.Read
                Dim username As String = reader.GetSqlString(0).ToString()
                Dim emailAddress As String = reader.GetSqlString(1).ToString()
                listOfStudents.Add(New StudentAccount(username, emailAddress))
            End While

            Return listOfStudents
        End Using
    End Function

    ''' <summary>
    ''' Gets the admin account, along with its information. Does not include the account's password
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>AdminAccount object, which includes its information</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Function GetAdminAccount(username As String) As AdminAccount
        Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
        Dim ifAdminAccountExists As Boolean = accountChecker.IfAdminAccountExists(username)

        If Not ifAdminAccountExists Then
            Throw New AccountDoesNotExistException(username)
        End If

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)

            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            command.CommandText = String.Format("SELECT [{0}], [{1}] FROM [{2}] WHERE [{3}] = @UsernameValue",
                                  ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME,
                                  ADMIN_TABLE_NAME, ADMIN_TABLE_USERNAME_COLUMN_NAME)
            command.Parameters.Add(New SqlParameter("UsernameValue", username))

            Dim reader As SqlDataReader = command.ExecuteReader()
            If reader.Read Then
                Dim usernameOfAcc As String = reader.GetSqlString(0).ToString()
                Dim privilageMode As PrivilageMode = PrivilageMode.MapStringToAdminPrivilageMode(reader.GetSqlString(1).ToString(), PrivilageMode.NORMAL_ADMIN)
                Return New AdminAccount(usernameOfAcc, privilageMode)
            Else
                Return Nothing
            End If
        End Using
    End Function

    ''' <summary>
    ''' Gets all admin accounts, along with their information. This does not include the account's passwords
    ''' </summary>
    ''' <returns>A list of admin accounts containing their information</returns>
    Public Function GetAllAdminAccounts() As IReadOnlyList(Of AdminAccount)
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            Dim sqlCom As String = String.Format("Select [{0}], [{1}] FROM [{2}]",
                                       ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME, ADMIN_TABLE_NAME)
            command.CommandText = sqlCom
            Dim reader As SqlDataReader = command.ExecuteReader()

            Dim listOfAdmins As IList(Of AdminAccount) = New List(Of AdminAccount)
            While reader.Read
                Dim username As String = reader.GetSqlString(0).ToString()
                Dim privilageModeOfAdmin As String = reader.GetSqlString(1).ToString()
                listOfAdmins.Add(New AdminAccount(username, PrivilageMode.MapStringToAdminPrivilageMode(privilageModeOfAdmin, PrivilageMode.NORMAL_ADMIN)))
            End While

            Return listOfAdmins
        End Using
    End Function

End Class
