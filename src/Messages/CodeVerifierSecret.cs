using System.Text.Json.Serialization;

namespace ITfoxtec.Identity.Messages
{
    public class CodeVerifierSecret
    {
        /// <summary>
        /// A cryptographically random string that is used to correlate the authorization request to the token request.
        /// </summary>
        [JsonPropertyName("code_verifier")]
        public string CodeVerifier { get; set; }
    }
}
