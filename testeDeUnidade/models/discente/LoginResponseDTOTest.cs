namespace testeDeUnidade.models.discente
{
    using NUnit.Framework;
    using Back_end.Models;

    public class LoginResponseDTOTests
    {
        private LoginResponseDto login;

        [SetUp]
        public void Setup()
        {
            login = new LoginResponseDto();
        }

        [Test]
        public void LoginResponseDTOGetSetUserId()
        {
            login.UserId = "1"; 
            Assert.That(login.UserId, Is.EqualTo("1"));
        }

        [Test]
        public void LoginResponseDTOGetSetToken()
        {
            login.Token = "#ABC123"; 
            Assert.That(login.Token, Is.EqualTo("#ABC123"));
        }
    }
}
