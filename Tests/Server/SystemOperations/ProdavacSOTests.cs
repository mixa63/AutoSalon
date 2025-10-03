using Common.Domain;
using DBBroker;
using Moq;
using ServerApp.SystemOperations.ProdavacSO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tests.Server.SystemOperations
{
    public class ProdavacSOTests
    {
        private void InvokeProtectedExecute(object so)
        {
            var method = so.GetType().GetMethod(
                "ExecuteConcreteOperation",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            method.Invoke(so, null);
        }

        #region PrijaviProdavacSO

        [Fact]
        public void PrijaviProdavacSO_ShouldReturnProdavac_WhenValidCredentials()
        {
            var username = "user1";
            var password = "pass1";
            var prodavac = new Prodavac { Username = username, Password = password };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Prodavac>()))
                      .Returns(new List<IEntity> { prodavac });

            var so = new PrijaviProdavacSO(username, password);
            typeof(PrijaviProdavacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.NotNull(so.Result);
            Assert.Equal(username, so.Result.Username);
        }

        [Fact]
        public void PrijaviProdavacSO_ShouldReturnNull_WhenInvalidCredentials()
        {
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetByCondition(It.IsAny<Prodavac>()))
                      .Returns(new List<IEntity>());

            var so = new PrijaviProdavacSO("user", "wrong");
            typeof(PrijaviProdavacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Null(so.Result);
        }

        [Theory]
        [InlineData("", "pass")]
        [InlineData("user", "")]
        [InlineData("", "")]
        public void PrijaviProdavacSO_ShouldThrow_WhenMissingCredentials(string username, string password)
        {
            var so = new PrijaviProdavacSO(username, password);
            typeof(PrijaviProdavacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, new Mock<IBroker>().Object);

            var ex = Assert.Throws<TargetInvocationException>(() => InvokeProtectedExecute(so));
            Assert.IsType<Exception>(ex.InnerException);
            Assert.Contains("Morate uneti korisničko ime i lozinku", ex.InnerException.Message);
        }

        #endregion

        #region VratiListuSviProdavacSO

        [Fact]
        public void VratiListuSviProdavacSO_ShouldReturnAllProdavci()
        {
            var prodavci = new List<IEntity>
            {
                new Prodavac { IdProdavac = 1 },
                new Prodavac { IdProdavac = 2 }
            };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Prodavac>())).Returns(prodavci);

            var so = new VratiListuSviProdavacSO();
            typeof(VratiListuSviProdavacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Equal(2, so.Result.Count);
            Assert.Contains(so.Result, p => p.IdProdavac == 1);
            Assert.Contains(so.Result, p => p.IdProdavac == 2);
        }

        [Fact]
        public void VratiListuSviProdavacSO_ShouldReturnEmptyList_WhenNoProdavci()
        {
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Prodavac>())).Returns(new List<IEntity>());

            var so = new VratiListuSviProdavacSO();
            typeof(VratiListuSviProdavacSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Empty(so.Result);
        }

        #endregion
    }
}
