using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Threading;
using TwitterConnector.oAuth.Exceptions;

namespace TwitterConnector.OAuth
{
    /// <summary>
    /// Consumer object stands for consumer in OAuth protocol.
    /// </summary>
    /// <remarks>
    ///  This object does follows.
    /// <list type="number">
    /// <item><description>Obtain unauthorized request token from Service Provider.</description></item>
    /// <item><description>Request access token responding authenticated request token.</description></item>
    /// <item><description>Access protected resource with access token</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// using System;
    ///using System.Collections.Generic;
    ///using System.Text;
    ///using System.Diagnostics;
    ///using System.Net;
    ///
    ///using OAuthLib;
    ///
    ///namespace OAuthDevSupport
    ///{
    ///    class Program
    ///    {
    ///        static void Main(string[] args)
    ///        {
    ///            try
    ///          {
    ///                Consumer c =
    ///                   new Consumer("yourConsumerKey", "yourConsumerSecret");
    ///
    ///                RequestToken reqToken =
    ///                    c.ObtainUnauthorizedRequestToken("http://twitter.com/oauth/request_token", "http://twitter.com/");
    ///
    ///                Process.Start(
    ///                    Consumer.BuildUserAuthorizationURL(
    ///                        "http://twitter.com/oauth/authorize",
    ///                        reqToken
    ///                    )
    ///                );
    ///
    ///                Console.Out.WriteLine("Input verifier");
    ///                String verifier = Console.In.ReadLine();
    ///                verifier = verifier.TrimEnd('\r', '\n');
    ///                AccessToken accessToken =
    ///                   c.RequestAccessToken(verifier, reqToken, "http://twitter.com/oauth/access_token", "http://twitter.com/");
    ///
    ///                HttpWebResponse resp =
    ///                    c.AccessProtectedResource(
    ///                        accessToken,
    ///                        "http://twitter.com/statuses/home_timeline.xml",
    ///                        "GET",
    ///                        "http://twitter.com/",
    ///                            new Parameter[]{ 
    ///                            new Parameter("since_id","your since id") 
    ///                        }
    ///                        );
    ///
    ///            }
    ///            catch (Exception ex)
    ///            {
    ///                Console.WriteLine(ex.Message);
    ///            }
    ///        }
    ///    }
    ///}
    /// </code>
    /// </example>
    public class Consumer
    {
        private readonly String _consumerKey;
        private readonly String _consumerSecret;

        private WebProxy _webProxy;

        /// <summary>
        /// HTTP Proxy to use when communicate with Service Provider.
        /// </summary>
        public WebProxy Proxy
        {
            get
            {
                return _webProxy;
            }

            set
            {
                _webProxy = value;
            }
        }

        /// <summary>
        /// Construct new Consumer instance.
        /// </summary>
        /// <param name="consumerKey">Key of consumer</param>
        /// <param name="consumerSecret">Secret of consumer</param>
        public Consumer(
            String consumerKey,
            String consumerSecret
            )
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        /// <summary>
        /// Obtain unauthorized request token from service provider
        /// </summary>
        /// <param name="requestTokenUrl">Request token URL</param>
        /// <param name="realm">Realm for obtaining request token</param>
        /// <returns>Obtained request token</returns>
        public RequestToken ObtainUnauthorizedRequestToken(
            String requestTokenUrl,
            String realm
            )
        {
            Parameter[] responseParameter = null;
            return ObtainUnauthorizedRequestToken(
                requestTokenUrl,
                null,
                realm,
                null,
                ref responseParameter
            );
        }

        /// <summary>
        /// Obtain unauthorized request token from service provider
        /// </summary>
        /// <param name="requestTokenUrl">Request token URL</param>
        /// <param name="realm">Realm for obtaining request token</param>
        /// <param name="responseParameter" >Parameters returned in response</param>
        /// <returns>Obtained request token</returns>
        public RequestToken ObtainUnauthorizedRequestToken(
            String requestTokenUrl,
            String realm,
            ref Parameter[] responseParameter
            )
        {
            return ObtainUnauthorizedRequestToken(
                requestTokenUrl,
                null,
                realm,
                null,
                ref responseParameter
            );
        }

