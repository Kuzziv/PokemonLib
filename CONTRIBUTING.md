# Contributing to PokemonGameLib

Thank you for considering contributing to **PokemonGameLib**! Whether it's fixing bugs, improving documentation, or adding new features, your contributions are greatly appreciated. To ensure a smooth collaboration, please follow these guidelines.

## Table of Contents

- [Getting Started](#getting-started)
- [How to Contribute](#how-to-contribute)
  - [Reporting Issues](#reporting-issues)
  - [Submitting Changes](#submitting-changes)
  - [Creating Pull Requests](#creating-pull-requests)
- [Coding Guidelines](#coding-guidelines)
- [Running Tests](#running-tests)
- [Code of Conduct](#code-of-conduct)

## Getting Started

1. **Fork the Repository**: Start by forking the repository to your GitHub account.

2. **Clone Your Fork**: Clone your forked repository to your local machine.

   ```bash
   git clone https://github.com/Kuzziv/pokemonLib.git
   cd pokemonLib
   ```

3. **Create a Branch**: Create a new branch to work on your changes.

   ```bash
   git checkout -b your-branch-name
   ```

4. **Install Dependencies**: Ensure you have the necessary dependencies installed.

   ```bash
   dotnet restore
   ```

5. **Build the Project**: Build the project to ensure everything is set up correctly.

   ```bash
   dotnet build
   ```

## How to Contribute

### Reporting Issues

If you find a bug or have a suggestion for improvement, please create an issue on GitHub:

1. **Search Existing Issues**: Before opening a new issue, please check if the issue has already been reported.

2. **Create a New Issue**: If your issue hasn’t been reported, [create a new issue](https://github.com/kuzziv/pokemonLib/issues) and provide the following information:
   - **Description**: A clear and concise description of the issue.
   - **Steps to Reproduce**: Detailed steps to reproduce the issue.
   - **Expected Behavior**: What you expected to happen.
   - **Actual Behavior**: What actually happened.
   - **Environment**: Details about your environment (e.g., .NET version, operating system).

### Submitting Changes

1. **Commit Your Changes**: Once you’ve made your changes, commit them with a clear and descriptive message.

   ```bash
   git add .
   git commit -m "Description of the change"
   ```

2. **Test Your Changes**: Run the existing tests and, if applicable, add new tests to cover your changes.

   ```bash
   dotnet test
   ```

3. **Push to Your Fork**: Push your changes to your forked repository.

   ```bash
   git push origin your-branch-name
   ```

### Creating Pull Requests

When your changes are ready, submit a pull request (PR):

1. **Open a Pull Request**: Go to the original repository and [create a pull request](https://github.com/kuzziv/pokemonLib/pulls) from your fork and branch.

2. **Describe Your Changes**: Provide a detailed description of your changes in the PR description. Mention any issues your PR fixes (e.g., "Fixes #issue_number").

3. **Review Process**: Your PR will be reviewed by the maintainers. They may request changes or ask questions. Please be responsive and address any feedback.

4. **Merge**: Once your PR is approved, it will be merged into the main branch. Congratulations on your contribution!

## Coding Guidelines

To maintain consistency and quality in the codebase, please adhere to the following guidelines:

1. **Follow the .NET Naming Conventions**: Use PascalCase for class and method names, camelCase for variables and parameters.

2. **Write Clear and Concise Code**: Ensure your code is easy to read and understand. Avoid unnecessary complexity.

3. **Comment Your Code**: Provide comments where necessary, especially for complex logic. Ensure your comments are clear and helpful.

4. **Adhere to SOLID Principles**: Ensure your code follows the SOLID principles for object-oriented design.

5. **Write Tests**: Ensure that all new features and bug fixes include appropriate test coverage. Use [xUnit](https://xunit.net/) for writing unit tests.

## Running Tests

To ensure that your changes do not break existing functionality, run the tests using the following command:

```bash
dotnet test
```

If you add new features or fix bugs, please include tests that verify your changes.

## Code of Conduct

This project adheres to a [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to [Mads72q2@edu.zealand.dk](mailto:Mads72q2@edu.zealand.dk).

---

Thank you for your contribution! We look forward to collaborating with you to improve **PokemonGameLib**.
