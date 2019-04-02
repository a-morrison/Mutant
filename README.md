# Mutant
[![Build Status](https://dev.azure.com/amorrison17/amorrison17/_apis/build/status/a-morrison.Mutant?branchName=master)](https://dev.azure.com/amorrison17/amorrison17/_build/latest?definitionId=1&branchName=master)
### Migration using TFS and Ant
*Disclaimer* Never been tested on an OS other than Windows 10.

## About
Mutant is a tool currently in development to make deployments to Salesforce and the Force.com stack easier.
Its main use case is to facilitate deployments using git along with some other CI/CD tool such as Jenkins or Azure Pipelines.

## Backlog
A publicly accessable backlog and roadmap for Mutant can be found [here](https://dev.azure.com/amorrison17/Mutant/_boards/board/t/Mutant%20Team/Features)
although it may not be up to date.

## Usage
TODO

## Prerequisites
TODO

## Building From Source (Never been tested)
In theory, all you should have to do is to fork this repository, clone in Visual Studio, then build the project.
Once the build is successful if you navigate to the output directory which should look something like
`C:\<Path to your local repository>\Mutant\bin\Release\netcoreapp2.1` you should be able to run the program 
using the command `dotnet Mutant.dll`.
