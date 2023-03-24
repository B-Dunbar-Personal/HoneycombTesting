# Honeycomb Testing

## What is this

This is a demo application to demonstrate integration testing with containers.
This had a supporting presentation to demonstrate the theory to others.

This allows us to fail fast by testing the real application with a database in the pipeline, protecting our environment's and supporting quicker feedback.
Our tests are much more simple when testing against the container vs the normal mocking style.

Refactoring is easier, so long as the response model doesn't change from our endpoint. We don't need to change of our tests, the mocks in the unit tests however would change. Our tests become less brittle and require less work when making large changes over the application.

There are two sets of tests:
- [One using the container](Demo.Api.Tests/SimpleDockerTests.cs)
- [One using Moq](Demo.Api.Tests/SimpleTestWithoutDocker.cs)

They are testing the same result.
Notice the easier readability of the tests using the container, as they are testing the api endpoint and the result.

## Running the project
### Requirements
- Visual Studio
- NetCore 6 or later installed
- Docker (Must be running)

Start Docker

Navigate to the root folder and run 
```
docker compose up -d'
```

Restore the project and run the tests from the testing window in Visual Studio
