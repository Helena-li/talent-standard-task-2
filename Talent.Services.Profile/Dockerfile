FROM microsoft/dotnet:2.0.0-runtime
WORKDIR /dotnetapp
COPY ./.bin/Docker .
ENV ASPNETCORE_URLS http://*:60190
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Mars.Services.Profile.dll
