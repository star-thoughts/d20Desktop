using AspNetCore.Identity.Mongo.Model;

namespace d20Web.Identity
{
    public class ApplicationUser : MongoUser
    {
        /// <summary>
        /// Gets or sets whether or not an officer verified the account
        /// </summary>
        public bool IsVerified { get; set; }
        /// <summary>
        /// Gets or sets whether this use is considered the default site admin
        /// </summary>
        /// <remarks>
        /// The site admin's password cannot be changed by others, but can be changed by the site admin
        /// </remarks>
        public bool IsSiteAdmin { get; set; }
        /// <summary>
        /// Gets or sets whether or not the user has been locked out
        /// </summary>
        /// <remarks>
        /// Use in conjunction with a failed sign-in attempt to determine if the user was lcoked out before the sign-in attempt
        /// </remarks>
        public bool WasLockedOut { get; set; }
    }
}
