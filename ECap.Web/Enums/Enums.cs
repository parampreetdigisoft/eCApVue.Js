namespace ECap.Web
{
    public enum LoginStatusCode
    {
        valid = 0,
        notvalid = -4,
        notfound = -5,
        restrictedUser = -6
    }

    public enum UserRole
    {
        admin = 0,
    }

    public enum ManageUserFilter
    {
        Name,
        UserID,
        Creation_Date,
        Client_Group,
        Group_Admin,
        User_No
    }


}
