FROM ubuntu:latest

SHELL ["/bin/bash", "-c"]

# create working directory
RUN mkdir -p /broker/tempwebapp/WEB-INF/lib
WORKDIR /broker

# copy broker code into container
ADD BrokerServer /broker/
COPY BrokerDocker/BrokerWebSocket.jar /broker/tempwebapp/WEB-INF/lib
COPY BrokerDocker/run.sh .

# install prerequisite packages
RUN apt update && apt upgrade -y
RUN TZ="Africa/Johannesburg" apt install tzdata -y
RUN apt install openjdk-17-jre-headless wget unzip -y

# install kafka
RUN wget https://archive.apache.org/dist/kafka/2.8.0/kafka_2.13-2.8.0.tgz
RUN mkdir /opt/kafka
RUN tar -xf kafka_2.13-2.8.0.tgz -C /opt/kafka --strip-components=1
COPY BrokerDocker/server.properties /opt/kafka/config

# install gradle
RUN mkdir /opt/gradle
RUN wget https://services.gradle.org/distributions/gradle-7.2-bin.zip
RUN unzip -d /opt/gradle gradle-7.2-bin.zip
ENV PATH="/opt/gradle/gradle-7.2/bin:${PATH}"

# run kafka
ENV KAFKA_CREATE_TOPIC="bash /opt/kafka/bin/kafka-topics.sh --create --topic {topic} --partitions 3 --bootstrap-server localhost:9092"
ENV KAFKA_DELETE_TOPIC="bash /opt/kafka/bin/kafka-topics.sh --bootstrap-server localhost:9092 --delete --topic {topic}"
ENV BROKER_CLIENT_PORT="6666"

RUN chmod +x run.sh

ENTRYPOINT ["bash", "run.sh"]
