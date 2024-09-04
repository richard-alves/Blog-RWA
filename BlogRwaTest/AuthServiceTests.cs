using BlogR.Data;
using BlogR.Entities;
using BlogR.Services.Implementations;
using BlogR.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogR.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private BlogDbContext _dbContext;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IUserService> _userServiceMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "BlogDbTest")
                .Options;

            _dbContext = new BlogDbContext(options);

            _configurationMock = new Mock<IConfiguration>();
            _userServiceMock = new Mock<IUserService>();

            _authService = new AuthService(_dbContext, _configurationMock.Object, _userServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Register_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new UserModel { Username = "testuser" };
            var password = "TestPassword123";

            _userServiceMock.Setup(x => x.UserExists(It.IsAny<string>())).ReturnsAsync(false);
            _configurationMock.Setup(x => x["Jwt:KeyToken"]).Returns("DummySecretKeyForJWT12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");

            // Act
            var token = await _authService.Register(user, password);

            // Assert
            var createdUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == user.Username);

            Assert.IsNotNull(createdUser);
            Assert.AreEqual(user.Username, createdUser.Username);
            Assert.IsNotNull(token);
            Assert.IsInstanceOf<string>(token);
        }

        [Test]
        public void Register_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var user = new UserModel { Username = "existinguser" };
            var password = "TestPassword123";

            _userServiceMock.Setup(x => x.UserExists(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _authService.Register(user, password));
            Assert.That(ex.Message, Is.EqualTo("Usuário já existe"));
        }

        [Test]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = "TestPassword123";

            var hmac = new HMACSHA512();
            var user = new UserModel
            {
                Username = "testuser",
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            _configurationMock.Setup(x => x["Jwt:KeyToken"]).Returns("DummySecretKeyForJWT12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");

            // Act
            var token = await _authService.Login(user.Username, password);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsInstanceOf<string>(token);
        }


        [Test]
        public void Login_ShouldThrowException_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new UserModel
            {
                Username = "testuser",
                PasswordHash = new HMACSHA512().ComputeHash(Encoding.UTF8.GetBytes("TestPassword123")),
                PasswordSalt = new HMACSHA512().Key
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            _configurationMock.Setup(x => x["Jwt:KeyToken"]).Returns("DummySecretKeyForJWT12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _authService.Login(user.Username, "WrongPassword"));
            Assert.That(ex.Message, Is.EqualTo("Senha incorreta"));
        }

        [Test]
        public void Login_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            _configurationMock.Setup(x => x["Jwt:KeyToken"]).Returns("DummySecretKeyForJWT12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _authService.Login("nonexistentuser", "SomePassword"));
            Assert.That(ex.Message, Is.EqualTo("Usuário não encontrado"));
        }
    }
}
