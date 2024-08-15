#!/bin/bash

mkdir DemoApp/src/DemoApp.Domain/Entities
cp User.cs DemoApp/src/DemoApp.Domain/Entities

mkdir DemoApp/tests/DemoApp.Domain.Tests/UnitTests
cp UserTests.cs DemoApp/tests/DemoApp.Domain.Tests/UnitTests

# Delete all Class1.cs files in the project
find DemoApp -name UnitTest1.cs -type f -delete

#cd DemoApp
dotnet test -v detailed DemoApp/tests/DemoApp.Domain.Tests/DemoApp.Domain.Tests.csproj