        /// <summary>
        /// Obtain unauthorized request token from service provider
        /// </summary>
        /// <param name="requestTokenUrl">Request token URL</param>
        /// <param name="callbackURL">An absolute URL to which the Service Provider will redirect the User back when the Obtaining User Authorization step is completed.</param>
        /// <param name="realm">Realm for obtaining request token</param>
        /// <returns>Obtained request token</returns>
        public RequestToken ObtainUnauthorizedRequestToken(
            String requestTokenUrl,
            String callbackURL,
            String realm
            )
        {
            Parameter[] responseParameter = null;
            return ObtainUnauthorizedRequestToken(
                requestTokenUrl,
                callbackURL,
                realm,
                null,
                ref responseParameter
            );
        }

        /// <summary>
        /// Obtain unauthorized request token from service provider
        /// </summary>
        /// <param name="requestTokenUrl">Request token URL</param>
        /// <param name="callbackURL">An absolute URL to which the Service Provider will redirect the User back when the Obtaining User Authorization step is completed.</param>
        /// <param name="realm">Realm for obtaining request token</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="responseParameters" >Parameters returned in response</param>
        /// <returns>Obtained request token</returns>
        public RequestToken ObtainUnauthorizedRequestToken(
            String requestTokenUrl,
            String callbackURL,
            String realm,
            Parameter[] additionalParameters,
            ref Parameter[] responseParameters
            )
        {
            return ObtainUnauthorizedRequestToken(
                requestTokenUrl,
                callbackURL,
                realm,
                additionalParameters,
                ref responseParameters,
                CreateRequestInDefault
            );
        }

        /// <summary>
        /// Obtain unauthorized request token from service provider
        /// </summary>
        /// <param name="requestTokenUrl">Request token URL</param>
        /// <param name="callbackURL">An absolute URL to which the Service Provider will redirect the User back when the Obtaining User Authorization step is completed.</param>
        /// <param name="realm">Realm for obtaining request token</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="responseParameters" >Parameters returned in response</param>
        /// <param name="createRequest" >Factory method to create request object</param>
        /// <returns>Obtained request token</returns>
        public RequestToken ObtainUnauthorizedRequestToken(
            String requestTokenUrl,
            String callbackURL,
            String realm,
            Parameter[] additionalParameters,
            ref Parameter[] responseParameters,
            RequestFactoryMethod createRequest
        )
        {

            if (additionalParameters == null)
                additionalParameters = new Parameter[0];

            String oauth_consumer_key = _consumerKey;
            String oauth_signature_method = "HMAC-SHA1";
            String oauth_timestamp =
                ((DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks) / (1000 * 10000)).ToString();
            String oauth_nonce =
                Guid.NewGuid().ToString();
            String oauth_callback =
                (!string.IsNullOrEmpty(callbackURL) ?
                    callbackURL :
                    "oob"
                );

            HttpWebRequest req = createRequest(requestTokenUrl);

            if (_webProxy != null)
                req.Proxy = _webProxy;

            req.Method = "POST";

            String oauth_signature =
                CreateHMACSHA1Signature(
                    req.Method,
                    requestTokenUrl,
                    Parameter.ConCatAsArray(
                        new Parameter[]{
                            new Parameter("oauth_consumer_key",oauth_consumer_key),
                            new Parameter ("oauth_signature_method",oauth_signature_method ),
                            new Parameter ("oauth_timestamp",oauth_timestamp),
                            new Parameter ("oauth_nonce",oauth_nonce ),
                            new Parameter ("oauth_callback",oauth_callback)
                        },
                        additionalParameters
                    ),
                    _consumerSecret,
                    null
                );

            req.Headers.Add(
                "Authorization: OAuth " +
                "realm=\"" + realm + "\"," +
                "oauth_consumer_key=\"" + Parameter.EncodeParameterString(oauth_consumer_key) + "\"," +
                "oauth_signature_method=\"" + Parameter.EncodeParameterString(oauth_signature_method) + "\"," +
                "oauth_signature=\"" + Parameter.EncodeParameterString(oauth_signature) + "\"," +
                "oauth_timestamp=\"" + Parameter.EncodeParameterString(oauth_timestamp) + "\"," +
                "oauth_nonce=\"" + Parameter.EncodeParameterString(oauth_nonce) + "\"," +
                "oauth_callback=\"" + Parameter.EncodeParameterString(oauth_callback) + "\"" +
                (additionalParameters.Length > 0 ?
                    "," + Parameter.ConCat(additionalParameters, "\"") :
                    ""
                )
            );

            HttpWebResponse resp = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                responseParameters = Parameter.Parse(sr.ReadToEnd());

                String reqToken = null;
                String reqTokenSecret = null;

                foreach (Parameter param in responseParameters)
                {
                    if (param.Name == "oauth_token")
                        reqToken = param.Value;
                    if (param.Name == "oauth_token_secret")
                        reqTokenSecret = param.Value;
                }

                if (reqToken == null || reqTokenSecret == null)
                    throw new InvalidOperationException();

                return new RequestToken(reqToken, reqTokenSecret);

            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }

        }


