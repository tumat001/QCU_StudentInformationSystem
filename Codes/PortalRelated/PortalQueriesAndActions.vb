Imports System.Data.SqlClient
Imports System.Threading
Imports System.Windows.Forms


'TODO (PortalQueryAndActions) Future Plans
'
'Main things to add here:
'2) Action to take when default admin is not present (maybe due to assignment of new source/storage of stuffs. or a freakout situation)
'   - Maybe make a method that creates a default admin, which only works when no default admin is present
'   - Or something else (which involves making all methods non-static)... Creating
'     an instance of this class will create a Default admin when one is not present
'     (Laborful, but not sure about merits of doing so. <Look at refactor a), it may have something>
'
Public Class PortalQueriesAndActions

    Friend Shared ReadOnly PORTAL_DATABASE_NAME As String = "QCUSIS_Database"
    Friend Shared ReadOnly MASTER_DATABASE_NAME As String = "master"

    'Friend Shared ReadOnly PORTAL_DATABASE_CONNECTION_STRING_WITH_NAME As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\OnlineViewingOfGrades\App_Data\QCUSIS_Database.mdf;Integrated Security=True;Initial Catalog=" & PORTAL_DATABASE_NAME

    'Friend Shared ReadOnly PORTAL_DATABASE_CONNECTION_STRING As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\OnlineViewingOfGrades\App_Data\QCUSIS_Database.mdf;Integrated Security=True"
    Friend Shared ReadOnly PORTAL_DATABASE_CONNECTION_STRING As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\OnlineViewingOfGrades\App_Data\QCUSIS_Database.mdf;Integrated Security=True;Initial Catalog=" & PORTAL_DATABASE_NAME
    Friend Shared ReadOnly MASTER_DATABASE_CONNECTION_STRING As String = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mat\source\repos\OnlineViewingOfGrades\App_Data\master.mdf;Integrated Security=True;"


    Friend Shared ReadOnly STUDENT_TABLE_NAME As String = "StudentAccountTable"
    Friend Shared ReadOnly STUDENT_TABLE_USERNAME_COLUMN_NAME As String = "Username"
    Friend Shared ReadOnly STUDENT_TABLE_PASSWORD_COLUMN_NAME As String = "Password"
    Friend Shared ReadOnly STUDENT_TABLE_EMAIL_ADDRESS_COLUMN_NAME As String = "EmailAddress"
    Friend Shared ReadOnly STUDENT_TABLE_DISABLED_COLUMN_NAME As String = "Disabled"

    Friend Shared ReadOnly ADMIN_TABLE_NAME As String = "AdminAccountTable"
    Friend Shared ReadOnly ADMIN_TABLE_USERNAME_COLUMN_NAME As String = "Username"
    Friend Shared ReadOnly ADMIN_TABLE_PASSWORD_COLUMN_NAME As String = "Password"
    Friend Shared ReadOnly ADMIN_TABLE_PRIVILAGE_COLUMN_NAME As String = "AdminPrivilage"

    Friend Shared ReadOnly DOCUMENT_TABLE_NAME As String = "RequestDocumentTable"
    Friend Shared ReadOnly DOCUMENT_TABLE_ID_COLUMN_NAME As String = "Id"
    Friend Shared ReadOnly DOCUMENT_TABLE_NAME_COLUMN_NAME As String = "Name"

#Region "DI"
    Friend Shared ReadOnly Property NewUsernameConstraint As IStringConstraints = New UsernamePasswordConstraint()
    Friend Shared ReadOnly Property NewPasswordConstraint As IStringConstraints = New UsernamePasswordConstraint()
    Friend Shared ReadOnly Property NewEmailAddressConstraint As IStringConstraints = New EmailAddressConstraint()

    Friend Shared ReadOnly Property SaltGenerator As ISaltGenerator = New BCryptSaltGenerator()
    Friend Shared ReadOnly Property Hasher As IHasher = New BCryptHasher()
    Friend Shared ReadOnly Property HashValueComparator As IHashValueComparator = New BCryptHashValueComparator()

    'TODO Add random password generator library/API call here
    Shared Property RandomPasswordGenerator As IRandomPasswordGenerator = New MockRandomPasswordGenerator()

