### Build the container
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
RUN mkdir -p /src

WORKDIR /src

### copy the source code
COPY src /src

### Build the app
RUN dotnet restore && \
    dotnet publish --no-restore -c Release -o /app


###########################################################

### Build the runtime container
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
EXPOSE 4120
WORKDIR /app

### create a user
RUN groupadd -g 4120 mikv && \
    useradd -r  -u 4120 -g mikv mikv && \
### dotnet needs a home directory for the secret store
    mkdir -p /home/mikv && \
    chown -R mikv:mikv /home/mikv

### run as mikv user
USER mikv

### copy the app
COPY --from=build /app .

ENTRYPOINT [ "dotnet",  "mikv.dll" ]
