Imports System.Data.SqlClient

Public Class IfAccountExistsQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Checks whether the student account associated with the given username exists
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>True if the student account with the given username exists, false otherwise</returns>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function IfStudentAccountExists(username As String) As Boolean
        PortalQueriesAndActions.NewUsernameConstraint.Evaluate(username)

        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            command.CommandText = "SELECT  " + STUDENT_TABLE_USERNAME_COLUMN_NAME + " FROM [StudentAccountTable] WHERE " + STUDENT_TABLE_USERNAME_COLUMN_NAME + " = @UsernameValue"
            command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
            command.Parameters.Add(New SqlParameter("UsernameValue", username))

            Dim studentReader As SqlDataReader = command.ExecuteReader()
            Dim studentFound As Boolean = studentReader.Read()
            studentReader.Close()

            Return studentFound

            Return False

        End Using
    End Function

    ''' <summary>
    ''' Checks whether the admin account associated with the given username exists
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>True if the admin account with the given username exists, false otherwise</returns>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function IfAdminAccountExists(username As String) As Boolean
        PortalQueriesAndActions.NewUsernameConstraint.Evaluate(username)

        Dim connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()

        command.CommandText = "SELECT * FROM [AdminAccountTable] WHERE " + ADMIN_TABLE_USERNAME_COLUMN_NAME + " = @UsernameValue"
        command.Parameters.Add(New SqlParameter("AdminAccountTable", ADMIN_TABLE_NAME))
        command.Parameters.Add(New SqlParameter("UsernameValue", username))

        Dim adminReader As SqlDataReader = command.ExecuteReader()
        If adminReader.Read Then
            connection.Close()
            Return True
        Else
            connection.Close()
            Return False
        End If
    End Function

End Class
