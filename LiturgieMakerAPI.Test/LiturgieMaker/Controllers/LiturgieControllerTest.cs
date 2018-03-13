using System;
using System.Linq;
using AutoMapper;
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
        private const int STATUSCODE_BADREQUEST = 400;
        private const int STATUSCODE_NOTFOUND = 404;

        private Mock<LiturgieRepository> _liturgieRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private LiturgieController _controller;

        private Liturgie _valideActieveLiturgie;

        public LiturgieControllerTest()
        {
            _mapperMock = new Mock<IMapper>();
            _liturgieRepositoryMock = new Mock<LiturgieRepository>(null);
            _controller = new LiturgieController(_liturgieRepositoryMock.Object, _mapperMock.Object);

            SetupLiturgie();
        }

        private void SetupLiturgie()
        {
            _valideActieveLiturgie = LiturgieMakerInitializer.BuildLiturgie("Test", DateTime.Now, DateTime.Now, false);
            _valideActieveLiturgie.Id = 456;
        }

        [Fact]
        public void Get_AlsBestaat_DanDtoTerug()
        {
            // Given
            MockGetLiturgie(_valideActieveLiturgie);
            MockLiturgieMapper(_valideActieveLiturgie, BuildLiturgieDto(_valideActieveLiturgie));

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
            Assert.Equal(STATUSCODE_NOTFOUND, result.StatusCode);
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
            Assert.Equal(STATUSCODE_BADREQUEST, result.StatusCode);
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
            Assert.Equal(STATUSCODE_BADREQUEST, result.StatusCode);
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

        [Fact]
        public void Delete_AlsNietBestaandeLiturgie_DanNotFound()
        {
            //Given
            var testId = 890859;
            MockGetLiturgie(null);

            //When
            var result = _controller.Delete(testId) as NotFoundObjectResult;
            var message = result?.Value as string;

            //Then
            Assert.NotNull(result);
            Assert.Equal(STATUSCODE_NOTFOUND, result.StatusCode);
            Assert.Equal(LiturgieController.ERROR_LITURGIE_BESTAAT_NIET, message);
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

        private LiturgieDto BuildLiturgieDto(Liturgie liturgie)
        {
            return new LiturgieDto
            {
                Id = liturgie.Id,
                Titel = liturgie.Titel,
                Aanvangsdatum = liturgie.Aanvangsdatum,
                Publicatiedatum = liturgie.Publicatiedatum
            };
        }

        private void MockLiturgieMapper(Liturgie source, LiturgieDto target)
        {
            _mapperMock.Setup(mock => mock.Map<LiturgieDto>(source))
                .Returns(target);
        }

        private void MockGetLiturgie(Liturgie liturgie)
        {
            _liturgieRepositoryMock.Setup(mock => mock.GetLiturgie(It.IsAny<long>()))
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