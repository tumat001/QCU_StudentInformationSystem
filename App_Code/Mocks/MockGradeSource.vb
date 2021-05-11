Public Class MockGradeSource
    Implements IGradeSource

    Public Function GetLatestSchoolYearGradesOfStudent(studentNumber As String, semesterTerm As Integer) As SemesterGrades Implements IGradeSource.GetLatestSchoolYearGradesOfStudent
        'If studentNumber.Equals("18-1917") Then
        If True Then
            Return GetGradeOfStudent181917()
        End If

        Return Nothing
    End Function

    Private Function GetGradeOfStudent181917() As SemesterGrades
        Dim oopGradeBuilder As SubjectGrade.Builder = New SubjectGrade.Builder()
        oopGradeBuilder.Class = "SBIT2B"
        oopGradeBuilder.Code = "OOP101"
        oopGradeBuilder.Description = "Object Oriented Programming"
        oopGradeBuilder.Grade = "3.00"
        oopGradeBuilder.Professor = "Mx generic_type"
        oopGradeBuilder.Remarks = "Passing"
        oopGradeBuilder.StudentNumber = "18-1917"
        oopGradeBuilder.Units = 3

        Dim sadGradeBuilder As SubjectGrade.Builder = New SubjectGrade.Builder()
        sadGradeBuilder.Class = "SBIT2B"
        sadGradeBuilder.Code = "SAD101"
        sadGradeBuilder.Description = "System Analysis and Design"
        sadGradeBuilder.Grade = "2.50"
        sadGradeBuilder.Professor = "Mrx generic_type"
        sadGradeBuilder.Remarks = "Passing"
        sadGradeBuilder.StudentNumber = "18-1917"
        sadGradeBuilder.Units = 3

        Dim gradeList As IList(Of SubjectGrade) = New List(Of SubjectGrade) From {
            oopGradeBuilder.Build(), sadGradeBuilder.Build()
        }

        Dim semesterGrades As SemesterGrades = New SemesterGrades("18-1917", gradeList, "2019-2020", "2nd")

        Return semesterGrades
    End Function

End Class
