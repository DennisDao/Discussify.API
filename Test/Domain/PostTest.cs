using Castle.Core.Resource;
using Domain.AggegratesModel.PostAggegrate;
using Domain.AggegratesModel.PostAggegrate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Domain
{
    public class PostTest
    {
        [Test]
        public void Given_Creating_A_New_Post_Should_Succeed()
        {
            // Act
            var post = Post.Create(1, "Test title", "test description");

            // Assert
            Assert.IsNotNull(post);
            Assert.That(post.Title, Is.EqualTo("Test title"));
            Assert.That(post.Description, Is.EqualTo("test description"));
        }

        [Test]
        public void Given_Creating_A_New_Post_With_Empty_Title_Should_Fail()
        {
            // Arrange
            var post = Post.Create(1, string.Empty, "test description");

            // Act
            var ex = Assert.Throws<PostInvalidStateException>(post.Validate);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Title cannot be empty"));
        }

        [Test]
        public void Given_Creating_A_New_Post_With_A_Invalid_UserId_Should_Fail()
        {
            // Arrange
            var post = Post.Create(0, "test", "test description");

            // Act
            var ex = Assert.Throws<PostInvalidStateException>(post.Validate);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid user Id"));
        }

        [Test]
        public void Given_Creating_A_New_Post_With_Empty_Description_Should_Fail()
        {
            // Arrange
            var post = Post.Create(1, "test", string.Empty);

            // Act
            var ex = Assert.Throws<PostInvalidStateException>(post.Validate);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Description cannot be empty"));
        }

        [Test]
        public void Given_Adding_A_Comment_To_A_Post_Should_Pass()
        {
            // Arrange
            var post = Post.Create(1, "test", "test");
            var comment = Comment.Create(1, 1, "hello");

            // Act
            post.AddComment(comment);

            // Assert
            Assert.That(post.Comments.ToList()[0], Is.EqualTo(comment));
            Assert.That(post.Comments.ToList()[0].Content, Is.EqualTo("hello"));
        }

        [Test]
        public void Given_Adding_A_Comment_To_A_Post_Should_Raise_A_Domain_Event()
        {
            // Arrange
            var post = Post.Create(1, "test", "test");
            var comment = Comment.Create(1, 1, "hello");

            // Act
            post.AddComment(comment);

            // Assert
            Assert.That(post.DomainEvents.Count, Is.EqualTo(1));
        }

        [Test]
        public void Given_Clearing_Domain_Event_Should_Pass()
        {
            // Arrange
            var post = Post.Create(1, "test", "test");
            var comment = Comment.Create(1, 1, "hello");

            // Act
            post.AddComment(comment);
            post.DomainEvents.Clear();

            // Assert
            Assert.That(post.DomainEvents.Count, Is.EqualTo(0));
        }
    }
}
