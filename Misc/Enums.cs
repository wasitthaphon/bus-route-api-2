namespace BusRouteApi.Misc
{
    [Flags]
    public enum Role
    {
        Manager,
        CarCenter,
        Clerk,
        Admin,
        Guest
    }

    [Flags]
    public enum RouteType
    {
        General,
        Special
    }
}