#End Region

    Public ReadOnly Property Executor As String
    Public ReadOnly Property StudentAccountRelated As StudentQueriesAndActions
    Public ReadOnly Property AdminAccountRelated As AdminQueriesAndActions

    ''' <summary>
    ''' Creates a PortalAccountDatabaseActions, which allows one to make changes related to the portal's accounts.
    ''' <br /><br />
    ''' The username will be used to determine the privilage level, and this determines what actions can or cannot be done.
    ''' </summary>
    ''' <param name="username">The username of an account that will act as the executor of this object's actions</param>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Sub New(username As String)
        If IfAccountExists(username) Then
            Me.Executor = username
            StudentAccountRelated = New StudentQueriesAndActions(username)
            AdminAccountRelated = New AdminQueriesAndActions(username)
        Else
            Throw New AccountDoesNotExistException(username)
        End If
    End Sub

    Public Class StudentQueriesAndActions

        Private ReadOnly Property Executor As String
        Public ReadOnly Property SyncStudentAccountRelated As SyncStudentQueriesAndActions

        Friend Sub New(username As String)
            Me.Executor = username
            Me.SyncStudentAccountRelated = New SyncStudentQueriesAndActions(username)
        End Sub

        ''' <summary>
        ''' Attemps to create a student account with the provided username and password.<br></br>
        ''' Note that this would fail if an account (student or admin) already has a username that is the same as the provided.
        ''' </summary>
        ''' <param name="buildInfo"></param>
        ''' <param name="password"></param>
        ''' <returns>True if the account was successfuly created, False otherwise</returns>
        ''' <exception cref="AccountAlreadyExistsException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function AttemptCreateStudentAccount(buildInfo As StudentAccount.Builder, password As String) As Boolean
            Return CreateStudentAccount(buildInfo, password)
        End Function

        ''' <summary>
        ''' Attemps to create a student account with the provided username. The password given will be randomly generated.<br></br>
        ''' Note that this would fail if an account (student or admin) already has a username that is the same as the provided.
        ''' </summary>
        ''' <param name="buildInfo"></param>
        ''' <returns>The generated password for the student account. Returns Nothing when the creation of the account has failed</returns>
        ''' <exception cref="AccountAlreadyExistsException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function AttemptCreateStudentAccount(buildInfo As StudentAccount.Builder) As String
            Dim generatedPassword = GenerateDefaultPassword()
            Dim accountCreated As Boolean = AttemptCreateStudentAccount(buildInfo, generatedPassword)

            If accountCreated Then
                Return generatedPassword
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' </summary>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="AccountAlreadyExistsException"></exception>
        Private Function CreateStudentAccount(buildInfo As StudentAccount.Builder, password As String) As Boolean
            Dim createAccountAction As CreateStudentAccountAction = New CreateStudentAccountAction(Executor)
            Return createAccountAction.CreateStudentAccount(buildInfo, password)
        End Function

        ''' <summary>
        ''' Checks whether the student account associated with the given username exists
        ''' </summary>
        ''' <param name="username"></param>
        ''' <returns>True if the student account with the given username exists, false otherwise</returns>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Shared Function IfStudentAccountExists(username As String) As Boolean
            Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
            Return accountChecker.IfStudentAccountExists(username)
        End Function


        ''' <summary>
        ''' Deletes the student account associated with the given username
        ''' </summary>
        ''' <param name="username"></param>
        ''' <returns>True when the student account is deleted, false otherwise</returns>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        Public Function DeleteStudentAccount(username As String) As Boolean
            Dim deleteAccountAction As DeleteStudentAccountAction = New DeleteStudentAccountAction(Executor)
            Return deleteAccountAction.DeleteStudentAccount(username)
        End Function

        ''' <summary>
        ''' Deletes all student accounts to connected database responsible for carrying student accounts
        ''' </summary>
        ''' <returns>The number of accounts deleted</returns>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        Public Function DeleteAllStudentAccounts() As Integer
            Dim deleteAccountAction As DeleteStudentAccountAction = New DeleteStudentAccountAction(Executor)
            Return deleteAccountAction.DeleteAllStudentAccounts()
        End Function

        ''' <summary>
        ''' Gets student account information with the provided username. This information does not contain the account's password
        ''' </summary>
        ''' <param name="username">The account username whose information should be gotten from</param>
        ''' <returns>A student account that contains information. Returns Nothing when no account with such username exists</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        Public Shared Function GetStudentAccount(username As String) As StudentAccount
            Dim accountGetter As GetAccountQuery = New GetAccountQuery()
            Return accountGetter.GetStudentAccount(username)
        End Function

        ''' <summary>
        ''' Gets the <see cref="PrivilageMode"/> of the student account.
        ''' </summary>
        ''' <param name="username"></param>
        ''' <returns></returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        Public Shared Function GetPrivilageModeOfStudentAccount(username As String) As PrivilageMode
            Dim privilageGetter As GetPrivilageModeOfAccountQuery = New GetPrivilageModeOfAccountQuery()
            Return privilageGetter.GetPrivilageModeOfStudentAccount(username)
        End Function

        ''' <summary>
        ''' Gets all student accounts, along with their information. This does not include the account's passwords
        ''' </summary>
        ''' <returns>A list of student accounts containing their information</returns>
        Public Shared Function GetAllStudentAccounts() As IReadOnlyList(Of StudentAccount)
            Dim accountGetter As GetAccountQuery = New GetAccountQuery()
            Return accountGetter.GetAllStudentAccounts()
        End Function

        ''' <summary>
        ''' Gets all student account usernames in the database
        ''' </summary>
        ''' <returns>A list of student usernames</returns>
        Public Shared Function GetAllStudentAccountUsernames() As IReadOnlyList(Of String)
            Dim allUsernameGetter As GetAccountUsernameQuery = New GetAccountUsernameQuery()
            Return allUsernameGetter.GetAllStudentAccountUsernames()
        End Function

        ''' <summary>
        ''' Determines whether the password of the account with the provided username is equal to the provided password
        ''' </summary>
        ''' <param name="username">The account with the username to check the password of</param>
        ''' <param name="password">The password</param>
        ''' <returns>True if password of account matches with provided password. False if password does not match</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Shared Function IsPasswordOfStudentAccountEqualTo(username As String, password As String) As Boolean
            Dim passwordChecker As IsPasswordOfAccountEqualQuery = New IsPasswordOfAccountEqualQuery()
            Return passwordChecker.IsPasswordOfStudentAccountEqualTo(username, password)
        End Function

        ''' <summary>
        ''' Determines whether the password of the account with the provided username is equal to the provided password<br></br>
        ''' Also factors in the state of if the student account is disabled.
        ''' </summary>
        ''' <param name="username">The account with the username to check the password of</param>
        ''' <param name="password">The password</param>
        ''' <returns>True if password of account matches with provided password, and is not disabled. False if password does not match</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Shared Function AttemptLogInAsStudent(username As String, password As String) As Boolean
            Dim logInSim As AttemptLogInAsStudentQuery = New AttemptLogInAsStudentQuery()
            Return logInSim.AttemptLogInAsStudent(username, password)
        End Function

        ''' <summary>
        ''' Change the current password of the provided username with the provided password
        ''' <br /><br />
        ''' No privilage level is required when changing one's own password
        ''' </summary>
        ''' <param name="username">The account with the username to change the password of</param>
        ''' <param name="newPassword">The new password to use</param>
        ''' <returns>True if password of account was changed (even if the new password is equal to the old one). False otherwise</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function ChangePasswordOfStudentAccount(username As String, newPassword As String) As Boolean
            If Executor.Equals(username) Then
                Dim changeOwnPasswordAction As ChangePasswordOfOwnAccountAction = New ChangePasswordOfOwnAccountAction(Executor)
                Return changeOwnPasswordAction.ChangePasswordOfOwnAccount(newPassword)
            Else
                Dim changePasswordAction As ChangePasswordOfOtherAccountAction = New ChangePasswordOfOtherAccountAction(Executor)
                Return changePasswordAction.ChangePasswordOfStudentAccount(username, newPassword)
            End If

        End Function

        ''' <summary>
        ''' Changes the email address of the student account into the provided email address
        ''' </summary>
        ''' <param name="username"></param>
        ''' <param name="newEmail"></param>
        ''' <returns>True if the email address of the account was changed, false otherwise</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function ChangeEmailAddressOfStudentAccount(username As String, newEmail As String) As Boolean
            Dim changeEmailAction As ChangeEmailAddressOfAccountAction = New ChangeEmailAddressOfAccountAction(Executor)
            Return changeEmailAction.ChangeEmailAddressOfStudentAccount(username, newEmail)
        End Function

        ''' <summary>
        ''' Changes the current disabled state of the provided username with the provided value
        ''' </summary>
        ''' <param name="username">The account with the username to change the password of</param>
        ''' <param name="newDisabledState">The new disabled statue to use</param>
        ''' <returns>True if disabled state of account was changed (even if the previous value is the same). False otherwise</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        Public Function ChangeDisabledStateOfStudentAccount(username As String, newDisabledState As Boolean) As Boolean
            Dim changeStateAction As ChangeDisabledStateOfStudentAccountAction = New ChangeDisabledStateOfStudentAccountAction(Executor)
            Return changeStateAction.ChangedDisabledStateOfStudentAccount(username, newDisabledState)
        End Function


        Class SyncStudentQueriesAndActions

            Private ReadOnly Property Executor As String

            Friend Sub New(username As String)
                Me.Executor = username
            End Sub

            ''' <summary>
            ''' Collects a list of students in provided student source that do not have an account based on their username.<br></br>
            ''' A student's student number is used (as the username) to determine if they possess an account.
            ''' </summary>
            ''' <param name="studentSource"></param>
            ''' <returns>A list of students who do not have an account</returns>
            Public Shared Function PeekAccountlessStudents(studentSource As IStudentSource) As IReadOnlyList(Of Student)
                Dim peeker As PeekAccountlessStudentsQuery = New PeekAccountlessStudentsQuery()
                Return peeker.PeekAccountlessStudents(studentSource)
            End Function

            ''' <summary>
            ''' Collects a list of accounts whose username (student number for this implementation) cannot be found in the student source
            ''' </summary>
            ''' <param name="studentSource"></param>
            ''' <returns>A list of student accounts not in the student source</returns>
            Public Shared Function PeekStudentAccountsNotInStudentSource(studentSource As IStudentSource) As IReadOnlyList(Of StudentAccount)
                Dim peeker As PeekStudentAccountsNotInSourceQuery = New PeekStudentAccountsNotInSourceQuery()
                Return peeker.PeekStudentAccountsNotInStudentSource(studentSource)
            End Function

            ''' <summary>
            ''' Creates accounts for students in provided student source who do not yet have an account (based on username)<br></br>
            ''' Relies on <see cref="PeekAccountlessStudents(IStudentSource)"/> to determine accounts to be created
            ''' </summary>
            ''' <param name="studentSource"></param>
            ''' <returns>A <see cref="SyncCreationOfStudentAccountsResults"/> that contains a list of students whose accounts were not created, 
            ''' and a dictionary containing the generated accounts with their generated passwords
            ''' </returns>
            ''' <exception cref="PrivilageLevelNotMetException"></exception>
            Public Function CreateAccountsForAccountlessStudents(studentSource As IStudentSource) As SyncCreationOfStudentAccountsResults
                Dim creator As CreateAccountsForAccountlessStudentsAction = New CreateAccountsForAccountlessStudentsAction(Executor)
                Return creator.CreateAccountsForAccountlessStudents(studentSource)
            End Function

            ''' <summary>
            ''' Deletes accounts of students whose student cannot be found in the provided studetntSource (based on username)<br></br>
            ''' Relies on <see cref="PeekStudentAccountsNotInStudentSource(IStudentSource)"/> to determine accounts to be deleted
            ''' </summary>
            ''' <param name="studentSource"></param>
            ''' <returns>A list of student accounts that should be deleted but were not, if any.</returns>
            ''' <exception cref="PrivilageLevelNotMetException"></exception>
            Public Function DeleteStudentAccountsNotInStudentSource(studentSource As IStudentSource) As IReadOnlyList(Of StudentAccount)
                Dim deleter As DeleteStudentAccountsNotInSourceAction = New DeleteStudentAccountsNotInSourceAction(Executor)
                Return deleter.DeleteStudentAccountsNotInStudentSource(studentSource)
            End Function

        End Class

    End Class

    Class AdminQueriesAndActions

        Private ReadOnly Property Executor As String

        Friend Sub New(username As String)
            Me.Executor = username
        End Sub

        ''' <summary>
        ''' Attemps to create an admin account with the provided attributes in the provided <see cref="AdminAccount.Builder"/>.<br /><br />
        ''' Passing <see cref="PrivilageMode.DEFAULT_ADMIN"/> as the admin privilage mode will cause this method to return false, 
        ''' since a new default admin cannot be created.
        ''' </summary>
        ''' <param name="buildInfo"></param>
        ''' <param name="password"></param>
        ''' <returns>True, if the admin account was created. False otherwise.</returns>
        ''' <exception cref="AccountAlreadyExistsException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function AttemptCreateAdminAccount(buildInfo As AdminAccount.Builder, password As String) As Boolean
            Dim adminPrivilage As PrivilageMode = buildInfo.PrivilageMode
            If adminPrivilage.Equals(PrivilageMode.DEFAULT_ADMIN) Then
                Return False
            Else
                Return CreateAdminAccount(buildInfo, password)
            End If
        End Function

        ''' <summary>
        ''' Attemps to create an admin account with the provided attributes in the provided <see cref="AdminAccount.Builder"/>. A default password will be generated for this account<br /><br />
        ''' Passing <see cref="PrivilageMode.DEFAULT_ADMIN"/> as the admin privilage mode will cause this method to return false, because a new default admin cannot be created.
        ''' </summary>
        ''' <param name="buildInfo"></param>
        ''' <returns>The generated password for this account. If the account was not created due to some error, then Nothing is returned</returns>
        ''' <exception cref="AccountAlreadyExistsException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function AttemptCreateAdminAccount(buildInfo As AdminAccount.Builder) As String
            Dim generatedPassword As String = GenerateDefaultPassword()
            Dim successfullyCreated As Boolean = AttemptCreateAdminAccount(buildInfo, GenerateDefaultPassword())

            If successfullyCreated Then
                Return generatedPassword
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Private Function CreateAdminAccount(buildInfo As AdminAccount.Builder, password As String) As Boolean
            Dim createAccountAction As CreateAdminAccountAction = New CreateAdminAccountAction(Executor)
            Return createAccountAction.CreateAdminAccount(buildInfo, password)
        End Function

        ''' <summary>
        ''' Checks whether the admin account associated with the given username exists
        ''' </summary>
        ''' <param name="username"></param>
        ''' <returns>True if the admin account with the given username exists, false otherwise</returns>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Shared Function IfAdminAccountExists(username As String) As Boolean
            Dim accountChecker As IfAccountExistsQuery = New IfAccountExistsQuery()
            Return accountChecker.IfAdminAccountExists(username)
        End Function

        ''' <summary>
        ''' Deletes specified admin account. The deletion fails if the specified admin account is the default admin.
        ''' </summary>
        ''' <returns>True if the account was deleted, False otherwise</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        Public Function DeleteAdminAccount(username As String) As Boolean
            Dim deleteAccountAction As DeleteAdminAccountAction = New DeleteAdminAccountAction(Executor)
            Return deleteAccountAction.DeleteAdminAccount(username)
        End Function

        ''' <summary>
        ''' Deletes all admin accounts, except for the default admin
        ''' </summary>
        ''' <returns>Number of deleted accounts</returns>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        Public Function DeleteAllAdminAccounts() As Integer
            Dim deleteAccountAction As DeleteAdminAccountAction = New DeleteAdminAccountAction(Executor)
            Return deleteAccountAction.DeleteAllAdminAccounts()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns>Returns the username of the default admin, or null if the admin does not exist</returns>
        Public Shared Function GetDefaultAdminUsername() As String
            Dim getter As GetDefaultAdminUsernameQuery = New GetDefaultAdminUsernameQuery()
            Return getter.GetDefaultAdminUsername()
        End Function

        ''' <summary>
        ''' Gets all admin accounts, along with their information. This does not include the account's passwords
        ''' </summary>
        ''' <returns>A list of admin accounts containing their information</returns>
        Public Shared Function GetAllAdminAccounts() As IReadOnlyList(Of AdminAccount)
            Dim getter As GetAccountQuery = New GetAccountQuery()
            Return getter.GetAllAdminAccounts()
        End Function

        Public Shared Function GetAllAdminUsernamesByPrivilageMode(privilageMode As PrivilageMode) As IReadOnlyList(Of String)
            Dim allUsernameGetter As GetAccountUsernameQuery = New GetAccountUsernameQuery()
            Return allUsernameGetter.GetAllAdminUsernamesByPrivilageMode(privilageMode)
        End Function

        ''' <summary>
        ''' Gets the admin account, along with its information. Does not include the account's password
        ''' </summary>
        ''' <param name="username"></param>
        ''' <returns>AdminAccount object, which includes its information</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        Public Shared Function GetAdminAccount(username As String) As AdminAccount
            Dim accountGetter As GetAccountQuery = New GetAccountQuery()
            Return accountGetter.GetAdminAccount(username)
        End Function

        ''' <summary>
        ''' Gets the <see cref="PrivilageMode"/> of an admin account.
        ''' </summary>
        ''' <param name="username"></param>
        ''' <returns>The privilage mode of the admin account</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        Public Shared Function GetPrivilageModeOfAdminAccount(username As String) As PrivilageMode
            Dim privilageGetter As GetPrivilageModeOfAccountQuery = New GetPrivilageModeOfAccountQuery()
            Return privilageGetter.GetPrivilageModeOfAdminAccount(username)
        End Function

        ''' <summary>
        ''' Determines whether the password of the account with the provided username is equal to the provided password
        ''' </summary>
        ''' <param name="username">The account with the username to check the password of</param>
        ''' <param name="password">The password</param>
        ''' <returns>True if password of account matches with provided password. False otherwise</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Shared Function IsPasswordOfAdminAccountEqualTo(username As String, password As String) As Boolean
            Dim passwordChecker As IsPasswordOfAccountEqualQuery = New IsPasswordOfAccountEqualQuery()
            Return passwordChecker.IsPasswordOfAdminAccountEqualTo(username, password)
        End Function

        ''' <summary>
        ''' Changes the current password of the provided username with the provided password
        ''' <br /><br />
        ''' No privilage level is required when changing one's own password
        ''' </summary>
        ''' <param name="username">The account with the username to change the password of</param>
        ''' <param name="newPassword">The new password to use</param>
        ''' <returns>True if password of account was changed (even if the new password is equal to the old one). False otherwise</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        ''' <exception cref="StringConstraintsViolatedException"></exception>
        Public Function ChangePasswordOfAdminAccount(username As String, newPassword As String) As Boolean
            If Executor.Equals(username) Then
                Dim changeOwnPasswordAction As ChangePasswordOfOwnAccountAction = New ChangePasswordOfOwnAccountAction(Executor)
                Return changeOwnPasswordAction.ChangePasswordOfOwnAccount(newPassword)
            Else
                Dim changePasswordAction As ChangePasswordOfOtherAccountAction = New ChangePasswordOfOtherAccountAction(Executor)
                Return changePasswordAction.ChangePasswordOfAdminAccount(username, newPassword)
            End If
        End Function

        ''' <summary>
        ''' Changes the privilage mode of an admin account to the provided privilage mode
        ''' </summary>
        ''' <param name="username">The account to change the privilage mode of</param>
        ''' <param name="newPrivilageMode">The new privilage mode. Cannot be <see cref="PrivilageMode.DEFAULT_ADMIN"/></param>
        ''' <returns>True if privilage mode of provided account was changed. False otherwise (
        ''' or if provided privilage mode was <see cref="PrivilageMode.DEFAULT_ADMIN"/>)</returns>
        ''' <exception cref="AccountDoesNotExistException"></exception>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        Public Function ChangePrivilageModeOfAdminAccount(username As String, newPrivilageMode As PrivilageMode) As Boolean
            Dim changePrivilageAction As ChangePrivilageModeOfAdminAccountAction = New ChangePrivilageModeOfAdminAccountAction(Executor)
            Return changePrivilageAction.ChangePrivilageModeOfAdminAccount(username, newPrivilageMode)
        End Function

        ''' <summary>
        ''' Changes the list of requestable documents (found in student's side - request document page)
        ''' </summary>
        ''' <param name="newList"></param>
        ''' <returns></returns>
        ''' <exception cref="PrivilageLevelNotMetException"></exception>
        Public Function UpdateRequestDocumentsList(newList As IList(Of String)) As Boolean
            Dim updateListAction As UpdateRequestDocumentListAction = New UpdateRequestDocumentListAction(Executor)
            Return updateListAction.UpdateRequestDocumentList(newList)
        End Function

        Public Shared Function GetAllRequestDocumentOptionsList() As IReadOnlyList(Of String)
            Dim getListAction As GetAllRequestDocumentOptionsListQuery = New GetAllRequestDocumentOptionsListQuery()
            Return getListAction.GetAllRequestDocumentOptionList()
        End Function


    End Class


#Region "NonSpeficic Account Actions"

    ''' <summary>
    ''' Checks for a student or admin account with the provided username.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>True if a student or admin account with the provided username exists. False otherwise</returns>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Shared Function IfAccountExists(username As String) As Boolean
        Return StudentQueriesAndActions.IfStudentAccountExists(username) OrElse AdminQueriesAndActions.IfAdminAccountExists(username)
    End Function

    ''' <summary>
    ''' Gets the <see cref="PrivilageMode"/> of the account.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns></returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    Public Shared Function GetPrivilageModeOfAccount(username As String) As PrivilageMode
        If Not IfAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If

        If StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Return StudentQueriesAndActions.GetPrivilageModeOfStudentAccount(username)
        Else
            Return AdminQueriesAndActions.GetPrivilageModeOfAdminAccount(username)
        End If

    End Function

    ''' <summary>
    ''' Deletes the student or admin account with the provided username.
    ''' </summary>
    ''' <param name="username"></param>
    ''' <returns>True if the deletion procedure succeeded. False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Public Function DeleteAccount(username As String) As Boolean

        Dim actionAsExecutor As PortalQueriesAndActions = New PortalQueriesAndActions(Executor)
        If StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Return actionAsExecutor.StudentAccountRelated.DeleteStudentAccount(username)
        ElseIf AdminQueriesAndActions.IfAdminAccountExists(username) Then
            Return actionAsExecutor.AdminAccountRelated.DeleteAdminAccount(username)
        Else
            Throw New AccountDoesNotExistException(username)
        End If
    End Function

    ''' <summary>
    ''' Checks if the student or admin account with the provided username has a password equal to the one provided
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="password"></param>
    ''' <returns>True if the password provided is equal to the password of the account with the provided username. False otherwise</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Shared Function IsPasswordOfAccountEqualTo(username As String, password As String) As Boolean
        If Not IfAccountExists(username) Then
            Throw New AccountDoesNotExistException(username)
        End If

        If StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Return StudentQueriesAndActions.IsPasswordOfStudentAccountEqualTo(username, password)
        Else
            Return AdminQueriesAndActions.IsPasswordOfAdminAccountEqualTo(username, password)
        End If
    End Function

    ''' <summary>
    ''' Changes the password of the student or admin account with the provided username.
    ''' <br /><br />
    ''' No privilage level is required when changing one's own password
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="newPassword"></param>
    ''' <returns>True if the password change procedure succeeded. False otherwise (and if account does not exist)</returns>
    ''' <exception cref="AccountDoesNotExistException"></exception>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    ''' <exception cref="StringConstraintsViolatedException"></exception>
    Public Function ChangePasswordOfAccount(username As String, newPassword As String) As Boolean
        Dim actionAsExecutor As PortalQueriesAndActions = New PortalQueriesAndActions(Executor)
        If StudentQueriesAndActions.IfStudentAccountExists(username) Then
            Return actionAsExecutor.StudentAccountRelated.ChangePasswordOfStudentAccount(username, newPassword)
        ElseIf AdminQueriesAndActions.IfAdminAccountExists(username) Then
            Return actionAsExecutor.AdminAccountRelated.ChangePasswordOfAdminAccount(username, newPassword)
        Else
            Throw New AccountDoesNotExistException(username)
        End If
    End Function

    Private Shared Function GenerateDefaultPassword() As String
        Return RandomPasswordGenerator.GenerateRandomPassword
    End Function


    ''' <summary>
    ''' Prompts the user to select a location to save the would be created backup database.<br></br>
    ''' This then creates a database when the prompt is finished
    ''' </summary>
    ''' <returns>True if the database backup was sucessfully created.</returns>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Public Function PromptCreateDatabaseBackup() As Boolean
        If AdminQueriesAndActions.IfAdminAccountExists(Executor) Then
            Dim privMode As PrivilageMode = AdminQueriesAndActions.GetPrivilageModeOfAdminAccount(Executor)

            If privMode Is PrivilageMode.DEFAULT_ADMIN Or privMode Is PrivilageMode.SUPER_ADMIN Then
                Dim iniFileName As String = GetDatabaseSaveFileName()

                Dim saveFileDialogInst = New SaveFileDialog()
                saveFileDialogInst.FileName = iniFileName
                saveFileDialogInst.CheckPathExists = True
                saveFileDialogInst.AddExtension = True
                saveFileDialogInst.DefaultExt = ".bak"
                saveFileDialogInst.Filter = "Bak files (*.bak)|*.bak|All files (*.*)|*.*"

                If New DialogInvoker(saveFileDialogInst).Invoke() = DialogResult.OK Then
                    Return CreateDatabaseBackup(saveFileDialogInst.FileName)
                Else
                    Return False
                End If

            Else
                Throw New PrivilageLevelNotMetException(privMode)
            End If
        Else
            Return False
        End If

    End Function

    Shared Function GetDatabaseSaveFileName() As String
        'Return "PAD_Databases_" & String.Format("{0:mmddyyyy_hhmmss}", Today)
        Return "QCUSIS_Databases_" & Now.ToString("MMddyyyy_hhmmss")
    End Function

    Private Function CreateDatabaseBackup(path As String) As Boolean
        Using connection As SqlConnection = New SqlConnection(PORTAL_DATABASE_CONNECTION_STRING)
            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            command.Connection = connection

            Try
                Dim sqlCom As String = String.Format("BACKUP DATABASE {1} TO DISK = '{0}'",
                    path, PORTAL_DATABASE_NAME)

                command.CommandText = sqlCom
                command.ExecuteNonQuery()


                Return True
            Catch e As Exception
                Throw e
                Return False
            End Try

        End Using
    End Function



    ''' <summary>
    ''' Prompts the user to select a file that will be loaded as the backup.
    ''' </summary>
    ''' <returns>True if the database backup was sucessfully loaded.</returns>
    ''' <exception cref="PrivilageLevelNotMetException"></exception>
    Public Function PromptLoadDatabaseBackup() As Boolean
        If AdminQueriesAndActions.IfAdminAccountExists(Executor) Then
            Dim privMode As PrivilageMode = AdminQueriesAndActions.GetPrivilageModeOfAdminAccount(Executor)

            If privMode Is PrivilageMode.DEFAULT_ADMIN Or privMode Is PrivilageMode.SUPER_ADMIN Then

                Dim openFileDialogInst = New OpenFileDialog()
                openFileDialogInst.CheckPathExists = True
                openFileDialogInst.Filter = "Bak files (*.bak)|*.bak|All files (*.*)|*.*"

                If New DialogInvoker(openFileDialogInst).Invoke() = DialogResult.OK Then
                    Return LoadDatabaseBackup(openFileDialogInst.FileName)
                Else
                    Return False
                End If

            Else
                Throw New PrivilageLevelNotMetException(privMode)
            End If
        Else
            Return False
        End If

    End Function


    Private Function LoadDatabaseBackup(path As String) As Boolean
        Using connection As SqlConnection = New SqlConnection(MASTER_DATABASE_CONNECTION_STRING)

            connection.Open()
            Dim command As SqlCommand = connection.CreateCommand()

            command.Connection = connection

            Try
                Dim sqlCom As String = String.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE {0} FROM DISK = '{1}' WITH RECOVERY;",
                    PORTAL_DATABASE_NAME, path)

                command.CommandText = sqlCom
                command.ExecuteNonQuery()


                Return True
            Catch e As Exception

                Return False
            End Try

        End Using
    End Function

#End Region

#Region "Dialog Invoker"

    Public Class DialogInvoker


        Private dialog As CommonDialog
        Private threadInvoker As Thread
        Private dialogResult As DialogResult

        Public Sub New(dialog As CommonDialog)
            Me.dialog = dialog

            threadInvoker = New Thread(New ThreadStart(AddressOf InvokeMethod))
            threadInvoker.SetApartmentState(ApartmentState.STA)

            dialogResult = DialogResult.None
        End Sub

        Public Function Invoke() As DialogResult
            threadInvoker.Start()
            threadInvoker.Join()
            Return dialogResult
        End Function


        Private Sub InvokeMethod()
            dialogResult = dialog.ShowDialog()
        End Sub

    End Class
#End Region

End Class
