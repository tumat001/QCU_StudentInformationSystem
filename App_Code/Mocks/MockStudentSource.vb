Imports System.Data.SqlClient

Public Class MockStudentSource
    Implements IStudentSource

    Private Shared ReadOnly connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\OnlineViewingOfGrades\App_Data\MockDatabases\MockStudentDatabase.mdf;Integrated Security=True"
    Private Shared ReadOnly studentTableName As String = "StudentTable"
    Private Shared ReadOnly studentNumberColumnName As String = "StudentNumber"

    Private Shared ReadOnly studentTable_studentNumberCol As Integer = 0
    Private Shared ReadOnly studentTable_firstNameCol As Integer = 1
    Private Shared ReadOnly studentTable_middleNameCol As Integer = 2
    Private Shared ReadOnly studentTable_lastNameCol As Integer = 3
    Private Shared ReadOnly studentTable_auxNameCol As Integer = 4
    Private Shared ReadOnly studentTable_courseCol As Integer = 5
    Private Shared ReadOnly studentTable_campusCol As Integer = 6
    Private Shared ReadOnly studentTable_emailAddressCol As Integer = 7

    Public Function GetAllStudents() As IReadOnlyList(Of Student) Implements IStudentSource.GetAllStudents
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = "SELECT * FROM [StudentTable]"
        command.Parameters.Add(New SqlParameter("StudentTable", studentTableName))

        Dim reader As SqlDataReader = command.ExecuteReader()

        Dim studentList As IList(Of Student) = New List(Of Student)
        While reader.Read
            studentList.Add(CreateStudentFromReader(reader))
        End While

        connection.Close()
        Return studentList
    End Function

    Private Function CreateStudentFromReader(ByRef reader As SqlDataReader) As Student
        Dim studentNumber As String = reader.GetSqlString(studentTable_studentNumberCol).ToString
        Dim firstName As String = reader.GetSqlString(studentTable_firstNameCol).ToString
        Dim middleName As String = reader.GetSqlString(studentTable_middleNameCol).ToString
        Dim lastName As String = reader.GetSqlString(studentTable_lastNameCol).ToString
        Dim auxName As String = reader.GetSqlString(studentTable_auxNameCol).ToString
        Dim course As String = reader.GetSqlString(studentTable_courseCol).ToString
        Dim campus As String = reader.GetSqlString(studentTable_campusCol).ToString
        Dim emailAddress As String = reader.GetSqlString(studentTable_emailAddressCol).ToString

        Return New Student(studentNumber, firstName, middleName, lastName, auxName, course, campus, emailAddress)
    End Function

    Public Function GetAllStudentNumbers() As IReadOnlyList(Of String) Implements IStudentSource.GetAllStudentNumbers
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = "SELECT [StudentNumberColumnName] FROM [StudentTable]"
        command.Parameters.Add(New SqlParameter("StudentTable", studentTableName))
        command.Parameters.Add(New SqlParameter("StudentNumberColumnName", studentNumberColumnName))

        Dim reader As SqlDataReader = command.ExecuteReader()
        Dim studentNumberList As IList(Of String) = New List(Of String)
        While reader.Read
            studentNumberList.Add(reader.GetString(studentTable_studentNumberCol))
        End While

        connection.Close()
        Return studentNumberList
    End Function

    Public Function GetStudent(studentNumber As String) As Student Implements IStudentSource.GetStudent
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = "SELECT * FROM [StudentTable]"
        command.Parameters.Add(New SqlParameter("StudentTable", studentTableName))

        Dim reader As SqlDataReader = command.ExecuteReader()

        Dim student As Student = Nothing
        If reader.Read Then
            student = CreateStudentFromReader(reader)
        End If

        connection.Close()
        Return student
    End Function

End Class
