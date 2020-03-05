using Android.App;
using Android.OS;
using Android.Widget;
using System;
//using Xamarin_GoogleAuth.Authentication;
//using Xamarin_GoogleAuth.Services;

namespace GoogleContact
{
    [Activity(Label = "GoogleContact", MainLauncher = true)]
    public class MainActivity : Activity, IGoogleAuthenticationDelegate
    {
        public static GoogleAuthenticator Auth;

        public void OnAuthenticationCanceled()
        {
            new AlertDialog.Builder(this)
                          .SetTitle("Authentication canceled")
                          .SetMessage("You didn't completed the authentication process")
                          .Show();
        }

        public async void OnAuthenticationCompleted(GoogleOAuthToken token)
        {
            var googleService = new GoogleService();
            var email =
                await googleService.GetEmailAsync(
                    token.TokenType,
                    token.AccessToken);

            // Display it on the UI
            var displayName = FindViewById<TextView>(Resource.Id.displayName);
            displayName.Text = $"Connected with {token.AccessToken}";
        }

        public void OnAuthenticationFailed(string message, Exception exception)
        {
            new AlertDialog.Builder(this)
                           .SetTitle(message)
                           .SetMessage(exception?.ToString())
                           .Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Auth = new GoogleAuthenticator(
            Configuration.ClientId,
            Configuration.Scope,
            Configuration.RedirectUrl,
            this);

            var googleLoginButton = FindViewById<Button>(Resource.Id.googleLoginButton);
            googleLoginButton.Click += OnGoogleLoginButtonClicked;
        }

        private void OnGoogleLoginButtonClicked(object sender, EventArgs e)
        {
            // Display the activity handling the authentication
            var authenticator = Auth.GetAuthenticator();
            var intent = authenticator.GetUI(this);
            StartActivity(intent);
        }
    }
    public static class Configuration
    {
        public const string ClientId =
            "391686982705-fnqbb3fua8i9kq8m97o5j2g0lk5mdnjo.apps.googleusercontent.com";

        public const string Scope = "email";

        public const string RedirectUrl =
            "com.GoogleContact.GoogleContact:/oauth2redirect";
    }
}

