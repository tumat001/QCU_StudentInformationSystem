Imports System.Data.SqlClient

Public Class IsPasswordOfAccountEqualQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Determines whether the password of the account with the provided username is equal to the provided password
    ''' </summary>
    ''' <param name="username">The account with the username to check the password of</param>
    ''' <param name="password">The password</param>
    ''' <returns>True if password of account matches with provided password. False if password does not match</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function IsPasswordOfStudentAccountEqualTo(username As String, password As String) As Boolean
        PortalQueriesAndActions.NewUsernameConstraint.Evaluate(username)
        PortalQueriesAndActions.NewPasswordConstraint.Evaluate(password)

        Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
        Dim ifStudentAccountExists As Boolean = accountChecker.IfStudentAccountExists(username)

        If Not ifStudentAccountExists Then
            Throw New AccountDoesNotExistException(username)
        End If

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            Dim sqlCom As String = String.Format("Select [{0}] FROM [StudentAccountTable] WHERE [{1}] = @UsernameValue",
                                       STUDENT_TABLE_PASSWORD_COLUMN_NAME, STUDENT_TABLE_USERNAME_COLUMN_NAME)
            command.CommandText = sqlCom
            command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
            command.Parameters.Add(New SqlParameter("UsernameValue", username))
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read Then
                Dim result As Boolean = PortalQueriesAndActions.HashValueComparator.ValueEquals(password, reader.GetSqlString(0).ToString)
                Return result
            Else
                Throw New AccountDoesNotExistException(username)
            End If
        End Using

    End Function

    ''' <summary>
    ''' Determines whether the password of the account with the provided username is equal to the provided password
    ''' </summary>
    ''' <param name="username">The account with the username to check the password of</param>
    ''' <param name="password">The password</param>
    ''' <returns>True if password of account matches with provided password. False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function IsPasswordOfAdminAccountEqualTo(username As String, password As String) As Boolean
        PortalQueriesAndActions.NewUsernameConstraint.Evaluate(username)
        PortalQueriesAndActions.NewPasswordConstraint.Evaluate(password)

        Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
        Dim ifAdminAccountExists As Boolean = accountChecker.IfAdminAccountExists(username)

        If Not ifAdminAccountExists Then
            Throw New AccountDoesNotExistException(username)
        End If


        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            Dim sqlCom As String = String.Format("Select [{0}] FROM [{1}] WHERE [{2}] = @UsernameValue",
                                   ADMIN_TABLE_PASSWORD_COLUMN_NAME, ADMIN_TABLE_NAME, ADMIN_TABLE_USERNAME_COLUMN_NAME)
            command.CommandText = sqlCom
            command.Parameters.Add(New SqlParameter("UsernameValue", username))
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read Then
                Dim result As Boolean = PortalQueriesAndActions.HashValueComparator.ValueEquals(password, reader.GetSqlString(0).ToString())

                Return result
            Else
                Throw New AccountDoesNotExistException(username)
            End If
        End Using
    End Function

End Class
