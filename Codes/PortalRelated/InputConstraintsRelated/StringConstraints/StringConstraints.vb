Public Class StringMustContainCharacterConstraint
    Implements IStringConstraints

    Public Class StringMustContainCharacterException
        Inherits StringConstraintsViolatedException

        Public ReadOnly Property MustContainCharacters As IReadOnlyList(Of String)

        Public Sub New(mustContainCharacterList As IReadOnlyList(Of String), customErrorMessage As String)
            MyBase.New(customErrorMessage)
            Me.MustContainCharacters = mustContainCharacterList
        End Sub

        Public Sub New(mustContainCharacterList As IReadOnlyList(Of String))
            Me.New(mustContainCharacterList, "must contain " + GenerateMessageListOfConstraints(mustContainCharacterList))
        End Sub

        Private Shared Function GenerateMessageListOfConstraints(mustContainCharacterList As IReadOnlyList(Of String)) As String
            Dim stringBuilder As StringBuilder = New StringBuilder()

            If mustContainCharacterList.Count > 0 Then
                For i As Integer = 0 To mustContainCharacterList.Count - 1
                    stringBuilder.Append(mustContainCharacterList.Item(i))

                    If i = mustContainCharacterList.Count - 2 Then
                        stringBuilder.Append("and ")
                    ElseIf Not i = mustContainCharacterList.Count - 1 Then
                        stringBuilder.Append(" ")
                    End If
                Next
            End If

            Return stringBuilder.ToString
        End Function

    End Class

    Public ReadOnly Property mustContainCharacterList As IReadOnlyList(Of String)
    Public Property customErrorMessage As String

    ''' <summary>
    ''' Sets the constraints to the provided strings. The text must contain all the strings provided to pass the constraint
    ''' </summary>
    ''' <param name="characters"></param>
    ''' <param name="errorMessage">The custom error message in the thrown <see cref="StringConstraintsViolatedException"/> when calling <see cref="Evaluate(String)"/></param>
    Public Sub New(ByVal characters As List(Of String), ByVal errorMessage As String)
        mustContainCharacterList = characters.ToList
        Me.customErrorMessage = errorMessage
    End Sub

    ''' <summary>
    ''' Sets the constraints to the provided strings. The text must contain all the strings provided to pass the constraint
    ''' </summary>
    ''' <param name="characters"></param>
    Public Sub New(ByVal characters As List(Of String))
        mustContainCharacterList = characters.ToList
        customErrorMessage = Nothing
    End Sub

    ''' <summary>
    ''' Sets the constraints to the provided strings. The text must contain all the strings provided to pass the constraint
    ''' </summary>
    ''' <param name="characters"></param>
    Public Sub New(ByVal ParamArray characters() As String)
        Me.New(characters.ToList)
    End Sub

    ''' <summary>
    ''' Evaluates the text to see if text follows the given constraints.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if the text follows the constraints. Throws exception otherwise.</returns>
    ''' <exception cref="StringMustContainCharacterException"></exception>
    ''' <exception cref="NullReferenceException"></exception>
    Public Function Evaluate(text As String) As Boolean Implements IStringConstraints.Evaluate
        If text Is Nothing Then
            Throw New NullReferenceException()
        End If

        For Each character As String In mustContainCharacterList
            If Not text.Contains(character) Then
                If customErrorMessage IsNot Nothing Then
                    Throw New StringMustContainCharacterException(mustContainCharacterList, customErrorMessage)
                Else
                    Throw New StringMustContainCharacterException(mustContainCharacterList)
                End If
            End If
        Next

        Return True
    End Function

    ''' <summary>
    ''' Evaluates the text to see if text follows the given constraints.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if the text follows the constraints. False otherwise (or if text is null).</returns>
    Public Function TryEvaluate(text As String) As Boolean Implements IStringConstraints.TryEvaluate
        Try
            Return Evaluate(text)
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class

Public Class StringMustNotContainCharacterConstraint
    Implements IStringConstraints

    Public Class StringMustNotContainCharacterException
        Inherits StringConstraintsViolatedException

        Public ReadOnly Property MustNotContainCharacters As IReadOnlyList(Of String)

        Public Sub New(mustNotContainCharacterList As IReadOnlyList(Of String), errorMessage As String)
            MyBase.New(errorMessage)
            Me.MustNotContainCharacters = mustNotContainCharacterList
        End Sub

        Public Sub New(mustNotContainCharacterList As IReadOnlyList(Of String))
            Me.New(mustNotContainCharacterList, "must not contain " + GenerateMessageListOfConstraints(mustNotContainCharacterList))
        End Sub

        Private Shared Function GenerateMessageListOfConstraints(mustNotContainCharacters As IReadOnlyList(Of String)) As String
            Dim stringBuilder As StringBuilder = New StringBuilder()

            If mustNotContainCharacters.Count > 0 Then
                For i As Integer = 0 To mustNotContainCharacters.Count - 1
                    stringBuilder.Append(mustNotContainCharacters.Item(i))

                    If i = mustNotContainCharacters.Count - 2 Then
                        stringBuilder.Append("and ")
                    ElseIf Not i = mustNotContainCharacters.Count - 1 Then
                        stringBuilder.Append(" ")
                    End If
                Next
            End If

            Return stringBuilder.ToString
        End Function

    End Class

    Public ReadOnly Property MustNotContainCharacters As IReadOnlyList(Of String)
    Public Property customErrorMessage As String

    ''' <summary>
    ''' Sets the constraints to the provided strings. The text must not contain all the strings provided to pass the constraint
    ''' </summary>
    ''' <param name="characters"></param>
    ''' <param name="customErrorMessage">The custom error message in the thrown <see cref="StringConstraintsViolatedException"/> when calling <see cref="Evaluate(String)"/></param>
    Public Sub New(ByVal characters As IReadOnlyList(Of String), ByVal customErrorMessage As String)
        MustNotContainCharacters = characters.ToList
        Me.customErrorMessage = customErrorMessage
    End Sub

    ''' <summary>
    ''' Sets the constraints to the provided strings. The text must not contain all the strings provided to pass the constraint
    ''' </summary>
    ''' <param name="characters"></param>
    Public Sub New(ByVal characters As IReadOnlyList(Of String))
        MustNotContainCharacters = characters.ToList
        customErrorMessage = Nothing
    End Sub

    ''' <summary>
    ''' Sets the constraints to the provided strings. The text must not contain all the strings provided to pass the constraint
    ''' </summary>
    ''' <param name="characters"></param>
    Public Sub New(ByVal ParamArray characters() As String)
        Me.New(characters.ToList)
    End Sub

    ''' <summary>
    ''' Evaluates the text to see if text follows the given constraints.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if the text follows the constraints. Throws exception otherwise.</returns>
    ''' <exception cref="StringMustNotContainCharacterException"></exception>
    Public Function Evaluate(text As String) As Boolean Implements IStringConstraints.Evaluate
        For Each character As String In MustNotContainCharacters
            If text.Contains(character) Then
                If customErrorMessage IsNot Nothing Then
                    Throw New StringMustNotContainCharacterException(MustNotContainCharacters, customErrorMessage)
                Else
                    Throw New StringMustNotContainCharacterException(MustNotContainCharacters)
                End If
            End If
        Next

        Return True
    End Function

    ''' <summary>
    ''' Evaluates the text to see if text follows the given constraints.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if the text follows the constraints. False otherwise.</returns>
    Public Function TryEvaluate(text As String) As Boolean Implements IStringConstraints.TryEvaluate
        Try
            Return Evaluate(text)
        Catch ex As StringConstraintsViolatedException
            Return False
        End Try
    End Function
End Class

Public Class StringMustBeWithinLengthBoundsConstraint
    Implements IStringConstraints

    Public Class StringLowerThanBoundsException
        Inherits StringConstraintsViolatedException

        Public ReadOnly Property violatingValue As Integer
        Public ReadOnly Property minimumBound As Integer

        Public Sub New(violatingValue As Integer, minimumBound As Integer, errorMessage As String)
            MyBase.New(errorMessage)
            Me.violatingValue = violatingValue
            Me.minimumBound = minimumBound
        End Sub

        Public Sub New(violatingValue As Integer, minimumBound As Integer)
            Me.New(violatingValue, minimumBound, String.Format("length is below {0}. Must be at least {1}", violatingValue, minimumBound))
        End Sub

    End Class

    Public Class StringHigherThanBoundsException
        Inherits StringConstraintsViolatedException

        Public ReadOnly Property violatingValue As Integer
        Public ReadOnly Property maximumBound As Integer

        Public Sub New(violatingValue As Integer, maximumBound As Integer, errorMessage As String)
            MyBase.New(errorMessage)
            Me.violatingValue = violatingValue
            Me.maximumBound = maximumBound
        End Sub

        Public Sub New(violatingValue As Integer, maximumBound As Integer)
            Me.New(violatingValue, maximumBound, String.Format("length is above {0}. Must be at least {1}", violatingValue, maximumBound))
        End Sub

    End Class

    Public ReadOnly Property MinLengthInclusive As Integer
    Public ReadOnly Property MaxLengthInclusive As Integer
    Private ReadOnly errorMessage As String

    Public Sub New(minLengthInclusive As Integer, maxLengthInclusive As Integer, customErrorMessage As String)
        Me.MinLengthInclusive = minLengthInclusive
        Me.MaxLengthInclusive = maxLengthInclusive
        Me.errorMessage = customErrorMessage
    End Sub

    Public Sub New(minLengthInclusive As Integer, maxLengthInclusive As Integer)
        Me.MinLengthInclusive = minLengthInclusive
        Me.MaxLengthInclusive = maxLengthInclusive
        errorMessage = GenerateErrorMessage()
    End Sub

    Private Function GenerateErrorMessage()
            Return String.Format("must be {0} characters or more, and only up to {1} characters",
                        MinLengthInclusive, MaxLengthInclusive)
        End Function

    ''' <summary>
    ''' Evaluates the text to see if the length is within bounds of the constraint
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if text passes constraints. Throws <see cref="StringConstraintsViolatedException"/> otherwise</returns>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function Evaluate(text As String) As Boolean Implements IStringConstraints.Evaluate
        If text.Length >= MinLengthInclusive And text.Length <= MaxLengthInclusive Then
            Return True
        ElseIf text.Length < MinLengthInclusive Then
            Throw New StringLowerThanBoundsException(text.Length, MinLengthInclusive)
        Else
            Throw New StringHigherThanBoundsException(text.Length, MaxLengthInclusive)
        End If
    End Function

    ''' <summary>
    ''' Evaluates the text to see if the length is within bounds of the constraint
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>True if text passes constraints. False otherwise</returns>
    Public Function TryEvaluate(text As String) As Boolean Implements IStringConstraints.TryEvaluate
            Try
                Return Evaluate(text)
            Catch ex As StringConstraintsViolatedException
                Return False
            End Try
        End Function
    End Class
