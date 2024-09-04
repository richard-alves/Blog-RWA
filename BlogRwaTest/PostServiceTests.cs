using BlogR.Data;
using BlogR.Entities;
using BlogR.Hubs;
using BlogR.Services.Implementations;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogR.Tests.Services
{
    [TestFixture]
    public class PostServiceTests
    {
        private BlogDbContext _dbContext;
        private Mock<IHubContext<NotificationHub>> _hubContextMock;
        private PostService _postService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "BlogDbTest")
                .Options;

            _dbContext = new BlogDbContext(options);

            _hubContextMock = new Mock<IHubContext<NotificationHub>>();

            _postService = new PostService(_dbContext, _hubContextMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllPosts_ShouldReturnAllPosts()
        {
            // Arrange
            var user = new UserModel { Id = 1, Username = "testuser" };
            var posts = new List<PostModel>
            {
                new PostModel { Id = 1, Title = "Post 1", Content = "Content 1", User = user },
                new PostModel { Id = 2, Title = "Post 2", Content = "Content 2", User = user }
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.Posts.AddRangeAsync(posts);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _postService.GetAllPosts();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task CreatePost_ShouldCreatePostAndSendNotification()
        {
            // Arrange
            var user = new UserModel { Id = 1, Username = "testuser" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var post = new PostModel { Id = 1, Title = "New Post", Content = "Post Content", UserId = user.Id };

            var mockClientProxy = new Mock<IClientProxy>();

            _hubContextMock.Setup(x => x.Clients.All).Returns(mockClientProxy.Object);

            // Act
            var result = await _postService.CreatePost(post);

            // Assert
            Assert.AreEqual("New Post", result.Title);
            Assert.AreEqual("Post Content", result.Content);
            Assert.AreEqual(user.Id, result.UserId);

            mockClientProxy.Verify(
                x => x.SendCoreAsync("ReceiveMessage",
                    It.Is<object[]>(o => o.Length == 2 && (string)o[0] == user.Username && (string)o[1] == post.Title),
                    default),
                Times.Once);
        }


        [Test]
        public async Task UpdatePost_ShouldUpdatePost_WhenUserIsValid()
        {
            // Arrange
            var user = new UserModel { Id = 1, Username = "testuser" };
            await _dbContext.Users.AddAsync(user);

            var post = new PostModel { Id = 1, Title = "Old Post", Content = "Old Content", UserId = user.Id };
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            post.Title = "Updated Post";
            post.Content = "Updated Content";

            // Act
            var result = await _postService.UpdatePost(post, user.Id);

            // Assert
            Assert.AreEqual("Updated Post", result.Title);
            Assert.AreEqual("Updated Content", result.Content);
        }

        [Test]
        public void UpdatePost_ShouldThrowException_WhenUserIsInvalid()
        {
            // Arrange
            var post = new PostModel { Id = 1, Title = "Post", Content = "Content", UserId = 1 };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _postService.UpdatePost(post, 2));
            Assert.That(ex.Message, Is.EqualTo("Você só pode alterar e remover posts criado por você."));
        }

        [Test]
        public async Task DeletePost_ShouldDeletePost_WhenUserIsValid()
        {
            // Arrange
            var user = new UserModel { Id = 1, Username = "testuser" };
            await _dbContext.Users.AddAsync(user);

            var post = new PostModel { Id = 1, Title = "Post to Delete", Content = "Post to Delete", UserId = user.Id };
            await _dbContext.Posts.AddAsync(post);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _postService.DeletePost(post.Id, user.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(await _dbContext.Posts.FindAsync(post.Id));
        }

        [Test]
        public async Task DeletePost_ShouldReturnFalse_WhenPostDoesNotExist()
        {
            // Act
            var result = await _postService.DeletePost(999, 1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
