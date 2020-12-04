using AuthenticationServices;
using Foundation;
using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;
using UIKit;

namespace PrankChat.Mobile.iOS.ApplicationServices.ExternalAuth.AppleSignIn
{
    public class AppleSignInService : NSObject, IAppleSignInService, IASAuthorizationControllerDelegate, IASAuthorizationControllerPresentationContextProviding
	{
		private TaskCompletionSource<AppleAuthDataModel> _loginTaskCompletionSource;

		public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
		{
			return UIApplication.SharedApplication.KeyWindow;
		}

		public async Task<AppleAuthDataModel> LoginAsync()
		{
			_loginTaskCompletionSource = new TaskCompletionSource<AppleAuthDataModel>();
			var appleIdProvider = new ASAuthorizationAppleIdProvider();
			var request = appleIdProvider.CreateRequest();
			request.RequestedScopes = new[] { ASAuthorizationScope.Email, ASAuthorizationScope.FullName };

			var authorizationController = new ASAuthorizationController(new[] { request });
			authorizationController.Delegate = this;
			authorizationController.PresentationContextProvider = this;
			authorizationController.PerformRequests();

			var result = await _loginTaskCompletionSource.Task;
			_loginTaskCompletionSource = null;
			return result;
		}

		[Export("authorizationController:didCompleteWithAuthorization:")]
		public void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
		{
			if (authorization.GetCredential<ASAuthorizationAppleIdCredential>() is ASAuthorizationAppleIdCredential appleIdCredential)
			{
				_loginTaskCompletionSource.TrySetResult(new AppleAuthDataModel(string.Empty,
																			   appleIdCredential.Email,
																			   appleIdCredential.IdentityToken.ToString(),
																			   string.Empty,
																			   string.Empty));
				return;
			}

			if (authorization.GetCredential<ASPasswordCredential>() is ASPasswordCredential passwordCredential)
			{
				_loginTaskCompletionSource.TrySetResult(new AppleAuthDataModel(passwordCredential.User,
																			   string.Empty,
																			   string.Empty,
																			   string.Empty,
																			   passwordCredential.Password)); 
				return;
			}

			if (authorization.GetCredential<ASAuthorizationSingleSignOnCredential>() is ASAuthorizationSingleSignOnCredential ssoCredentials)
			{
				_loginTaskCompletionSource.TrySetResult(new AppleAuthDataModel(string.Empty,
																			   string.Empty,
																			   ssoCredentials.IdentityToken.ToString(),
																			   ssoCredentials.AccessToken.ToString(),
																			   string.Empty));
				return;
			}
			_loginTaskCompletionSource.TrySetResult(null);
		}

		[Export("authorizationController:didCompleteWithError:")]
		public void DidComplete(ASAuthorizationController controller, NSError error)
		{
			_loginTaskCompletionSource.TrySetResult(null);
		}
	}
}