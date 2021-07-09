Imports System.Data.SqlClient

Public Class UpdateRequestDocumentListAction
    Inherits BasePortalAction

    Private Shared ReadOnly ALLOWED_PRIVILAGE_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        PrivilageMode.NORMAL_ADMIN, PrivilageMode.SUPER_ADMIN, PrivilageMode.DEFAULT_ADMIN
        }

    ReadOnly Property ExecutorUsername As String

    ''' <summary>
    ''' Instantiates a <see cref="CreateAdminAccountAction"/> that allows the provided executor to create accounts.
    ''' </summary>
    ''' <param name="executorUsername"></param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Sub New(executorUsername As String)
        MyBase.New(ALLOWED_PRIVILAGE_MODES, executorUsername)
        Me.ExecutorUsername = executorUsername
    End Sub


    Public Function UpdateRequestDocumentList(docList As IList(Of String)) As Boolean
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()
            Dim sqlTrans As SqlTransaction = connection.BeginTransaction()

            command.Connection = connection
            command.Transaction = sqlTrans

            Dim success As Boolean = False
            Try
                Dim sqlCom As String = String.Format("DELETE FROM {0}; ",
                                    DOCUMENT_TABLE_NAME)
                command.CommandText = sqlCom


                command.ExecuteNonQuery()
                sqlTrans.Commit()
                success = True

            Catch e As Exception

                sqlTrans.Rollback()
                success = False
            End Try

            'Then insert items -----------------
            If success And docList.Count > 0 Then
                Dim insCommand As SqlCommand = connection.CreateCommand()
                Dim insSqlTrans As SqlTransaction = connection.BeginTransaction()

                insCommand.Connection = connection
                insCommand.Transaction = insSqlTrans


                Dim sqlCom As String = String.Format("INSERT INTO [{0}] ([{1}]) VALUES ",
                                        DOCUMENT_TABLE_NAME, DOCUMENT_TABLE_NAME_COLUMN_NAME)

                Dim counter As Integer = 0
                For Each item As String In docList
                    Dim paramName As String = String.Format("@Item{0}", counter)
                    sqlCom = sqlCom & String.Format("({0})", paramName)

                    insCommand.Parameters.Add(New SqlParameter(paramName, item))
                    counter += 1

                    If docList.Count > counter Then
                        sqlCom = sqlCom & ", "
                    Else
                        sqlCom = sqlCom & ";"
                    End If
                Next

                Try

                    insCommand.CommandText = sqlCom
                    insCommand.ExecuteNonQuery()
                    success = True
                    insSqlTrans.Commit()

                Catch ex As SqlException

                    success = False
                    insSqlTrans.Rollback()
                End Try

            End If


            'Finally, return the success var
            Return success
        End Using
    End Function
End Class
