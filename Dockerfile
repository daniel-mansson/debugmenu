# Dockerfile

FROM mcr.microsoft.com/dotnet/sdk:7.0

# Install NodeJs
RUN apt-get -y update \
    && apt-get install -y curl \
    && curl -sL https://deb.nodesource.com/setup_20.x | bash - \ 
    && apt-get install -y nodejs \
    && apt-get clean \
    && echo 'node verions:' $(node -v) \
    && echo 'npm version:' $(npm -v) \
    && echo 'dotnet version:' $(dotnet --version)

RUN npm install -g pnpm

WORKDIR /app
COPY debug-menu-web2/pnpm-lock.yaml ./
COPY debug-menu-web2/package.json  ./
RUN pnpm install --frozen-lockfile

COPY debug-menu-web2/ .
RUN pnpm build

EXPOSE 3000
CMD ["node", "build"]