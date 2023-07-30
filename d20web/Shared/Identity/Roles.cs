using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d20Web.Identity
{
    /// <summary>
    /// Contains role information
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Can manage other user accounts
        /// </summary>
        public const string ManageUsers = "manageusers";
        /// <summary>
        /// Can manage permissions on user accounts
        /// </summary>
        public const string ManageUserPermissions = "manageuserpermissions";

        /// <summary>
        /// Gets all roles, for updating the identity database
        /// </summary>
        /// <returns>Collection of all roles</returns>
        public static IEnumerable<string> GetAllRoles()
        {
            yield return ManageUsers;
            yield return ManageUserPermissions;
        }

        /// <summary>
        /// Gets roles used for configuring first admin
        /// </summary>
        /// <returns>Collection of all roles for site admin</returns>
        public static IEnumerable<string> GetSiteAdminRoles()
        {
            yield return ManageUsers;
            yield return ManageUserPermissions;
        }
    }
}
