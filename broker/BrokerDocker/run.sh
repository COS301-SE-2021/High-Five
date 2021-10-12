#!/bin/bash

#/opt/kafka/bin/zookeeper-server-start.sh /opt/kafka/config/zookeeper.properties
/opt/kafka/bin/zookeeper-server-start.sh /opt/kafka/config/zookeeper.properties > /dev/null 2>&1 &
sleep 30
/opt/kafka/bin/kafka-server-start.sh /opt/kafka/config/server.properties > /dev/null 2>&1 &
sleep 30

/opt/kafka/bin/kafka-topics.sh --create --topic SERVER_REGISTRATION --partitions 1 --bootstrap-server localhost:9092

gradle run