        /// <summary>
        /// Request access token responding to authenticated request token.
        /// </summary>
        /// <param name="verifier">Verifier string for authenticaed request token</param>
        /// <param name="requestToken">Authenticated request token</param>
        /// <param name="accessTokenUrl">Access token URL</param>
        /// <param name="realm">Realm for requesting access token</param>
        /// <returns>Responding access token</returns>
        public AccessToken RequestAccessToken(
            String verifier,
            RequestToken requestToken,
            String accessTokenUrl,
            String realm)
        {
            Parameter[] responseParameters = null;
            return RequestAccessToken(
                verifier,
                requestToken,
                accessTokenUrl,
                realm,
                null,
                ref responseParameters
            );
        }

        /// <summary>
        /// Request access token responding to authenticated request token.
        /// </summary>
        /// <param name="verifier">Verifier string for authenticaed request token</param>
        /// <param name="requestToken">Authenticated request token</param>
        /// <param name="accessTokenUrl">Access token URL</param>
        /// <param name="realm">Realm for requesting access token</param>
        /// <param name="responseParameters" >Parameters returned in response</param>       
        /// <returns>Responding access token</returns>
        public AccessToken RequestAccessToken(
            String verifier,
            RequestToken requestToken,
            String accessTokenUrl,
            String realm,
            ref Parameter[] responseParameters)
        {
            return RequestAccessToken(
                verifier,
                requestToken,
                accessTokenUrl,
                realm,
                null,
                ref responseParameters
            );
        }

        /// <summary>
        /// Request access token responding to authenticated request token.
        /// </summary>
        /// <param name="verifier">Verifier string for authenticaed request token</param>
        /// <param name="requestToken">Authenticated request token</param>
        /// <param name="accessTokenUrl">Access token URL</param>
        /// <param name="realm">Realm for requesting access token</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="responseParameters" >Parameters returned in response</param>
        /// <returns>Responding access token</returns>
        public AccessToken RequestAccessToken(
            String verifier,
            RequestToken requestToken,
            String accessTokenUrl,
            String realm,
            Parameter[] additionalParameters,
            ref Parameter[] responseParameters)
        {
            return RequestAccessToken(
                verifier,
                requestToken,
                accessTokenUrl,
                realm,
                additionalParameters,
                ref responseParameters,
                CreateRequestInDefault
            );
        }

