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
    public class LiturgieRepositoryTest : IDisposable
    {
        LiturgieMakerContext _context;
        LiturgieRepository _repository;

        Liturgie _valideActieveLiturgie;

        public LiturgieRepositoryTest()
        {
            _context = new LiturgieMakerContext(new DbContextOptionsBuilder<LiturgieMakerContext>().UseInMemoryDatabase("LiturgieMaker").Options);
            _repository = new LiturgieRepository(_context);

            SetupLiturgie();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private void SetupLiturgie()
        {
            _valideActieveLiturgie = LiturgieMakerInitializer.BuildLiturgie("Test", DateTime.Now, DateTime.Now, false);
            _repository.SaveLiturgie(_valideActieveLiturgie);
        }

        [Fact]
        public void Get_AlsBestaat_DanTeruggeven()
        {
            //When
            var liturgie = _repository.GetLiturgie(_valideActieveLiturgie.Id.Value);

            //Then`
            AssertEqualLiturige(_valideActieveLiturgie, liturgie);
        }

        [Fact]
        public void Delete_AlsAllesGoed_DanLiturgieOpDeleted()
        {
            //When
            _repository.DeleteLiturgie(_valideActieveLiturgie);

            //Then
            var liturgie = _context.Liturgie.Where(l => l.Id == _valideActieveLiturgie.Id.Value).SingleOrDefault();
            Assert.True(liturgie.Deleted);
        }

        private void AssertEqualLiturige(Liturgie expected, Liturgie actually)
        {
            Assert.NotNull(actually);
            Assert.Equal(expected.Id.Value, actually.Id.Value);
            Assert.Equal(expected.Titel, actually.Titel);
            Assert.Equal(expected.Aanvangsdatum, actually.Aanvangsdatum);
        }
    }
}