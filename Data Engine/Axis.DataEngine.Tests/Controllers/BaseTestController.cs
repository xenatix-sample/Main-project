using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Axis.DataEngine.Tests.Controllers
{
    [TestClass]
    public class BaseTestController<TProvider, TController>
        where TProvider : class
        where TController: class
    {
        #region Class Variables

        protected TProvider _dataProvider;
        protected TController _controller;
        protected Mock<TProvider> _mock;

        #endregion

        #region Test Methods

        [TestInitialize]
        public virtual void Initialize()
        {
            _mock = new Mock<TProvider>();
            _dataProvider = _mock.Object;
        }

        #endregion
    }
}
