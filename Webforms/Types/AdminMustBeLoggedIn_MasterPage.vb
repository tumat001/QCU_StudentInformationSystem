Public MustInherit Class AdminMustBeLoggedIn_MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim loggedInUser As String = Session.Item(SessionConstants.LOGGED_IN_USER)
        If loggedInUser Is Nothing OrElse Not PortalQueriesAndActions.AdminQueriesAndActions.IfAdminAccountExists(loggedInUser) Then
            LogOut()
            Return
        End If

        OnPageLoad()
    End Sub

    Protected MustOverride Sub OnPageLoad()

    Public Sub LogOut()
        Session.Remove(SessionConstants.LOGGED_IN_USER)
        Session.Remove(SessionConstants.REDIRECT_HOME_ADDRESS_OF_GENERIC_PAGE)
        Response.RedirectPermanent(PageUrlConstants.LOG_IN_PAGE_URL)
    End Sub

End Class
