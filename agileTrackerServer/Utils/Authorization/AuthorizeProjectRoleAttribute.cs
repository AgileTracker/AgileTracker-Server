using agileTrackerServer.Models.Enums;

namespace agileTrackerServer.Utils.Authorization;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeProjectRoleAttribute : Attribute
{
    public MemberRole[] Roles { get; }

    public AuthorizeProjectRoleAttribute(params MemberRole[] roles)
    {
        Roles = roles;
    }
}