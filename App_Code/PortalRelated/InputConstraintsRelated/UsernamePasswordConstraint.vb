Public Class UsernamePasswordConstraint
    Implements IStringConstraints

    Public Shared ReadOnly Property MinimumLength As Integer = 5
    Public Shared ReadOnly Property MaximumLength As Integer = 255
    Public Shared ReadOnly Property NotAllowedCharacters As IReadOnlyList(Of String) = New List(Of String) From {
        "(", ")", "[", "]", ";", "<", ">", "=", ","
    }

    Public Shared ReadOnly Property Constraints As IReadOnlyList(Of IStringConstraints) = New List(Of IStringConstraints) From {
        New StringMustNotContainCharacterConstraint(NotAllowedCharacters),
        New StringMustBeWithinLengthBoundsConstraint(MinimumLength, MaximumLength)
    }

    ''' <summary>
    ''' Evaluates the candidate new username or password to see if it can be accepted by the set constraints
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if the input passes the constraints. False otherwise. This method may throw <see cref="StringConstraintsViolatedException"/></returns>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function Evaluate(text As String) As Boolean Implements IStringConstraints.Evaluate
        For Each constraint As IStringConstraints In Constraints
            constraint.Evaluate(text)
        Next
        Return True
    End Function

    Public Function TryEvaluate(text As String) As Boolean Implements IStringConstraints.TryEvaluate
        Try
            Evaluate(text)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

End Class
