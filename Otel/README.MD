


# spuštění elasticku
docker network create elastic

docker run -d --name es01 --net elastic -p 9200:9200 -p 8200:8200 -m 1GB docker.elastic.co/elasticsearch/elasticsearch:8.9.2
docker run -d --name kib01 --net elastic -p 5601:5601 -p 8201:8200 docker.elastic.co/kibana/kibana:8.9.2


docker exec -it es01 /usr/share/elasticsearch/bin/elasticsearch-reset-password -u elastic
docker exec -it es01 /usr/share/elasticsearch/bin/elasticsearch-create-enrollment-token -s kibana

docker exec -it kib01 bin/kibana-verification-code



