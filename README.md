# Mutant
[![Build Status](https://dev.azure.com/amorrison17/Mutant/_apis/build/status/a-morrison.Mutant?branchName=master)](https://dev.azure.com/amorrison17/Mutant/_build/latest?definitionId=1&branchName=master)
### Migration using TFS and Ant
*Disclaimer* Never been tested on an OS other than Windows 10.

## About
Mutant is a tool currently in development to make deployments to Salesforce and the Force.com stack easier.
Its main use case is to facilitate deployments using git along with some other CI/CD tool such as Jenkins or Azure Pipelines.

[Nuget](https://www.nuget.org/packages/AMorrison.Mutant/)

## Backlog
A publicly accessable backlog and roadmap for Mutant can be found [here](https://dev.azure.com/amorrison17/Mutant/_workitems/recentlyupdated)
although it may not be up to date.

## Usage
Type `Mutant help` or `Mutant help <command>` for help.
### Init
```
'Init' - Initializes process with common parameters like the required username and password. Should be called first.

Expected usage: Mutant Init <options> 
<options> available:
  -t, --target-url=VALUE     Required. URL of target org.
  -u, --username=VALUE       Required. Username
  -p, --password=VALUE       Required. Password
  -d, --working-directory=VALUE
                             Required. Full path of working directory.
```

### Deploy
```
'Deploy' - Deploys changes

Expected usage: Mutant Deploy <options> 
<options> available:
  -d, --deployment-type[=VALUE]          Optional. If not used, tool defaults to selective
                               deployment. Comprehensive deployment. Pushes all
                               objects regardless of status.
  -t, --test-level[=VALUE]    Optional. Specifies test level.
  -c, --base-commit[=VALUE]  Optional. Deploys changes from HEAD to specified
                               commit hash.
```

## Prerequisites
TODO

## Building From Source (Never been tested)
In theory, all you should have to do is to fork this repository, clone in Visual Studio, then build the project.
Once the build is successful if you navigate to the output directory which should look something like
`C:\<Path to your local repository>\Mutant\bin\Release\netcoreapp2.1` you should be able to run the program 
using the command `dotnet Mutant.dll`.
