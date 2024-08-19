# PokemonGameLib.Tests

**PokemonGameLib.Tests** is a test project designed to verify the functionality and correctness of the `PokemonGameLib` library through unit testing.

## Prerequisites

To work with the test project, you’ll need:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## Setup

To set up the test project:

1. **Clone the Repository**

   If you haven't already cloned the repository, do so with:

   ```bash
   git clone https://github.com/Kuzziv/PokemonLib.git
   cd PokemonLib
   ```

2. **Restore NuGet Packages**

   Restore the packages for both the main library and the test project:

   ```bash
   dotnet restore
   ```

3. **Build the Solution**

   Build the solution to ensure everything is configured correctly:

   ```bash
   dotnet build
   ```

## Running Tests

The project uses [xUnit](https://xunit.net/) for unit testing. To run the tests:

1. **Navigate to the Test Project Directory**

   ```bash
   cd PokemonGameLib.Tests
   ```

2. **Run the Tests**

   Execute the following command to run all tests:

   ```bash
   dotnet test
   ```

   This will execute all tests in the `PokemonGameLib.Tests` project and display the results in the terminal.

## Adding New Tests

To add new test cases:

1. **Open the Test Project**

   Open the `PokemonGameLib.Tests` project in your preferred code editor.

2. **Create New Test Classes or Methods**

   - Add new test files or classes as needed in the `PokemonGameLib.Tests` directory.
   - Write test methods to cover new features or bug fixes. Use the `[Fact]` attribute for individual test methods or `[Theory]` for parameterized tests.

   Example of a simple test method:

   ```csharp
   using Xunit;

   public class ExampleTests
   {
       [Fact]
       public void ExampleTestMethod()
       {
           // Arrange
           int expected = 5;
           int actual = 2 + 3;

           // Act & Assert
           Assert.Equal(expected, actual);
       }
   }
   ```

3. **Run Tests**

   After adding or modifying tests, run `dotnet test` again to ensure everything works as expected.

## Troubleshooting

- **Test Failures**: If tests fail, review the test output to diagnose issues. Ensure any failing tests are addressed promptly.
- **Dependency Issues**: Ensure all dependencies are restored and up to date by running `dotnet restore`.

## Contribution

Contributions to the test project are welcome! To contribute:

1. **Fork the Repository**: Create a fork of the repository on GitHub.
2. **Create a Branch**: Create a new branch for your changes.
3. **Make Changes**: Implement your changes and add tests if applicable.
4. **Submit a Pull Request**: Submit a pull request with a description of your changes.

Please ensure that your contributions adhere to the [code of conduct](../CODE_OF_CONDUCT.md) and [contributing guidelines](../CONTRIBUTING.md).

## License

The test project is licensed under the [MIT License](../LICENSE). See the [LICENSE](../LICENSE) file for more details.

## Contact

For any questions or support regarding the test project, please contact the author:

- **Name**: Mads Ludvigsen
- **Email**: [Mads72q2@edu.zealand.dk](mailto:Mads72q2@edu.zealand.dk)
