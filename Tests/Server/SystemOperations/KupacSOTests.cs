using Common.Domain;
using DBBroker;
using Moq;
using ServerApp.SystemOperations.KupacSO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tests.Server.SystemOperations
{
    public class KupacSOTests
    {
        private void InvokeProtectedExecute(object so)
        {
            var method = so.GetType().GetMethod(
                "ExecuteConcreteOperation",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            method.Invoke(so, null);
        }

        #region KreirajKupacSO

        [Fact]
        public void KreirajKupacSO_ShouldCreateKupacWithId()
        {
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.AddWithReturnId(It.IsAny<Kupac>())).Returns(101);

            var so = new KreirajKupacSO(new Kupac());
            typeof(KreirajKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.NotNull(so.Result);
            Assert.Equal(101, so.Result.IdKupac);
            Assert.Equal("", so.Result.Email);
        }

        #endregion

        #region ObrisiKupacSO

        [Fact]
        public void ObrisiKupacSO_ShouldDeleteKupac_WhenNoUgovori()
        {
            var kupac = new Kupac { IdKupac = 1 };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Ugovor>())).Returns(new List<IEntity>());

            var so = new ObrisiKupacSO(kupac);
            typeof(ObrisiKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.True(so.Result);
            mockBroker.Verify(b => b.Delete(It.IsAny<Kupac>()), Times.Once);
        }

        [Fact]
        public void ObrisiKupacSO_ShouldThrow_WhenKupacHasUgovori()
        {
            var kupac = new Kupac { IdKupac = 1 };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Ugovor>()))
                      .Returns(new List<IEntity> { new Ugovor() });

            var so = new ObrisiKupacSO(kupac);
            typeof(ObrisiKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            var ex = Assert.Throws<TargetInvocationException>(() => InvokeProtectedExecute(so));
            Assert.IsType<InvalidOperationException>(ex.InnerException);
        }

        #endregion

        #region PretraziKupacSO

        [Fact]
        public void PretraziKupacSO_ShouldReturnFizickoLice()
        {
            var kriterijum = new Kupac { IdKupac = 1 };
            var fl = new FizickoLice { IdKupac = 1 };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<FizickoLice>()))
                      .Returns(new List<IEntity> { fl });

            var so = new PretraziKupacSO(kriterijum);
            typeof(PretraziKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.NotNull(so.Result);
            Assert.Equal(fl.IdKupac, so.Result.IdKupac);
        }

        [Fact]
        public void PretraziKupacSO_ShouldReturnNull_WhenNoKupac()
        {
            var kriterijum = new Kupac { IdKupac = 999 };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<FizickoLice>())).Returns(new List<IEntity>());
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<PravnoLice>())).Returns(new List<IEntity>());
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Kupac>())).Returns(new List<IEntity>());

            var so = new PretraziKupacSO(kriterijum);
            typeof(PretraziKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Null(so.Result);
        }

        #endregion

        #region PromeniKupacSO

        [Fact]
        public void PromeniKupacSO_ShouldUpdateKupac()
        {
            var fl = new FizickoLice { IdKupac = 1, Email = "test@test.com" };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<FizickoLice>())).Returns(new List<IEntity> { fl });

            var so = new PromeniKupacSO(fl);
            typeof(PromeniKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.True(so.Result);
            mockBroker.Verify(b => b.Update(It.IsAny<Kupac>()), Times.AtLeastOnce);
        }

        #endregion

        #region VratiListuKupacSO

        [Fact]
        public void VratiListuKupacSO_ShouldReturnListOfKupci()
        {
            var kupci = new List<IEntity>
            {
                new Kupac { IdKupac = 1 },
                new Kupac { IdKupac = 2 }
            };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Kupac>())).Returns(kupci);

            var so = new VratiListuKupacSO(new Kupac());
            typeof(VratiListuKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Equal(2, so.Result.Count);
            Assert.Contains(so.Result, k => k.IdKupac == 1);
            Assert.Contains(so.Result, k => k.IdKupac == 2);
        }

        [Fact]
        public void VratiListuSviKupacSO_ShouldReturnAllKupci()
        {
            var kupci = new List<IEntity>
            {
                new Kupac { IdKupac = 1 },
                new Kupac { IdKupac = 2 },
                new Kupac { IdKupac = 3 }
            };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Kupac>())).Returns(kupci);

            var so = new VratiListuSviKupacSO();
            typeof(VratiListuSviKupacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Equal(3, so.Result.Count);
        }

        #endregion
    }
}
