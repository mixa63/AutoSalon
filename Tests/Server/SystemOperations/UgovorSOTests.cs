using Common.Domain;
using DBBroker;
using Moq;
using ServerApp.SystemOperations.UgovorSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Tests.Server.SystemOperations
{
    public class UgovorSOTests
    {
        private void InvokeProtectedExecute(object so)
        {
            var method = so.GetType().GetMethod(
                "ExecuteConcreteOperation",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            method.Invoke(so, null);
        }

        #region KreirajUgovorSO

        [Theory]
        [InlineData(1, 2, 100)]
        [InlineData(5, 10, 555)]
        public void KreirajUgovorSO_ShouldCreateUgovorWithCorrectValues(int idProdavac, int idKupac, int idUgovor)
        {
            
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetFirstId(It.IsAny<Prodavac>())).Returns(idProdavac);
            mockBroker.Setup(b => b.GetFirstId(It.IsAny<Kupac>())).Returns(idKupac);
            mockBroker.Setup(b => b.AddWithReturnId(It.IsAny<Ugovor>())).Returns(idUgovor);

            var so = new KreirajUgovorSO(new Ugovor());
            typeof(KreirajUgovorSO)
                .GetField("broker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            
            InvokeProtectedExecute(so);

            
            var result = so.Result;
            Assert.NotNull(result);
            Assert.Equal(idProdavac, result.Prodavac.IdProdavac);
            Assert.Equal(idKupac, result.Kupac.IdKupac);
            Assert.Equal(idUgovor, result.IdUgovor);
            Assert.Equal(0.2, result.PDV);
            Assert.Equal(DateTime.Now.Date, result.Datum);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        public void KreirajUgovorSO_ShouldThrow_WhenNoProdavacOrKupac(int prodavacId, int kupacId)
        {
            
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetFirstId(It.IsAny<Prodavac>())).Returns(prodavacId);
            mockBroker.Setup(b => b.GetFirstId(It.IsAny<Kupac>())).Returns(kupacId);

            var so = new KreirajUgovorSO(new Ugovor());
            typeof(KreirajUgovorSO)
                .GetField("broker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            
            var ex = Assert.Throws<TargetInvocationException>(() => InvokeProtectedExecute(so));

            // Provera da li je unutrašnji izuzetak pravi
            Assert.IsType<Exception>(ex.InnerException);
            Assert.True(ex.InnerException.Message.Contains("Mora postojati bar jedan"));
        }

        #endregion

        #region PretraziUgovorSO

        [Fact]
        public void PretraziUgovorSO_ShouldReturnUgovorWithStavkeAndAutomobili()
        {
            
            var ugovor = new Ugovor { IdUgovor = 1 };
            var stavka = new StavkaUgovora { Rb = 1, Automobil = new Automobil { IdAutomobil = 10 }, Ugovor = ugovor };
            var automobil = new Automobil { IdAutomobil = 10, Model = "Tesla" };

            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.Is<Ugovor>(u => u == u)))
                      .Returns(new List<IEntity> { ugovor });
            mockBroker.Setup(b => b.GetByCondition(It.Is<StavkaUgovora>(s => s.Ugovor.IdUgovor == 1)))
                      .Returns(new List<IEntity> { stavka });
            mockBroker.Setup(b => b.GetByCondition(It.Is<Automobil>(a => a.IdAutomobil == 10)))
                      .Returns(new List<IEntity> { automobil });

            var so = new PretraziUgovorSO(new Ugovor { IdUgovor = 1 });
            typeof(PretraziUgovorSO)
                .GetField("broker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            
            InvokeProtectedExecute(so);

            
            Assert.NotNull(so.Result);
            Assert.Single(so.Result.Stavke);
            Assert.Equal("Tesla", so.Result.Stavke[0].Automobil.Model);
            Assert.Equal(1, so.Result.Stavke[0].Rb);
        }

        [Fact]
        public void PretraziUgovorSO_ShouldReturnNull_WhenNoUgovorFound()
        {
            
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Ugovor>())).Returns(new List<IEntity>());

            var so = new PretraziUgovorSO(new Ugovor { IdUgovor = 999 });
            typeof(PretraziUgovorSO)
                .GetField("broker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            
            InvokeProtectedExecute(so);

            
            Assert.Null(so.Result);
        }

        #endregion

        #region PromeniUgovorSO

        [Fact]
        public void PromeniUgovorSO_ShouldUpdateUgovorAndStavke()
        {
            
            var stavka1 = new StavkaUgovora { Automobil = new Automobil { IdAutomobil = 1 } };
            var stavka2 = new StavkaUgovora { Automobil = new Automobil { IdAutomobil = 2 } };
            var ugovor = new Ugovor { IdUgovor = 100, Stavke = new List<StavkaUgovora> { stavka1, stavka2 } };

            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<StavkaUgovora>()))
                      .Returns(new List<IEntity>
                      {
                          new StavkaUgovora { Rb = 1, Ugovor = ugovor },
                          new StavkaUgovora { Rb = 2, Ugovor = ugovor }
                      });

            var so = new PromeniUgovorSO(ugovor);
            typeof(PromeniUgovorSO)
                .GetField("broker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            
            InvokeProtectedExecute(so);

            
            Assert.True(so.Result);
            mockBroker.Verify(b => b.Update(ugovor), Times.Once);
            mockBroker.Verify(b => b.Delete(It.IsAny<StavkaUgovora>()), Times.Exactly(2));
            mockBroker.Verify(b => b.Add(It.IsAny<StavkaUgovora>()), Times.Exactly(2));
            Assert.Equal(1, stavka1.Rb);
            Assert.Equal(2, stavka2.Rb);
        }

        #endregion

        #region VratiListuUgovorSO

        [Fact]
        public void VratiListuUgovorSO_ShouldReturnListOfUgovoriWithCorrectIds()
        {
            
            var ugovori = new List<IEntity>
            {
                new Ugovor { IdUgovor = 1 },
                new Ugovor { IdUgovor = 2 }
            };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Ugovor>())).Returns(ugovori);

            var so = new VratiListuUgovorSO(new Ugovor());
            typeof(VratiListuUgovorSO)
                .GetField("broker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            
            InvokeProtectedExecute(so);

            
            Assert.Equal(2, so.Result.Count);
            Assert.Contains(so.Result, u => u.IdUgovor == 1);
            Assert.Contains(so.Result, u => u.IdUgovor == 2);
        }

        #endregion
    }
}
