# Build Phase
# Starts from the .NET SDK 8.0 image, which includes tools for building .NET apps.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Sets the current working DIR /APP (currently empty)
WORKDIR /App

# Copies all files to the /App folder, which now contains the source files
COPY . ./
# Creates /obj folder which downloads source files from Nugget
RUN dotnet restore
# Creates and /out folder and stores all the publish app files such as dll's
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/sdk:8.0 As Runtime
WORKDIR /App

# Copies all publish files into the container
COPY --from=build /App/out .

# Defines the first file to run - which executes dotnet ipcheckapp.dll
ENTRYPOINT ["dotnet", "ipcheckapp.dll"]
