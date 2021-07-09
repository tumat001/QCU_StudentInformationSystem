Imports System.Data.SqlClient

Public Class GetAllRequestDocumentOptionsListQuery
    Inherits BasePortalQuery

    Public Function GetAllRequestDocumentOptionList() As IReadOnlyList(Of String)
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            command.Connection = connection

            Dim listOfDocuNames As IList(Of String) = New List(Of String)
            Try
                Dim sqlCom As String = String.Format("SELECT [{0}] FROM [{1}]",
                                    DOCUMENT_TABLE_NAME_COLUMN_NAME, DOCUMENT_TABLE_NAME)
                command.CommandText = sqlCom


                Dim reader = command.ExecuteReader()
                While reader.Read()
                    listOfDocuNames.Add(reader.GetString(0))
                End While

            Catch e As Exception

            End Try


            Return listOfDocuNames
        End Using
    End Function
End Class
