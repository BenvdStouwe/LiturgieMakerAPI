using System;
using System.Linq;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Model;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LiturgieMakerAPI.Test.LiturgieMaker.Repositories
{
    public class LiturgieRepositoryTest
    {
        LiturgieMakerContext _context;
        LiturgieRepository _repository;

        public LiturgieRepositoryTest()
        {
            _context = new LiturgieMakerContext(new DbContextOptionsBuilder<LiturgieMakerContext>().UseInMemoryDatabase("LiturgieMaker").Options);
            _repository = new LiturgieRepository(_context);
        }

        private Liturgie SetupLiturgie()
        {
            var liturgie = LiturgieMakerInitializer.BuildLiturgie("Test", DateTime.Now, DateTime.Now, false);
            _repository.SaveLiturgie(liturgie);
            return liturgie;
        }

        [Fact]
        public void GetAlles_AlsErDeletedZijn_DanNegeren()
        {
            //Given
            var liturgie = SetupLiturgie();
            var verwijderdeLiturgie = LiturgieMakerInitializer.BuildLiturgie("Verwijderd", DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(-1), deleted: true);
            _repository.SaveLiturgie(verwijderdeLiturgie);

            //When
            var result = _repository.GetLiturgieen();

            //Then
            Assert.Single(result);
            Assert.Equal(liturgie.Id.Value, result.First().Id.Value);
        }

        [Fact]
        public void GetMetId_AlsBestaat_DanTeruggeven()
        {
            //Given 
            var liturgie = SetupLiturgie();

            //When
            var liturgieUitDb = _repository.GetLiturgie(liturgie.Id.Value);

            //Then`
            AssertEqualLiturige(liturgie, liturgieUitDb);
        }

        [Fact]
        public void GetMetId_AlsNietBestaat_DanReturnNull()
        {
            //Given
            var testId = 7891237;

            //When
            var liturgie = _repository.GetLiturgie(testId);

            //Then
            Assert.Null(liturgie);
        }

        [Fact]
        public void Delete_AlsAllesGoed_DanLiturgieOpDeleted()
        {
            //Given
            var liturgie = SetupLiturgie();

            //When
            _repository.DeleteLiturgie(liturgie);

            //Then
            var liturgieUitDb = _context.Liturgie.Where(l => l.Id == liturgie.Id.Value).SingleOrDefault();
            Assert.True(liturgieUitDb.Deleted);
        }

        private void AssertEqualLiturige(Liturgie expected, Liturgie actually)
        {
            Assert.NotNull(actually);
            Assert.Equal(expected.Id.Value, actually.Id.Value);
            Assert.Equal(expected.Titel, actually.Titel);
            Assert.Equal(expected.Aanvangsdatum, actually.Aanvangsdatum);
            Assert.Equal(expected.Publicatiedatum, actually.Publicatiedatum);
            Assert.Equal(expected.Deleted, actually.Deleted);
        }
    }
}