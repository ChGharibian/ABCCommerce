using ABCCommerceDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ABCCommerce.Server.Services;

public class PermissionService
{
    string[] roleQueue = [PermissionLevel.Personal, PermissionLevel.Owner, PermissionLevel.Admin, PermissionLevel.Member];
    public ABCCommerceContext AbcDb { get; }
    public PermissionService(ABCCommerceContext abcDb)
    {
        AbcDb = abcDb;
    }
    public bool IsMember(int userId, int sellerId)
    {
        return AbcDb.UserSellers.Any(u => u.UserId == userId && u.SellerId == sellerId);
    }
    public bool GetPermissionLevel(int userId, int sellerId, [NotNullWhen(true)] out string? permission)
    {
        permission = AbcDb.UserSellers.Where(u => u.UserId == userId && u.SellerId == sellerId).Select(u => u.Role).FirstOrDefault();
        return permission is not null;
    }
    public bool RoleExists(string role) => Array.IndexOf(roleQueue, role) >= 0;
    public bool HasPermission(string targetRole, string userRole)
    {
        int targetLevel = Array.IndexOf(roleQueue, targetRole);
        int userLevel = Array.IndexOf(roleQueue, userRole);
        if (targetLevel == -1 || userLevel == -1)
        {
            return false;
        }
        return userLevel <= targetLevel;
    }
    public bool HasExplicitPermission(string targetRole, string userRole)
    {
        int targetLevel = Array.IndexOf(roleQueue, targetRole);
        int userLevel = Array.IndexOf(roleQueue, userRole);
        if (targetLevel == -1 || userLevel == -1)
        {
            return false;
        }
        return userLevel < targetLevel;
    }

    public void SetPermission(int userId, int sellerId, string newRole)
    {
        var userSeller = AbcDb.UserSellers.Where(u => u.UserId == userId && u.SellerId == sellerId).FirstOrDefault();
        if (userSeller is null)
        {
            userSeller = new ABCCommerceDataAccess.Models.UserSeller()
            {
                UserId = userId,
                SellerId = sellerId
            };
            AbcDb.UserSellers.Add(userSeller);
        }
        userSeller.Role = newRole;
        AbcDb.SaveChanges();
    }

    public void DeleteMember(int userId, int sellerId)
    {
        AbcDb.UserSellers.Where(u => u.UserId == userId && u.SellerId == sellerId).ExecuteDelete();
        AbcDb.SaveChanges();
    }
}
public static class PermissionLevel
{
    public static string Owner { get; } = nameof(Owner);
    public static string Admin { get; } = nameof(Admin);
    public static string Member { get; } = nameof(Member);
    public static string Personal { get; } = nameof(Personal);
}