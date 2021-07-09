Public Interface IGradeSource

    Function GetLatestSemGradesOfStudent(ByVal studentNumber As String) As SemesterGrades

    Function GetGradesOfStudentInSemYear(studentNumber As String, sem As Integer, year As Integer) As SemesterGrades

    'Function GetAllGradesOfStudent(studentNumber As String) As IReadOnlyList(Of SemesterGrades)

    Function GetAllSemYearCourseWithStudent(studentNumber As String) As IReadOnlyList(Of String)

    Function ConvertSemYearCourseStringToSemAndYearArray(semYear As String) As String()

End Interface
