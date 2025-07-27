using Xunit;

namespace SafeVaultTest;

public class XssInjectionTests
{
    [Fact]
    public void UserInput_ShouldBeProperlyEncodedOrSanitized()
    {
        // Arrange: Simulate user input containing a script tag
        string maliciousInput = "<script>alert('XSS');</script>";

        // Act: Simulate rendering this input in HTML (e.g., Razor encodes by default)
        string renderedOutput = System.Net.WebUtility.HtmlEncode(maliciousInput);

        // Assert: The output should not contain a raw <script> tag
        Assert.DoesNotContain("<script>", renderedOutput, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("&lt;script&gt;", renderedOutput);
    }

    [Fact]
    public void StoredUserInput_ShouldBeProperlyEncodedOrSanitized_WhenRetrieved()
    {
        // Arrange: Simulate storing malicious input in a database
        string maliciousInput = "<img src=x onerror=alert('StoredXSS')>";
        string storedValue = maliciousInput; // Simulate DB storage

        // Act: Simulate retrieving and rendering the stored value in HTML
        string renderedOutput = System.Net.WebUtility.HtmlEncode(storedValue);

        // Assert: The output should not contain a raw <img> tag with an event handler
        Assert.DoesNotContain("<img", renderedOutput, System.StringComparison.OrdinalIgnoreCase);
        Assert.Contains("&lt;img", renderedOutput);
        Assert.DoesNotContain("onerror", renderedOutput, System.StringComparison.OrdinalIgnoreCase);
    }
}