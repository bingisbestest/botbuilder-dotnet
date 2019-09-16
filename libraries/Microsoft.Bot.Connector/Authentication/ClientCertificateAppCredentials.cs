﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Microsoft.Bot.Connector.Authentication
{
    /// <summary>
    /// ClientCertificateAppCredentials auth implementation and cache.
    /// </summary>
    public class ClientCertificateAppCredentials : AppCredentials
    {
        private readonly ClientAssertionCertificate clientCertificate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCertificateAppCredentials"/> class.
        /// </summary>
        /// <param name="clientCertificate">Client certificate to be presented for authentication.</param>
        /// <param name="channelAuthTenant">Optional. The oauth token tenant.</param>
        /// <param name="customHttpClient">Optional <see cref="HttpClient"/> to be used when acquiring tokens.</param>
        /// <param name="logger">Optional <see cref="ILogger"/> to gather telemetry data while acquiring and managing credentials.</param>
        public ClientCertificateAppCredentials(ClientAssertionCertificate clientCertificate, string channelAuthTenant = null, HttpClient customHttpClient = null, ILogger logger = null)
            : base(channelAuthTenant, customHttpClient, logger)
        {
            this.clientCertificate = clientCertificate ?? throw new ArgumentNullException(nameof(clientCertificate));
            MicrosoftAppId = clientCertificate.ClientId;
        }

        /// <inheritdoc/>
        protected override Lazy<AdalAuthenticator> BuildAuthenticator()
        {
            return new Lazy<AdalAuthenticator>(
                () =>
                new AdalAuthenticator(
                    this.clientCertificate,
                    new OAuthConfiguration() { Authority = OAuthEndpoint, Scope = OAuthScope },
                    this.CustomHttpClient,
                    this.Logger),
                LazyThreadSafetyMode.ExecutionAndPublication);
        }
    }
}
