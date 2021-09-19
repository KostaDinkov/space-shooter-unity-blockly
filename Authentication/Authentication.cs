using System;

using System.Linq;

using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Authentication
{
    public class Authientication

    {
        private static string ClientId = "7d74f671-c740-4bc6-bbe5-d935a0273401";
        private static string Tenant = "25da1cb8-a81d-435b-b0cb-0c3d266ea76f";
        private static string Instance = "https://login.microsoftonline.com/";
        private static IPublicClientApplication clientApp;
        string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        private string resultText;
        private string tokenInfoText;

        //Set the scope for API call to user.read
        string[] scopes = new string[] { "user.read" };

        public static IPublicClientApplication PublicClientApp { get { return clientApp; } }
        public static void CreateApplication()
        {
            var builder = PublicClientApplicationBuilder.Create(ClientId)
                .WithAuthority($"{Instance}{Tenant}")
                .WithDefaultRedirectUri();
            clientApp = builder.Build();
        }

        public  async void CallGraphButton_Click()
        {
            AuthenticationResult authResult = null;
            var app = PublicClientApp;
            this.resultText = string.Empty;
            this.tokenInfoText = string.Empty;

            IAccount firstAccount;

            var accounts = await app.GetAccountsAsync();
            firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await app.AcquireTokenSilent(scopes, firstAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent. 
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await app.AcquireTokenInteractive(scopes)
                        .WithAccount(firstAccount)
                        
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    this.resultText = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                this.resultText = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return;
            }

            if (authResult != null)
            {
                this.resultText = await GetHttpContentWithToken(graphAPIEndpoint, authResult.AccessToken);
                DisplayBasicTokenInfo(authResult);
                //Todo remove login button and continue
                
            }
        }

        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);

                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private async void SignOutButton_Click(object sender)
        {
            var accounts = await PublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    await PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                    this.resultText = "User has signed-out";
                    //this.CallGraphButton.Visibility = Visibility.Visible;
                    //this.SignOutButton.Visibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    this.resultText = $"Error signing-out user: {ex.Message}";
                }
            }


        }
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            this.tokenInfoText = "";
            if (authResult != null)
            {
                this.tokenInfoText += $"Username: {authResult.Account.Username}" + Environment.NewLine;
                this.tokenInfoText += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
            }
        }

    }
}
