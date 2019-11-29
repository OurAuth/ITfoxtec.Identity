using System.Text.Json.Serialization;

namespace ITfoxtec.Identity.Messages
{
    /// <summary>
    /// OAuth 2.0 Authorization Request.
    /// </summary>
    public class AuthorizationRequest
    {
        /// <summary>
        /// REQUIRED. OAuth 2.0 Response Type value that determines the authorization processing flow to be used, including what parameters are returned from the endpoints used.
        /// </summary>
        [JsonPropertyName("response_type")]
        public string ResponseType { get; set; }

        /// <summary>
        /// REQUIRED. OAuth 2.0 Client Identifier valid at the Authorization Server.
        /// </summary>
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// REQUIRED. Redirection URI to which the response will be sent.
        /// </summary>
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// OAuth 2.0 OPTIONAL. The scope of the access request as described by OAuth 2.0 Section 3.3.
        /// OIDC REQUIRED. OpenID Connect requests MUST contain the openid scope value.
        /// The value of the scope parameter is expressed as a list of space-delimited, case-sensitive strings.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        ///  RECOMMENDED. An opaque value used by the client to maintain state between the request and callback.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
