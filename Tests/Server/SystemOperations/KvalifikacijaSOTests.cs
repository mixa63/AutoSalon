using Common.Domain;
using DBBroker;
using Moq;
using ServerApp.SystemOperations.KvalifikacijaSO;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Tests.Server.SystemOperations
{
    public class KvalifikacijaSOTests
    {
        private void InvokeProtectedExecute(object so)
        {
            var method = so.GetType().GetMethod(
                "ExecuteConcreteOperation",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            method.Invoke(so, null);
        }

        #region UbaciKvalifikacijaSO

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(555)]
        public void UbaciKvalifikacijaSO_ShouldSetResultId(int generatedId)
        {
            var kvalifikacija = new Kvalifikacija();
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.AddWithReturnId(It.IsAny<Kvalifikacija>())).Returns(generatedId);

            var so = new UbaciKvalifikacijaSO(kvalifikacija);
            typeof(UbaciKvalifikacijaSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.NotNull(so.Result);
            Assert.Equal(generatedId, so.Result.IdKvalifikacija);
        }

        [Fact]
        public void UbaciKvalifikacijaSO_ShouldThrow_WhenKvalifikacijaIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UbaciKvalifikacijaSO(null));
        }

        #endregion

        #region VratiListuSviKvalifikacijaSO

        [Fact]
        public void VratiListuSviKvalifikacijaSO_ShouldReturnAllKvalifikacije()
        {
            var kvalifikacije = new List<IEntity>
            {
                new Kvalifikacija { IdKvalifikacija = 1 },
                new Kvalifikacija { IdKvalifikacija = 2 }
            };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Kvalifikacija>())).Returns(kvalifikacije);

            var so = new VratiListuSviKvalifikacijaSO();
            typeof(VratiListuSviKvalifikacijaSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Equal(2, so.Result.Count);
            Assert.Contains(so.Result, k => k.IdKvalifikacija == 1);
            Assert.Contains(so.Result, k => k.IdKvalifikacija == 2);
        }

        [Fact]
        public void VratiListuSviKvalifikacijaSO_ShouldReturnEmptyList_WhenNoKvalifikacije()
        {
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Kvalifikacija>())).Returns(new List<IEntity>());

            var so = new VratiListuSviKvalifikacijaSO();
            typeof(VratiListuSviKvalifikacijaSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Empty(so.Result);
        }

        #endregion
    }
}
