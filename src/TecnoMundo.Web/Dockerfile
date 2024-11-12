FROM node:21 as builder

WORKDIR /app

COPY . .

RUN npm install

RUN npm run build

FROM nginx:alpine

RUN rm -rf /usr/share/nginx/html/*

COPY  nginx.conf /etc/nginx/nginix.conf

COPY --from=builder /app/dist/tecno-mundo.web/browser usr/share/nginx/html

COPY  mime.types /etc/nginx/mime.types

EXPOSE 80

RUN echo "mainFileName=\"\$(ls /usr/share/nginx/html/main*.js)\" && \
          envsubst '\$BASE_API_GATEWAY ' < \${mainFileName} > main.tmp && \
          mv main.tmp  \${mainFileName} && nginx -g 'daemon off;'" > run.sh

ENTRYPOINT ["sh", "run.sh"]