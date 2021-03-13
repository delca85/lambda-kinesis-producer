FROM public.ecr.aws/lambda/dotnet:core3.1 AS base  

FROM mcr.microsoft.com/dotnet/sdk:3.1 as build  
WORKDIR /src  
COPY ["KinesisProducer.csproj", "base/"]  
RUN dotnet restore "base/KinesisProducer.csproj"  

WORKDIR "/src"  
COPY . .  
RUN dotnet build "KinesisProducer.csproj" --configuration Release --output /app/build  

FROM build AS publish
RUN dotnet publish "KinesisProducer.csproj" \  
    --configuration Release \   
    --runtime linux-x64 \  
    --self-contained false \   
    --output /app/publish \  
    -p:PublishReadyToRun=true

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish/ /var/task/
CMD ["KinesisProducer::KinesisProducer.Function::FunctionHandler"]