Public Interface IHasher

    Function HashWithoutSalt(password As String) As String
    Function HashWithSalt(password As String, salt As String) As String
    Function HashWithGeneratedSalt(password As String) As String

End Interface
