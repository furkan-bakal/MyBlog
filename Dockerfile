ARG DOTNET_VERSION=9.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Önce sadece proje dosyaları: kaynak değiştiğinde restore katmanı yeniden çalışmasın.
COPY MyBlog.sln ./
COPY Core/Core.csproj             Core/
COPY Repository/Repository.csproj Repository/
COPY Service/Service.csproj       Service/
COPY WebApi/WebApi.csproj         WebApi/
RUN dotnet restore MyBlog.sln

COPY Core/       Core/
COPY Repository/ Repository/
COPY Service/    Service/
COPY WebApi/     WebApi/

RUN dotnet publish WebApi/WebApi.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
WORKDIR /app

# curl yalnızca HEALTHCHECK için; aspnet imajında ne curl ne wget geliyor.
RUN apt-get update \
 && apt-get install -y --no-install-recommends curl \
 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish ./

# wwwroot süreç başlamadan var olmalı, yoksa WebRootPath null kalır ve UseStaticFiles()
# sessizce hiçbir şey servis etmez (WebApi/wwwroot/uploads .gitignore'da).
# chown named volume oluşturulmadan önce yapılıyor ki volume doğru sahiplikle tohumlansın.
RUN mkdir -p /app/wwwroot/uploads/articles /app/wwwroot/uploads/content /keys \
 && chown -R $APP_UID:$APP_UID /app/wwwroot /keys

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

HEALTHCHECK --interval=15s --timeout=3s --start-period=60s --retries=5 \
  CMD curl -fsS http://127.0.0.1:8080/health || exit 1

USER $APP_UID
ENTRYPOINT ["dotnet", "WebApi.dll"]
