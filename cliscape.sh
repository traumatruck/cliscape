#!/bin/bash
cd "$(dirname "$0")/CliScape.Cli"
dotnet run -- "$@"