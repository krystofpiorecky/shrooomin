# Minimal Ubuntu image to run Unity headless builds
FROM ubuntu:22.04

# Set working directory
WORKDIR /app

# Install missing runtime libs Unity needs
RUN apt-get update && apt-get install -y \
  libglib2.0-0 \
  libx11-6 \
  libxcursor1 \
  libxrandr2 \
  libxi6 \
  && rm -rf /var/lib/apt/lists/*

# Copy your built server files
COPY Builds/LinuxServer/ ./

# Expose your Mirror server ports (TCP + UDP)
EXPOSE 7777/tcp
EXPOSE 7777/udp

# Run the server
ENTRYPOINT ["./MyGameServer.x86_64", "-batchmode", "-nographics"]
