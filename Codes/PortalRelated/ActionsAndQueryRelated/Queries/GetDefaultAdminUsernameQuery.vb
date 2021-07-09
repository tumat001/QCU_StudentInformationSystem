Imports System.Data.SqlClient

Public Class GetDefaultAdminUsernameQuery
    Inherits BasePortalQuery

    Public Sub New()

    End Sub

    Public Function GetDefaultAdminUsername() As String
        Dim connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()

        command.CommandText = String.Format("SELECT [{0}] FROM [{1}] WHERE [{2}] = @DefaultAdmin",
                              ADMIN_TABLE_USERNAME_COLUMN_NAME, ADMIN_TABLE_NAME, ADMIN_TABLE_PRIVILAGE_COLUMN_NAME)
        command.Parameters.Add(New SqlParameter("DefaultAdmin", PrivilageMode.DEFAULT_ADMIN.PrivilageModeAsString))

        Dim reader As SqlDataReader = command.ExecuteReader()

        Dim username As String = Nothing

        If reader.Read() Then
            username = reader.GetSqlString(0).ToString()
        End If
        connection.Close()
        Return username
    End Function

End Class
