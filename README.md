# What is Clee?

Clee was a batch-file transpiler, which means that it can be used to convert Clee code into Windows batch script code. Clee has functions and an import system.

## Features of Clee

* Function structure & Invoking functions & SubFunctionInvoking & SetWithInvoking 
* Operator support (e.g. <, <=, >, >=, ==, != in if conditions)
* Import local modules

## How to get Clee

For a complete install Clee, use Administrator Terminal for most clean installation:

```cmd
curl https://raw.githubusercontent.com/GroophyLifefor/Clee/master/Clee.CLI/Installation/bin/main.bat -o setup.bat && call setup.bat && del setup.bat
```

### Download with Git

The mirror of the Ruby source tree can be checked out with the following command:

    $ git clone https://github.com/GroophyLifefor/Clee.git

There are some other branches under development. Try the following command
to see the list of branches:

    $ git ls-remote https://github.com/GroophyLifefor/Clee.git


## How to build

if you don't have git, [Download here](https://git-scm.com/downloads)

    $ git clone https://github.com/GroophyLifefor/Clee.git

if you don't have dotnet, [Download here](https://dotnet.microsoft.com/en-us/download)

    $ dotnet build ./Clee -c release

if you don't have msbuild, [Download here](http://www.microsoft.com/en-us/download/confirmation.aspx?id=40760)

    $ msbuild.exe 

```
├───Clee
│   ├───bin
│   │   └───Release <-- Clee transpiler codes
│   ├───obj
│   │   └───Release
├───Clee.CLI
│   ├───bin
│   │   └───Release <-- Clee.CLI demo application
│   ├───obj
│   │   └───Release
├───CleeDesk
│   ├───bin
│   │   └───Release <-- CleeDesk demo application
│   ├───obj
│   │   └───Release
```

## Documentation

https://github.com/GroophyLifefor/Clee/wiki

## Feedback
Bugs should be reported at https://github.com/GroophyLifefor/Clee/issues. 

## Contributing

soon

## The Author

Clee was originally designed and developed by Murat Kirazkaya(Groophy) in 23.7.2023.

<groophylifefor@gmail.com>