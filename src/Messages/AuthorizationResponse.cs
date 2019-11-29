﻿using System.Text.Json.Serialization;

namespace ITfoxtec.Identity.Messages
{
    /// <summary>
    /// OAuth 2.0 Authorization Response.
    /// </summary>
    public class AuthorizationResponse
    {
        /// <summary>
        /// OAuth 2.0 REQUIRED. The authorization code generated by the authorization server.
        /// OIDC Implicit / Hybrid Flow OPTIONAL.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// REQUIRED if the "state" parameter was present in the client authorization request.The exact value received from the client.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        #region Error
        /// <summary>
        /// If error REQUIRED. A single ASCII [USASCII] error code.
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; }

        /// <summary>
        /// If error OPTIONAL. Human-readable ASCII [USASCII] text providing additional information, used to assist the client developer in understanding the error that occurred.
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// If error OPTIONAL. A URI identifying a human-readable web page with information about the error, used to provide the client developer with additional information about the error.
        /// </summary>
        [JsonPropertyName("error_uri")]
        public string ErrorUri { get; set; }
        #endregion
    }
}
