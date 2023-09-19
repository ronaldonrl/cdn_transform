
#  CDN convert log file

This project have with transform objective one file in format MINHA CDN to Agora Format.


## Stack

**Console Application:** .Net 6, Serilog

**Unit Test:** xUnit, Moq


## Functionalities

- Download Log File
- Read Log File
- Validate Log File
- Process Log File to convert MINHA CDN to Agora


## Run Projetct

To run the application, enter in directory "_CandidateTesting.RonaldoRodriguesLopes.Application_" and use the following command:

```bash
  dotnet run https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt ./output/minhaCdn1.txt
```

To run the Test, enter directory "_CandidateTesting.RonaldoRodriguesLopes.Test_" and use the following command:

```bash
  dotnet test
```