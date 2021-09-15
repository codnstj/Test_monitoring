# Test_monitoring
.Net Core 5.0 을 이용해서 간단한 프로젝트를 생성한뒤 이를 Docker 와 인스턴스를 이용해서 배포후 EFK 모니터링 을 시스템을 간단히 구축 해봅니다.
## DockerCompose.yml
```yml
version: '3.9'

volumes:
  test-logs:

services:
  fluentd-tester:
    image:  test-monitoring:latest
    volumes:
        - test-logs:/app/logs
    depends_on:
        - "fluentd"
    expose:
        - "8080"
    ports:
        - "8080:80"

  fluentd:
    image: my_fluentd:0.0.1
    user: root
    volumes:
      - test-logs:/app/logs/fluentd-tester
    depends_on:
      - "elasticsearch"
    ports:
      - "24224:24224"
      - "24224:24224/udp"

  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:7.10.2
    environment:
      - "discovery.type=single-node"
    expose:
      - "9200"
    ports:
      - "9200:9200"

  #http://localhost:5601/app/management/kibana/indexPatterns
  #Add index pattern: fluentd-*
  #Time field: @timestam
  kibana:
    image: kibana:7.10.1
    depends_on:
      - "elasticsearch"
    ports:
      - "5601:5601"

  test_monitoring:
    image: ${DOCKER_REGISTRY-}testmonitoring
    build:
      context: .
      dockerfile: test_MOnitoring/Dockerfile
```
## fluentd.conf
```conf
#/fluentd/conf/fluent.conf
<source>
  @type tail
  path /app/logs/fluentd-tester/*.log
  pos_file /app/logs/td-agent/logtest.log.pos
  tag logtest
  <parse>
    @type json
  </parse>
</source>
<match *.**>
  @type copy
  <store>
    @type elasticsearch
    host elasticsearch
    port 9200
    logstash_format true
    logstash_prefix fluentd
    logstash_dateformat %Y%m%d
    include_tag_key true
    type_name access_log
    tag_key @log_name
    flush_interval 1s
  </store>
  <store>
    @type stdout
  </store>
</match>
```
- .Net 서버 는 test-monitoring 으로 빌드해줌
- Fluentd 관련 설정 은 config 파일을 설정해줘야해서 config 파일만 따로 작성후 빌드 
