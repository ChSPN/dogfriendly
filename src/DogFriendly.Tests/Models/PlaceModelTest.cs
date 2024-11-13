using DogFriendly.Domain.Command.Favorites;
using DogFriendly.Domain.Models;
using DogFriendly.Domain.Queries.Places;
using MediatR;
using Moq;

namespace DogFriendly.Tests.Models
{
    [TestClass]
    public class PlaceModelTests
    {
        private Mock<IMediator> _mediatorMock;
        private PlaceModel _placeModel;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _placeModel = new PlaceModel(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task AddFavorite_ShouldSendAddPlaceFavoriteCommand()
        {
            // Arrange
            var favoriteId = 1;
            var userEmail = "test@example.com";

            // Act
            await _placeModel.AddFavorite(favoriteId, userEmail);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<AddPlaceFavoriteCommand>(cmd => cmd.FavoriteId == favoriteId && cmd.UserEmail == userEmail), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task AddPhotos_ShouldSendPlacePhotosCommand()
        {
            // Arrange
            var photos = new Dictionary<string, Stream>();
            var userEmail = "test@example.com";

            // Act
            await _placeModel.AddPhotos(photos, userEmail);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<PlacePhotosCommand>(cmd => cmd.Photos == photos && cmd.UserEmail == userEmail), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Save_ShouldSendPlaceSaveCommand()
        {
            // Arrange
            var userEmail = "test@example.com";

            // Act
            await _placeModel.Save(userEmail);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<PlaceSaveCommand>(cmd => cmd.Place == _placeModel), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task LoadViewModel_ShouldSendPlaceViewQuery()
        {
            // Arrange
            var placeId = 1;
            var userEmail = "test@example.com";

            // Act
            await PlaceModel.LoadViewModel(_mediatorMock.Object, placeId, userEmail);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<PlaceViewQuery>(query => query.PlaceId == placeId && query.UserEmail == userEmail), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Search_ShouldSendPlaceListViewQuery()
        {
            // Arrange
            var request = new PlaceListViewQuery();

            // Act
            await PlaceModel.Search(_mediatorMock.Object, request);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<PlaceListViewQuery>(query => query == request), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
