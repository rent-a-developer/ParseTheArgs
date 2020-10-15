# How to create a release

## Create NuGet package

- Open Visual Studio Developer Command Prompt
- Change directory to the root directory of this solution
- Run "dotnet pack -c Release ParseTheArgs\ParseTheArgs.csproj"

## Sign NuGet package

- Open Visual Studio Developer Command Prompt
- Change directory to the root directory of this solution
- Run "nuget sign ParseTheArgs\bin\Release\ParseTheArgs.1.0.2.nupkg -CertificatePath C:\Path\To\CodeSigningCertificate.pfx -Timestamper http://timestamp.comodoca.com/rfc3161"
- Enter the encryption password for the code signing certificate.

## Sign NuGet symbols package

- Open Visual Studio Developer Command Prompt
- Change directory to the root directory of this solution
- Run "nuget sign ParseTheArgs\bin\Release\ParseTheArgs.1.0.2.snupkg -CertificatePath C:\Path\To\CodeSigningCertificate.pfx -Timestamper http://timestamp.comodoca.com/rfc3161"
- Enter the encryption password for the code signing certificate.