        /// <summary>
        /// Request access token responding to authenticated request token.
        /// </summary>
        /// <param name="verifier">Verifier string for authenticaed request token</param>
        /// <param name="requestToken">Authenticated request token</param>
        /// <param name="accessTokenUrl">Access token URL</param>
        /// <param name="realm">Realm for requesting access token</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="responseParameters" >Parameters returned in response</param>
        /// <param name="createRequest" >Factory method to create request object</param>
        /// <returns>Responding access token</returns>
        public AccessToken RequestAccessToken(
            String verifier,
            RequestToken requestToken,
            String accessTokenUrl,
            String realm,
            Parameter[] additionalParameters,
            ref Parameter[] responseParameters,
            RequestFactoryMethod createRequest)
        {

            if (additionalParameters == null)
                additionalParameters = new Parameter[0];

            String oauth_consumer_key = _consumerKey;
            String oauth_token = requestToken.TokenValue;
            String oauth_signature_method = "HMAC-SHA1";
            String oauth_timestamp =
                ((DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks) / (1000 * 10000)).ToString();
            String oauth_nonce =
                Guid.NewGuid().ToString();

            HttpWebRequest req = createRequest(accessTokenUrl);

            if (_webProxy != null)
                req.Proxy = _webProxy;

            req.Method = "POST";

            String oauth_signature =
                CreateHMACSHA1Signature(
                    req.Method,
                    accessTokenUrl,
                    Parameter.ConCatAsArray(
                        new Parameter[]{
                            new Parameter("oauth_consumer_key",oauth_consumer_key),
                            new Parameter("oauth_token",oauth_token ),
                            new Parameter ("oauth_signature_method",oauth_signature_method ),
                            new Parameter ("oauth_timestamp",oauth_timestamp),
                            new Parameter ("oauth_nonce",oauth_nonce ),
                            new Parameter ("oauth_verifier",verifier ),
                        },
                        additionalParameters
                    ),
                    _consumerSecret,
                    requestToken.TokenSecret
                );

            req.Headers.Add(
                "Authorization: OAuth " +
                "realm=\"" + realm + "\"," +
                "oauth_consumer_key=\"" + Parameter.EncodeParameterString(oauth_consumer_key) + "\"," +
                "oauth_token=\"" + Parameter.EncodeParameterString(oauth_token) + "\"," +
                "oauth_signature_method=\"" + Parameter.EncodeParameterString(oauth_signature_method) + "\"," +
                "oauth_signature=\"" + Parameter.EncodeParameterString(oauth_signature) + "\"," +
                "oauth_timestamp=\"" + Parameter.EncodeParameterString(oauth_timestamp) + "\"," +
                "oauth_nonce=\"" + Parameter.EncodeParameterString(oauth_nonce) + "\"," +
                "oauth_verifier=\"" + Parameter.EncodeParameterString(verifier) + "\"" +
                (additionalParameters.Length > 0 ?
                    "," + Parameter.ConCat(additionalParameters, "\"") :
                    ""
                )
            );

            HttpWebResponse resp = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());

                responseParameters =
                    Parameter.Parse(sr.ReadToEnd());

                String accessToken = null;
                String accessTokenSecret = null;
                foreach (Parameter param in responseParameters)
                {
                    if (param.Name == "oauth_token")
                        accessToken = param.Value;

                    if (param.Name == "oauth_token_secret")
                        accessTokenSecret = param.Value;
                }

                if (accessToken == null || accessTokenSecret == null)
                    throw new InvalidOperationException();

