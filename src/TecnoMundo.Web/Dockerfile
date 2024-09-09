FROM node:alpine

WORKDIR /app

COPY . ./

RUN npm install -g @angular/cli

RUN npm install

FROM nginx:alpine
COPY --from=build /app/dist/TecnoMundo.Web /usr/share/nginx/html

EXPOSE 8080

COPY config.template.json /usr/share/nginx/html/assets/
COPY entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]