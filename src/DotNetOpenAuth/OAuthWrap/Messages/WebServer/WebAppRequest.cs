﻿//-----------------------------------------------------------------------
// <copyright file="WebAppRequest.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.OAuthWrap.Messages {
	using System;
	using System.Diagnostics.Contracts;
	using DotNetOpenAuth.Messaging;
	using DotNetOpenAuth.OAuthWrap.ChannelElements;

	/// <summary>
	/// A message sent by a web application Client to the AuthorizationServer
	/// via the user agent to obtain authorization from the user and prepare
	/// to issue an access token to the Consumer if permission is granted.
	/// </summary>
	public class WebAppRequest : MessageBase, IMessageWithClientState {
		/// <summary>
		/// The type of message.
		/// </summary>
		[MessagePart(Protocol.type, IsRequired = true)]
		private const string Type = "web_server";

		/// <summary>
		/// Initializes a new instance of the <see cref="WebAppRequest"/> class.
		/// </summary>
		/// <param name="userAuthorizationEndpoint">The Authorization Server's user authorization URL to direct the user to.</param>
		/// <param name="version">The protocol version.</param>
		internal WebAppRequest(Uri userAuthorizationEndpoint, Version version)
			: base(version, MessageTransport.Indirect, userAuthorizationEndpoint) {
			Contract.Requires<ArgumentNullException>(userAuthorizationEndpoint != null);
			Contract.Requires<ArgumentNullException>(version != null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WebAppRequest"/> class.
		/// </summary>
		/// <param name="authorizationServer">The authorization server.</param>
		internal WebAppRequest(AuthorizationServerDescription authorizationServer)
			: this(authorizationServer.AuthorizationEndpoint, authorizationServer.Version) {
			Contract.Requires<ArgumentNullException>(authorizationServer != null);
			Contract.Requires<ArgumentException>(authorizationServer.Version != null);
			Contract.Requires<ArgumentException>(authorizationServer.AuthorizationEndpoint != null);
		}

		/// <summary>
		/// Gets or sets state of the client that should be sent back with the authorization response.
		/// </summary>
		/// <value>
		/// An opaque value that Clients can use to maintain state associated with this request. 
		/// </value>
		/// <remarks>
		/// REQUIRED. The client identifier as described in Section 3.4  (Client Credentials). 
		/// </remarks>
		[MessagePart(Protocol.state, IsRequired = false, AllowEmpty = true)]
		public string ClientState { get; set; }

		/// <summary>
		/// Gets or sets the identifier by which this client is known to the Authorization Server.
		/// </summary>
		[MessagePart(Protocol.client_id, IsRequired = true, AllowEmpty = false)]
		internal string ClientIdentifier { get; set; }

		/// <summary>
		/// Gets or sets the callback URL.
		/// </summary>
		/// <value>
		/// An absolute URL to which the Authorization Server will redirect the User back after
		/// the user has approved the authorization request.
		/// </value>
		/// <remarks>
		/// REQUIRED unless a redirection URI has been established between the client and authorization server via other means. An absolute URI to which the authorization server will redirect the user-agent to when the end-user authorization step is completed. The authorization server MAY require the client to pre-register their redirection URI. The redirection URI MUST NOT include a query component as defined by [RFC3986]  (Berners-Lee, T., Fielding, R., and L. Masinter, “Uniform Resource Identifier (URI): Generic Syntax,” January 2005.) section 3 if the state parameter is present. 
		/// </remarks>
		[MessagePart(Protocol.redirect_uri, IsRequired = false, AllowEmpty = false)]
		internal Uri Callback { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the authorization server is
		/// required to redirect the browser back to the client immediately.
		/// </summary>
		/// <remarks>
		/// OPTIONAL. The parameter value must be set to true or false. If set to true, the authorization server MUST NOT prompt the end-user to authenticate or approve access. Instead, the authorization server attempts to establish the end-user's identity via other means (e.g. browser cookies) and checks if the end-user has previously approved an identical access request by the same client and if that access grant is still active. If the authorization server does not support an immediate check or if it is unable to establish the end-user's identity or approval status, it MUST deny the request without prompting the end-user. Defaults to false  if omitted. 
		/// </remarks>
		[MessagePart(Protocol.immediate, IsRequired = false, AllowEmpty = false)]
		internal bool Immediate { get; set; }
	}
}