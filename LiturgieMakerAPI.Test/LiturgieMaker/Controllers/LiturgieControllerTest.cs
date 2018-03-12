using System;
using System.Linq;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.LiturgieMaker.Controllers;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LiturgieMakerAPI.Test.LiturgieMaker.Controllers
{
    public class LiturgieControllerTest
    {
        private Mock<LiturgieRepository> _liturgieRepositoryMock;
        private LiturgieController _controller;

        private Liturgie _valideActieveLiturgie;

        public LiturgieControllerTest()
        {
            Setup();
        }

        private void Setup()
        {
            _liturgieRepositoryMock = new Mock<LiturgieRepository>(null);
            _controller = new LiturgieController(_liturgieRepositoryMock.Object);

            SetupLiturgie();
        }

        private void SetupLiturgie()
        {
            _valideActieveLiturgie = LiturgieMakerInitializer.NieuweLiturgie("Test", DateTime.Now, DateTime.Now, false);
            _valideActieveLiturgie.Id = 456;
        }

        [Fact(Skip = "IMapper werkt nog niet mee")]
        public void Get_AlsBestaat_DanDtoTerug()
        {
            // Given
            MockGetLiturgie(_valideActieveLiturgie);

            // When
            var result = _controller.Get(_valideActieveLiturgie.Id.Value) as OkObjectResult;
            var liturgieDto = result?.Value as LiturgieDto;

            // Then
            Assert.NotNull(liturgieDto);
            Assert.Equal(_valideActieveLiturgie.Titel, liturgieDto.Titel);
        }

        [Fact]
        public void Get_AlsNietBestaat_DanNotFound()
        {
            // Given
            MockGetLiturgie(null);
            long testId = 78234;

            // When
            var result = _controller.Get(testId) as NotFoundObjectResult;
            var message = result?.Value as string;

            // Then
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(LiturgieController.ERROR_LITURGIE_BESTAAT_NIET, message);
        }

        [Fact]
        public void Post_AlsMetId_DanBadRequest()
        {
            //Given
            var liturgieDto = BuildLiturgieDto(54136);

            //When
            var result = _controller.Post(liturgieDto) as BadRequestObjectResult;
            var message = result?.Value as string;

            //Then
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(LiturgieController.ERROR_NIET_VALIDE_LITURGIE, message);
        }

        [Fact]
        public void Put_AlsAnderIdDanRoute_DanBadRequest()
        {
            //Given
            var liturgieDto = BuildLiturgieDto(1234);
            long testId = 12345;

            //When
            var result = _controller.Put(testId, liturgieDto) as BadRequestObjectResult;
            var message = result?.Value as string;

            //Then
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(LiturgieController.ERROR_NIET_VALIDE_LITURGIE, message);
        }

        [Fact]
        public void Delete_AlsBestaandeLiturgie_DanOpDeletedZetten()
        {
            // Given 
            MockGetLiturgie(_valideActieveLiturgie);
            MockDeleteLiturgie(_valideActieveLiturgie);

            // When
            _controller.Delete(_valideActieveLiturgie.Id.Value);

            // Then
            _liturgieRepositoryMock.Verify();
        }

        private LiturgieDto BuildLiturgieDto(long? id = null)
        {
            return new LiturgieDto
            {
                Id = id,
                Titel = "Hoi",
                Aanvangsdatum = DateTime.Now,
                Publicatiedatum = DateTime.Now
            };
        }

        private void MockGetLiturgie(Liturgie liturgie)
        {
            _liturgieRepositoryMock.Setup(mock => mock.GetLiturgie(It.Is<long>(l => l == liturgie.Id.Value)))
                .Returns(liturgie)
                .Verifiable("Liturgie ophalen");
        }

        private void MockDeleteLiturgie(params Liturgie[] liturgieen)
        {
            _liturgieRepositoryMock.Setup(mock => mock.DeleteLiturgie(It.Is<Liturgie>(l => liturgieen.Select(_l => _l.Id).Contains(l.Id))))
                .Verifiable("Liturgie op deleted zetten");
        }
    }
}