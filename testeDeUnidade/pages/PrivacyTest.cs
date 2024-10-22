using Back_end.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace testeDeUnidade.pages
{
    public class PrivacyTests
    {
        private PrivacyModel privacyModel;
        private ILogger<PrivacyModel> logger;

        [SetUp]
        public void Setup()
        {
            logger = new Logger<PrivacyModel>(new LoggerFactory());
            privacyModel = new PrivacyModel(logger);
        }

        [Test]
        public void OnGet()
        {
            Assert.DoesNotThrow(() => privacyModel.OnGet(), "OnGet should not throw an exception.");
        }

        [Test]
        public void PrivacyModel()
        {
            Assert.IsNotNull(privacyModel, "PrivacyModel should be instantiated.");
        }
    }
}
