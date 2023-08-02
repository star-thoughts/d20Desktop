using d20Web.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace d20Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Cosntructs a new <see cref=""/>
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="roleManager">Role manager</param>
        /// <param name="signInManager">Sign in manager</param>
        /// <param name="jwtConfig">Configuration for JWT generation</param>
        public AuthController(UserManager<ApplicationUser> userManager,
                              RoleManager<ApplicationRole> roleManager,
                              SignInManager<ApplicationUser> signInManager,
                              JwtConfiguration jwtConfig)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtConfiguration _jwtConfig;

        /// <summary>
        /// Logs in a user and gets their user information
        /// </summary>
        /// <param name="info">Login information</param>
        /// <returns>Result of the operation</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([Required, FromBody] LoginRequest info)
        {
            if (info == null
                || string.IsNullOrWhiteSpace(info.UserName)
                || string.IsNullOrEmpty(info.Password))
                return BadRequest();

            ApplicationUser? appUser = _userManager.GetUser(info.UserName);

            //  Unverified users cannot log in
            if (appUser == null || !appUser.IsVerified)
                return Unauthorized();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(appUser, info.Password, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                ApplicationRole[] roles = _roleManager.Roles.ToArray();

                string token = AuthenticationHelper.GenerateJwtToken(appUser, _jwtConfig, roles);
                LoginResponse response = new LoginResponse() { Token = token, UserName = info.UserName, RoleMap = GetRoleMap() };

                return Ok(response);
            }
            //  If they didn't succeed and it was locked out, then audit that
            if (result.IsLockedOut && !appUser.WasLockedOut)
            {
                //  Show them as locked out now
                appUser.WasLockedOut = true;
                await _userManager.UpdateAsync(appUser);
            }
            return Unauthorized();
        }

        /// <summary>
        /// Registers an account, which will still need to be verified by an officer
        /// </summary>
        /// <param name="request">Acount information</param>
        /// <returns>Result of the operation</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([Required, FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrEmpty(request.Password))
                return BadRequest();

            ApplicationUser? appUser = new ApplicationUser() { UserName = request.UserName, IsVerified = false };
            IdentityResult result = await _userManager.CreateAsync(appUser, request.Password);
            appUser = _userManager.GetUser(request.UserName);

            return Ok(new RegisterResult()
            {
                IsSuccess = result.Succeeded,
                Errors = result.Errors.Select(p => p.Description).ToArray(),
            });
        }

        /// <summary>
        /// Deletes the given user
        /// </summary>
        /// <param name="userName">Name of the user to delete</param>
        /// <returns>Result of the operation</returns>
        [HttpDelete("users/{userName}")]
        [Authorize(Roles = Roles.ManageUsers, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser([Required] string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest();

            ApplicationUser? user = _userManager.GetUser(userName);

            if (user != null)
            {
                if (string.Equals(userName, User.Identity?.Name, StringComparison.OrdinalIgnoreCase))
                    return Unauthorized();

                await _userManager.DeleteAsync(user);
            }

            return Ok();
        }

        /// <summary>
        /// Gets a collection of users in the system
        /// </summary>
        /// <returns>Collection of users</returns>
        [HttpGet("users")]
        [Authorize(Roles = Roles.ManageUsers, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsers()
        {
            ApplicationUser[] appUsers = _userManager.Users.ToArray();
            List<UserInfo> users = new List<UserInfo>();

            foreach (ApplicationUser appUser in appUsers)
            {
                users.Add(await appUser.ToUserInfo(_userManager));
            }

            return Ok(new GetUsersResponse() { Users = users });
        }
        /// <summary>
        /// Gets a collection of users in the system
        /// </summary>
        /// <returns>Collection of users</returns>
        [HttpGet("users/{userName}")]
        [Authorize(Roles = Roles.ManageUsers, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsers(string userName)
        {
            ApplicationUser? user = _userManager.GetUser(userName);
            if (user == null)
                return NotFound();

            UserInfo result = await user.ToUserInfo(_userManager);

            return Ok(result);
        }

        /// <summary>
        /// Verifies a user, allowing them to log in
        /// </summary>
        /// <param name="userName">Name of the user to verify</param>
        /// <returns>Result of the opreation</returns>
        [HttpPost("users/{userName}/verify")]
        [Authorize(Roles = Roles.ManageUsers, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> VerifyUser([Required] string userName)
        {
            ApplicationUser? user = _userManager.GetUser(userName);
            if (user == null)
                return NotFound();

            user.IsVerified = true;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Updates the user's roles
        /// </summary>
        /// <param name="userName">Name of the user to update</param>
        /// <param name="roles">Comma separated list of roles</param>
        /// <returns>Resultof the operation</returns>
        [HttpPost("users/{userName}/roles")]
        [Authorize(Roles = Roles.ManageUserPermissions, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserRoles([Required] string userName, [Required] string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
                roles = string.Empty;

            ApplicationUser? user = _userManager.GetUser(userName);
            if (user != null)
            {
                IList<string> currentRoles = await _userManager.GetRolesAsync(user);
                string[] setRoles = roles.Split(new string[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
                string[] allRoles = Roles.GetAllRoles().ToArray();

                //  Make sure the roles are actually real
                if (!setRoles.All(i => allRoles.Contains(i)))
                    return BadRequest();

                string[] remove = currentRoles.Except(setRoles).ToArray();
                string[] add = setRoles.Except(currentRoles).ToArray();

                await _userManager.AddToRolesAsync(user, add);
                await _userManager.RemoveFromRolesAsync(user, remove);
            }

            return Ok();
        }

        /// <summary>
        /// Forces a change to a user's password
        /// </summary>
        /// <param name="userName">Name of the user</param>
        /// <param name="password">Password to change to</param>
        /// <returns>Result of the operation</returns>
        [HttpPost("users/{userName}/password")]
        [Authorize(Roles = Roles.ManageUsers, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> OverridePassword([Required] string userName, [Required] string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return BadRequest();
            if (string.IsNullOrWhiteSpace(password))
                return BadRequest();

            ApplicationUser? user = _userManager.GetUser(userName);
            if (user == null)
                return NotFound();
            if (user.IsSiteAdmin)
                return Unauthorized();

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, password);

            return Ok(new RegisterResult()
            {
                IsSuccess = result.Succeeded,
                Errors = result.Errors.Select(p => p.Description).ToArray(),
            });
        }

        /// <summary>
        /// Updates an existing password using the original password for verification
        /// </summary>
        /// <param name="userName">Name of the user to change</param>
        /// <param name="originalPassword">User's original password</param>
        /// <param name="password">User's new password</param>
        /// <returns></returns>
        [HttpPut("users/{userName}/password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([Required] string userName, [Required] string originalPassword, [Required] string password)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest();
            if (string.IsNullOrEmpty(password))
                return BadRequest();
            if (string.IsNullOrEmpty(originalPassword))
                return BadRequest();

            ApplicationUser? user = _userManager.GetUser(userName);
            if (user == null)
                return NotFound();

            IdentityResult result = await _userManager.ChangePasswordAsync(user, originalPassword, password);

            return Ok(new RegisterResult()
            {
                IsSuccess = result.Succeeded,
                Errors = result.Errors.Select(p => p.Description).ToArray(),
            });
        }

        /// <summary>
        /// Locks or unlocks a user's account
        /// </summary>
        /// <param name="userName">Name of the user to lock or unlock</param>
        /// <param name="lockout">Whether to lock the account, or false to unlock it</param>
        /// <returns>Result of the operation</returns>
        [HttpPut("users/{userName}/lock")]
        [Authorize(Roles = Roles.ManageUsers, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LockUser([Required] string userName, [Required] bool lockout)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest();

            ApplicationUser? user = _userManager.GetUser(userName);
            if (user == null)
                return NotFound();

            if (string.Equals(userName, User.Identity?.Name, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            IdentityResult? result = null;
            if (lockout)
            {
                //  Set the user as locked out
                result = await _userManager.SetLockoutEnabledAsync(user, enabled: true);
                //  Set the end date for the lockout
                if (result.Succeeded)
                    result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }
            else
            {
                //  Set the user as not locked out
                result = await _userManager.SetLockoutEnabledAsync(user, enabled: false);
                if (result.Succeeded)
                    await _userManager.ResetAccessFailedCountAsync(user);
            }

            if (!result.Succeeded)
                throw new LockoutFailedException(result);


            return Ok();
        }

        #region Helpers
        private Dictionary<string, string> GetRoleMap()
        {
            Dictionary<string, string> roles = new Dictionary<string, string>();
            foreach (var role in _roleManager.Roles)
            {
                if (!string.IsNullOrWhiteSpace(role.Name))
                    roles[role.Id.ToString()] = role.Name;
            }
            return roles;
        }
        #endregion
    }
}
