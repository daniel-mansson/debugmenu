# Dockerfile


FROM node:16-alpine

RUN npm install -g pnpm

WORKDIR /app
COPY debug-menu-web2/pnpm-lock.yaml ./
COPY debug-menu-web2/package.json  ./
RUN pnpm install --frozen-lockfile

COPY debug-menu-web2/ .
RUN pnpm build

EXPOSE 3000
CMD ["node", "run", "start"]