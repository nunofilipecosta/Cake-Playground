using System;
using Xunit;

namespace web.tests
{
    public class Class1
    {
        [Xunit.Fact]
        public void Add()
        {
            Assert.True(true);
        }

        [Xunit.Fact]
        public void Subtract()
        {
            Assert.False(false);
        }
    }


}
