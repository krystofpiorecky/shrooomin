# SHROOOMIN

## Server

### Build

Inside Editor -> top menu -> Build -> Build Linux Server
The build files will be generated inside Builds/LinuxServer

then make a new docker image and publish it
```sh
docker build -t shrooomin-server:latest .
docker tag shrooomin-server:latest krystofpiorecky/shrooomin-server:latest
docker push krystofpiorecky/shrooomin-server:latest
```

### Coolify Configuration

Ports Exposes = 7777
Ports Mappings = 7777:7777/tcp,7777:7777/udp

```sh
docker login
```
