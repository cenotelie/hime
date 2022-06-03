FROM buildpack-deps:22.04-curl
LABEL maintainer="Laurent Wouters <lwouters@cenotelie.fr>" vendor="Association Cénotélie" description="Hime Build environment"

# Custom Home
RUN mkdir /home/builder
ENV HOME=/home/builder

# Add required packages
RUN apt update && apt install -y --no-install-recommends \
		locales \
		software-properties-common \
		apt-transport-https \
		gnupg \
		ca-certificates \
		build-essential \
		zip \
		git \
		pkg-config \
		libssl-dev \
		musl-tools \
    && rm -rf /var/lib/apt/lists/*

# Add dotnet core and Mono
RUN wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
	&& dpkg -i packages-microsoft-prod.deb \
	&& rm packages-microsoft-prod.deb
RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb https://download.mono-project.com/repo/ubuntu stable-bionic main" | tee /etc/apt/sources.list.d/mono-official-stable.list
RUN apt update && apt install -y --no-install-recommends \
		dotnet-sdk-6.0 \
		mono-devel \
    && rm -rf /var/lib/apt/lists/*

# Add JDK 8 and maven
RUN add-apt-repository ppa:openjdk-r/ppa && apt install -y --no-install-recommends \
		openjdk-8-jdk \
    && rm -rf /var/lib/apt/lists/*
RUN apt update && apt install -y --no-install-recommends \
		maven \
    && rm -rf /var/lib/apt/lists/*

# Add support for Rust
RUN curl https://sh.rustup.rs -sSf | sh -s -- -y && \
	export PATH="${HOME}/.cargo/bin:${PATH}" && \
    rustup target add x86_64-unknown-linux-musl && \
	mkdir /home/builder/.cargo/registry && \
	chmod -R a+rwX /home/builder

# Environment
RUN locale-gen fr_FR.UTF-8
ENV LC_ALL=fr_FR.UTF-8
ENV JAVA_HOME=/usr/lib/jvm/java-8-openjdk-amd64
ENV _JAVA_OPTIONS=-Duser.home=/home/builder
ENV PATH="${HOME}/.cargo/bin:${PATH}"
ENV RUST_BACKTRACE=1
ENV GPG_TTY=/dev/pts/0

# Specify volumes
VOLUME /src
VOLUME /home/builder/.cargo/registry