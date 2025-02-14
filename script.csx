public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
	{
        // Extract the header, into the components
        var requestHeaders = this.Context.Request.Headers;
        // There will only be one header with this name as we set it, so always get the first element
        string authHeader = requestHeaders.GetValues("notify_token").ElementAt(0);
        // Convert the string into an array
        // Notify removes extra dashes, so this split will always form a 11 element array, with the first element being the apikey name
        var auth = authHeader.Split('-');
        // Join the right parts of the apikey back together ready for JWT generation
        string serviceId = String.Join("-",auth,1,5);
        string secret = String.Join("-",auth,6,5);

        // Convert the secret and serviceid into the correct format JWT
        string token = GenerateToken(secret, serviceId);

        // Set the Authorization header to be the new token
        this.Context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // Make the HTTP request
        var response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);        
        
        return response;

	}

    public static string GenerateToken(string secret, string serviceId)
    {
        // Build the basic JSON structure for the header and payload
        string headerJson = "{\"typ\":\"JWT\",\"alg\":\"HS256\"}";
        string payloadJson = "{\"iss\":\"" + serviceId + "\",\"iat\":\"" + GetCurrentTimeInSeconds() + "\"}";
        
        // Convert them to Base64 strings
        var headerEncoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(headerJson));
        var payloadEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
        var data = headerEncoded + "." + payloadEncoded;
        
        // Hash using the secret passed in
        var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hashedData = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));
        var signature = System.Convert.ToBase64String(hashedData);
        
        // Create the token
        string token = headerEncoded.ToString() + "." + payloadEncoded.ToString() + "." + signature;

        return token;

    }

    public static double GetCurrentTimeInSeconds()
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return Math.Round((DateTime.UtcNow - epoch).TotalSeconds);
    }

}
