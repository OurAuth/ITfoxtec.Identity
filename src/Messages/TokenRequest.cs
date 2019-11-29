using System.Text.Json.Serialization;

namespace ITfoxtec.Identity.Messages
{
    /// <summary>
    /// OAuth 2.0 Access Token Request and OIDC Token Request.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// REQUIRED. OAuth 2.0 Grant Type value that determines the method used by the client to request authorization and the types supported by the authorization server.
        /// </summary>
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// REQUIRED in Authorization Code Grant. The authorization code received from the authorization server.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// REQUIRED in Refresh Token Grant. The refresh token issued to the client.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// REQUIRED in Extension Grants. The assertion can e.g. contain a Access Token or SAML 2.0 token.
        /// </summary>
        [JsonPropertyName("assertion")]
        public string Assertion { get; set; }
        
        /// <summary>
        ///  REQUIRED, if the "redirect_uri" parameter was included in the authorization request.
        /// </summary>
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// REQUIRED, if the client is not authenticating with the authorization server as described in OAuth 2.0 Section 3.2.1.
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// OPTIONAL. The scope of the access request as described by OAuth 2.0 Section 3.3.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// REQUIRED in Resource Owner Password Credentials Grant. The resource owner username.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// REQUIRED in Resource Owner Password Credentials Grant. The resource owner password.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
