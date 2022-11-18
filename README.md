# A tdd blog in asp dot net

instructions: https://github.com/theneubeck/tdd-blog-instructions


## Rules

* You can only run your code with tests
* Take turns. One person writes a test - The other writes the code.
* _Always_ write the test **first**

This repo uses
* dotnet version 6
* sqllite: https://www.sqlite.org/download.html

```
# some commands

# run test
dotnet test
# run specific test
dotnet test --filter CreateBlogPostTest
```


## Some links

* Integration testing in asp dotnet: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
* Asserting with fluent assertions https://fluentassertions.com/introduction
* Mock out third parties: https://github.com/justeat/httpclient-interception/blob/main/tests/HttpClientInterception.Tests/Examples.cs