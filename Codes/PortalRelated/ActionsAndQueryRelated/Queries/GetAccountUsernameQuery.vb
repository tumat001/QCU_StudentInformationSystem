Imports System.Data.SqlClient

Public Class GetAccountUsernameQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Gets all student account usernames in the database
    ''' </summary>
    ''' <returns>A list of student usernames</returns>
    Public Function GetAllStudentAccountUsernames() As IReadOnlyList(Of String)
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            Dim sqlCom As String = String.Format("Select [{0}] FROM [StudentAccountTable]",
                                   STUDENT_TABLE_USERNAME_COLUMN_NAME)
            command.CommandText = sqlCom
            command.Parameters.Add(New SqlParameter("StudentAccountTable", STUDENT_TABLE_NAME))
            Dim reader As SqlDataReader = command.ExecuteReader()

            Dim listOfStudentUsernames As IList(Of String) = New List(Of String)
            While reader.Read
                listOfStudentUsernames.Add(reader.GetSqlString(0).ToString())
            End While

            Return listOfStudentUsernames
        End Using
    End Function

    Public Function GetAllAdminUsernamesByPrivilageMode(privilageMode As PrivilageMode) As IReadOnlyList(Of String)
        Dim connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()

        command.CommandText = String.Format("SELECT [{0}] FROM [{1}] WHERE [{2}] = @PrivilageMode",
                              ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME)
        command.Parameters.Add(New SqlParameter("PrivilageMode", privilageMode.PrivilageModeAsString))

        Dim reader As SqlDataReader = command.ExecuteReader()

        Dim usernames As IList(Of String) = New List(Of String)

        While reader.Read()
            Dim username As String = reader.GetSqlString(0).ToString
            usernames.Add(username)
        End While

        connection.Close()
        Return usernames
    End Function

End Class
