#/fluentd/dockerfile
FROM fluentd:v1.14-1
COPY /conf/* /fluentd/etc/
USER root
RUN ["gem", "install", "fluent-plugin-elasticsearch", "--no-document", "--version", "4.3.3"]
USER fluent
