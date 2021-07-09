Public MustInherit Class BaseEmail

    Public ReadOnly Property RecipientAddress As String
    Public ReadOnly Property CompleteEmail As MimeKit.MimeMessage

    Protected Sub New(recipientAddress As String, completeEmail As MimeKit.MimeMessage)
        Me.RecipientAddress = recipientAddress
        Me.CompleteEmail = completeEmail
    End Sub

End Class
