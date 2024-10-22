using Back_end.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Diagnostics;

namespace testeDeUnidade.pages
{
    public class ErrorTests
    {
        private ErrorModel error;
        private ILogger<ErrorModel> logger;

        [SetUp]
        public void Setup()
        {
            logger = new Logger<ErrorModel>(new LoggerFactory());
            error = new ErrorModel(logger);

            error.PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Test]
        public void OnGet_SetId()
        {
            error.OnGet();
            Assert.That(error.RequestId, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void ShowRequestId()
        {
            error.RequestId = "12";
            var result = error.ShowRequestId;
            Assert.That(result, Is.True);
        }

        [Test]
        public void ShowRequestId_Null()
        {
            error.RequestId = null;
            var result = error.ShowRequestId;
            Assert.That(result, Is.False);
        }
    }
}
