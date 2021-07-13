Public Class PrivilageMode


    Public Shared ReadOnly NORMAL_ADMIN As PrivilageMode = New PrivilageMode("OVoG_NormalAdmin", "Normal Admin")
    Public Shared ReadOnly SUPER_ADMIN As PrivilageMode = New PrivilageMode("OVoG_SuperAdmin", "Super Admin")
    Public Shared ReadOnly DEFAULT_ADMIN As PrivilageMode = New PrivilageMode("OVoG_DefaultAdmin", "Default Admin")
    Public Shared ReadOnly STUDENT As PrivilageMode = New PrivilageMode("OVoG_Student", "Student")

    Public Shared ReadOnly ALL_MODES As IReadOnlyList(Of PrivilageMode) = New List(Of PrivilageMode) From {
        NORMAL_ADMIN, SUPER_ADMIN, DEFAULT_ADMIN, STUDENT
        }

    Public ReadOnly Property PrivilageModeAsString As String
    Public ReadOnly Property DisplayName As String

    Private Sub New(ByVal privilageMode As String, displayName As String)
        Me.PrivilageModeAsString = privilageMode
        Me.DisplayName = displayName
    End Sub

    Public Shared Function MapStringToAdminPrivilageMode(ByVal text As String, ByRef defaultValue As PrivilageMode) As PrivilageMode
        For Each mode As PrivilageMode In ALL_MODES
            If mode.PrivilageModeAsString.Equals(text) Then
                Return mode
            End If
        Next
        Return defaultValue
    End Function

    Public Overrides Function ToString() As String
        Return PrivilageModeAsString
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If obj.GetType = Me.GetType Then
            Dim otherMode As PrivilageMode = CType(obj, PrivilageMode)
            Return otherMode.PrivilageModeAsString.Equals(Me.PrivilageModeAsString)
        End If
        Return False
    End Function


End Class
