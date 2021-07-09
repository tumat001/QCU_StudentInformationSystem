''' <summary>
''' A template for an email to be sent to students when their account has been given a default password.
''' </summary>
Public Class DefaultPasswordEmailTemplate
    Inherits BaseEmail

    Private Sub New(recipient As String, completeMail As MimeKit.MimeMessage)
        MyBase.New(recipient, completeMail)
    End Sub

    Public Class Builder

        Private Shared ReadOnly EMAIL_FROM_NAME As String = "QCU Online Viewing of Grades Portal"
        Private Shared ReadOnly EMAIL_FROM As String = "ovog.testemail@gmail.com"
        Private Shared ReadOnly EMAIL_MESSAGE_BODY_GENERATOR As Func(Of String, String, String) =
            Function(recName As String, defaultPassword As String) As String
                Return String.Format(
                "Greetings {0}!
            
                The default password below has been assigned as your account's password
                {1}

                After resetting your account's password, please delete this message.
                ",
                recName, defaultPassword)
            End Function

        Public Property RecipientAddress As String = ""
        Public Property RecipientName As String = ""
        Private Property DefaultPassword As String = ""

        Public Sub New(recipientName As String, recipientAddress As String, defaultPassword As String)
            Me.RecipientName = recipientName
            Me.RecipientAddress = recipientAddress
            Me.DefaultPassword = defaultPassword
        End Sub

        Public Function Build() As DefaultPasswordEmailTemplate
            Dim msg As MimeKit.MimeMessage = New MimeKit.MimeMessage()
            msg.To.Add(New MimeKit.MailboxAddress(RecipientName, RecipientAddress))
            msg.From.Add(New MimeKit.MailboxAddress(EMAIL_FROM_NAME, EMAIL_FROM))
            msg.Body = BuildBodyOfMail()

            Return New DefaultPasswordEmailTemplate(RecipientAddress, msg)
        End Function

        Private Function BuildBodyOfMail() As MimeKit.MimeEntity
            Return New MimeKit.TextPart("plain") With {
                .Text = EMAIL_MESSAGE_BODY_GENERATOR.Invoke(RecipientName, DefaultPassword)
                }
        End Function

    End Class

End Class
