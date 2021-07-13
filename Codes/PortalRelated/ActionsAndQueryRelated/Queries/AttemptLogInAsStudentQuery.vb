Imports System.Data.SqlClient

Public Class AttemptLogInAsStudentQuery

    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Determines whether the password of the account with the provided username is equal to the provided password.<br></br>
    ''' Also factors in the state of if the student account is disabled.
    ''' </summary>
    ''' <param name="username">The account with the username to check the password of</param>
    ''' <param name="password">The password</param>
    ''' <returns>True if password of account matches with provided password, and if the account is not disabled. False if password does not match</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function AttemptLogInAsStudent(username As String, password As String) As Boolean
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

            Dim sqlCom As String = String.Format("Select [{0}] FROM [StudentAccountTable] WHERE [{1}] = @UsernameValue AND [{2}] = @NotDisabledValue",
                                       STUDENT_TABLE_PASSWORD_COLUMN_NAME, STUDENT_TABLE_USERNAME_COLUMN_NAME, STUDENT_TABLE_DISABLED_COLUMN_NAME)
            command.CommandText = sqlCom
            command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
            command.Parameters.Add(New SqlParameter("UsernameValue", username))
            command.Parameters.Add(New SqlParameter("NotDisabledValue", 0))
            Dim reader As SqlDataReader = command.ExecuteReader()

            If reader.Read Then
                Dim result As Boolean = PortalQueriesAndActions.HashValueComparator.ValueEquals(password, reader.GetSqlString(0).ToString)
                Return result
            Else
                Throw New AccountDoesNotExistException(username)
            End If
        End Using

    End Function

End Class
