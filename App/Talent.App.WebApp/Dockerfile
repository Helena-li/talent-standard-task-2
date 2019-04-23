FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /talentWebPortal
COPY ./.bin/Docker .
ENV ASPNETCORE_URLS http://*:61771
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Talent.App.WebApp.dll


