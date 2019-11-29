using System.Text.Json.Serialization;

namespace ITfoxtec.Identity.Messages
{
    public class CodeChallengeSecret
    {
        /// <summary>
        /// A challenge derived from the code verifier that is sent in the authorization request, to be verified against later.
        /// </summary>
        [JsonPropertyName("code_challenge")]
        public string CodeChallenge { get; set; }

        /// <summary>
        /// A method that was used to derive code challenge.
        /// </summary>
        [JsonPropertyName("code_challenge_method")]
        public string CodeChallengeMethod { get; set; }
    }
}
