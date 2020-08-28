using Microsoft.AspNetCore.Mvc.Testing;
using simple_aspnetcore_react_shared_validation;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests
{
    public class ValidationControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ValidationControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldReturnValidatorAsJson()
        {
            // Arrange
            var client = _factory.CreateClient();
            var expected = "{\"CreateUserDtoValidator\":{\"FirstName\":[{\"name\":\"NotEmptyValidator\",\"errorMessage\":\"'{PropertyName}' must not be empty.\"},{\"name\":\"MaximumLengthValidator\",\"errorMessage\":\"The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.\",\"Min\":0,\"Max\":100}],\"LastName\":[{\"name\":\"NotEmptyValidator\",\"errorMessage\":\"'{PropertyName}' must not be empty.\"},{\"name\":\"MaximumLengthValidator\",\"errorMessage\":\"The length of '{PropertyName}' must be {MaxLength} characters or fewer. You entered {TotalLength} characters.\",\"Min\":0,\"Max\":100}],\"Email\":[{\"name\":\"AspNetCoreCompatibleEmailValidator\",\"errorMessage\":\"'{PropertyName}' is not a valid email address.\"}],\"Age\":[{\"name\":\"NotNullValidator\",\"errorMessage\":\"'{PropertyName}' must not be empty.\"},{\"name\":\"GreaterThanOrEqualValidator\",\"errorMessage\":\"'{PropertyName}' must be greater than or equal to '{ComparisonValue}'.\",\"Comparison\":4,\"ValueToCompare\":18}],\"BirthDate\":[{\"name\":\"NotNullValidator\",\"errorMessage\":\"'{PropertyName}' must not be empty.\"},{\"name\":\"LessThanOrEqualValidator\",\"errorMessage\":\"'{PropertyName}' must be less than or equal to '{ComparisonValue}'.\",\"Comparison\":5,\"ValueToCompare\":\"2002-01-01T00:00:00\"}]}}";

            // Act
            var response = await client.GetAsync("api/validation");

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(expected, result);
        }
    }
}
