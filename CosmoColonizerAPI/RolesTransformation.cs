using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;

namespace CosmoColonizerAPI
{
    public class RolesTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var ci = (ClaimsIdentity)principal.Identity;

            // Extract the roles from resource_access.CosmoColonizer.roles
            var cosmoColonizerRoles = principal.FindFirst("resource_access")?.Value;
            if (!string.IsNullOrWhiteSpace(cosmoColonizerRoles))
            {
                try
                {
                    var parsedJson = JsonDocument.Parse(cosmoColonizerRoles);
                    var roles = parsedJson.RootElement
                                            .GetProperty("CosmoColonizer")
                                            .GetProperty("roles").EnumerateArray()
                                            .Select(role => role.GetString())
                                            .Where(role => role != null);

                    foreach (var role in roles)
                    {
                        ci.AddClaim(new Claim(ci.RoleClaimType, role));
                    }
                }
                catch (JsonException) // Handle JSON parsing errors
                {
                    // Log or handle the error as needed
                }
            }

            return Task.FromResult(principal);
        }
    }
}
