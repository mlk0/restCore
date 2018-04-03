FROM microsoft/aspnetcore-build:2.0 AS dnc-build
WORKDIR /app

COPY *.sln ./

RUN dotnet restore
COPY . ./
# RUN dotnet publish ./httpClientApi/httpClientApi.csproj -c Release -o buildDir
RUN dotnet publish ./httpClientApi/httpClientApi.csproj -c Release -o ..\buildDir

FROM microsoft/aspnetcore:2.0
WORKDIR /app

# COPY --from=dnc-build /app/httpClientApi/buildDir .
COPY --from=dnc-build /app/buildDir .

ENTRYPOINT [ "dotnet", "httpClientApi.dll" ]