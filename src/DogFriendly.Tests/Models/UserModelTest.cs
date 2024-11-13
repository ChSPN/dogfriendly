using DogFriendly.Domain.Command.Users;
using DogFriendly.Domain.Models;
using DogFriendly.Domain.Queries.Users;
using DogFriendly.Domain.ViewModels.Places;
using DogFriendly.Domain.ViewModels.Users;
using MediatR;
using Moq;

namespace DogFriendly.Tests.Models
{
    [TestClass]
    public class UserModelTests
    {
        private Mock<IMediator> _mediatorMock;
        private UserModel _userModel;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _userModel = new UserModel(_mediatorMock.Object, "test@example.com", "TestUser", 1);
        }

        [TestMethod]
        public async Task Create_ShouldReturnSuccessResponse_WhenUserIsCreated()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<UserNameExistQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UserPictureUploadCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("http://example.com/picture.jpg");
            _mediatorMock.Setup(m => m.Send(It.IsAny<UserRegisterCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var response = await _userModel.Create();

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(string.Empty, response.Message);
        }

        [TestMethod]
        public async Task Create_ShouldReturnFailureResponse_WhenUserNameExists()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<UserNameExistQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var response = await _userModel.Create();

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("Ce pseudo est déjà utilisé.", response.Message);
        }

        [TestMethod]
        public async Task GetPlaceFavorites_ShouldReturnFavorites()
        {
            // Arrange
            var favorites = new List<UserFavoriteViewModel> { new UserFavoriteViewModel() };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserFavoritesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(favorites);

            // Act
            var result = await _userModel.GetPlaceFavorites();

            // Assert
            Assert.AreEqual(favorites, result);
        }

        [TestMethod]
        public async Task GetPlaces_ShouldReturnPlaces()
        {
            // Arrange
            var places = new List<PlaceListViewModel>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserPlacesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(places);

            // Act
            var result = await _userModel.GetPlaces(1);

            // Assert
            Assert.AreEqual(places, result);
        }

        [TestMethod]
        public async Task GetPlaceReviews_ShouldReturnReviews()
        {
            // Arrange
            var reviews = new List<UserReviewViewModel> { new UserReviewViewModel() };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserReviewsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(reviews);

            // Act
            var result = await _userModel.GetPlaceReviews();

            // Assert
            Assert.AreEqual(reviews, result);
        }

        [TestMethod]
        public async Task IsExist_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<UserEmailExistQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            // Act
            var result = await _userModel.IsExist();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
