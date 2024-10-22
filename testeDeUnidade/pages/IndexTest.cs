using Back_end.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace testeDeUnidade.pages
{
    public class IndexTests
    {
        private IndexModel index;
        private ILogger<IndexModel> logger;

        [SetUp]
        public void Setup()
        {
            logger = new Logger<IndexModel>(new LoggerFactory());
            index = new IndexModel(logger);
        }

        [Test]
        public void OnGet()
        {
            Assert.DoesNotThrow(() => index.OnGet(), "OnGet should not throw an exception.");
        }

        [Test]
        public void IndexModel()
        {
            Assert.IsNotNull(index, "IndexModel should be instantiated.");
        }
    }
}
