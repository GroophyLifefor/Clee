namespace Clee.CLI.Resource;

public class CleeHelloWorld
{
    public static string Get()
        => """
@echo off

fn main()
{-
    ./print("Hello world!")
-}

fn print(var)
{-
    echo %var%
-}
""";
}