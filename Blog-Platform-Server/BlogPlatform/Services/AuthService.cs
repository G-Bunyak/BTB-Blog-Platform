#region Imports
using BlogPlatform.Interfaces.Helpers;
using BlogPlatform.Interfaces.Services;
using BlogPlatform.Models.AuthModels;
using BlogPlatform.Models.DatabaseModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#endregion

namespace BlogPlatform.Services
{
    public class AuthService : IAuthService
    {
        #region Properties
        private readonly IDatabaseHelper _databaseHelper;
        private IConfiguration config;

        private TimeSpan tokenLifetimeMinutes;
        private SigningCredentials issuerSigningCredentials;
        #endregion

        #region Constructor
        public AuthService(IDatabaseHelper databaseHelper, IConfiguration configuration)
        {
            config = configuration;
            _databaseHelper = databaseHelper;

            tokenLifetimeMinutes = TimeSpan.FromMinutes(int.Parse(config["AuthOptions:Lifetime"]));
            issuerSigningCredentials = new SigningCredentials(new SymmetricSecurityKey(
                                Encoding.ASCII.GetBytes(config["AuthOptions:Key"])), SecurityAlgorithms.HmacSha256);
        }
        #endregion

        #region Public methods
        public async Task<AuthResponseModel> GetTokenAsync(AuthRequestModel requestModel)
        {
            if (requestModel == null || string.IsNullOrEmpty(requestModel.Id) || string.IsNullOrEmpty(requestModel.Secret))
            {
                var response = new AuthResponseModel();
                response.Errors.Add("Id and Secret is required");
                return response;
            }

            var user = await _databaseHelper.Users.GetUserByLoginAsync(requestModel.Id);
            if (user == null)
            {
                var response = new AuthResponseModel();
                response.Errors.Add("User not found");
                return response;
            }

            if (!CheckUserPassword(requestModel.Secret, user))
            {
                var response = new AuthResponseModel();
                response.Errors.Add("Password is invalid");
                return response;
            }

            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            var jwtToken = new JwtSecurityToken(
                issuer: config["AuthOptions:Issuer"], audience: config["AuthOptions:Audience"], notBefore: now,
                claims: claimsIdentity.Claims, expires: now.Add(tokenLifetimeMinutes),
                signingCredentials: issuerSigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new AuthResponseModel() { AccessToken = encodedJwt };
        }

        public async Task<AuthResponseModel> CreateNewUserAsync(AuthRequestModel requestModel)
        {
            var response = new AuthResponseModel();
            
            if (requestModel == null || string.IsNullOrEmpty(requestModel.Id) || string.IsNullOrEmpty(requestModel.Secret))
            {
                response.Errors.Add("Id and Secret is required");
                return response;
            }

            UserModel? user = null;
            try
            {
                var existingUser = await _databaseHelper.Users.GetUserByLoginAsync(requestModel.Id);
                if (existingUser != null)
                {
                    response.Errors.Add("User with this id already exists");
                    return response;
                }

                user = await _databaseHelper.Users.InsertAsync(new UserModel()
                {
                    Login = requestModel.Id,
                    Password = requestModel.Secret
                });

            }
            catch (Exception)
            {
                response.Errors.Add("Database saving error");
                return response;
            }

            if (user == null)
            {
                response.Errors.Add("User not found");
                return response;
            }

            if (!CheckUserPassword(requestModel.Secret, user))
            {
                response.Errors.Add("Password is invalid");
                return response;
            }

            var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            var jwtToken = new JwtSecurityToken(
                issuer: config["AuthOptions:Issuer"], audience: config["AuthOptions:Audience"], notBefore: now,
                claims: claimsIdentity.Claims, expires: now.Add(tokenLifetimeMinutes),
                signingCredentials: issuerSigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new AuthResponseModel() { AccessToken = encodedJwt };
        }
        #endregion

        #region Private methods
        private bool CheckUserPassword(string password, UserModel user)
        {
            if (user.Password != password)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
