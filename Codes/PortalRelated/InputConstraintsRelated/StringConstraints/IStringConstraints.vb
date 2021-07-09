Public Interface IStringConstraints

    Function Evaluate(text As String) As Boolean

    Function TryEvaluate(text As String) As Boolean

End Interface
