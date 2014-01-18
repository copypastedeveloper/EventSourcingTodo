using System;
using ExpectedObjects;
using Example.Common.Utilities;
using NUnit.Framework;

namespace Example.Common.UnitTests
{
    [TestFixture]
    public class CloneableHelperTests
    {
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Clone_NullObject_ReturnsArgumentNullException()
        {
            CloneableHelper.Clone(null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void Clone_NonSerializableObject_ReturnsArgumentException()
        {
            CloneableHelper.Clone(new NonSerializableObject());
        }
    }

    public class NonSerializableObject
    {

    }

    
    [Serializable]
    public class SerializableObject
    {
        public string Username { get; set; }
    }
}
