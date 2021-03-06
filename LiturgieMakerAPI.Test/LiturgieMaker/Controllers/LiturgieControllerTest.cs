using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.LiturgieMaker.Controllers;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using LiturgieMakerAPI.Test.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LiturgieMakerAPI.Test.LiturgieMaker.Controllers
{
    public class LiturgieControllerTest
    {
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

        [Fact(Skip = "IMapper doet nog niet lief")]
        public void GetAll_AlsBestaat_DanLijstTerug()
        {
            //Given
            var liturgieen = new List<Liturgie>();
            liturgieen.Add(_valideActieveLiturgie);
            liturgieen.Add(LiturgieMakerInitializer.BuildLiturgie("Nog een", DateTime.Now.AddDays(5), DateTime.Now.AddDays(6)));
            var liturgieDtos = liturgieen.Select(l => BuildLiturgieDto(l));
            MockGetLiturgieen(liturgieen);
            MockLiturgieMapper(liturgieen, liturgieDtos);

            //When
            var result = _controller.Get(1, 10);

            //Then
            ActionResultTestHelper.AssertOk(result, liturgieDtos);
        }

        [Fact]
        public void GetMetId_AlsBestaat_DanDtoTerug()
        {
            // Given
            LiturgieDto dto = BuildLiturgieDto(_valideActieveLiturgie);
            MockGetLiturgie(_valideActieveLiturgie);
            MockLiturgieMapper(_valideActieveLiturgie, dto);

            // When
            var result = _controller.Get(_valideActieveLiturgie.Id.Value);

            // Then
            ActionResultTestHelper.AssertOk(result, dto);
        }

        [Fact]
        public void GetMetId_AlsNietBestaat_DanNotFound()
        {
            // Given
            MockGetLiturgie(null);
            long testId = 78234;

            // When
            var result = _controller.Get(testId);

            // Then
            ActionResultTestHelper.AssertNotFound(result, LiturgieController.ERROR_LITURGIE_BESTAAT_NIET);
        }

        [Fact]
        public void Post_AlsMetId_DanBadRequest()
        {
            //Given
            var liturgieDto = BuildLiturgieDto(54136);

            //When
            var result = _controller.Post(liturgieDto);

            //Then
            ActionResultTestHelper.AssertBadRequest(result, LiturgieController.ERROR_NIET_VALIDE_LITURGIE);
        }

        [Fact]
        public void Put_AlsAnderIdDanRoute_DanBadRequest()
        {
            //Given
            var liturgieDto = BuildLiturgieDto(1234);
            long testId = 12345;

            //When
            var result = _controller.Put(testId, liturgieDto);

            //Then
            ActionResultTestHelper.AssertBadRequest(result, LiturgieController.ERROR_NIET_VALIDE_LITURGIE);
        }

        [Fact]
        public void Delete_AlsBestaandeLiturgie_DanOpDeletedZetten()
        {
            // Given 
            MockGetLiturgie(_valideActieveLiturgie);
            MockDeleteLiturgie(_valideActieveLiturgie);

            // When
            var result = _controller.Delete(_valideActieveLiturgie.Id.Value);

            // Then
            _liturgieRepositoryMock.Verify();
            ActionResultTestHelper.AssertNoContent(result);
        }

        [Fact]
        public void Delete_AlsNietBestaandeLiturgie_DanNotFound()
        {
            //Given
            var testId = 890859;
            MockGetLiturgie(null);

            //When
            var result = _controller.Delete(testId);

            //Then
            ActionResultTestHelper.AssertNotFound(result, LiturgieController.ERROR_LITURGIE_BESTAAT_NIET);
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

        private void MockLiturgieMapper(IEnumerable<Liturgie> source, IEnumerable<LiturgieDto> target)
        {
            Type targetType = target.GetType();
            _mapperMock.Setup(mock => mock.Map<IEnumerable<LiturgieDto>>(source))
                .Returns(target);
        }

        private void MockGetLiturgieen(IEnumerable<Liturgie> liturgieen)
        {
            _liturgieRepositoryMock.Setup(mock => mock.GetLiturgieen(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(liturgieen)
                .Verifiable("Liturgie ophalen");
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