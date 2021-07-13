Imports System.Data.SqlClient

Public NotInheritable Class MockGradeSource
    Implements IGradeSource

    Private Shared ReadOnly connectionString As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\OnlineViewingOfGrades\App_Data\MockDatabases\MockStudentDatabase.mdf;Integrated Security=True"
    Private Shared ReadOnly semesterTableName As String = "SemesterGradeTable"

    Private Shared ReadOnly studentNumberColumnName As String = "StudentNumber"
    Private Shared ReadOnly codeColumnName As String = "Code"
    Private Shared ReadOnly classColumnName As String = "Class"
    Private Shared ReadOnly descriptionColumnName As String = "Description"
    Private Shared ReadOnly gradeColumnName As String = "Grade"
    Private Shared ReadOnly professorColumnName As String = "Professor"
    Private Shared ReadOnly unitsColumnName As String = "Units"
    Private Shared ReadOnly semesterColumnName As String = "Semester"
    Private Shared ReadOnly yearColumnName As String = "Year"
    Private Shared ReadOnly courseIdColumnName As String = "CourseId"

    Private Shared ReadOnly studentNumberColumnNum As Integer = 0
    Private Shared ReadOnly codeColumnNum As Integer = 1
    Private Shared ReadOnly classColumnNum As Integer = 2
    Private Shared ReadOnly descriptionColumnNum As Integer = 3
    Private Shared ReadOnly gradeColumnNum As Integer = 4
    Private Shared ReadOnly professorColumnNum As Integer = 5
    Private Shared ReadOnly unitsColumnNum As Integer = 6
    Private Shared ReadOnly semesterColumnNum As Integer = 7
    Private Shared ReadOnly yearColumnNum As Integer = 8
    Private Shared ReadOnly courseIdColumnNum As Integer = 9


    Private Function ConstructSemesterGrade(ByRef reader As SqlDataReader) As SemesterGrades
        If reader.Read() Then
            Dim studentNumber As String = reader.GetString(studentNumberColumnNum).ToString()
            Dim semester As Integer = reader.GetInt32(semesterColumnNum)
            Dim year As Integer = reader.GetInt32(yearColumnNum)

            Dim subjGradesList As IList(Of SubjectGrade) = New List(Of SubjectGrade)
            Dim firstSubjGrade As SubjectGrade = ConstructSubjectGrade(reader, studentNumber)
            subjGradesList.Add(firstSubjGrade)

            While reader.Read
                subjGradesList.Add(ConstructSubjectGrade(reader, studentNumber))
            End While

            Return New SemesterGrades(studentNumber, subjGradesList, year, year + 1, semester)

        Else
            Return Nothing
        End If
    End Function

    Private Function ConstructSubjectGrade(ByRef reader As SqlDataReader, studentNumber As String) As SubjectGrade
        Dim classAsText As String = reader.GetString(classColumnNum).ToString()
        Dim code As String = reader.GetString(codeColumnNum).ToString()
        Dim description As String = reader.GetString(descriptionColumnNum).ToString()
        Dim grade As String = reader.GetString(gradeColumnNum).ToString()
        Dim professor As String = reader.GetString(professorColumnNum).ToString()
        Dim units As String = reader.GetInt32(unitsColumnNum)
        Dim course As String = reader.GetString(courseIdColumnNum).ToString()

        Dim subjGradeBuilder As SubjectGrade.Builder = New SubjectGrade.Builder()
        subjGradeBuilder.Class = classAsText
        subjGradeBuilder.Code = code
        subjGradeBuilder.Description = description
        subjGradeBuilder.Grade = grade
        subjGradeBuilder.Professor = professor
        subjGradeBuilder.Remarks = GetRemarks(grade)
        subjGradeBuilder.StudentNumber = studentNumber
        subjGradeBuilder.Units = units
        subjGradeBuilder.Course = course

        Return subjGradeBuilder.Build()
    End Function

    Private Function GetRemarks(grade As String) As String
        Try
            Dim gradeNum As Decimal = Decimal.Parse(grade)
            If gradeNum = 5.0 Then
                Return "Failing"
            Else
                Return "Passing"
            End If

        Catch e As Exception
            Return "Invalid"
        End Try
    End Function


    Public Function GetLatestSemGradesOfStudent(studentNumber As String) As SemesterGrades Implements IGradeSource.GetLatestSemGradesOfStudent
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = String.Format("SELECT [{0}], [{1}] FROM [{2}] WHERE {3} = @StudentNumber ORDER BY {1} DESC, {0} DESC",
            semesterColumnName, yearColumnName, semesterTableName, studentNumberColumnName)
        command.Parameters.Add(New SqlParameter("StudentNumber", studentNumber))

        Dim reader As SqlDataReader = command.ExecuteReader()

        Dim highestSem As Integer = 0
        Dim highestYear As Integer = 0
        Dim hasResult As Boolean = False

        Try
            If reader.Read() Then
                highestSem = reader.GetInt32(0)
                highestYear = reader.GetInt32(1)
                hasResult = True
            End If
        Catch e As SqlException

        End Try

        connection.Close()

        If hasResult Then
            Return GetGradesOfStudentInSemYear(studentNumber, highestSem, highestYear)
        Else
            Return Nothing
        End If
    End Function


    Public Function GetGradesOfStudentInSemYear(studentNumber As String, sem As Integer, year As Integer) As SemesterGrades Implements IGradeSource.GetGradesOfStudentInSemYear
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = String.Format("SELECT * FROM [{0}] WHERE {1} = @StudentNumber AND {2} = @semester AND {3} = @year",
            semesterTableName, studentNumberColumnName, semesterColumnName, yearColumnName)
        command.Parameters.Add(New SqlParameter("StudentNumber", studentNumber))
        command.Parameters.Add(New SqlParameter("semester", sem))
        command.Parameters.Add(New SqlParameter("year", year))

        Dim reader As SqlDataReader = command.ExecuteReader()

        Dim semGrade As SemesterGrades = Nothing

        Try
            semGrade = ConstructSemesterGrade(reader)
        Catch e As SqlException

        End Try

        connection.Close()
        Return semGrade
    End Function


    'Public Function GetAllGradesOfStudent(studentNumber As String) As IReadOnlyList(Of SemesterGrades) Implements IGradeSource.GetAllGradesOfStudent
    '    Dim connection As SqlConnection = New SqlConnection(connectionString)

    '    connection.Open()
    '    Dim command As SqlCommand = connection.CreateCommand()
    '    command.CommandText = String.Format("SELECT TOP 1 [{0}], [{1}] FROM [{2}] WHERE {3} = @StudentNumber ORDER BY {1} DESC, {0} DESC",
    '        semesterColumnName, yearColumnName, semesterTableName, studentNumberColumnName)
    '    command.Parameters.Add(New SqlParameter("StudentNumber", studentNumber))

    '    Dim reader As SqlDataReader = command.ExecuteReader()
    '    Dim semGradesList As IList(Of SemesterGrades) = New List(Of SemesterGrades)

    '    While reader.Read()
    '        Dim sem As Integer = reader.GetInt32(0)
    '        Dim year As Integer = reader.GetInt32(1)

    '        semGradesList.Add(GetGradesOfStudentInSemYear(studentNumber, sem, year))
    '    End While

    '    connection.Close()

    '    Return semGradesList
    'End Function


    Public Function GetAllSemYearCourseWithStudent(studentNumber As String) As IReadOnlyList(Of String) Implements IGradeSource.GetAllSemYearCourseWithStudent
        Dim connection As SqlConnection = New SqlConnection(connectionString)

        connection.Open()
        Dim command As SqlCommand = connection.CreateCommand()
        command.CommandText = String.Format("SELECT DISTINCT [{0}], [{1}], [{2}] FROM [{3}] WHERE {4} = @StudentNumber ORDER BY {1} DESC, {0} DESC",
            semesterColumnName, yearColumnName, courseIdColumnName, semesterTableName, studentNumberColumnName)
        command.Parameters.Add(New SqlParameter("StudentNumber", studentNumber))

        Dim reader As SqlDataReader = command.ExecuteReader()
        Dim semYearList As IList(Of String) = New List(Of String)

        Try
            While reader.Read()
                Dim sem As Integer = reader.GetInt32(0)
                Dim year As Integer = reader.GetInt32(1)
                Dim course As String = reader.GetString(2).ToString()

                semYearList.Add(String.Format("{0}-{1}-{2}", sem, year, course))
            End While
        Catch e As SqlException

        End Try

        connection.Close()

        Return semYearList
    End Function

    Public Function ConvertSemYearCourseStringToSemAndYearArray(semYear As String) As String() Implements IGradeSource.ConvertSemYearCourseStringToSemAndYearArray
        Return semYear.Split("-")
    End Function

End Class
