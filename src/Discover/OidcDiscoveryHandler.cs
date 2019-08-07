﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ITfoxtec.Identity.Discovery
{
    /// <summary>
    /// Call OIDC Discovery and cache result.
    /// </summary>
    public class OidcDiscoveryHandler : IDisposable
    {
        private readonly CancellationTokenSource cleanUpCancellationTokenSource;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string defaultOidcDiscoveryUri;
        private readonly int defaultExpiresIn;
        private Dictionary<string, (OidcDiscovery, DateTimeOffset)> oidcDiscoveryCache = new Dictionary<string, (OidcDiscovery, DateTimeOffset)>();
        private Dictionary<string, (JsonWebKeySet, DateTimeOffset)> jsonWebKeySetCache = new Dictionary<string, (JsonWebKeySet, DateTimeOffset)>();

        /// <summary>
        /// Call OIDC Discovery and cache result.
        /// </summary>
        /// <param name="httpClientFactory">The IHttpClientFactory instance.</param>
        /// <param name="defaultOidcDiscoveryUri">The default OIDC Discovery uri.</param>
        /// <param name="defaultExpiresIn">The default expires in seconds.</param>
        public OidcDiscoveryHandler(IHttpClientFactory httpClientFactory, string defaultOidcDiscoveryUri = null, int defaultExpiresIn = 3600)
        {
            this.httpClientFactory = httpClientFactory;
            this.defaultOidcDiscoveryUri = defaultOidcDiscoveryUri;
            this.defaultExpiresIn = defaultExpiresIn;

            cleanUpCancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(async () => { await CleanOldCacheItems(); }, cleanUpCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private async Task CleanOldCacheItems()
        {
            while (true)
            {
                var ct = cleanUpCancellationTokenSource.Token;
                ct.ThrowIfCancellationRequested();
                await Task.Delay(new TimeSpan(0, 5, 0), ct);

                ct.ThrowIfCancellationRequested();
                foreach (var item in oidcDiscoveryCache)
                {
                    (var oidcDiscovery, var oidcDiscoveryValidUntil) = item.Value;
                    if (oidcDiscoveryValidUntil < DateTimeOffset.UtcNow)
                    {
                        oidcDiscoveryCache.Remove(item.Key);
                    }
                }

                ct.ThrowIfCancellationRequested();
                foreach (var item in jsonWebKeySetCache)
                {
                    (var jsonWebKeySet, var jsonWebKeySetValidUntil) = item.Value;
                    if (jsonWebKeySetValidUntil < DateTimeOffset.UtcNow)
                    {
                        jsonWebKeySetCache.Remove(item.Key);
                    }
                }
            }
        }

        /// <summary>
        /// Call OIDC Discovery endpoint or read the OIDC Discovery from the cache.
        /// </summary>
        /// <param name="oidcDiscoveryUri">The OIDC Discovery uri. If not specified the default OIDC Discovery uri is used.</param>
        /// <param name="expiresIn">The expires in seconds. If not specified the default expires in is used.</param>
        /// <returns>Return OIDC Discovery result.</returns>
        public async Task<OidcDiscovery> GetOidcDiscoveryAsync(string oidcDiscoveryUri = null, int? expiresIn = null)
        {
            oidcDiscoveryUri = oidcDiscoveryUri ?? defaultOidcDiscoveryUri;
            expiresIn = expiresIn ?? defaultExpiresIn;

            if(oidcDiscoveryCache.ContainsKey(oidcDiscoveryUri))
            {
                (var oidcDiscovery, var oidcDiscoveryValidUntil) = oidcDiscoveryCache[oidcDiscoveryUri];
                if(oidcDiscoveryValidUntil >= DateTimeOffset.UtcNow)
                {
                    return oidcDiscovery;
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Get, oidcDiscoveryUri);
            var client = httpClientFactory.CreateClient();
            using (var response = await client.SendAsync(request))
            {
                // Handle the response
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result = await response.Content.ReadAsStringAsync();
                        var oidcDiscovery = result.ToObject<OidcDiscovery>();
                        var oidcDiscoveryValidUntil = DateTimeOffset.UtcNow.AddSeconds(expiresIn.Value);
                        oidcDiscoveryCache.Add(oidcDiscoveryUri, (oidcDiscovery, oidcDiscoveryValidUntil));
                        return oidcDiscovery;

                    default:
                        throw new Exception($"Error, Status Code OK expected. StatusCode={response.StatusCode}. OidcDiscoveryUri='{oidcDiscoveryUri}'.");
                }
            }
        }

        /// <summary>
        /// Call OIDC Discovery Keys endpoint or read the OIDC Discovery Keys from the cache.
        /// </summary>
        /// <param name="oidcDiscoveryUri">The OIDC Discovery uri, used to resolve the Keys endpoint. If not specified the default OIDC Discovery uri is used.</param>
        /// <param name="expiresIn">The expires in seconds. If not specified the default expires in is used.</param>
        /// <returns>Return OIDC Discovery Keys result.</returns>
        public async Task<JsonWebKeySet> GetOidcDiscoveryKeysAsync(string oidcDiscoveryUri = null, int? expiresIn = null)
        {
            oidcDiscoveryUri = oidcDiscoveryUri ?? defaultOidcDiscoveryUri;
            expiresIn = expiresIn ?? defaultExpiresIn;

            if (jsonWebKeySetCache.ContainsKey(oidcDiscoveryUri))
            {
                (var jsonWebKeySet, var jsonWebKeySetValidUntil) = jsonWebKeySetCache[oidcDiscoveryUri];
                if (jsonWebKeySetValidUntil >= DateTimeOffset.UtcNow)
                {
                    return jsonWebKeySet;
                }
            }

            var oidcDiscovery = await GetOidcDiscoveryAsync(oidcDiscoveryUri, expiresIn);
            var request = new HttpRequestMessage(HttpMethod.Get, oidcDiscovery.JwksUri);
            var client = httpClientFactory.CreateClient();
            using (var response = await client.SendAsync(request))
            {
                // Handle the response
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var result = await response.Content.ReadAsStringAsync();
                        var jsonWebKeySet = result.ToObject<JsonWebKeySet>();
                        var jsonWebKeySetValidUntil = DateTimeOffset.UtcNow.AddSeconds(expiresIn.Value);
                        jsonWebKeySetCache.Add(oidcDiscoveryUri, (jsonWebKeySet, jsonWebKeySetValidUntil));
                        return jsonWebKeySet;

                    default:
                        throw new Exception($"Error, Status Code OK expected. StatusCode={response.StatusCode}. OidcDiscoveryJwksUri='{oidcDiscovery.JwksUri}'.");
                }
            }
        }

        bool isDisposed = false;
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
                cleanUpCancellationTokenSource.Cancel();
            }
        }
    }
}