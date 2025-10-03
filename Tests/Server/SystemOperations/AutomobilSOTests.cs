using Common.Domain;
using DBBroker;
using Moq;
using ServerApp.SystemOperations.AutomobilSO;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tests.Server.SystemOperations
{
    public class AutomobilSOTests
    {
        private void InvokeProtectedExecute(object so)
        {
            var method = so.GetType().GetMethod(
                "ExecuteConcreteOperation",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
            method.Invoke(so, null);
        }

        #region VratiListuSviAutomobilSO

        [Fact]
        public void VratiListuSviAutomobilSO_ShouldReturnAllAutomobili()
        {
            var automobili = new List<IEntity>
            {
                new Automobil { IdAutomobil = 1, Model = "Tesla" },
                new Automobil { IdAutomobil = 2, Model = "BMW" }
            };
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Automobil>())).Returns(automobili);

            var so = new VratiListuSviAutomobilSO();
            typeof(VratiListuSviAutomobilSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Equal(2, so.Result.Count);
            Assert.Contains(so.Result, a => a.IdAutomobil == 1 && a.Model == "Tesla");
            Assert.Contains(so.Result, a => a.IdAutomobil == 2 && a.Model == "BMW");
        }

        [Fact]
        public void VratiListuSviAutomobilSO_ShouldReturnEmptyList_WhenNoAutomobili()
        {
            var mockBroker = new Mock<IBroker>();
            mockBroker.Setup(b => b.GetAll(It.IsAny<Automobil>())).Returns(new List<IEntity>());

            var so = new VratiListuSviAutomobilSO();
            typeof(VratiListuSviAutomobilSO)
                .GetField("broker", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(so, mockBroker.Object);

            InvokeProtectedExecute(so);

            Assert.Empty(so.Result);
        }

        #endregion
    }
}
