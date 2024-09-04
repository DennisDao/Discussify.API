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

        [Test]
        public void Create_ValidParameters_ShouldCreatePost()
        {
            // Arrange
            var userId = 1;
            var title = "Sample Title";
            var description = "Sample Description";

            // Act
            var post = Post.Create(userId, title, description);

            // Assert
            Assert.AreEqual(userId, post.UserId);
            Assert.AreEqual(title, post.Title);
            Assert.AreEqual(description, post.Description);
            Assert.IsTrue(post.WhenCreated <= DateTime.UtcNow);
            Assert.IsTrue(post.WhenUpdated <= DateTime.UtcNow);
        }

        [Test]
        public void ChangeImage_ValidImage_ShouldUpdateImageAndWhenUpdated()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");
            var newImage = "image.png";

            // Act
            post.ChangeImage(newImage);

            // Assert
            Assert.AreEqual(newImage, post.Image);
            Assert.IsTrue(post.WhenUpdated > post.WhenCreated);
        }

        [Test]
        public void ChangeImage_EmptyImage_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.ChangeImage(""));
            Assert.AreEqual("Image is empty", ex.Message);
        }

        [Test]
        public void SetCategory_ValidCategory_ShouldUpdateCategoryAndWhenUpdated()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");
            var category = Category.Create("Tech", "test", "image");

            // Act
            post.SetCategory(category);

            // Assert
            Assert.AreEqual(category, post.Category);
            Assert.IsTrue(post.WhenUpdated > post.WhenCreated);
        }

        [Test]
        public void SetCategory_NullCategory_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.SetCategory(null));
            Assert.AreEqual("Category is empty", ex.Message);
        }

        [Test]
        public void AddTags_ValidTag_ShouldAddTag()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");
            var tag =  Tag.Create("Tech");

            // Act
            post.AddTags(tag);

            // Assert
            Assert.Contains(tag, (System.Collections.ICollection)post.Tags);
        }

        [Test]
        public void AddTags_NullTag_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.AddTags(null));
            Assert.AreEqual("Tag is empty", ex.Message);
        }

        [Test]
        public void AddComment_ValidComment_ShouldAddCommentAndRaiseEvent()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");
            var comment =  Comment.Create(1, 1,"This is a comment");

            // Act
            post.AddComment(comment);

            // Assert
            Assert.Contains(comment, (System.Collections.ICollection)post.Comments);
            Assert.IsTrue(post.DomainEvents.Count > 0);
        }

        [Test]
        public void AddComment_NullComment_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "Sample Description");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.AddComment(null));
            Assert.AreEqual("Comment is empty", ex.Message);
        }

        [Test]
        public void Validate_InvalidUserId_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(0, "Sample Title", "Sample Description");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.Validate());
            Assert.AreEqual("Invalid user Id", ex.Message);
        }

        [Test]
        public void Validate_EmptyTitle_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(1, "", "Sample Description");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.Validate());
            Assert.AreEqual("Title cannot be empty", ex.Message);
        }

        [Test]
        public void Validate_EmptyDescription_ShouldThrowPostInvalidStateException()
        {
            // Arrange
            var post = Post.Create(1, "Sample Title", "");

            // Act & Assert
            var ex = Assert.Throws<PostInvalidStateException>(() => post.Validate());
            Assert.AreEqual("Description cannot be empty", ex.Message);
        }
    }
}
