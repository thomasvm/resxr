using Resxr;
using Xunit;

namespace Resxr.Tests
{
    public class ResxWriterTests
    {
        [Fact]
        public void Constructor_WithoutContent_Succeeds()
        {
            var writer = new ResxWriter();
        }
    }
}