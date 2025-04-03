using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;
using Web.UI.ViewModels;

namespace Web.UI.Controllers
{
    [Authorize]
    public class UIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UIController> _logger;

        public UIController(IHttpClientFactory httpClientFactory,
            ILogger<UIController> logger)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            await LogIdentityInformation();

            return View(new IndexViewModel());
        }

        public async Task LogIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var userClaimsStringBuilder = new StringBuilder();
            foreach (var claim in User.Claims)
            {
                userClaimsStringBuilder.AppendLine(
                    $"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }

            // log token & claims
            _logger.LogInformation($"Identity token & user claims: " +
                                   $"\n{identityToken} \n{userClaimsStringBuilder}");
        }
    }
}
