Public Interface IGradeSource

    Function GetLatestSchoolYearGradesOfStudent(ByVal studentNumber As String, semesterTerm As Integer) As SemesterGrades

End Interface
