using System;
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

        const string ExampleXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <data name=""Common_AuthenicationFailed"" xml:space=""preserve"">
    <value>Authentifikation fehlgeschlagen</value>
  </data>
  <!-- single line comment -->
  <data name=""Common_Billable"" xml:space=""preserve"">
    <value>Verrechenbar</value>
  </data>
  <!-- multi
  line
  comment -->
  <data name=""Common_BreakTime"" xml:space=""preserve"">
    <value>Pausen</value>
  </data>
</root>
";

        [Fact]
        public void Constructor_WithXml_Succeeds()
        {
            var writer = new ResxWriter(ExampleXml);
        }

        [Fact]
        public void Contains_OnExistingKey_ReturnsTrue()
        {
            var writer = new ResxWriter(ExampleXml);

            Assert.True(writer.Contains("Common_Billable"));
        }

        [Fact]
        public void Contains_OnMissinggKey_ReturnsFalse()
        {
            var writer = new ResxWriter(ExampleXml);

            Assert.False(writer.Contains("SomethingElse"));
        }

        [Fact]
        public void Set_OnNewKey_AddsNode()
        {
            var writer = new ResxWriter(ExampleXml);

            writer.Set("Test", "New value");

            Assert.Equal(4, writer.Elements().Count);
        }

        [Fact]
        public void Set_OnExistingKey_SetsValue()
        {
            var writer = new ResxWriter(ExampleXml);

            writer.Set("Common_Billable", "New value");

            Assert.Equal(3, writer.Elements().Count);
        }

        [Fact]
        public void Export_Succeeds()
        {
            var writer = new ResxWriter(ExampleXml);

            writer.Set("Common_Billable", "New value");

            string result = writer.Export();
        }
    }
}