                return new AccessToken(accessToken, accessTokenSecret);

            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }
        }

        /// <summary>
        /// Access protected resource with access token
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="urlString">URL string for accessing protected resource</param>
        /// <param name="method">HTTP method to access</param>
        /// <param name="authorizationRealm">realm for accessing protected resource</param>
        /// <param name="queryParameters">Query parameter to be sent</param>
        /// <returns>HttpWebResponse from protected resource</returns>
        public HttpWebResponse AccessProtectedResource(
            AccessToken accessToken,
            String urlString,
            String method,
            String authorizationRealm,
            Parameter[] queryParameters
        )
        {
            return AccessProtectedResource(
                accessToken,
                urlString,
                method,
                authorizationRealm,
                queryParameters,
                null);
        }

        /// <summary>
        /// Access protected resource with access token
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="urlString">URL string for accessing protected resource</param>
        /// <param name="method">HTTP method to access</param>
        /// <param name="authorizationRealm">realm for accessing protected resource</param>
        /// <param name="queryParameters">Query parameter to be sent</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <returns>HttpWebResponse from protected resource</returns>
        public HttpWebResponse AccessProtectedResource(
            AccessToken accessToken,
            String urlString,
            String method,
            String authorizationRealm,
            Parameter[] queryParameters,
            Parameter[] additionalParameters)
        {
            return AccessProtectedResource(
                            accessToken,
                            urlString,
                            method,
                            authorizationRealm,
                            queryParameters,
                            additionalParameters,
                            CreateRequestInDefault
                        );
        }

        /// <summary>
        /// Access protected resource with access token
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="urlString">URL string for accessing protected resource</param>
        /// <param name="method">HTTP method to access</param>
        /// <param name="authorizationRealm">realm for accessing protected resource</param>
        /// <param name="queryParameters">Query parameter to be sent</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="createRequest" >Factory method to create request object</param>
        /// <returns>HttpWebResponse from protected resource</returns>
        public HttpWebResponse AccessProtectedResource(
            AccessToken accessToken,
            String urlString,
            String method,
            String authorizationRealm,
            Parameter[] queryParameters,
            Parameter[] additionalParameters,
            RequestFactoryMethod createRequest)
        {

            HttpWebRequest req = null;
            IAsyncResult result =
                BeginAccessProtectedResource(accessToken,
                    urlString,
                    method,
                    authorizationRealm,
                    queryParameters,
                    additionalParameters,
                    out req,
                    null,
                    null,
                    createRequest
                );

            return GetResponseInSynch(req, result);

        }

        /// <summary>
        /// Begins an asynchronous access to protected resource with access token 
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="urlString">URL string for accessing protected resource</param>
        /// <param name="method">HTTP method to access</param>
        /// <param name="authorizationRealm">realm for accessing protected resource</param>
        /// <param name="queryParameters">Query parameter to be sent</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="request">Request that starts asynchronous access </param>
        /// <param name="callback">callback to pass request object</param>
        /// <param name="state">state to pass request object </param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response. </returns>
        public IAsyncResult BeginAccessProtectedResource(
            AccessToken accessToken,
            String urlString,
            String method,
            String authorizationRealm,
            Parameter[] queryParameters,
            Parameter[] additionalParameters,
            out HttpWebRequest request,
            AsyncCallback callback,
            Object state)
        {
            return BeginAccessProtectedResource(
                            accessToken,
                            urlString,
                            method,
                            authorizationRealm,
                            queryParameters,
                            additionalParameters,
                            out request,
                            callback,
                            state,
                            CreateRequestInDefault
                );
        }

        /// <summary>
        /// Begins an asynchronous access to protected resource with access token 
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="urlString">URL string for accessing protected resource</param>
        /// <param name="method">HTTP method to access</param>
        /// <param name="authorizationRealm">realm for accessing protected resource</param>
        /// <param name="queryParameters">Query parameter to be sent</param>
        /// <param name="additionalParameters">Parameters added to Authorization header</param>
        /// <param name="request">Request that starts asynchronous access </param>
        /// <param name="callback">callback to pass request object</param>
        /// <param name="state">state to pass request object </param>
        /// <param name="createRequest" >Factory method to create request object</param>
        /// <returns>An IAsyncResult that references the asynchronous request for a response. </returns>
        public IAsyncResult BeginAccessProtectedResource(
            AccessToken accessToken,
            String urlString,
            String method,
            String authorizationRealm,
            Parameter[] queryParameters,
            Parameter[] additionalParameters,
            out HttpWebRequest request,
            AsyncCallback callback,
            Object state,
            RequestFactoryMethod createRequest)
        {
            if (additionalParameters == null)
                additionalParameters = new Parameter[0];

            if (!(method.Equals("GET") || method.Equals("POST")))
                throw new ArgumentException(
                    "Method must be GET or POST"
                    );

            Uri uri = new Uri(urlString);
            if (uri.Query.Length > 0)
                throw new ArgumentException(
                    "Query parameter must not be passed included in url.\r\n" +
                    "Pass them via queryParameters parameter of this method."
                    );

            if (queryParameters == null)
                queryParameters = new Parameter[0];

            String oauth_consumer_key = _consumerKey;
            String oauth_token = accessToken.TokenValue;
            String oauth_signature_method = "HMAC-SHA1";
            String oauth_timestamp = Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
            String oauth_nonce = Guid.NewGuid().ToString();


            request =
                createRequest(
                    urlString +
                    (
                        method.Equals("GET") &&
                        queryParameters.Length > 0 ?
                        "?" + Parameter.ConCat(queryParameters) :
                        ""
                        )
                );

            if (_webProxy != null)
                request.Proxy = _webProxy;

            //Twitter service does not accept expect100continue
            request.ServicePoint.Expect100Continue = false;

            request.Method = method;
            

            String oauth_signature =
                CreateHMACSHA1Signature(
                    request.Method,
                    urlString,
                    Parameter.ConCatAsArray(
                        new Parameter[]{
                                new Parameter("oauth_consumer_key",oauth_consumer_key),
                                new Parameter("oauth_token",oauth_token ),
                                new Parameter ("oauth_signature_method",oauth_signature_method ),
                                new Parameter ("oauth_timestamp",oauth_timestamp),
                                new Parameter ("oauth_nonce",oauth_nonce ),
                                new Parameter ("oauth_version", "1.0")
                            },
                        additionalParameters,
                        queryParameters
                    ),
                    _consumerSecret,
                    accessToken.TokenSecret
                );

            request.Headers.Add(
                "Authorization: OAuth " +
                "realm=\"" + authorizationRealm + "\"," +
                "oauth_consumer_key=\"" + Parameter.EncodeParameterString(oauth_consumer_key) + "\", " +
                "oauth_nonce=\"" + Parameter.EncodeParameterString(oauth_nonce) + "\", " +
                "oauth_signature=\"" + Parameter.EncodeParameterString(oauth_signature) + "\", " +
                "oauth_signature_method=\"" + Parameter.EncodeParameterString(oauth_signature_method) + "\", " +
                "oauth_timestamp=\"" + Parameter.EncodeParameterString(oauth_timestamp) + "\", " +
                "oauth_token=\"" + Parameter.EncodeParameterString(oauth_token) + "\", " +
                "oauth_version=\"" + Parameter.EncodeParameterString("1.0") + "\"" +
                (additionalParameters.Length > 0 ?
                    ", " + Parameter.ConCat(additionalParameters, "\"") :
                    ""
                )
            );

            if (method.Equals("POST"))
            {
                request.ContentType = "application/x-www-form-urlencoded";

                String contents = Parameter.ConCat(queryParameters);
                byte[] contentsBytes = Encoding.ASCII.GetBytes(contents);
                request.ContentLength = contentsBytes.Length;


                using (Stream s = request.GetRequestStream())
                {
                    s.Write(contentsBytes, 0, contentsBytes.Length);
                }
            }


            return request.BeginGetResponse(callback, state);

        }


        private static String CreateHMACSHA1Signature(
            String method,
            String url,
            Parameter[] parameterArray,
            String consumerSecret,
            String tokenSecret)
        {

            if (consumerSecret == null)
                throw new NullReferenceException();

            if (tokenSecret == null)
                tokenSecret = "";

            method = method.ToUpper();

            url = url.ToLower();
            Uri uri = new Uri(url);
            url =
                uri.Scheme + "://" +
                uri.Host +
                ((uri.Scheme.Equals("http") && uri.Port == 80 ||
                            uri.Scheme.Equals("https") && uri.Port == 443) ?
                            "" :
                            uri.Port.ToString()
                ) +
                uri.AbsolutePath;

            String concatenatedParameter =
                Parameter.ConcatToNormalize(parameterArray);

            HMACSHA1 alg = new HMACSHA1
                (
                    encode(
                        Parameter.EncodeParameterString(consumerSecret) + "&" +
                        Parameter.EncodeParameterString(tokenSecret)
                    )
                );
            string test = Parameter.EncodeParameterString(method) + "&" +
                            Parameter.EncodeParameterString(url) + "&" +
                            Parameter.EncodeParameterString(concatenatedParameter);
            return
                System.Convert.ToBase64String(
                    alg.ComputeHash(
                        encode(
                            Parameter.EncodeParameterString(method) + "&" +
                            Parameter.EncodeParameterString(url) + "&" +
                            Parameter.EncodeParameterString(concatenatedParameter)
                        )
                    )
                );

        }


        /// <summary>
        /// Build user authorization URL to authorize request token
        /// </summary>
        /// <param name="userAuthorizationUrl">User authorization URL served by Service Provider</param>
        /// <param name="requestToken">Request token</param>
        /// <returns>user authorization URL to authorize request token</returns>
        public static String BuildUserAuthorizationURL(
            String userAuthorizationUrl,
            RequestToken requestToken
            )
        {

            Uri uri = new Uri(userAuthorizationUrl);

            return
                uri.OriginalString +
                (uri.Query != null && uri.Query.Length > 0 ?
                "&" : "?") +
                "oauth_token=" + Parameter.EncodeParameterString(requestToken.TokenValue);

        }

        private static byte[] encode(String val)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.ASCII);

            sw.Write(val);
            sw.Flush();

            return ms.ToArray();

        }

        private static String decode(byte[] val)
        {
            MemoryStream ms = new MemoryStream(val);
            StreamReader sr = new StreamReader(ms, Encoding.ASCII);
            return sr.ReadToEnd();

        }

        private static HttpWebRequest CreateRequestInDefault(String uri)
        {
            return (HttpWebRequest)WebRequest.Create(uri);
        }

        private static HttpWebResponse GetResponseInSynch(HttpWebRequest req, IAsyncResult syncResult)
        {

            Thread timeOutCatchThread = null;
            HttpWebResponse result = null;
            HttpWebResponse response = null;
            Object timeOutCatchLockObject = new Object();

            try
            {

                if (req.Timeout > 0)
                {
                    timeOutCatchThread =
                        new Thread(
                            delegate()
                            {
                                DateTime waitStartClock = DateTime.Now;

                                while (DateTime.Now.Ticks < waitStartClock.Ticks + req.Timeout*10000)
                                {
                                    lock (timeOutCatchLockObject)
                                    {
                                        if (result != null)
                                            return;
                                    }

                                    Thread.Sleep(100);

                                }

                                req.Abort();

                            }
                            );

                    timeOutCatchThread.Start();
                }

                response = (HttpWebResponse) req.EndGetResponse(syncResult);

                lock (timeOutCatchLockObject)
                {
                    result = response;
                }

                timeOutCatchThread.Join(1000);

                return result;

            }
            catch (WebException wex)
            {
                int rateLimit = 0;
                int limitRemaining = 0;
                DateTime rateReset = DateTime.MinValue;
                if (GetInfoFromResponse(wex.Response, out rateLimit, out limitRemaining, out rateReset))
                {
                    throw new OAuthHttpWebRequestException("Error getting response from URL.", rateLimit, limitRemaining,
                        rateReset, wex);
                }
                else
                {

                    throw new OAuthHttpWebRequestException("Error getting response from URL.", wex);
                }
            }
            catch (Exception ex)
            {
                throw new OAuthHttpWebRequestException("Error getting response from URL.",  ex);
            }
            finally
            {
                if (timeOutCatchThread != null &&
                    (timeOutCatchThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                {
                    timeOutCatchThread.Abort();
                }
            }
        }

        internal static bool GetInfoFromResponse(WebResponse resp, out int rateLimit, out int limitRemaining, out DateTime reset)
        {
            rateLimit = 0;
            limitRemaining = 0;
            reset = DateTime.MinValue;

            bool found = false;
            if (resp != null)
            {
                for (int i = 0; i < resp.Headers.Keys.Count; i++)
                {
                    string s = resp.Headers.GetKey(i);
                    if (s == "x-rate-limit-limit")
                    {
                        rateLimit = int.Parse(resp.Headers.GetValues(i).First());
                        found = true;
                    }
                    if (s == "x-rate-limit-remaining")
                    {
                        limitRemaining = int.Parse(resp.Headers.GetValues(i).First());
                        found = true;
                    }
                    if (s == "x-rate-limit-reset")
                    {
                        long unixTime = long.Parse(resp.Headers.GetValues(i).First());
                        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        reset = epoch.AddSeconds(unixTime);
                        reset = reset.ToLocalTime();
                        found = true;
                    }
                }
            }
            return found;
        }

        /// <summary>
        /// User can specify the factory method for creating request instance passing RequestFactoryMethod delegate.
        /// Specifying the delegate, user can access much of the HttpWebRequest object.
        /// RequestFactoryMethod delegate can be used in calling ObtainUnauthorizedRequestToken, RequestAccessToken, AccessProtectedResource, BeginAccessProtectedResource methods.
        /// </summary>
        public delegate HttpWebRequest RequestFactoryMethod(String uri);

    }
}
